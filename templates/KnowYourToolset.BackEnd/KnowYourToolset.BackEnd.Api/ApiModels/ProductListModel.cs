using AutoMapper;
using KnowYourToolset.BackEnd.Api.Data.Entities;

namespace KnowYourToolset.BackEnd.Api.ApiModels;

public record ProductListModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Department { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Distributor { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class ProductListMappings: Profile
{
    public ProductListMappings()
    {
        CreateMap<Product, ProductListModel>();
        CreateMap<(List<Product>, int), (List<ProductListModel>, int)>()
            .ForMember(
                dest => dest.Item1,
                opt => opt.MapFrom(src => src.Item1))
            .ForMember(
                dest => dest.Item2,
                opt => opt.MapFrom(src => src.Item2));
    }
}
