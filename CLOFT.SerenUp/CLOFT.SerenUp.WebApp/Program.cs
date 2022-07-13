using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CLOFT.SerenUp.WebApp.Data;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using CLOFT.SerenUp.WebApp.Services;
using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);

AWSOptions awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials(builder.Configuration.GetConnectionString("accessKey"), builder.Configuration.GetConnectionString("secretKey"))
};
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>(awsOptions);

builder.Services.AddScoped<IBraceletsService, BraceletsService>();

//Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCognitoIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseStaticFiles();
app.Run();
