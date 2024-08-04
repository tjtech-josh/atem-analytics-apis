namespace TJTech.APIs.ChatGTP.Data.Models;

public class Chat
{
    public Guid id { get; set; }
    public string prompt { get; set; }
    public string? input_text { get; set; }
    public string? response_text { get; set; }
    public string? error_message { get; set; }
    public short current_stage { get; set; }
    public DateTime? stage_updated_at { get; set; }
    public string? stage_updated_by_id { get; set; }
    public string? stage_updated_by_name { get; set; }
    public short validated { get; set; }
    public DateTime? last_validated_at { get; set; }
    public string? last_validated_by_id { get; set; }
    public string? last_validated_by_name { get; set; }
    public short processed { get; set; }
    public DateTime? last_processed_at { get; set; }
    public string? last_processed_by_id { get; set; }
    public string? last_processed_by_name { get; set; }
    public short deleted { get; set; } 
    public DateTime? deleted_at { get; set; }
    public string? deleted_by_id { get; set; }
    public string? deleted_by_name { get; set; }
    public DateTime created_at { get; set; }
    public string created_by_id { get; set; }
    public string created_by_name { get; set; }
}