using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Azure;
using Backend.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsProduct
    {
        public ProductRequestDTO productRequestDTO { get { return new ProductRequestDTO(ProductID, ProdcutName,Quantity,Price,Weight,SupplierID,Cost,Description,Image); } }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ProductID {set;get;}
        public string ProdcutName {set;get;}
        public int Quantity {set;get;}
        public decimal Price {set;get;}
        public int Weight {set;get;}
        public decimal Cost { get; set; }
        public string? Description { set; get; }
        public string Image { get; set; } // Store image as binary data
        public int SupplierID {set;get;}
        public clsSupplier Supplier {set;get;}

    public clsProduct (ProductRequestDTO productRequestDTO , enMode mode = enMode.AddNew){
        this.ProductID = productRequestDTO.ProductID;
        this.ProdcutName = productRequestDTO.ProdcutName;
        this.Quantity = productRequestDTO.Quantity;
        this.Price = productRequestDTO.Price;
        this.Weight = productRequestDTO.Weight;
        this.SupplierID = productRequestDTO.SupplierID;
        this.Cost = productRequestDTO.Cost;
        this.Image = productRequestDTO.Image;
        this.Description = productRequestDTO.Description;

        this.Supplier = clsSupplier.Find(SupplierID);


        this.Mode = mode;
}
    private bool _AddNewProduct(){
        this.ProductID = clsProductData.AddNewProduct(productRequestDTO);
        return (this.ProductID != -1);
    }

    private bool _UpdateProduct(){
        return clsProductData.UpdateProduct(this.ProductID, this.ProdcutName, this.Quantity, this.Price, this.Weight, this.SupplierID);
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
    public async static Task<List<ProductResponseDTO>> GetAllProduct()
    {
        return await clsProductData.GetAllProduct();

    }

    public static clsProduct Find(int id)
    {
        ProductRequestDTO productRequestDTO = clsProductData.GetProductInfoByProductID(id);
        if (productRequestDTO != null)
        {
            clsProduct product = new clsProduct(productRequestDTO, enMode.Update);
            return product;
        }
        return null;
    }

}
}