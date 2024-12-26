using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using web_api.DAL;
using web_api.Models;

namespace web_api.Services
{
    public class ElectricityService : IElectricityService
    {
        private readonly ICsvDataService dataService;
        private readonly ILogger<ElectricityService> logger;
        private readonly IElectricityDataRepository dataRepository;

        public ElectricityService(
            ICsvDataService dataService,
            ILogger<ElectricityService> logger,
            IElectricityDataRepository dataRepository
            )
        {
            this.dataService = dataService;
            this.logger = logger;
            this.dataRepository = dataRepository;
        }

        public async Task<ICollection<ElectricityData>> FetchAndFilterCsvDataAsync()
        {
            try
            {
                logger.LogInformation("Fetching electricity data from sources");

                var firstResponse = await dataService.Fetch("https://data.gov.lt/media/filer_public/b2/3d/b23d5d9d-7f07-49a5-9ad8-8ec8917cdf82/2024-10.csv");
                var secondResponse = await dataService.Fetch("https://data.gov.lt/media/filer_public/be/39/be390ff0-8972-474e-a044-9f6f6f5f589a/2024-09.csv");

                var firstData = dataService.GetElectricityData(firstResponse);
                var secondData = dataService.GetElectricityData(secondResponse);

                logger.LogInformation("Successfully fetched and processed data from both sources");

                var comboData = firstData.Concat(secondData).ToList();

                logger.LogInformation("Trying add to database");
                await dataRepository.AddRange(comboData);
                logger.LogInformation("Successfully added data in db");

                return comboData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching and filtering electricity data");
                throw;
            }
        }

        public async Task<ICollection<ElectricityData>> GetElectricityDataAsync()
        {
            try
            {
                var data = await dataRepository.ToList();
                logger.LogInformation("Successfully get data from database");

                return data;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting data from db");
                throw;
            }
        }
    }
}
