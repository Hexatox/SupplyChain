using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Backend.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsUser
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID {set;get;}
        public string UserName {set;get;}
        public string Password {set;get;}
        public string Name {set;get;}
        public string Address {set;get;}
        public string PhoneNumber {set;get;}
        public string Email {set;get;}
        public int Permissions {set;get;}
    public clsUser (){
        this.UserID = -1;
        this.UserName = "";
        this.Password = "";
        this.Name = "";
        this.Address = "";
        this.PhoneNumber = "";
        this.Email = "";
        this.Permissions = -1;


        this.Mode = enMode.AddNew;
}
    private clsUser (int UserID, string UserName, string Password, string Name, string Address, string PhoneNumber, string Email, int Permissions){
        this.UserID = UserID;
        this.UserName = UserName;
        this.Password = Password;
        this.Name = Name;
        this.Address = Address;
        this.PhoneNumber = PhoneNumber;
        this.Email = Email;
        this.Permissions = Permissions;



        this.Mode = enMode.Update;
}
    private bool _AddNewUser(){
        this.UserID = clsUserData.AddNewUser(this.UserName, this.Password, this.Name, this.Address, this.PhoneNumber, this.Email, this.Permissions);
        return (this.UserID != -1);
    }
    private bool _UpdateUser(){
        return clsUserData.UpdateUser(this.UserID, this.UserName, this.Password, this.Name, this.Address, this.PhoneNumber, this.Email, this.Permissions);
    }
    public static clsUser Find(int UserID){
        string UserName = "";
        string Password = "";
        string Name = "";
        string Address = "";
        string PhoneNumber = "";
        string Email = "";
        int Permissions = -1;

        bool IsFound = clsUserData.GetUserInfoByUserID(
            UserID, ref UserName, ref Password, ref Name, ref Address, ref PhoneNumber, ref Email, ref Permissions);

        if (IsFound){
            return new clsUser(UserID, UserName, Password, Name, Address, PhoneNumber, Email, Permissions);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewUser())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateUser();

        }

        return false;
    }
    public bool Delete()
    {
        return clsUserData.DeleteUser(this.UserID); 
    }
    public static bool IsUserExist(int UserID)
    {
        return clsUserData.IsUserExist(UserID); 
    }
    public static List<UserResponseDTO> GetAllUser()
    {
        return clsUserData.GetAllUser();

    }

    }
}