# SaludPortal.Web — Portal del Paciente

Portal de pacientes (Blazor Server) de la plataforma de salud pública de Neuquén, integrado con
**Andes**. Este repositorio es **autocontenido**: no depende del panel Admin ni de Aspire.

## Proyectos


| Proyecto                  | Rol                                                       |
| ------------------------- | --------------------------------------------------------- |
| `SaludPortal.Web`         | Portal Blazor Server                                      |
| `AndesServices`           | Librería: interfaces, servicios, entidades y DTOs de Andes |
| `SaludPortal.Application` | Casos de uso y modelos limpios consumidos por Web         |


Solución: [`SaludPortal.Web.slnx`](SaludPortal.Web.slnx).

## Requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/download) (9.0.200+ para `.slnx`)
- Docker (opcional, para publicar imagen)

## Build y ejecución

```bash
dotnet build SaludPortal.Web.slnx
dotnet run --project SaludPortal.Web
```

### Docker

```bash
docker build -t salud-web .
```

> **Importante:** no agregar `RuntimeIdentifier` a los pasos de `restore`/`publish` del Dockerfile.
> `AndesServices` es un proyecto referenciado y eso provoca el error `NETSDK1152` por salidas duplicadas.

## Configuración relevante

| Sección                                   | Propósito                                      |
| ----------------------------------------- | ---------------------------------------------- |
| `urlServicios`                            | URLs de Andes (Prod/Demo)                      |
| `urlLASCHyBS`                             | Backend de resultados de laboratorio           |
| `ApiXroadssAndes`                         | Gateway X-Road (RENAPER, RANIA)                |
| `SaludConfiguracion` (`saludConfig.json`) | Paginación y rangos de fecha de laboratorios   |
| `AdminLogs:ApiKey` / `AdminLogs:BaseUrl`  | Envío opcional de logs/telemetría al Admin     |

`saludConfig.json` se carga explícitamente en `SaludPortal.Web/Program.cs`
(`AddJsonFile(..., optional: false)`).

## Autenticación

Los pacientes inician sesión contra Andes (`modules/mobileApp/login`) vía `AuthController`
(`POST /api/auth/login`). El JWT se guarda en la cookie `MiSalud` (expiración deslizante de 1 hora).

## Observabilidad (opcional)

El portal puede enviar logs (`Information+`) y telemetría de uso al panel **SaludPortal.Admin**
(otro repositorio) por HTTP:

- `POST /api/logs` y `POST /api/telemetry`
- Header `X-Api-Key` = `AdminLogs:ApiKey`

**Si `AdminLogs:ApiKey` está vacío, el pipeline queda desactivado** y el portal funciona sin Admin.

Para activarlo:

| Clave               | Ejemplo Development              | Notas                                              |
| ------------------- | -------------------------------- | -------------------------------------------------- |
| `AdminLogs:ApiKey`  | misma que `LogIngestion:ApiKey` del Admin | Debe coincidir exactamente               |
| `AdminLogs:BaseUrl` | `https://localhost:7180`         | URL pública o de red donde corre el Admin          |

En Docker/producción, apuntar `AdminLogs__BaseUrl` a la URL real del Admin (no usar service discovery de Aspire).
