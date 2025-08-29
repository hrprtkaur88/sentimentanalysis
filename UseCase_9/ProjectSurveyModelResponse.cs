namespace PolicyComplianceCheckerApi.Models;

public record ProjectSurveyModelResponse
{
    public int PROJECT_ID { get; set; }
    public List<TeamSurvey> TeamSurveys { get; set; }
}

public record TeamSurvey
{
    public int Survey_ID { get; set; }
    public string Survey_Period { get; set; }
    public string Survey_Status { get; set; }
    public string Memberfirm_ID { get; set; }
    public List<SurveyResponse> Responses { get; set; }
}

public record SurveyResponse
{
    public int Survey_Response_ID { get; set; }
    public string Delivery_To_Plan_Question { get; set; }
    public string Delivery_To_Plan_Answer { get; set; }
    public string Client_Value_Question { get; set; }
    public string Client_Value_Answer { get; set; }
    public string Quality_Question { get; set; }
    public string Quality_Answer { get; set; }
    public string Escalation_Question { get; set; }
    public string Escalation_Answer { get; set; }
    public string Wellbeing_Question { get; set; }
    public string Wellbeing_Answer { get; set; }
    public string Delivery_To_Plan_Comment { get; set; }
    public string Client_Value_Comment { get; set; }
    public string Quality_Comment { get; set; }
    public string Escalation_Comment { get; set; }
    public string Wellbeing_Comment { get; set; }
    public string Level { get; set; }
    public string Location { get; set; }
    public string Overall_Comments { get; set; }
    public ResponseFeedback responseFeedback { get; set; }
}
