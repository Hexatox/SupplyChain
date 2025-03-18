using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Contracts.Contracts;
using Contracts.Contracts.Order;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsDriver
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DriverID {set;get;}
        public bool IsAvailable {set;get;}
        public int WeightCapacity {set;get;}
        public string Vehicle {set;get;}
        public int UserID {set;get;}
        public int SupplierID {set;get;}
        public clsUser User {set;get;}
        public clsSupplier Supplier {set;get;}
    public clsDriver (){
        this.DriverID = -1;
        this.IsAvailable = false;
        this.WeightCapacity = -1;
        this.Vehicle = "";
        this.UserID = -1;
        this.SupplierID = -1;


        this.Mode = enMode.AddNew;
}
    private clsDriver (int DriverID, bool IsAvailable, int WeightCapacity, string Vehicle, int UserID, int SupplierID){
        this.DriverID = DriverID;
        this.IsAvailable = IsAvailable;
        this.WeightCapacity = WeightCapacity;
        this.Vehicle = Vehicle;
        this.UserID = UserID;
        this.SupplierID = SupplierID;

        this.User = clsUser.Find(UserID);
        this.Supplier = clsSupplier.Find(SupplierID);


        this.Mode = enMode.Update;
}
    private bool _AddNewDriver(){
        this.DriverID = clsDriverData.AddNewDriver(this.IsAvailable, this.WeightCapacity, this.Vehicle, this.UserID, this.SupplierID);
        return (this.DriverID != -1);
    }
    private bool _UpdateDriver(){
        return clsDriverData.UpdateDriver(this.DriverID, this.IsAvailable, this.WeightCapacity, this.Vehicle, this.UserID, this.SupplierID);
    }
    public static clsDriver Find(int DriverID){
        bool IsAvailable = false;
        int WeightCapacity = -1;
        string Vehicle = "";
        int UserID = -1;
        int SupplierID = -1;

        bool IsFound = clsDriverData.GetDriverInfoByDriverID(
            DriverID, ref IsAvailable, ref WeightCapacity, ref Vehicle, ref UserID, ref SupplierID);

        if (IsFound){
            return new clsDriver(DriverID, IsAvailable, WeightCapacity, Vehicle, UserID, SupplierID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewDriver())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateDriver();

        }

        return false;
    }
    public bool Delete()
    {
        return clsDriverData.DeleteDriver(this.DriverID); 
    }
    public static bool IsDriverExist(int DriverID)
    {
        return clsDriverData.IsDriverExist(DriverID); 
    }
    public static DataTable GetAllDriver()
    {
        return clsDriverData.GetAllDriver();

    }

        public static async Task<List<DriverOrdersDTO>> GetDriverOrders(int DriverID)
        {
            return await clsDriverData.GetDriverOrders(DriverID);
        }

    }
}