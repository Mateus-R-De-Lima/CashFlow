using CashFlow.Application.AutoMapper;
using CashFlow.Application.Services.LoggerUser;
using CashFlow.Application.UseCases.DoLogin;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Reports.Excel;
using CashFlow.Application.UseCases.Reports.PDF;
using CashFlow.Application.UseCases.User.GetProfile;
using CashFlow.Application.UseCases.User.Register;
using CashFlow.Application.UseCases.User.UpdateProfile;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddAplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }


        public static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapping>();
            });
        }

        public static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();

            services.AddScoped<IGetAllExpenseUseCase, GetAllExpenseUseCase>();

            services.AddScoped<IGetByIdExpenseUseCase, GetByIdExpenseUseCase>();

            services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();

            services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();


            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

            services.AddScoped<ILoggerUser, LoggerUser>();

            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();

            services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();

            services.AddScoped<IGenerateExpensesReportPDFUseCase, GenerateExpensesReportPDFUseCase>();

        }
    }
}
