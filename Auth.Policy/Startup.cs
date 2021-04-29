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
            //���ڲ��Ե������֤
            services.AddAuthentication(defaultScheme: "Cookies")
                .AddCookie(option =>
                {
                    option.LoginPath = "/Authorization/Login";
                    option.LogoutPath = "/Authorization/LoginOut";
                    option.AccessDeniedPath = "/Authorization/AccessDenied"; //û�з���Ȩ����ת��ҳ��
                });

            //��Ӳ���
            services.AddAuthorization(config =>
            {
                config.AddPolicy("MyPolicy", option => //�����ǲ�������
                {
                    //�����ǹ���Ա��
                    option.RequireRole("Admin");
                });
                config.AddPolicy("MyPolicy2", option =>
                {
                    //�����Ǳ༭�û���
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
