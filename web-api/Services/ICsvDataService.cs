using web_api.Models;

namespace web_api.Services;

public interface ICsvDataService
{
    Task<string> Fetch(string url);

    ICollection<ElectricityData> GetElectricityData(string response);
}