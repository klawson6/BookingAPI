using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

/*
 * 
 * THIS IS ADAPTED CODE FROM MICROSOFT .NET TUTORIALS I DO NOT CLAIM TO HAVE WRITTEN IT ALL
 * 
 */
namespace BookingAPI {

    /*
     * The Startup class is where:
     *      Services required by the app are configured.
     *      The request handling pipeline is defined.
     */

    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<HotelDBContext>(opt =>
              opt.UseInMemoryDatabase("Hotels")); // Make a database for Hotels
            services.AddDbContext<BookingDBContext>(opt =>
             opt.UseInMemoryDatabase("Bookings")); // Make a database for Bookings
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
