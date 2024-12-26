using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using web_api.Models;
using Microsoft.Extensions.Logging;

namespace web_api.Services;

public class CsvDataService : ICsvDataService
{
    private readonly ILogger<CsvDataService> logger;

    public CsvDataService(ILogger<CsvDataService> logger)
    {
        this.logger = logger;
    }

    public async Task<string> Fetch(string url)
    {
        try
        {
            logger.LogInformation("Fetching data from URL: {Url}", url);
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            logger.LogInformation("Successfully fetched data from URL: {Url}", url);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching data from URL: {Url}", url);
            throw;
        }
    }

    public ICollection<ElectricityData> GetElectricityData(string response)
    {
        try
        {
            logger.LogInformation("Processing electricity data");

            using (var reader = new StringReader(response))
            using (var csv = new CsvReader(reader,
                       new CsvConfiguration(CultureInfo.InvariantCulture)
                       {
                           Delimiter = ";",
                           HasHeaderRecord = true
                       })
                  )
            {
                var records = csv.GetRecords<CsvRecord>().Where(r => r.OBJ_PAVADINIMAS == "BUTAS").ToList();

                var aggregatedData = records
                    .GroupBy(r => r.TINKLAS)
                    .Select(g => new ElectricityData
                    {
                        Region = g.Key,
                        EnergyUsed = g.Sum(r => r.PPlus),
                        EnergyProduced = g.Sum(r => r.PMinus)
                    })
                    .ToList();

                logger.LogInformation("Successfully processed electricity data");

                return aggregatedData;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing electricity data");
            throw;
        }
    }
}