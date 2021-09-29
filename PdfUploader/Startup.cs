using PdfUploader.Models;
using PdfUploader.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;

namespace PdfUploader
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
            //services.AddControllersWithViews(options =>
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            var version = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
               .ToString("yyyy-MM-dd HH:mm");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Uploader", Version = version });

            });

            BsonClassMap.RegisterClassMap<PdfFile>();
            services.Configure<DatabaseStoreSettings>(
            Configuration.GetSection(nameof(DatabaseStoreSettings)));

            services.AddSingleton<IDatabaseStoreSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseStoreSettings>>().Value);

            services.AddTransient<IFile, PdfFile>();
            services.AddScoped<IFileUploaderService, FileUploaderService>();
            services.AddTransient<IDatabaseRepository, DatabaseRepository>();


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Uploader");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
