using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using AutoMapper;
using PoohAPI.Application;

namespace PoohAPI
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
            #region AutoMapper configuration    

            services.AddAutoMapper();

            var mapperConfig = AutoMapperInit.InitMappings();

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            #endregion

            //Dependency Injection
            DependencyInit.Init(services);

            #region Swagger configuration
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "ELBHO/HBO-stagemarkt API",
                        Version = "V1",
                        Description = "An API providing endpoints for reading/writing operations on: vacancies, companies, users",
                        Contact = new Contact
                        {
                            Name = "Joshua Volkers",
                            Email = "563871@student.inholland.nl"
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                s.DescribeAllEnumsAsStrings();
            });
            #endregion

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "ELBHO/HBO-stagemarkt API");
                s.RoutePrefix = string.Empty;
            });

            app.UseDeveloperExceptionPage();
        }
    }
}
