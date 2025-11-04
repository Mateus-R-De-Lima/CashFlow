using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations
{
    public static class DataBaseMigration
    {

        public async static Task MigrateDatabase(IServiceProvider service)
        {
            var dbContext = service.GetRequiredService<CashFlowDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
