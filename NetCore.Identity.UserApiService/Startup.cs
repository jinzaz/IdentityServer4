using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Identity.UserApiService
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
            services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
             {
                 options.Authority = Configuration.GetSection("IdentityUrl").Value;//配置IdentityServer的授权地址
                 options.RequireHttpsMetadata = false;//不需要https
                 options.Audience = OAuthConfig.UserApi.ApiName2;//api的name，需要和config的名称相同
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     RequireAudience = true,
                     ValidateAudience = true,
                     ClockSkew = TimeSpan.FromSeconds(0),
                 };
             });
            //netCore 2.0/2.1
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = "http://localhost:9500";
            //    options.RequireHttpsMetadata = false;
            //    options.ApiName = OAuthConfig.UserApi.ApiName2;
            //    options.LegacyAudienceValidation = true;
            //    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//时间偏移
            //});
            services.AddAuthorization(options => {
                options.AddPolicy("ApiV2", builder => {
                    builder.RequireScope(OAuthConfig.UserApi.ApiName2);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
