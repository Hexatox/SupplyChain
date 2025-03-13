using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsCustomer
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int CustomerID {set;get;}
        public int UserID {set;get;}
        public clsUser User {set;get;}
    public clsCustomer (){
        this.CustomerID = -1;
        this.UserID = -1;


        this.Mode = enMode.AddNew;
}
    private clsCustomer (int CustomerID, int UserID){
        this.CustomerID = CustomerID;
        this.UserID = UserID;

        this.User = clsUser.Find(UserID);


        this.Mode = enMode.Update;
}
    private bool _AddNewCustomer(){
        this.CustomerID = clsCustomerData.AddNewCustomer(this.UserID);
        return (this.CustomerID != -1);
    }
    private bool _UpdateCustomer(){
        return clsCustomerData.UpdateCustomer(this.CustomerID, this.UserID);
    }
    public static clsCustomer Find(int CustomerID){
        int UserID = -1;

        bool IsFound = clsCustomerData.GetCustomerInfoByCustomerID(
            CustomerID, ref UserID);

        if (IsFound){
            return new clsCustomer(CustomerID, UserID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewCustomer())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateCustomer();

        }

        return false;
    }
    public bool Delete()
    {
        return clsCustomerData.DeleteCustomer(this.CustomerID); 
    }
    public static bool IsCustomerExist(int CustomerID)
    {
        return clsCustomerData.IsCustomerExist(CustomerID); 
    }
    public static DataTable GetAllCustomer()
    {
        return clsCustomerData.GetAllCustomer();

    }

    }
}