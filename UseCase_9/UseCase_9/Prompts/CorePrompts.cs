namespace UseCase_9.Prompts;
public record CorePrompts
{
    public static string GetSystemPrompt()
    {
        var systemPrompt = @"Analyze both free-text comments and structured Likert scale responses
Weight negative comments more heavily (they often contain actionable insights)
Consider the professional context (workplace survey feedback)
Extract key themes and concerns from each category
Provide both numerical sentiment scores (-1 to +1) and descriptive labels
Account for mixed sentiment within individual responses

Your response should be in JSON using the following structure as an example:

{
  ""project_id"": 10000,
  ""sentiment_analysis"": {
    ""overall_sentiment"": {
      ""score"": -0.15,
      ""label"": ""Slightly Negative"",
      ""confidence"": 0.87
    },
    ""category_breakdown"": {
      ""delivery_to_plan"": {
        ""sentiment_score"": 0.75,
        ""sentiment_label"": ""Positive"",
        ""key_themes"": [""confidence in delivery"", ""on-time completion"", ""project structure""]
      },
      ""client_value"": {
        ""sentiment_score"": 0.25,
        ""sentiment_label"": ""Neutral-Positive"",
        ""key_themes"": [""roadmap concerns"", ""changing priorities"", ""tactical solutions""]
      },
      ""quality"": {
        ""sentiment_score"": -0.45,
        ""sentiment_label"": ""Negative"",
        ""key_themes"": [""time pressure"", ""rushed deadlines"", ""quality impact"", ""insufficient planning""]
      },
      ""escalation"": {
        ""sentiment_score"": -0.55,
        ""sentiment_label"": ""Negative"",
        ""key_themes"": [""communication barriers"", ""junior staff concerns ignored"", ""leadership disconnect""]
      },
      ""wellbeing"": {
        ""sentiment_score"": -0.85,
        ""sentiment_label"": ""Very Negative"",
        ""key_themes"": [""high stress levels"", ""excessive overtime"", ""work-life balance issues""]
      }
    },
    ""positive_highlights"": [
      ""Project has evolved with better structure and processes"",
      ""Great place for professional growth"",
      ""Strong team collaboration and respect""
    ],
    ""areas_of_concern"": [
      ""Significant wellbeing and stress issues"",
      ""Quality compromised due to tight timelines"",
      ""Communication gaps with leadership"",
      ""Frequent priority changes affecting delivery""
    ],
    ""response_count"": 3,
    ""survey_period"": ""May 2023"",
    ""member_firm"": ""AP -India""
  }}";
        return systemPrompt;
    }
}