using System.ComponentModel.DataAnnotations;

namespace UseCase_9.Models;

public record AzureOpenAIOptions
{
    public const string AzureOpenAI = "AzureOpenAIOptions";

    [Required]
    public string DeploymentName { get; set; }

    [Required]
    public string EndPoint { get; set; }

    [Required]
    public string ApiKey { get; set; }

    [Required]
    public int MaxTokens { get; set; }

    [Required]
    public int RetryCount { get; set; } = 3;

    [Required]
    public int RetryDelayInSeconds { get; set; } = 60;
}
