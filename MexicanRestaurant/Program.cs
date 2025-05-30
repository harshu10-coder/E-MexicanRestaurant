using MexicanRestaurant.Data;
using MexicanRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
<<<<<<< HEAD
    .AddRoles<IdentityRole>()
=======
>>>>>>> c1a8722ef73c4dbd4fec6dbf10f1525714f22f4d
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession(options=>{
    options.IdleTimeout = TimeSpan.FromMinutes(30);//Set your Timeout here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
<<<<<<< HEAD

app.UseAuthentication();
app.UseAuthorization();

// create scope for identity
var scopeFactory=app.Services.GetRequiredService<IServiceScopeFactory>();
using(var scope =scopeFactory.CreateScope())
{
    await IdentityConfig.CreateAdminUserAsync(scope.ServiceProvider);
}



app.MapControllerRoute(
    name:"areas",
    pattern: "{area:exists}/{controller=Ingredient}/{action=Index}/{id?}");
    

=======
app.UseAuthorization();

>>>>>>> c1a8722ef73c4dbd4fec6dbf10f1525714f22f4d
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ingredient}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
