using AutoMapper;
using CashFlow.Application.AutoMapper;
using CashFlow.Domain.Repositories.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CommonTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            // Criar um ILoggerFactory "fake" ou real para testes
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("AutoMapper", LogLevel.Debug);
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapping>();
            }, loggerFactory);
                       

            return mapperConfig.CreateMapper();
        }
    }

}

