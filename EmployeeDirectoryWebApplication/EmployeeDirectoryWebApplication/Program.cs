using EmployeeDirectoryWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectoryWebApplication;
using Microsoft.AspNetCore.Identity;
using EmployeeDirectoryWebApplication.Areas.Identity;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<EmployeeAppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Demo")));


builder.Services.AddDefaultIdentity<UserAuthentication>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EmployeeAppDbContext>();

var app = builder.Build();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Create the "Admin" role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Create the "Customer" role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
}

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
endpoints.MapControllerRoute(
     name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
}
);
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
