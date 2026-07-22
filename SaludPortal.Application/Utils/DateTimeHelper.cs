using System.Globalization;

namespace SaludPortal.Application.Utils
{
    /// <summary>
    /// Helper para manejo de fechas con zona horaria de Argentina (UTC-3)
    /// </summary>
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo ArgentinaTimeZone;

        static DateTimeHelper()
        {
            try
            {
                // Intentar obtener la zona horaria de Argentina
                ArgentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback para sistemas Linux/Mac
                try
                {
                    ArgentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
                }
                catch
                {
                    // Si no se encuentra, crear un offset fijo de UTC-3
                    ArgentinaTimeZone = TimeZoneInfo.CreateCustomTimeZone(
                        "Argentina Standard Time",
                        TimeSpan.FromHours(-3),
                        "Argentina Standard Time",
                        "Argentina Standard Time");
                }
            }
        }

        /// <summary>
        /// Convierte una fecha UTC a hora local de Argentina (UTC-3)
        /// </summary>
        public static DateTime ToArgentinaTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
            {
                // Si no tiene kind, asumimos que es UTC
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            else if (utcDateTime.Kind == DateTimeKind.Local)
            {
                // Si es local, convertir a UTC primero
                utcDateTime = utcDateTime.ToUniversalTime();
            }

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, ArgentinaTimeZone);
        }

        /// <summary>
        /// Convierte una fecha nullable UTC a hora local de Argentina (UTC-3)
        /// </summary>
        public static DateTime? ToArgentinaTime(DateTime? utcDateTime)
        {
            if (utcDateTime == null)
                return null;

            return ToArgentinaTime(utcDateTime.Value);
        }

        /// <summary>
        /// Obtiene la fecha actual en hora de Argentina
        /// </summary>
        public static DateTime NowArgentina()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ArgentinaTimeZone);
        }

        public static DateTime? ParseDateTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim();

            // RENAPER / Andes usan dd/MM/yyyy: probar exacto primero para no invertir día/mes
            // con TryParse + InvariantCulture (MM/dd/yyyy).
            if (DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                return fecha.Date;

            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                return fecha.Date;

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                return fecha.Date;

            return null;
        }
    }
}
