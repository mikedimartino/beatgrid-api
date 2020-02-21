using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using AutoMapper;
using BeatGrid.Application.Cognito;
using BeatGrid.Application.Map;
using BeatGrid.Application.Services;
using BeatGrid.Application.Validators;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BeatGrid.Rest
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }); ;

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeatGrid API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(AutoMapperProfile));

            // AWS
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddSingleton<IDynamoDBContext>(x =>
            {
                var ddb = x.GetRequiredService<IAmazonDynamoDB>();
                var config = new DynamoDBContextConfig
                {
                    TableNamePrefix = _environment.IsDevelopment() ? "dev_" : ""
                };
                return new DynamoDBContext(ddb, config);
            });
            services.AddAWSService<IAmazonCognitoIdentityProvider>();
            services.AddAWSService<IAmazonS3>();

            // Configure JWT Authentication
            // http://snevsky.com/blog/dotnet-core-authentication-aws-cognito
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = CognitoHelper.ClientId;
                    options.Authority = $"https://cognito-idp.{CognitoHelper.Region}.amazonaws.com/{CognitoHelper.PoolId}";
                });


            // Custom
            services.AddSingleton<IBeatService, BeatService>();
            services.AddSingleton<IBeatRepository, BeatRepository>();
            services.AddSingleton<IPublicBeatRepository, PublicBeatRepository>();
            services.AddSingleton<ISoundService, SoundService>();
            services.AddSingleton<ISoundRepository, SoundRepository>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ICognitoHelper, CognitoHelper>();
            services.AddSingleton<IValidator<Beat>, BeatValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeatGrid API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
