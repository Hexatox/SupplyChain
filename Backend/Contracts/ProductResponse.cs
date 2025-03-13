namespace Backend.Contracts;

public class ProductResponseDTO
{
    public string ProdcutName { set; get; } = String.Empty; 
    public int Quantity {set;get;}
    public decimal Price {set;get;}
    public int Weight {set;get;}
    public int SupplierID {set;get;}
}