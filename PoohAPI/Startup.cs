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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PoohAPI.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

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

                //var security = new Dictionary<string, IEnumerable<string>>
                //{
                //    {"Bearer", new string[] { }}
                //};

                //s.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});

                //s.AddSecurityRequirement(security);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                s.DescribeAllEnumsAsStrings();
            });
            #endregion

            #region JWT validation configuration

            var tokenOptions = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(5),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "poohapi",
                ValidAudience = "poohapi",
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("D2n2skmv8xY3ZcSzgc9eMwWjYzXMPXHtWKarHxscXeZN6FbX6qkeBsw88txVPRyHf4j2VkEH4XZLskGgKSJHHybhjVXAHXEYMw8z6gGTG58wT8y49bJ8ezMJNXhFz9Vd"))
            };

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenOptions;
                    options.SaveToken = true;
                });
                //.AddFacebook(facebookOptions =>
                //{
                //    facebookOptions.AppId = "";
                //    facebookOptions.AppSecret = "";
                //});
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

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "ELBHO/HBO-stagemarkt API");
                s.RoutePrefix = string.Empty;
            });
        }
    }
}
