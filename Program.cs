using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Session;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
//In-Memory

services.AddDistributedMemoryCache();
services.AddSession();              
// Add framework services.
services.AddMvc(options => options.EnableEndpointRouting = false);
services.AddControllers(options => options.EnableEndpointRouting = false);
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();  
app.UseMvc();  
app.UseAuthorization();
GetSqlData.AddErrors();
Console.WriteLine("Welcome To Schoolwind");
Console.WriteLine("Control+click on one of those two localhost links and everything is good to go!");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 
app.Run();


