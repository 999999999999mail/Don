using Don.Common;
using Don.Common.AuthPolicy;
using Don.Common.Messages;
using Don.Common.Middleware;
using Don.Infrastructure.Jwt;
using Don.Infrastructure.Redis;
using Don.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;

namespace Don.Web.API
{
    public class Startup
    {
        private IConfigurationSection redisConf;

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

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse { Code = 401, Msg = "Unauthorized" },
                                Formatting.Indented, 
                                new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
                        }
                    };
                });
            services.Configure<JwtOptions>(jwtConf);
            #endregion

            #region Authorization
            services.AddSingleton<IAuthorizationHandler, TokenSessionHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TokenSession", policy => policy.Requirements.Add(new TokenSessionRequirement(int.Parse(jwtConf["Expiry"]))));
            });
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
                options.IncludeXmlComments(Path.Combine(xmlDocPath, "Don.Web.API.xml"));
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Any");

            app.UseAuthentication();

            app.UseMiddleware<VisitMiddleware>();

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
