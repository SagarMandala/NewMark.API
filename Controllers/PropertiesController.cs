using Microsoft.AspNetCore.Mvc;
using NewMark.API.Service;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IBlobService _blobService;

    public PropertiesController(IBlobService blobService)
    {
        _blobService = blobService;
    }


    /// <summary>
    /// HTTP GET endpoint to retrieve data from Azure Blob Storage.
    /// Invokes the blob service to fetch and deserialize JSON data.
    /// Returns the data as a strongly-typed C# object.
    /// </summary>
    /// <returns>HTTP 200 with JSON object on success, or HTTP 500 on failure.</returns>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetBlobData()
    {
        try
        {
            var data = await _blobService.GetBlobDataAsync();
            return Ok(data);
        }
        catch (Exception ex)
        {
            // Already logged in service
            return StatusCode(500, "An error occurred while retrieving blob data.");
        }
    }
}
