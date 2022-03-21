using _Generate.Data;
using _Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


/**
 * EntityFramework Commands
 * 
 * ## Create
 * dotnet ef migrations add InitialCreate
 *
 * ## Update
 * dotnet ef database update
 */
// Local Database Connection
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlite("Data Source=./Database.db",
        b => b.MigrationsAssembly("_Generate"));
});

builder.Services.AddTransient<IPostService, PostService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
