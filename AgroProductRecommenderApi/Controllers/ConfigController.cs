using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace AgroProductRecommenderApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ConfigController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("GetConfig")]
    public IActionResult GetConfig()
    {
        var runtimeConfigs = _configuration.AsEnumerable()
            .Where(config => config.Key.StartsWith("Runtime"))
            .ToDictionary(config => config.Key, config => config.Value);

        return Ok(runtimeConfigs);
    }
}