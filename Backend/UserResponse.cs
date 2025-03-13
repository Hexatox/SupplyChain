namespace Backend.Contracts;

public class UserResponse
{
    public class UserRequestDTO
    {
        public string UserName {set;get;} = String.Empty;
        public string Password {set;get;}= String.Empty;
        public string Name {set;get;}= String.Empty;
        public string Address {set;get;}= String.Empty;
        public string PhoneNumber {set;get;}= String.Empty;
        public string Email {set;get;}= String.Empty;
        public int Permissions {set;get;}
    }
}