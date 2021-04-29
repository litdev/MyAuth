using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Policy
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
            //基于策略的身份验证
            services.AddAuthentication(defaultScheme: "Cookies")
                .AddCookie(option =>
                {
                    option.LoginPath = "/Authorization/Login";
                    option.LogoutPath = "/Authorization/LoginOut";
                    option.AccessDeniedPath = "/Authorization/AccessDenied"; //没有访问权限跳转的页面
                });

            //添加策略
            services.AddAuthorization(config =>
            {
                config.AddPolicy("MyPolicy", option => //这里是策略名称
                {
                    //必须是管理员组
                    option.RequireRole("Admin");
                });
                config.AddPolicy("MyPolicy2", option =>
                {
                    //必须是编辑用户组
                    option.RequireRole("Edit");
                });
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
