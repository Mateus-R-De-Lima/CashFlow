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
            CreateMap<RequestRegisterExpenseJson, Expense>();
        }

        private void AutoMappingResponse()
        {
            CreateMap<Expense, ResponseExpenseJson>();

            CreateMap<Expense, ResponseShortExpenseJson>();
        }
    }
}
