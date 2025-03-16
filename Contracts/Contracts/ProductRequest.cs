using Microsoft.AspNetCore.Http;

namespace Backend.Contracts;

public class ProductRequestDTO
{
    public int ProductID { get; set; }
    public string ProdcutName { set; get; } = String.Empty; 
    public int Quantity {set;get;}
    public decimal Price {set;get;}
    public int Weight {set;get;}
    public decimal Cost { get; set; }
    public string? Description {set;get;}
    public string? Image { get; set; } // Base64 string of the image
    public int SupplierID {set;get;}
    public IFormFile? file { get;set; }

    public ProductRequestDTO(int ProductID , string ProdcutName, int Quantity, decimal Price, int Weight, int SupplierID, decimal Cost, string? Description, string? Image)
    {
        this.ProductID = ProductID;
        this.ProdcutName = ProdcutName;
        this.Quantity = Quantity;
        this.Price = Price;
        this.Weight = Weight;
        this.SupplierID = SupplierID;
        this.Cost = Cost;
        this.Image = Image;
        this.Description = Description;
    }

    public ProductRequestDTO() { }

}