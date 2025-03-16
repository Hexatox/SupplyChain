namespace Backend.Contracts;

public class ProductResponseDTO
{
    public int ProductID { get; set; }
    public string ProdcutName { set; get; } = String.Empty; 
    public int Quantity {set;get;}
    public decimal Price {set;get;}
    public string? Image { get;set;}
}