using System;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using web_api.Models;
using web_api.Services;

public class ElectricityServiceTests
{
    private readonly Mock<ICsvDataService> mockDataService;
    private readonly Mock<ILogger<ElectricityService>> mockLogger;
    private readonly ElectricityService electricityService;

    public ElectricityServiceTests()
    {
        mockDataService = new Mock<ICsvDataService>();
        mockLogger = new Mock<ILogger<ElectricityService>>();
        electricityService = new ElectricityService(mockDataService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task FetchAndFilterCsvDataAsync_ShouldReturnCombinedData_WhenBothResponsesAreValid()
    {
        var firstResponse = "valid_csv_data_1";
        var secondResponse = "valid_csv_data_2";
        var firstData = new List<ElectricityData>
        {
            new ElectricityData { Region = "Region1", EnergyUsed = 100, EnergyProduced = 200 }
        };
        var secondData = new List<ElectricityData>
        {
            new ElectricityData { Region = "Region2", EnergyUsed = 150, EnergyProduced = 250 }
        };

        mockDataService.SetupSequence(ds => ds.Fetch(It.IsAny<string>()))
            .ReturnsAsync(firstResponse)
            .ReturnsAsync(secondResponse);
        mockDataService.Setup(ds => ds.GetElectricityData(firstResponse)).Returns(firstData);
        mockDataService.Setup(ds => ds.GetElectricityData(secondResponse)).Returns(secondData);

        var result = await electricityService.FetchAndFilterCsvDataAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Region == "Region1" && r.EnergyUsed == 100 && r.EnergyProduced == 200);
        Assert.Contains(result, r => r.Region == "Region2" && r.EnergyUsed == 150 && r.EnergyProduced == 250);
    }


    [Fact]
    public async Task FetchAndFilterCsvDataAsync_ShouldThrowException_WhenFetchFails()
    {
        mockDataService.Setup(ds => ds.Fetch(It.IsAny<string>())).ThrowsAsync(new HttpRequestException("Network error"));
        
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => electricityService.FetchAndFilterCsvDataAsync());

        Assert.Contains("Network error", exception.Message);
    }


    [Fact]
    public async Task FetchAndFilterCsvDataAsync_ShouldHandleEmptyResponses()
    {
        var emptyResponse = string.Empty;
        mockDataService.Setup(ds => ds.Fetch(It.IsAny<string>())).ReturnsAsync(emptyResponse);
        mockDataService.Setup(ds => ds.GetElectricityData(emptyResponse)).Returns(new List<ElectricityData>());

        var result = await electricityService.FetchAndFilterCsvDataAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
