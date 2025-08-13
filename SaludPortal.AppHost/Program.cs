var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.SaludPortal_ApiService>("apiservice");

builder.AddProject<Projects.SaludPortal_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.AndesServices>("andesservices");

builder.Build().Run();
