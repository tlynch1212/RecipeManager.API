using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Import;
using RecipeManager.Core.Models;
using System;
using System.Collections.Generic;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [DisableRequestSizeLimit]
    public class ImportDataController : ControllerBase
    {
        private readonly ILogger<ImportDataController> _logger;
        private readonly IImportService _importService;

        public ImportDataController(ILogger<ImportDataController> logger, IImportService importService)
        {
            _logger = logger;
            _importService = importService;
        }

        [HttpPost]
        public IActionResult Import([FromBody] List<ImportModel> data)
        {
            BackgroundJob.Enqueue(() => _importService.StartImport(data));
            return Ok("Job Started Successfully.");
        }

        [HttpPost("restart")]
        public IActionResult Restart([FromQuery] int jobId, [FromBody] List<ImportModel> data)
        {
            var job = _importService.CheckStatus(jobId);
            if (job != null)
            {
                BackgroundJob.Enqueue(() => _importService.RestartImport(job, data));
                return Ok($"Job {jobId} Restarted Successfully.");
            }else
            {
                return Ok($"Job {jobId} does not exist.");
            }

        }

        [HttpGet("status")]
        public IActionResult Status([FromQuery] int jobId)
        {
            try
            {
                var job = _importService.CheckStatus(jobId);

                if (job != null)
                {
                    return Ok(job);
                }
                else
                {
                    return Ok($"Job {jobId} does not exist.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}