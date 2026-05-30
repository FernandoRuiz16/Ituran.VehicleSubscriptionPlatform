using Ituran.Application;
using Ituran.Infrastructure;
using Ituran.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();