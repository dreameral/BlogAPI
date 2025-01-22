using BlogAPI.Service.Import;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Api.Import;

[Route("ImportBlogs")]
public class ImportBlogsController : ApiController
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ImportBlogsController(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    [HttpPost]
    public IActionResult ImportCsv()
    {
        // Trigger the background job to start importing posts
        _backgroundJobClient.Enqueue<CsvImportService>(x => x.ImportPostsFromCsvAsync("https://fleetcor-cvp.s3.eu-central-1.amazonaws.com/blog-posts.csv"));

        return Ok("CSV import started.");
    }
}