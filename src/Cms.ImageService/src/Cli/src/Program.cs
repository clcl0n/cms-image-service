using System.Threading.Tasks;
using Cms.ImageService.Cli.Commands;
using Cms.ImageService.Infrastructure;
using Cms.Cli;
using Cms.Cli.Extensions;

namespace Cms.ImageService.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CliBuilder.CreateCliBuilder(args);

        builder.Services.AddCliInfrastructure();

        builder.Services.AddCommand<SetupDocumentDatabaseCommand>("setup-document-database");

        await CliBuilder.RunCliAsync(builder);
    }
}
