using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add DbContext with SQL Server connection
        builder.Services.AddDbContext<ECommerceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add session service
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout (you can adjust this)
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; // Make the session cookie essential
        });

        var app = builder.Build();
        app.UseStaticFiles();
        // Configure the HTTP request pipeline.
        app.UseRouting();

        // Enable session middleware
        app.UseSession();  // This is important to use session

        // Map the default controller route
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });

        app.Run();
    }
}
