using Don.Common;
using Don.Common.AuthPolicy;
using Don.Common.Messages;
using Don.Common.Middleware;
using Don.Infrastructure.Extensions;
using Don.Infrastructure.Jwt;
using Don.Infrastructure.Redis;
using Don.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Don.Adm.API
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
            #region Cors
            services.AddCors(options =>
            {
                options.AddPolicy("Any", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            #endregion

            #region DbContext
            services
                .AddDbContext<DonContext>(options => options.UseSqlServer(Configuration.GetSection("SysConf")["ConnectionString"]))
                .AddUnitOfWork<DonContext>();
            #endregion

            #region Redis
            var redisConf = Configuration.GetSection("RedisConf");
            services.AddRedis(options =>
            {
                options.ConnectionString = redisConf.GetSection("Redis_Default")["Connection"];
                options.InstanceName = redisConf.GetSection("Redis_Default")["InstanceName"];
            });
            #endregion

            var jwtConf = Configuration.GetSection("JwtConf");

            #region Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConf["Issuer"],
                        ValidAudience = jwtConf["Audience"],
                        IssuerSigningKey = JwtSecurityKey.Create(jwtConf["Secret"])
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseBase { Code = -401, Msg = "Unauthorized" },
                                Formatting.Indented, 
                                new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
                        }
                    };
                });
            services.Configure<JwtOptions>(jwtConf);
            #endregion

            services.AddMvc();

            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Don.API.Help",
                    Version = "v1"
                });
                var xmlDocPath = PlatformServices.Default.Application.ApplicationBasePath;
                options.IncludeXmlComments(Path.Combine(xmlDocPath, "Don.Common.xml"));
                options.IncludeXmlComments(Path.Combine(xmlDocPath, "Don.Adm.API.xml"));
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Any");

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var err = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    ResponseBase resp = new ResponseBase();
                    if (err == null)
                    {
                        resp.Code = -500;
                        resp.Msg = "Unknown Exception";
                        loggerFactory.CreateLogger<Startup>().LogError($"Unknown Exception: {context.Request.Path}|{context.Request.GetClientIP()}");
                    }
                    else
                    {
                        resp.Code = -501;
                        resp.Msg = "Unhandled Exception";
                        loggerFactory.CreateLogger<Startup>().LogError($"Unhandled Exception: {err.Message}\r\n{err.StackTrace}"); 
                    }
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(resp, 
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
                });
            });

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();
#if DEBUG
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Don.API Help Endpoint");
            });
#endif
        }
    }
}
