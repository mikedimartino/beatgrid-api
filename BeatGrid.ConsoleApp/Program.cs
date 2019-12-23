using BeatGrid.ConsoleApp.Services;
using BeatGrid.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddDefaultAWSOptions(config.GetAWSOptions())
                .AddAWSService<IAmazonDynamoDB>()
                .AddSingleton<IDynamoDBContext>(x =>
                {
                    var ddb = x.GetRequiredService<IAmazonDynamoDB>();
                    var config = new DynamoDBContextConfig
                    {
                        TableNamePrefix = "dev_"
                    };
                    return new DynamoDBContext(ddb, config);
                })
                .AddSingleton<ITestUtil, TestUtil>()
                .AddSingleton<IBeatRepository, BeatRepository>()
                .BuildServiceProvider();

            // Do work
            var utilService = serviceProvider.GetService<ITestUtil>();
            utilService.Test();
        }

    }
}
