using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Backend.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsSupplier
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int SupplierID {set;get;}
        public int UserID {set;get;}
        public clsUser User {set;get;}
    public clsSupplier (){
        this.SupplierID = -1;
        this.UserID = -1;


        this.Mode = enMode.AddNew;
}
    private clsSupplier (int SupplierID, int UserID){
        this.SupplierID = SupplierID;
        this.UserID = UserID;

        this.User = clsUser.Find(UserID);


        this.Mode = enMode.Update;
}
    private bool _AddNewSupplier(){
        this.SupplierID = clsSupplierData.AddNewSupplier(this.UserID);
        return (this.SupplierID != -1);
    }
    private bool _UpdateSupplier(){
        return clsSupplierData.UpdateSupplier(this.SupplierID, this.UserID);
    }
    public static clsSupplier Find(int SupplierID){
        int UserID = -1;

        bool IsFound = clsSupplierData.GetSupplierInfoBySupplierID(
            SupplierID, ref UserID);

        if (IsFound){
            return new clsSupplier(SupplierID, UserID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewSupplier())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateSupplier();

        }

        return false;
    }
    public bool Delete()
    {
        return clsSupplierData.DeleteSupplier(this.SupplierID); 
    }
    public static bool IsSupplierExist(int SupplierID)
    {
        return clsSupplierData.IsSupplierExist(SupplierID); 
    }
    public static List<SupplierResponseDTO> GetAllSupplier()
    {
        return clsSupplierData.GetAllSupplier();

    }

    }
}