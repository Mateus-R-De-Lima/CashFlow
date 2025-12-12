using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Domain.Security.Token;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        private CashFlow.Domain.Entities.User _user;
        public string GetEmail() => _user.Email;
        public string GetName() => _user.Name;

        public string _password;
        public string GetPassword() => _password;

        private string _token;
        public string GetToken() => _token;

        private CashFlow.Domain.Entities.Expense _expense;
        public long GetExpenseId() => _expense.Id;


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var tokenGenarator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenarator>();

                    StartDatabase(dbContext, passwordEncripter);

                    _token = tokenGenarator.Generate(_user);
                });
        }


        private void StartDatabase(CashFlowDbContext dbContext,IPasswordEncripter passwordEncripter)
        {
            AddUsers(dbContext, passwordEncripter);
            AddExpenses(dbContext, _user);

            dbContext.SaveChanges();
        }


        private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            _user = UserBuilder.Build();
            _password = _user.Password;
            _user.Password = passwordEncripter.Encrypt(_password);
            dbContext.Users.Add(_user);

        }

        private void AddExpenses(CashFlowDbContext dbContext, User user)
        {
            _expense = ExpenseBuilder.Build(user);
            dbContext.Expenses.Add(_expense);
        }

       
    }
}
