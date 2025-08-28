using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DavidBekeris.Data;
using DavidBekeris.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DavidBekerisContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

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
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5000); // Prod only
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapPost("email", async (string email, IAmazonSimpleEmailService emailService, ContactFormModel settings) =>
{
    var request = new SendEmailRequest
    {
        Source = settings.Email,
        Destination = new Destination
        {
            ToAddresses = [email]
        },
        Message = new Message
        {
            Subject = new Content("Test mail from program line 60"),
            Body = new Body
            {
                Html = new Content("""
                    <html>
                    <body> 
                    <p> Test message from program line 66</p>
                    </body>
                    </html>
                    """)
            }
        }
    };
    var response = await emailService.SendEmailAsync(request);

    return Results.Ok(new { response.MessageId, response.HttpStatusCode });
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
