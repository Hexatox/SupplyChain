using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Contracts.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsOrder
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int OrderID {set;get;}
        public decimal TotalAmount {set;get;}
        public byte OrderStatus {set;get;}
        public int Quantity {set;get;}
        public DateTime OrderDate {set;get;}
        public DateTime? ReceiveDate {set;get;}
        public string Address {set;get;}
        public string? Feedback {set;get;}
        public int CustomerID {set;get;}
        public int ProductID {set;get;}
        public int DriverID {set;get;}
        public clsCustomer Customer {set;get;}
        public clsProduct Product {set;get;}
        public clsDriver Driver {set;get;}
    public clsOrder (){
        this.OrderID = -1;
        this.TotalAmount = -1;
        this.OrderStatus = 0;
        this.Quantity = -1;
        this.OrderDate = DateTime.Now;
        this.ReceiveDate = DateTime.Now;
        this.Address = "";
        this.Feedback = "";
        this.CustomerID = -1;
        this.ProductID = -1;
        this.DriverID = -1;


        this.Mode = enMode.AddNew;
}
    private clsOrder (int OrderID, decimal TotalAmount, byte OrderStatus, int Quantity, DateTime OrderDate, DateTime ReceiveDate, string Address, string Feedback, int CustomerID, int ProductID, int DriverID){
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

        this.Customer = clsCustomer.Find(CustomerID);
        this.Product = clsProduct.Find(ProductID);
        this.Driver = clsDriver.Find(DriverID);


        this.Mode = enMode.Update;
}
    private bool _AddNewOrder(){
        this.OrderID = clsOrderData.AddNewOrder(this.TotalAmount, this.OrderStatus, this.Quantity, this.OrderDate, this.ReceiveDate, this.Address, this.Feedback, this.CustomerID, this.ProductID, this.DriverID);
        return (this.OrderID != -1);
    }
    private bool _UpdateOrder(){
        return clsOrderData.UpdateOrder(this.OrderID, this.TotalAmount, this.OrderStatus, this.Quantity, this.OrderDate, this.ReceiveDate, this.Address, this.Feedback, this.CustomerID, this.ProductID, this.DriverID);
    }
    public static clsOrder Find(int OrderID){
        decimal TotalAmount = -1;
        byte OrderStatus = 0;
        int Quantity = -1;
        DateTime OrderDate = DateTime.Now;
        DateTime ReceiveDate = DateTime.Now;
        string Address = "";
        string Feedback = "";
        int CustomerID = -1;
        int ProductID = -1;
        int DriverID = -1;

        bool IsFound = clsOrderData.GetOrderInfoByOrderID(
            OrderID, ref TotalAmount, ref OrderStatus, ref Quantity, ref OrderDate, ref ReceiveDate, ref Address, ref Feedback, ref CustomerID, ref ProductID, ref DriverID);

        if (IsFound){
            return new clsOrder(OrderID, TotalAmount, OrderStatus, Quantity, OrderDate, ReceiveDate, Address, Feedback, CustomerID, ProductID, DriverID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewOrder())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateOrder();

        }

        return false;
    }
    public bool Delete()
    {
        return clsOrderData.DeleteOrder(this.OrderID); 
    }
    public static bool IsOrderExist(int OrderID)
    {
        return clsOrderData.IsOrderExist(OrderID); 
    }
    public static DataTable GetAllOrder()
    {
        return clsOrderData.GetAllOrder();

    }

    public static async Task<RevenueDto> GetTotalRevenuesAsync()
    {
        return await clsOrderData.GetTotalRevenuesAsync();
    }

        public static async Task<List<RecentSalesDTO>> GetRecentSalesAsync()
        {
            return await clsOrderData.GetRecentSalesAsync();
        }

        public static async Task<List<OrdersPerMonthDTO>> GetProductsForAllMonthsAsync()
        {
            return await clsOrderData.GetProductsForAllMonthsAsync();
        }

        public static async Task<List<CustomerOrdersDTO>> GetCustomerOrders(int CustomerID)
        {
            return await clsOrderData.GetCustomerOrders(CustomerID);
        }

    }
}