using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers;
using Server.Integrations;

namespace Server
{

    public class SubvilleApiImpl : ControllerBase, ISubvilleApiController
    {

        public Subversion Subversion { get; private set; } = new Subversion();
       

        public SubvilleApiImpl()
        {
            
            Debug.WriteLine("SubvilleApiImpl construct");
        }

        public async Task<ActionResult<TokenResponse>> GetTokenAsync(LoginRequest body)
        {

            return Ok((new TokenResponse()).ToJson());
            //return BadRequest("WTF");
            //throw new NotImplementedException();
        }

        public async Task<ActionResult<RepositoryDescriptionArray>> ListRepositoriesAsync(int? _limit, int? _skip) {
            return Ok((new RepositoryDescriptionArray( ){Total_results = 777}).ToJson());
        }


        public string[] GetRepositoriesList()
        {
            return null;
        }
       
    }


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public class AuthOptions {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
            public const int LIFETIME = 10; //Lifetime in minutes
            public static SymmetricSecurityKey GetSymmetricSecurityKey() {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddSingleton<ISubvilleApiController, SubvilleApiImpl>();

            services.AddRazorPages();
        }

        public class JWTInHeaderMiddleware {
            private readonly RequestDelegate _next;

            public JWTInHeaderMiddleware(RequestDelegate next) {
                _next = next;
            }

            public async Task Invoke(HttpContext context) {
                var name = "token";
                var cookie = context.Request.Cookies[name];

                if (cookie != null)
                    if (!context.Request.Headers.ContainsKey("Authorization"))
                        context.Request.Headers.Append("Authorization", "Bearer " + cookie);

                await _next.Invoke(context);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMiddleware<JWTInHeaderMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
