using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Contracts.Contracts;
using Contracts.Contracts.Order;
using DataAccess_Layer;

namespace Business_Layer
{
    public class clsOrder
    {
        public OrderRequestDTO orderRequestDTO { get {
                return new OrderRequestDTO(
            OrderID, TotalAmount, OrderStatus, Quantity, OrderDate, ReceiveDate, Address, Feedback, CustomerID, ProductID, DriverID);
            } }

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

    public clsOrder (OrderRequestDTO orderRequestDTO ,enMode mode = enMode.AddNew){
        this.OrderID = orderRequestDTO.OrderID;
        this.TotalAmount = orderRequestDTO.TotalAmount;
        this.OrderStatus = orderRequestDTO.OrderStatus;
        this.Quantity = orderRequestDTO.Quantity;
        this.OrderDate = orderRequestDTO.OrderDate;
        this.ReceiveDate = orderRequestDTO.ReceiveDate;
        this.Address = orderRequestDTO.Address;
        this.Feedback = orderRequestDTO.Feedback;
        this.CustomerID = orderRequestDTO.CustomerID;
        this.ProductID = orderRequestDTO.ProductID;
        this.DriverID = orderRequestDTO.DriverID;

        this.Customer = clsCustomer.Find(CustomerID);
        this.Product = clsProduct.Find(ProductID);
        this.Driver = clsDriver.Find(DriverID);


            this.Mode = mode;
}
        private async Task<bool> _AddNewOrderAsync()
        {
            this.OrderID = await clsOrderData.AddNewOrderAsync(orderRequestDTO);
            return (this.OrderID != -1);
        }

        private bool _UpdateOrder(){
        return clsOrderData.UpdateOrder(orderRequestDTO);
    }
    public static clsOrder Find(int OrderID){
        OrderRequestDTO orderRequestDTO = new OrderRequestDTO();
            orderRequestDTO = clsOrderData.GetOrderInfoByOrderID(
            OrderID);

        if (orderRequestDTO != null)
            {
            return new clsOrder(orderRequestDTO , enMode.Update);}
        else{ return null;}
    }
    public async Task<bool> SaveAsync()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (await _AddNewOrderAsync())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }
            case enMode.Update:
                // If _UpdateOrder() is synchronous, you can call it directly.
                // If it should be asynchronous, convert it as well.
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

        public static async Task<List<DeliveringOrders>> GetDeliveringOrders()
        {
            return await clsOrderData.GetDeliveringOrders();
        }


    }
}