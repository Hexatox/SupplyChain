using System;
using System.Security.Cryptography.X509Certificates;

public class CustomerRequestDTO
{
	public CustomerRequestDTO(int customerId , int userId)
	{
		CustomerID = customerId;
		UserID = userId;
	}
	public int CustomerID { get; set; }
	public int UserID { get; set; }

}
