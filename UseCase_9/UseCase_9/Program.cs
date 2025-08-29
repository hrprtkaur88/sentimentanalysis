using Asp.Versioning;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using UseCase_9.Models;
using UseCase_9.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(typeof(UseCase_9.Controllers.SentimentCheckerController).Assembly).AddControllersAsServices();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddOptions<AzureOpenAIOptions>()
           .Bind(builder.Configuration.GetSection("AzureOpenAIOptions"))
           .ValidateDataAnnotations();

builder.Services.AddSingleton(sp =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();
    var azureOpenAIOptions = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>();

    logger.LogInformation("Initializing OpenAI Client with endpoint: {Endpoint}", azureOpenAIOptions.Value.EndPoint);
    return new AzureOpenAIClient(new Uri(azureOpenAIOptions.Value.EndPoint!), new AzureKeyCredential(azureOpenAIOptions.Value.ApiKey!));
});
builder.Services.AddSingleton<IAzureOpenAIService>(sp =>
{
    var azureOpenAIOptions = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>();
    var logger = sp.GetRequiredService<ILogger<AzureOpenAIService>>();

    return new AzureOpenAIService(azureOpenAIOptions, logger);
});

builder.Services.AddSingleton<ISentimentCheckerService>(sp =>
{
    var azureOpenAIService = sp.GetRequiredService<IAzureOpenAIService>();
    var azureOpenAIOptions = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>();
    var logger = sp.GetRequiredService<ILogger<SentimentCheckerService>>();
    return new SentimentCheckerService(azureOpenAIService, azureOpenAIOptions, logger);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sentiment Checker API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();