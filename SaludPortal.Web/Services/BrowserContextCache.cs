namespace SaludPortal.Web.Services;

public class BrowserContextCache
{
    private volatile ClientContextPayload? _cached;

    public void Update(ClientContextPayload payload)
        => _cached = payload;

    public ClientContextPayload? Current => _cached;
}
