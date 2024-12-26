using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_api.DAL;
using web_api.Services;

namespace web_api.Controllers;

[ApiController]
public class ElectricityDataController : ControllerBase
{
    private readonly IElectricityService elecService;
    private readonly ILogger<ElectricityDataController> logger;

    public ElectricityDataController(IElectricityService service, ILogger<ElectricityDataController> logger)
    {
        this.elecService = service;
        this.logger = logger;
    }


    // Main task
    [HttpGet]
    [Route("/data")]
    public async Task<IActionResult> GetData()
    {
        try
        {
            logger.LogInformation("Request received for electricity data");
            var data = await elecService.FetchAndFilterCsvDataAsync();
            logger.LogInformation("Successfully retrieved electricity data");

            return Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving electricity data");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    // Get all data from db
    [HttpGet]
    [Route("/database")]
    public async Task<IActionResult> GetDataFromDb()
    {
        try
        {
            logger.LogInformation("Request from database");
            var data = await elecService.GetElectricityDataAsync();
            logger.LogInformation("Successfully");

            return Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred getting electricity data from database");
            return StatusCode(500, "An error occurred getting electricity data from database");
        }
    }
}