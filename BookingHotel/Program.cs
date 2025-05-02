using Microsoft.EntityFrameworkCore;
using BookingHotel.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookingDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:BookingHotelConnection"]);
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
});

builder.Services.AddScoped<IBookingRepository, EFBookingRepository>();


var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.UseSession();

SeedData.EnsurePopulated(app);

app.Run();
