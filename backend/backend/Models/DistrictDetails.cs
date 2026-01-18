using Microsoft.EntityFrameworkCore;

namespace backend.Models;

[Owned]
public class DistrictDetails
{
    public DistrictDetails() { }

    public DistrictDetails(int seats, string[] terytcodes)
    {
        Seats = seats;
        TerytCodes = terytcodes;
    }

    public int Seats { get; set; }
    public required string[] TerytCodes { get; set; }
    
}
