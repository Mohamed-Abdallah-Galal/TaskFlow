using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.DataTemp;

namespace TaskFlow.PLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // ✅ ADD YOUR DBCONTEXT TO DEPENDENCY INJECTION
            builder.Services.AddDbContext<TaskFlowDbContext>(options =>
                options.UseSqlServer("Server=.;Database=Motorak;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"));



            var app = builder.Build();





            // ✅ ADD SEEDER RIGHT HERE - between Build() and pipeline configuration
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TaskFlowDbContext>();
                    DbSeeder.Initialize(context);
                    Console.WriteLine("🎉 Database seeded successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Seeding error: {ex.Message}");
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
