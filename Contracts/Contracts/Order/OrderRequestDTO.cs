using System;

public class OrderRequestDTO
{
    public int OrderID { set; get; }
    public decimal TotalAmount { set; get; }
    public byte OrderStatus { set; get; }
    public int Quantity { set; get; }
    public DateTime OrderDate { set; get; }
    public DateTime? ReceiveDate { set; get; }
    public string Address { set; get; }
    public string? Feedback { set; get; }
    public int CustomerID { set; get; }
    public int ProductID { set; get; }
    public int DriverID { set; get; }

    public OrderRequestDTO(int OrderID, decimal TotalAmount, byte OrderStatus, int Quantity, DateTime OrderDate, DateTime? ReceiveDate, string Address, string? Feedback, int CustomerID, int ProductID, int DriverID)
    {
        this.OrderID = OrderID;
        this.TotalAmount = TotalAmount;
        this.OrderStatus = OrderStatus;
        this.Quantity = Quantity;
        this.OrderDate = OrderDate;
        this.ReceiveDate = ReceiveDate;
        this.Address = Address;
        this.Feedback = Feedback;
        this.CustomerID = CustomerID;
        this.ProductID = ProductID;
        this.DriverID = DriverID;
    }

    public OrderRequestDTO() { }
}
