using ApiCore.JsonFilter;
using ApiCore.Utils;
using ApiService.DefaultService;
using AspNet.Security.OAuth.Validation;
using CSRedis;
using LogCore.Filters;
using LogCore.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using PluginCore.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiService
{
    public class Startup
    {
        public IConfiguration config { get; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            config = configuration;
        }
        private PluginCoreContextImpl applicationContext = null;
        public void ConfigureServices(IServiceCollection services)
        {
            LogLevels logLevel = LogLevels.Info;
            int maxDays = 7;
            IConfigurationSection logConfig = config.GetSection("Log");
            string maxFileSize = "10MB";
            if (logConfig != null)
            {
                Enum.TryParse(logConfig["Level"] ?? "", out logLevel);
                int.TryParse(logConfig["SaveDays"], out maxDays);
                maxFileSize = logConfig["MaxFileSize"];
                if (string.IsNullOrEmpty(maxFileSize))
                {
                    maxFileSize = "10MB";
                }
            }
            string logFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
            LoggerManager.InitLogger(new LogConfig
            {
                LogBaseDir = logFolder,
                MaxFileSize = maxFileSize,
                LogLevels = logLevel,
                IsAsync = true,
                LogFileTemplate = LogFileTemplates.PerDayDirAndLogger,
                LogContentTemplate = LogLayoutTemplates.SimpleLayout
            });
            LoggerManager.SetLoggerAboveLevels(logLevel);
            LoggerManager.StartClear(maxDays, logFolder, LoggerManager.GetLogger("clear"));

            services.AddSingleton(config as IConfigurationRoot);
            ////services.AddMvc().AddViewComponentsAsServices();
            services.AddControllersWithViews().AddJsonOptions(options => {
                //����ʱ���ʽ
                options.JsonSerializerOptions.Converters.Add(new DateJsonConverter("yyyy-MM-dd HH:mm:ss", DateTimeZoneHandling.Local));
                //��ʹ���շ���ʽ��key
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //��ʹ���շ���ʽ��key
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //��ȡ������Ҫ��ת���ַ���ʱʹ�õı�����
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }).AddNewtonsoftJson(options => {
                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            services.AddMemoryCache();
            services.AddCors();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddJwtBearer(OAuthValidationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = config["AuthUrl"];
                options.RequireHttpsMetadata = false;
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnAuthenticationFailed = (context) =>
                    {
                        var msg = context.Exception;
                        //throw msg;
                        return Task.CompletedTask;
                    }
                };
                //ע��Ӧ�õ�AppNameֵ
                options.Audience = config["Audience"];
            });
            applicationContext = services.AddPlugin( options =>
            {
                options.ConnectionString = config["ConnectionStrings:DefaultConnection"];
                options.AuthUrl = config["AuthUrl"];
            });
            string redisConnectionString = $@"{config["Redis:Connection"]},password={config["Redis:Password"]},defaultDatabase ={config["Redis:DefaultDB"]} ";
            //��ʼ�� RedisHelper
            RedisHelper.Initialization(new CSRedisClient(redisConnectionString));
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            services.AddSingleton<HttpRequestLogScopeMiddleware>();
            var apppart = services.FirstOrDefault(x => x.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance;
            var assemblys = new List<Assembly>() { Assembly.GetExecutingAssembly() };
            if (apppart != null)
            {
                ApplicationPartManager apm = apppart as ApplicationPartManager;
                //���и�������
                PluginCoreContextImpl ac = PluginCoreContext.Current as PluginCoreContextImpl;
                ac.AdditionalAssembly.ForEach((a) =>
                {
                    assemblys.Add(a);
                    apm.ApplicationParts.Add(new AssemblyPart(a));
                });
            }
            // ע����־http
            services.AddUserDefined();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreWebApi", Version = "v1" });
                //��Http��������
                c.OrderActionsBy(o => o.GroupName);

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var pluginPath = Path.Combine(basePath, "Plugin");
                c.CustomSchemaIds(a => a.FullName);
                //�ҵ����������xml�ļ�
                DirectoryInfo dir = new DirectoryInfo(pluginPath);
                FileInfo[] fil = dir.GetFiles();
                ////List<string> pluginsXMLPath = new List<string>();
                foreach (var item in fil)
                {
                    //��ȡxml�ļ�
                    if (item.Name.EndsWith("Plugin.xml"))
                        c.IncludeXmlComments(item.FullName);
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the JWT Bearer token in format: Bearer {token}",
                    Name = "authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                };
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securityScheme, new List<string>() }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
            })
          .AddMvcCore()
          .AddApiExplorer();
           services.AddAutoMapper(assemblys);
            applicationContext.Init().Wait();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            app.UseWebApi();
            loggerFactory.AddProvider(new LoggerProvider());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();

            applicationContext = app.AddPlugin(serviceProvider,options =>
            {
              
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreWebApi");
                options.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.SetIsOriginAllowed(c => true);
                options.AllowCredentials();
            });


            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Check}/{action=Index}/{id?}");
            });
            applicationContext.InitApp().Wait();
        }
    }
}
