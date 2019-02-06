using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using hookup.API.Helpers;
using matcher.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace matcher.API
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
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddCors();
      services.AddScoped<IAuthRepository, AuthRepository>();
      services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)
                  ),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info
        {
          Version = "v1",
          Title = "hookup Api"
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change
        // this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();

        // sets up a global exception handler
        // writes the exception out
        app.UseExceptionHandler(builder =>
        {
          // the run method executes a differet pipeline
          builder.Run(async context =>
          {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
              context.Response.AddApplicationError(error.Error.Message);
              await context.Response.WriteAsync(error.Error.Message);
            }
          });
        });
      }


      //app.UseHttpsRedirection();
      app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      app.UseAuthentication();
      app.UseMvc();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "hookup Api");
      });
    }
  }
}
