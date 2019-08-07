using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using RepositorioSuprimentos.Models;
using RepositorioSuprimentos.App_Start.Identity;
using DbContext = System.Data.Entity.DbContext;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

[assembly: OwinStartup(typeof(RepositorioSuprimentos.Startup))]

namespace RepositorioSuprimentos {
    public class Startup {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        public void Configuration(IAppBuilder builder) {
            builder.CreatePerOwinContext<DbContext>(() => new IdentityDbContext<UsuarioAplicacao>("DefaultConnection"));

            builder.CreatePerOwinContext<IUserStore<UsuarioAplicacao>>((opcoes, contextoOwin) => {
                var dbContext = contextoOwin.Get<DbContext>();
                return new UserStore<UsuarioAplicacao>(dbContext);
            });

            builder.CreatePerOwinContext<UserManager<UsuarioAplicacao>>((opcoes, contextoOwin) => {
                var userStore = contextoOwin.Get<IUserStore<UsuarioAplicacao>>();
                var userManager = new UserManager<UsuarioAplicacao>(userStore);
                var userValidator = new UserValidator<UsuarioAplicacao>(userManager);

                userValidator.RequireUniqueEmail = true;
                userManager.UserValidator = userValidator;

                userManager.PasswordValidator = new SenhaValidador() {
                    TamanhoRequerido = 6,
                    ObrigatorioCaracteresEspeciais = true,
                    ObrigatorioDigitos = true,
                    ObrigatorioLowerCase = true,
                    ObrigatorioUpperCase = true
                };

                userManager.EmailService = new EmailServico();

                var dataProtectionProvider = opcoes.DataProtectionProvider;
                var dataProtectionProviderCreated = dataProtectionProvider.Create("ByteBank.Forum");

                userManager.UserTokenProvider = new DataProtectorTokenProvider<UsuarioAplicacao>(dataProtectionProviderCreated);

                userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
                userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                userManager.UserLockoutEnabledByDefault = true;

                return userManager;
            });

            builder.CreatePerOwinContext<SignInManager<UsuarioAplicacao, string>>((opcoes, contextoOwin) => {
                var userManager = contextoOwin.Get<UserManager<UsuarioAplicacao>>();
                var signInManager = new SignInManager<UsuarioAplicacao, string>(
                    userManager, contextoOwin.Authentication);
                return signInManager;
            });

            builder.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

        }
    }
}
