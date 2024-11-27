using AutoMapper;
using KnowYourToolset.BackEnd.Api.Data.Entities;

namespace KnowYourToolset.BackEnd.Api.ApiModels;

public record ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Department { get; set; } = null!;
    public List<string> Categories { get; set; } = [];
    public string Manufacturer { get; set; } = null!;
    public string Distributor { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Barcode { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = null!;
}

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductModel>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.Split(",", StringSplitOptions.TrimEntries)))
            .ReverseMap()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => string.Join(',', src.Categories)));
    }
}