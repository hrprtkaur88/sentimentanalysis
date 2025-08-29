namespace UseCase_9.Services
{
    public interface ISentimentCheckerService
    {
        Task<string> EvaluateSearchResultAsync(string surveyComments);
    }
}
