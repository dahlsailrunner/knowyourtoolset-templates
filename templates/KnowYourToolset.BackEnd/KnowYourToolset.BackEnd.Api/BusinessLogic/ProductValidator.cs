using FluentValidation;
using FluentValidation.Results;
using KnowYourToolset.BackEnd.Api.ApiModels;
using KnowYourToolset.BackEnd.Api.Data;
using System.Diagnostics;

namespace KnowYourToolset.BackEnd.Api.BusinessLogic;

public class ProductValidator : AbstractValidator<ProductModel>
{
    private readonly IProductRepository _repo;
    private readonly IHttpContextAccessor _context;
    private readonly ActivitySource _activitySource;

    public ProductValidator(IProductRepository repo, IHttpContextAccessor context, ActivitySource activitySource)
    {
        _repo = repo;
        _context = context;
        _activitySource = activitySource;

        if (_context.HttpContext!.Request.Method == HttpMethods.Post)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(NameIsUnique).WithMessage("A product with the same name already exists.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
        if (_context.HttpContext.Request.Method == HttpMethods.Put)
        {
            When(p => !string.IsNullOrWhiteSpace(p.Name), () =>
            {
                RuleFor(p => p.Name)
                    .MustAsync(async (dto, productName, token) => await NameIsUniqueForUpdate(dto.Id, productName, token))
                    .WithMessage("A product with the same name already exists.")
                    .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            });
        }       

        if (_context.HttpContext.Request.Method == HttpMethods.Post)
        {
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
        }
        if (_context.HttpContext.Request.Method == HttpMethods.Put)
        {
            When(p => !string.IsNullOrWhiteSpace(p.Description), () =>
            {
                RuleFor(x => x.Description)
                  .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
            });
        }        

        if (_context.HttpContext.Request.Method == HttpMethods.Post)
        {
            RuleFor(e => e.IsActive)
                .NotNull().WithMessage("{PropertyName} is required.");
        }
    }

    private async Task<bool> NameIsUnique(string? productName, CancellationToken token)
    {
        return !(await _repo.IsProductNameUnique(productName, token));
    }

    private async Task<bool> NameIsUniqueForUpdate(int id, string? productName, CancellationToken token)
    {
        return !(await _repo.IsProductNameUnique(id, productName, token));
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<ProductModel> context, 
        CancellationToken cancellation = default)
    {
        var activity = _activitySource.StartActivity($"VALIDATION {GetType().Name}");
        activity?.AddTag("productName", context.InstanceToValidate.Name);
        return base.ValidateAsync(context, cancellation);
    }
}
