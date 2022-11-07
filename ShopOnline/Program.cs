// Gọi phương thức để khởi tạo web server: Kestrel

using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using ShopOnline.SignalRLab;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopOnline.Filters;
//using MyRazorPage.Models;

var builder = WebApplication.CreateBuilder(args);

// Bổ sung kiến trúc Razor và container của web server
builder.Services.AddRazorPages().AddRazorPagesOptions(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    //o.Conventions.AddFolderApplicationModelConvention("/accounts/profile", model => model.Filters.Add(new MyFilter(new PRN221DBContext())));
    //o.Conventions.AddFolderApplicationModelConvention("/admin", model => model.Filters.Add(new MyFilter(new PRN221DBContext())));

});

// Add session 30 phút hết hạn
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddDbContext<PRN221DBContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("DB")
    )
);
// Add service cho pdf convert
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Add authen cho JWT
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

//}).AddJwtBearer(o =>
//{
//    o.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey
//        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = true
//    };
//});
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

// Build web server
var app = builder.Build();

// Use JWT
//app.UseAuthentication();
//app.UseAuthorization();

// Use resource in wwwroot
app.UseStaticFiles();
// Ánh xạ xử lý request của user tới website dựa trên kiến trúc Razor
app.MapRazorPages();
app.UseSession();

app.MapHub<SignalrServer>("/signalrServer");
// Thực thi ứng dụng web bằng web server
app.Run();


