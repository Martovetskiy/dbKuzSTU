var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Lab1_WebAPI>("lab1-webapi");

builder.Build().Run();
