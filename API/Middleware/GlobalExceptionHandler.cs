using API.Exceptions; 
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware; 
//IExceptionHandler is the interface for typed exception handling 
//AddEceptionHandler<T>() registers it; UseExceptionHandler() activates it. 
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        //1. Record the error
        logger.LogError(exception, "An expection occured: {Message}",exception.Message);

       //2. Translate the domain exception to a status code
       var statusCode = exception switch
       {
           JobNotFoundException => StatusCodes.Status404NotFound,
           DuplicateJobListingException => StatusCodes.Status409Conflict,
           _ => StatusCodes.Status500InternalServerError       
       }; 

       //3. Construct Problem Details response body
       var problemDetails = new ProblemDetails
       {
           Status = statusCode,
           Title =  exception.GetType().Name, //Get the exception type name as title
           Detail = exception.Message
       };
     
      //4 Write the status code and JSON body
      httpContext.Response.StatusCode = statusCode;
      httpContext.Response.ContentType = "application/problem+json";
      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken); 

      return true; 

    }
    //Helper method to get Problem details title
    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status404NotFound => "Resource Not Found",
        StatusCodes.Status409Conflict => "Resource Conflict",
        StatusCodes.Status204NoContent => "No Content",
        StatusCodes.Status200OK => "OK",
        _                             => "Internal server error"
    };

}