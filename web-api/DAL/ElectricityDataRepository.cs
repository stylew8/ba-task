using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using web_api.Models;

namespace web_api.DAL;

public class ElectricityDataRepository : IElectricityDataRepository
{
    private readonly AppDbContext dbContext;
    private readonly ILogger<ElectricityDataRepository> logger;

    public ElectricityDataRepository(AppDbContext appDbContext, ILogger<ElectricityDataRepository> logger)
    {
        this.dbContext = appDbContext;
        this.logger = logger;
    }

    public async Task AddRange(ICollection<ElectricityData> data)
    {
        try
        {
            await dbContext.ElectricityData.AddRangeAsync(data);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Successfully added a range of electricity data");
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "A database update error occurred while adding a range of electricity data");
            throw new Exception("An error occurred while adding data to the database. See inner exception for details", dbEx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while adding a range of electricity data");
            throw;
        }
    }

    public async Task<ICollection<ElectricityData>> ToList()
    {
        try
        {
            var list = await dbContext.ElectricityData.ToListAsync();
            logger.LogInformation("Successfully added a range of electricity data");

            return list;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred getting a list of electricity data");
            throw;
        }
    }
}