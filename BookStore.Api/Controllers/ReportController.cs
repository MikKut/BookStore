using BookStore.Application.Interfaces;
using BookStore.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilterAttribute<BookController>))]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the publication data of books for generating reports.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>A dictionary where the key is the year of publication and the value is the number of books published in that year.</returns>
        /// <response code="200">Returns the publication data of books.</response>
        /// <response code="400">If the request is invalid or cannot be processed, return an error message.</response>
        [AllowAnonymous]
        [HttpGet("GetBooksPublicationData")]
        [ProducesResponseType(typeof(IDictionary<int, int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IDictionary<int, int>>> GetBooksPublicationData(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _reportService.GetBooksPublicationDataAsync(cancellationToken);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
