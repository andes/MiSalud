FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SaludPortal.Web/SaludPortal.Web.csproj", "SaludPortal.Web/"]
COPY ["AndesServices/AndesServices.csproj", "AndesServices/"]
COPY ["SaludPortal.ServiceDefaults/SaludPortal.ServiceDefaults.csproj", "SaludPortal.ServiceDefaults/"]

RUN dotnet restore "SaludPortal.Web/SaludPortal.Web.csproj"

COPY . .

RUN dotnet publish "SaludPortal.Web/SaludPortal.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SaludPortal.Web.dll"]