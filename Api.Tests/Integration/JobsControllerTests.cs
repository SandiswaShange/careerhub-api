using System.Net;
using System.Net.Http.Json;
using API.DTOs;
using Xunit;

namespace API.Tests.Integration;

public class JobsControllerTests: IClassFixture<WebApplicationFactoryFixture>
{
/*These are integration tests.
Unlike the service unit tests, they spin up the actual API using WebApplicationFactory and send real HTTP requests.
This lets me verify routing, authentication, versioning, pagination headers, and ETag behaviour exactly as a frontend
application would experience them*/
    private readonly HttpClient _client;

    public JobsControllerTests(WebApplicationFactoryFixture factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetJobs_ReturnsOk()
    {
        var response =await _client.GetAsync("/api/v1/jobs");

        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }

    [Fact]
    public async Task GetJobs_ResponseIsPagedEnvelope()
    {
        var response = await _client.GetAsync("/api/v1/jobs?page=1&pageSize=5");

        response.EnsureSuccessStatusCode();

        var result =await response.Content.ReadFromJsonAsync<PagedResponse<JobListResponse>>();

        Assert.NotNull(result);
        Assert.Equal(1, result!.Page);
        Assert.Equal(5, result.PageSize);
        Assert.True(result.TotalCount >= 0);
    }

    [Fact]
    public async Task GetJobs_ResponseIncludesXTotalCountHeader()
    {
        var response =await _client.GetAsync("/api/v1/jobs");

        Assert.True(response.Headers.Contains("X-Total-Count"));
    }

    [Fact]
    public async Task GetJobs_WithoutVersion_ReturnsSameStatusAsV1()
    {
        var response1 = await _client.GetAsync("/api/jobs");

        var response2 = await _client.GetAsync("/api/v1/jobs");

        Assert.Equal(
            response1.StatusCode,
            response2.StatusCode);
    }

    [Fact]
    public async Task GetJobs_ResponseIncludesApiSupportedVersionsHeader()
    {
        var response = await _client.GetAsync("/api/v1/jobs");

        Assert.True(
            response.Headers.Contains(
                "api-supported-versions"));

        var version = response.Headers.GetValues("api-supported-versions").First();

        Assert.Contains("1.0", version);
    }

    [Fact]
    public async Task PostJob_WithoutToken_Returns401()
    {
        var request = new
        {
            Title = "Developer",
            Description = "Test",
            Company = "Bitcube",
            Location = "Cape Town"
        };

        var response = await _client.PostAsJsonAsync(
                "/api/v1/jobs",
                request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_WithoutToken_Returns401()
    {
        var request = new
        {
            ApplicantId = Guid.NewGuid(),
            JobListingId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/applications",request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetJobById_WithValidId_DoesNotReturn500()
    {
        var response = await _client.GetAsync($"/api/v1/jobs/{Guid.NewGuid()}");

        Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);

        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetJobById_ResponseIncludesETagHeader()
    {
        // Get first page of jobs
        var jobsResponse = await _client.GetFromJsonAsync<PagedResponse<JobListResponse>>("/api/v1/jobs");

        Assert.NotNull(jobsResponse);
        Assert.NotEmpty(jobsResponse!.Data);

        var jobId = jobsResponse.Data.First().Id;

        // Get specific job
        var response = await _client.GetAsync($"/api/v1/jobs/{jobId}");

        Assert.True(response.Headers.Contains("ETag"));

        var etag = response.Headers.GetValues("ETag").First();

        Assert.False(string.IsNullOrWhiteSpace(etag));
    }

    [Fact]
    public async Task GetJobById_WithMatchingETag_Returns304()
    {
        // Get a job ID
        var jobsResponse = await _client.GetFromJsonAsync<PagedResponse<JobListResponse>>("/api/v1/jobs");

        Assert.NotNull(jobsResponse);
        Assert.NotEmpty(jobsResponse!.Data);

        var jobId = jobsResponse.Data.First().Id;

        // First request
        var firstResponse = await _client.GetAsync($"/api/v1/jobs/{jobId}");

        var etag = firstResponse.Headers.GetValues("ETag").First();

        // Second request using If-None-Match
        var request = new HttpRequestMessage(HttpMethod.Get,$"/api/v1/jobs/{jobId}");

        request.Headers.TryAddWithoutValidation("If-None-Match",etag);

        var secondResponse = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotModified, secondResponse.StatusCode);
    }
}   