using Microsoft.EntityFrameworkCore;
using BookingHotel.Models;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookingDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:BookingHotelConnection"]);
});

builder.Services.AddScoped<IBookingRepository, EFBookingRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();
