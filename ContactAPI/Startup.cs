using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using ContactAPI.Helpers;
using ContactAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ContactAPI.Models;
using Microsoft.EntityFrameworkCore;
using ContactAPI.Repository;
using NLog;
using System.IO;

namespace ContactAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IContactRepository, ContactRepository>();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ContactService",
                    Description = "ASP.NET Core 3.1 Web API"
                });

                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["AppSettings:Issuer"],
                    ValidAudience = Configuration["AppSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"])) //Configuration["JwtToken:SecretKey"]  
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactService");
            });
        }
    }
}
