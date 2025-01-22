using BlogAPI.Helpers;

namespace BlogAPI.Service.Import;

using CsvHelper;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CsvImportService
{
    private readonly DataContext _context;
    private readonly ILogger<CsvImportService> _logger;
    private readonly HttpClient _httpClient;

    public CsvImportService(DataContext context, ILogger<CsvImportService> logger, HttpClient httpClient)
    {
        _context = context;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task ImportPostsFromCsvAsync(string url)
    {
        try
        {
            // Download CSV from the URL
            var response = await _httpClient.GetStreamAsync(url);
            
            using (var reader = new StreamReader(response))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<BlogPostCsv>();

                // Map the CSV records to your Post entity
                var posts = new List<Domain.Entity.Blog>();

                foreach (var record in records)
                {
                    var post = new Domain.Entity.Blog
                    {
                        Title = record.title,
                        Content = record.content,
                        FriendlyUrl = record.friendlyUrl,
                        DateCreated = DateTime.UtcNow,
                    };

                    posts.Add(post);
                }

                // Save the posts to the database
                await _context.Blogs.AddRangeAsync(posts);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error importing posts from CSV: {ex.Message}");
        }
    }
}
