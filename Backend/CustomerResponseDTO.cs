using System;

public class CustomerResponseDTO
{
    public int CustomerID { get; set; }
    public int UserID { get; set; }

    public CustomerResponseDTO(int customerId, int userId)
	{
        CustomerID = customerId;
        UserID = userId;
    }
}
