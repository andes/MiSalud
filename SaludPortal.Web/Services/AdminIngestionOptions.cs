namespace SaludPortal.Web.Services;

public sealed class AdminIngestionOptions
{
    public const string SectionName = "AdminLogs";

    public int QueueCapacity { get; set; } = 1000;
    public int MaxAttempts { get; set; } = 5;
    public int InitialRetryDelayMs { get; set; } = 500;
    public int MaxRetryDelayMs { get; set; } = 30_000;
    public int HttpTimeoutSeconds { get; set; } = 10;
    public int DrainTimeoutSeconds { get; set; } = 5;
}
