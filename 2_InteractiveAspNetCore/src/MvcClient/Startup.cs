using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace MvcClient
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
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // Adiciona os serviços de autenticação ao DI(Injeção de dependência).
            services.AddAuthentication(options =>
            {
                // Adiciona cookie para fazer login local do usuário.
                options.DefaultScheme = "Cookies";

                // Define que o usuário faça login usando o protocolo OpenID Connect.
                options.DefaultChallengeScheme = "oidc";
            })

            // Adiciona o manipulador que pode processar cookies.
            .AddCookie("Cookies")

            // Configura o manipulador que executa o protocolo OpenID Connect.
            .AddOpenIdConnect("oidc", options =>
            {
                // Indica onde o serviço de token confiável está localizado.
                options.Authority = "https://localhost:5001";

                // Identificação do cliente
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                // Mantém os tokens do IdentityServer no cookie.
                options.SaveTokens = true;

                options.Scope.Add("profile");

                /* Define se o manipulador deve ir para o endpoint de informações do usuário para
                recuperar ou não declarações adicionais */
                options.GetClaimsFromUserInfoEndpoint = true;
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
        }
    }
}
