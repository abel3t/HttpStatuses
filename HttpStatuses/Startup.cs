using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using HttpStatuses.Repository;

namespace HttpStatuses
{
    public class Startup
  {
    public Startup(IConfiguration configuration) => Configuration = configuration;  

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      var mongoDbOptions = new MongoDbOptions()
      {
        ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING"),
        Database = Environment.GetEnvironmentVariable("MONGODB_DB_NAME")
      };
            
      services.AddSingleton(mongoDbOptions);
      services.AddSingleton<IMongoClient, MongoClient>(x => new MongoClient(mongoDbOptions.ConnectionString));
      services.AddScoped<IMongoDbContext, MongoDbContext>();
      services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();  
      services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

      services.AddCors(options =>
      {
        options.AddDefaultPolicy(
          builder =>
          {
            builder.WithMethods("GET", "POST", "PUT", "PATCH");
          });
      });
      
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "HttpStatuses API",
          Description = "A simple example ASP.NET Core Web API",
          TermsOfService = new Uri("http://localhost:5000"),
          Contact = new OpenApiContact
          {
            Name = "Abel Tran",
            Email = string.Empty,
            Url = new Uri("https://github.com/abel3t"),
          },
          License = new OpenApiLicense
          {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
          }
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

      app.UseCors();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs");
        c.RoutePrefix = "";
      });
      
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}