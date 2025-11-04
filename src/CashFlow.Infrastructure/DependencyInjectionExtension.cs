using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {           
            AddDbContext(services,configuration);
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Service Expenses
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();

            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();

            services.AddScoped<IExpenseUpdateOnlyRepository, ExpensesRepository>();
            #endregion

            #region Service User
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddScoped<IPasswordEncripter, Security.BCrypt>();
            #endregion

        }
        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");

            var version = new Version(8, 0, 43);
            var serverVersion = new MySqlServerVersion(version);
            
            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
