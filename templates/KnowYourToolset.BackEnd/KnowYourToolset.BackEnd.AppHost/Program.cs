var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.KnowYourToolset_BackEnd_Api>("knowyourtoolset-backend-api");

builder.Build().Run();
