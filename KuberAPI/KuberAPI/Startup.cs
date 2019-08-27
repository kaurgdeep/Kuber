using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KuberAPI.Interfaces.Services;
using KuberAPI.Services;

namespace KuberAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "kuber.com",
                            ValidAudience = "kuber.com",
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[Constants.JWTSecurityKey]))
                        };
                    });
            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
            // var connection = @"Server=(localdb)\mssqllocaldb;Database=KuberDB;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddScoped<IEntityService<User>, UserService>();
            services.AddScoped<IEntityService<Ride>, RideService>();
            services.AddScoped<IEntityService<Address>, AddressService>();

            services.AddDbContext<KuberContext>
                (options => options.UseSqlServer(Configuration[Constants.DbConnection]));
            services.AddCors(o => o.AddPolicy(Constants.KuberServerCorsPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication(); //it should be before UseMvc
            app.UseCors(Constants.KuberServerCorsPolicy);

            app.UseMvc();
            //app.UseCors("KuberServerCorsPolicy");
        }
    }
}
