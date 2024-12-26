namespace web_api.Models;

public class ElectricityData
{
    public int Id { get; set; }
    public string Region { get; set; }
    public double? EnergyUsed { get; set; }
    public double? EnergyProduced { get; set; }
}