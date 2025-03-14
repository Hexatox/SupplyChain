using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Backend.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsProduct
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ProductID {set;get;}
        public string ProdcutName {set;get;}
        public int Quantity {set;get;}
        public decimal Price {set;get;}
        public int Weight {set;get;}
        public int SupplierID {set;get;}
        public clsSupplier Supplier {set;get;}
    public clsProduct (){
        this.ProductID = -1;
        this.ProdcutName = "";
        this.Quantity = -1;
        this.Price = -1;
        this.Weight = -1;
        this.SupplierID = -1;


        this.Mode = enMode.AddNew;
}
    private clsProduct (int ProductID, string ProdcutName, int Quantity, decimal Price, int Weight, int SupplierID){
        this.ProductID = ProductID;
        this.ProdcutName = ProdcutName;
        this.Quantity = Quantity;
        this.Price = Price;
        this.Weight = Weight;
        this.SupplierID = SupplierID;

        this.Supplier = clsSupplier.Find(SupplierID);


        this.Mode = enMode.Update;
}
    private bool _AddNewProduct(){
        this.ProductID = clsProductData.AddNewProduct(this.ProdcutName, this.Quantity, this.Price, this.Weight, this.SupplierID);
        return (this.ProductID != -1);
    }
    private bool _UpdateProduct(){
        return clsProductData.UpdateProduct(this.ProductID, this.ProdcutName, this.Quantity, this.Price, this.Weight, this.SupplierID);
    }
    public static clsProduct Find(int ProductID){
        string ProdcutName = "";
        int Quantity = -1;
        decimal Price = -1;
        int Weight = -1;
        int SupplierID = -1;

        bool IsFound = clsProductData.GetProductInfoByProductID(
            ProductID, ref ProdcutName, ref Quantity, ref Price, ref Weight, ref SupplierID);

        if (IsFound){
            return new clsProduct(ProductID, ProdcutName, Quantity, Price, Weight, SupplierID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewProduct())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateProduct();

        }

        return false;
    }
    public bool Delete()
    {
        return clsProductData.DeleteProduct(this.ProductID); 
    }
    public static bool IsProductExist(int ProductID)
    {
        return clsProductData.IsProductExist(ProductID); 
    }
    public static List<ProductResponseDTO> GetAllProduct()
    {
        return clsProductData.GetAllProduct();

    }

    }
}