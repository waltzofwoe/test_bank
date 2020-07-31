using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestBank.Models;
using TestBank.Service;
using TestBank.Utils;

namespace TestBank
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
            services.AddRazorPages()
                .AddRazorPagesOptions(opt =>
                {
                    opt.Conventions.AuthorizeFolder("/");
                    opt.Conventions.AllowAnonymousToPage("/Login");
                });
            services.AddDbContext<BankContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("BankContext")));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(arg=>arg.LoginPath= new Microsoft.AspNetCore.Http.PathString("/Login"));
            services.AddAuthorization();
            services.AddTransient<AuthorizationService>();
            services.AddTransient<ActionsService>();
            services.AddTransient<OperationService>();
            services.AddTransient<SendPaymentDocAction>();
            services.AddTransient<SendPaymentDocForeignAction>();
            services.AddTransient<SendToInspectionAction>();
            services.AddTransient<SendToPartnerAction>();
            services.AddTransient<ShowConfirmWindowAction>();
            services.AddTransient<SmsAction>();
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
                endpoints.MapRazorPages();
            });
        }
    }
}
