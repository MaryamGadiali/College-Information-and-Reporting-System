using College_Information_and_Reporting_System.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace College_Information_and_Reporting_System.Tests
{
    //To match accessibility of the base class
    public class Program
    {}

    public class TestWebApplicationFactory : WebApplicationFactory<Program>//boots the real api
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                //Remove the live db details
                var prodDbConfig = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (prodDbConfig != null)
                {
                    services.Remove(prodDbConfig);
                }

                //add test db
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestDb");
                });
            });
            
            //base.ConfigureWebHost(builder);
        }
    }
}
