using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Domain.Security.Token;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Expense { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Team_Admin { get; private set; } = default!;
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
                    var accessTokenGenarator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenarator>();


                    StartDatabase(dbContext, passwordEncripter, accessTokenGenarator);


                });
        }


        private void StartDatabase(CashFlowDbContext dbContext,
                                   IPasswordEncripter passwordEncripter,
                                   IAccessTokenGenarator accessTokenGenarator)
        {
           var user = AddUsersTeamMember(dbContext, passwordEncripter, accessTokenGenarator);
            AddExpenses(dbContext, user);

            dbContext.SaveChanges();
        }


        private User AddUsersTeamMember(CashFlowDbContext dbContext,
                              IPasswordEncripter passwordEncripter,
                              IAccessTokenGenarator accessTokenGenarator)
        {
            var user = UserBuilder.Build();
            var password = user.Password;
            user.Password = passwordEncripter.Encrypt(password);
            dbContext.Users.Add(user);

            var token = accessTokenGenarator.Generate(user);
            User_Team_Member = new UserIdentityManager(user, password, token);

            return user;
        }

        private void AddExpenses(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);
            dbContext.Expenses.Add(expense);

            Expense = new ExpenseIdentityManager(expense);
        }


    }
}
