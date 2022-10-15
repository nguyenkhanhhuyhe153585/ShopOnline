// Gọi phương thức để khởi tạo web server: Kestrel

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRazorPage.Models;
using MyRazorPage.SignalRLab;
//using MyRazorPage.Models;

var builder = WebApplication.CreateBuilder(args);

// Bổ sung kiến trúc Razor và container của web server
builder.Services.AddRazorPages().AddRazorPagesOptions(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
//builder.Services.AddDbContext<PRN221DBContext>();
// Add session 5 phút hết hạn
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(5));
builder.Services.AddDbContext<PRN221DBContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("DB")
    )
);


builder.Services.AddSignalR();

// Build web server
var app = builder.Build();

// Use resource in wwwroot
app.UseStaticFiles();
// Ánh xạ xử lý request của user tới website dựa trên kiến trúc Razor
app.MapRazorPages();
app.UseSession();

app.MapHub<SignalrServer>("/signalrServer");
// Thực thi ứng dụng web bằng web server
app.Run();


