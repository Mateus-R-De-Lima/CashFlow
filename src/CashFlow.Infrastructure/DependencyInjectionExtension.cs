using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Domain.Security.Token;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {           
          
            AddToken(services, configuration);
            AddRepositories(services);

            if (!configuration.IsTestEnvironment())
                AddDbContext(services, configuration);
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

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenarator>(configuration => new JwtTokenGenerator(expirationTimeMinutes,signingKey!));
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
