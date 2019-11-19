using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace SqlSugarDemo.API
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
            //注册swagger服务,定义1个或者多个swagger文档
            services.AddSwaggerGen(s =>
                {
                    //设置swagger文档相关信息
                    s.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Bingle API",
                        Description = "一个简单的ASP.NET Core Web API",
                        TermsOfService = new Uri("https://www.cnblogs.com/taotaozhuanyong"),
                        Contact = new OpenApiContact
                        {
                            Name = "bingle",
                            Email = string.Empty,
                            Url = new Uri("https://www.cnblogs.com/taotaozhuanyong"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "许可证",
                            Url = new Uri("https://www.cnblogs.com/taotaozhuanyong"),
                        }
                    });
                    //获取xml注释文件的目录
                    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                    // 启用xml注释
                    s.IncludeXmlComments(xmlPath);
                });
            services.AddControllers();
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
                app.UseExceptionHandler("/error");
            }
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
