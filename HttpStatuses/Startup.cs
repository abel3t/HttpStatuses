using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
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
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}