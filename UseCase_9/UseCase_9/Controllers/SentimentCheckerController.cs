using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UseCase_9.Models;
using UseCase_9.Services;
using System.Text;
using System.Text.Json;

namespace UseCase_9.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class SentimentCheckerController : ControllerBase
{
    private readonly ILogger<SentimentCheckerController> _logger;
    private readonly ISentimentCheckerService _sentimentCheckerService;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public SentimentCheckerController(
        ILogger<SentimentCheckerController> logger,
        ISentimentCheckerService sentimentCheckerService)
    {
        _logger = logger;
        _sentimentCheckerService = sentimentCheckerService;
    }

    [MapToApiVersion("1.0")]
    [HttpPost("sentiment-checker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SentimentChecker(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File cannot be null or empty");
        }

        ProjectSurveyModelRequest projectSurveyRequest = null;
        ProjectSurveyModelResponse projectSurveyResponse = new ProjectSurveyModelResponse();

        using var stream = file.OpenReadStream();
        try
        {
            projectSurveyRequest = await JsonSerializer.DeserializeAsync<ProjectSurveyModelRequest>(stream, _jsonSerializerOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Failed to deserialize JSON file: {ex}");
            return new BadRequestObjectResult("Invalid JSON file");
        }

        if (projectSurveyRequest == null)
        {
            _logger.LogError("Request body and file content are both null or empty");
            return new BadRequestObjectResult("Request body and file content cannot be both null or empty");
        }

        projectSurveyResponse.PROJECT_ID = projectSurveyRequest.PROJECT_ID;
        projectSurveyResponse.TeamSurveys = projectSurveyRequest.TeamSurveys
            .Select(ts => new TeamSurveyModelResponse
            {
                Survey_ID = ts.Survey_ID,
                Survey_Period = ts.Survey_Period,
                Survey_Status = ts.Survey_Status,
                Memberfirm_ID = ts.Memberfirm_ID,
                Responses = ts.Responses.Select(r => new SurveyResponseModelResponse
                {
                    Survey_Response_ID = r.Survey_Response_ID,
                    Delivery_To_Plan_Question = r.Delivery_To_Plan_Question,
                    Delivery_To_Plan_Answer = r.Delivery_To_Plan_Answer,
                    Client_Value_Question = r.Client_Value_Question,
                    Client_Value_Answer = r.Client_Value_Answer,
                    Quality_Question = r.Quality_Question,
                    Quality_Answer = r.Quality_Answer,
                    Escalation_Question = r.Escalation_Question,
                    Escalation_Answer = r.Escalation_Answer,
                    Wellbeing_Question = r.Wellbeing_Question,
                    Wellbeing_Answer = r.Wellbeing_Answer,
                    Delivery_To_Plan_Comment = r.Delivery_To_Plan_Comment,
                    Client_Value_Comment = r.Client_Value_Comment,
                    Quality_Comment = r.Quality_Comment,
                    Escalation_Comment = r.Escalation_Comment,
                    Wellbeing_Comment = r.Wellbeing_Comment,
                    Level = r.Level,
                    Location = r.Location,
                    Overall_Comments = r.Overall_Comments,
                    ResponseFeedback = null
                }).ToList()
            }).ToList();

        foreach (var teamSurvey in projectSurveyRequest.TeamSurveys)
        {
            foreach (var response in teamSurvey.Responses)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Delivery_To_Plan_Question: {response.Delivery_To_Plan_Question}");
                sb.AppendLine($"Delivery_To_Plan_Answer: {response.Delivery_To_Plan_Answer}");
                sb.AppendLine($"Client_Value_Question: {response.Client_Value_Question}");
                sb.AppendLine($"Client_Value_Answer: {response.Client_Value_Answer}");
                sb.AppendLine($"Quality_Question: {response.Quality_Question}");
                sb.AppendLine($"Quality_Answer: {response.Quality_Answer}");
                sb.AppendLine($"Escalation_Question: {response.Escalation_Question}");
                sb.AppendLine($"Escalation_Answer: {response.Escalation_Answer}");
                sb.AppendLine($"Wellbeing_Question: {response.Wellbeing_Question}");
                sb.AppendLine($"Wellbeing_Answer: {response.Wellbeing_Answer}");
                sb.AppendLine($"Delivery_To_Plan_Comment: {response.Delivery_To_Plan_Comment}");
                sb.AppendLine($"Client_Value_Comment: {response.Client_Value_Comment}");
                sb.AppendLine($"Quality_Comment: {response.Quality_Comment}");
                sb.AppendLine($"Escalation_Comment: {response.Escalation_Comment}");
                sb.AppendLine($"Wellbeing_Comment: {response.Wellbeing_Comment}");
                sb.AppendLine($"Level: {response.Level}");
                sb.AppendLine($"Location: {response.Location}");
                sb.AppendLine($"Overall_Comments: {response.Overall_Comments}");

                var sentimentResponse = await _sentimentCheckerService.EvaluateSearchResultAsync(sb.ToString());
                projectSurveyResponse.TeamSurveys
                    .First(ts => ts.Survey_ID == teamSurvey.Survey_ID)
                    .Responses
                    .First(r => r.Survey_Response_ID == response.Survey_Response_ID)
                    .ResponseFeedback = sentimentResponse;
            }
        }

        return new OkObjectResult(projectSurveyResponse);
    }
}