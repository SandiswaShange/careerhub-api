using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace API.Infrastructure.OpenApi;

public class JobListingDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title       = "Career Hub API";
        document.Info.Version     = "v1";
        document.Info.Description =
            "A hub to help people with jobs. " +
            "Requires JWT authentication to create, edit or delete jobs listings. " +
            "No authentication needed to view job listings. ";

        document.Info.Contact = new OpenApiContact
        {
            Name  = "Sandiswa Shange",
            Email = "sandiswa@gmail.com"
        };

        return Task.CompletedTask;
    }
}
