namespace SaludPortal.Web.Services;

public enum AppToastLevel
{
    Info,
    Warning,
    Error,
    Success
}

public sealed record AppToastMessage(Guid Id, string Message, AppToastLevel Level, int DurationMs);

public sealed class AppToastService
{
    public event Action<AppToastMessage>? OnShow;

    public void ShowWarning(string message, int durationMs = 5000)
    {
        Show(message, AppToastLevel.Warning, durationMs);
    }

    public void ShowError(string message, int durationMs = 5000)
    {
        Show(message, AppToastLevel.Error, durationMs);
    }

    public void Show(string message, AppToastLevel level, int durationMs = 5000)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        OnShow?.Invoke(new AppToastMessage(Guid.NewGuid(), message, level, durationMs));
    }
}
