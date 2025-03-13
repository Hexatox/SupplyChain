using System;

public class DriverResponseDTO
{
    public int DriverID { get; set; }
    public bool IsAvailable { get; set; }
    public int WeightCapacity { get; set; }
    public string Vehicle { get; set; }
    public int UserID { set; get; }
    public int SupplierID { set; get; }
    public DriverResponseDTO()
	{
        this.DriverID = DriverID;
        this.IsAvailable = IsAvailable;
        this.WeightCapacity = WeightCapacity;
        this.Vehicle = Vehicle;
        this.UserID = UserID;
        this.SupplierID = SupplierID;
    }

}
