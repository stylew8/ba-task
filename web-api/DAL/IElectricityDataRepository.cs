using web_api.Models;

namespace web_api.DAL;

public interface IElectricityDataRepository
{
    Task AddRange(ICollection<ElectricityData> data);
    Task<ICollection<ElectricityData>> ToList();
}