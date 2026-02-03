using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DavidBekeris.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

builder.Services.AddAWSService<IAmazonSimpleEmailService>();

builder.Services.Configure<ContactFormModel>(builder.Configuration.GetSection(ContactFormModel.ConfigurationSection));

var env = builder.Environment;

if (env.IsDevelopment())
{
    // Let dev use defaults or HTTPS ports
}
else
{
    builder.WebHost.ConfigureKestrel(static serverOptions =>
    {
        serverOptions.ListenAnyIP(5000); // Prod only
    });
}

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = Status307TemporaryRedirect;
//    options.HttpsPort = 5000;
//});

//if (!builder.Environment.IsDevelopment())
//{
//    builder.Services.AddHttpsRedirection(options =>
//    {
//        options.RedirectStatusCode = Status308PermanentRedirect;
//        options.HttpsPort = 443;
//    });
//}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
