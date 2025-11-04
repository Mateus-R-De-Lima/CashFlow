using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            AutoMappingRequest();
            AutoMappingResponse();
        }

        private void AutoMappingRequest()
        {
            CreateMap<RequestExpenseJson, Expense>();

            #region User
            CreateMap<RequestUserJson, User>()
                .ForMember(dest => dest.Password, config => config.Ignore());
            #endregion

        }

        private void AutoMappingResponse()
        {
            CreateMap<Expense, ResponseExpenseJson>();

            CreateMap<Expense, ResponseShortExpenseJson>();
        }
    }
}
