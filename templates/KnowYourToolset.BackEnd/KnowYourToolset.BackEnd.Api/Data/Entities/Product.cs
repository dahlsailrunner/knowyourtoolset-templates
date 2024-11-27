namespace KnowYourToolset.BackEnd.Api.Data.Entities;

public record Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Department { get; set; } = null!;
    public string Categories { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Distributor { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Barcode { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string CreatedBy { get; set; } = null!;
}
