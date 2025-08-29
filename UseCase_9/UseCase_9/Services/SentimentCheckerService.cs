using Microsoft.Extensions.Options;
using UseCase_9.Models;

namespace UseCase_9.Services
{
    public class SentimentCheckerService : ISentimentCheckerService
    {
        private readonly IAzureOpenAIService _azureOpenAIService;
        public SentimentCheckerService(
            IAzureOpenAIService azureOpenAIService,
            IOptions<AzureOpenAIOptions> azureOpenAIOptions,
            ILogger<SentimentCheckerService> logger)
        {
            _azureOpenAIService = azureOpenAIService;
            var options = azureOpenAIOptions.Value;
        }

        public async Task<string> EvaluateSearchResultAsync(string surveyComments)
        {
            var response = await _azureOpenAIService.AnalyzeSentimentAsync(surveyComments);
            return response;
        }
    }
}