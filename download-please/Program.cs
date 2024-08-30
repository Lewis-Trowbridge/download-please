using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;
using download_please.Utils;
using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton<IFileSystem, FileSystem>();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IDownloaderSelector, DownloaderSelector>();
builder.Services.AddSingleton<HttpFileDownloader>();
builder.Services.AddSingleton<DownloadBackgroundRunnerFactory>();
builder.Services.AddSingleton<FileUtils>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DownloadService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
