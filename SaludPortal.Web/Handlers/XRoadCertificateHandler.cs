using System.Security.Cryptography.X509Certificates;

namespace AndesServices.Handlers
{
    public class XRoadCertificateHandler : HttpClientHandler
    {
        public XRoadCertificateHandler(IConfiguration configuration, ILogger<XRoadCertificateHandler> logger)
        {
            var certPath = configuration["ApiXroadssAndes:CertPath"];
            var certPassword = configuration["ApiXroadssAndes:CertPassword"];

            if (!string.IsNullOrWhiteSpace(certPath))
            {
                if (File.Exists(certPath))
                {
                    try
                    {
                        var cert = X509CertificateLoader.LoadPkcs12FromFile(certPath, certPassword);
                        ClientCertificates.Add(cert);
                        logger.LogInformation("Certificado X-Road cargado correctamente desde {CertPath}", certPath);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error al cargar el certificado X-Road desde {CertPath}", certPath);
                    }
                }
            }
        }
    }
}
