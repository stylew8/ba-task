using web_api.Models;

namespace web_api.Services;

public interface IElectricityService
{
    Task<ICollection<ElectricityData>> FetchAndFilterCsvDataAsync();
    Task<ICollection<ElectricityData>> GetElectricityDataAsync();
    
}