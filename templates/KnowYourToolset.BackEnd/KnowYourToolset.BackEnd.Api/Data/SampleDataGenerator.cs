using Bogus;
using KnowYourToolset.BackEnd.Api.Data.Entities;

namespace KnowYourToolset.BackEnd.Api.Data;

public static class SampleDataGenerator
{
    public static void CreateSampleData(this AppDbContext context, int numberOfProducts)
    {
        if (context.Products.Any())
        {
            context.Products.RemoveRange(context.Products);
            context.SaveChanges();
        }
        var faker = new Faker<Product>()
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(7))
            .RuleFor(c => c.Price, f => Decimal.Parse(f.Commerce.Price()))
            .RuleFor(c => c.Categories, f => string.Join(',', f.Commerce.Categories(f.Random.Number(1, 5))))
            .RuleFor(c => c.Manufacturer, f => f.Company.CompanyName())
            .RuleFor(c => c.Distributor, f => f.Company.CompanyName())
            .RuleFor(c => c.Department, f => f.Commerce.Department())
            .RuleFor(c => c.Barcode, f =>  f.Commerce.Ean13())
            .RuleFor(c => c.IsActive, f => f.Random.Bool())
            .RuleFor(c => c.CreatedDate, f => f.Date.Past(3).ToUniversalTime())
            .RuleFor(c => c.UpdatedDate, (f, c) => c.IsActive == true
                    ? null
                    : f.Date.Between(c.CreatedDate, DateTime.Now.ToUniversalTime()))
            .RuleFor(c => c.CreatedBy, f => f.Person.UserName);

        context.Products.AddRange(faker.Generate(numberOfProducts));
        context.SaveChanges();
    }
}
