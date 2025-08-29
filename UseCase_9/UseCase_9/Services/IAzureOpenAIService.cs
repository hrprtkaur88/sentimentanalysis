namespace UseCase_9.Services;
public interface IAzureOpenAIService
{
    Task<string> AnalyzeSentimentAsync(string surveyContent);
    int MaxTokens { get; }
    int RetryCount { get; }
    int RetryDelayInSeconds { get; }
}