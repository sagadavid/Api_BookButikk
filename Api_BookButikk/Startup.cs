using Api_BookButikk.Data;
using Api_BookButikk.Model;
using Api_BookButikk.Repository;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_BookButikk
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookButikkDbContext>(options =>
            //options.UseSqlServer("Server=.;Database=BookButikkAPI;Integrated Security=True"));
            //instead of hardcoded connection string above, use configuration below
            options.UseSqlServer(Configuration.GetConnectionString("Default")));

            //identity roles comes from package
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BookButikkDbContext>()
                .AddDefaultTokenProviders();

            //add jwt configuration after appsettings
            //jwt as authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer(option =>
                   {
                       option.SaveToken = true;
                       option.RequireHttpsMetadata = false;
                       option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidAudience = Configuration["JWT:ValidAudience"],
                           ValidIssuer = Configuration["JWT:ValidIssuer"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                       };
                   });

            services.AddControllers().AddNewtonsoftJson();//package to patch added
            
            //configure repositories as in addtransient
            services.AddTransient<IBookRepository, BookRepository>();

            services.AddTransient<IAccountRepository, AccountRepository>();

            services.AddAutoMapper(typeof(Startup));//automapper is presented globally
            
            services.AddCors(option=>//added
            {
                option.AddDefaultPolicy(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
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

            app.UseCors();//cors enabled globally

            app.UseAuthentication();//after signup is managed and requires authorize attribute at controller or action

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
