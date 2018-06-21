using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using WebApi.Entities;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICityInfoRepository, SqlCityRepository>();
            services.AddTransient<IMailService, LocalMailService>();

            string connectionString = Configuration["ConnectionStrings:cityInfoContext"];
            services.AddDbContext<CityInfoContext>(o=>o.UseSqlServer(connectionString));

            services.AddMvc()
                .AddMvcOptions(o=>o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            /*services.AddAuthorization();
            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.ApiName = "api";
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("NLog.config");
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            InitAutoMapper();
            //redirect to https; 
            //for .net core > 2.0 use HTTPS Redirection Middleware (UseHttpsRedirection)
            var options = new RewriteOptions()
                  .AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 44326);

            app.UseRewriter(options);

            app.UseStatusCodePages();
            //app.UseAuthentication();
            app.UseMvc();
        }

        private void InitAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                cfg.CreateMap<Entities.City, Models.CityDto>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            });
        }
    }
}
