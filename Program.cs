using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Middleware;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RoomManagementDbContextBCS240032>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MID_BCS240032"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3)));

builder.Services.AddScoped<IRoomRepositoryBCS240032, RoomRepositoryBCS240032>();
builder.Services.AddScoped<IRoomTypeRepositoryBCS240032, RoomTypeRepositoryBCS240032>();
builder.Services.AddScoped<IRoomImageRepositoryBCS240032, RoomImageRepositoryBCS240032>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();



app.UseMiddleware<RequestLoggingMiddleware>();

//app.UseMiddleware<CheckDBMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RoomBCS240032}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
