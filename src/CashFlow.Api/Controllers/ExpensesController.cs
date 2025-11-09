using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseExpenseJson),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromServices] IRegisterExpenseUseCase registerExpenseUseCase,
                                      [FromBody] RequestExpenseJson request)
        {
            var response = await registerExpenseUseCase.Execute(request);
            return Created(string.Empty, response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ResponsesExpenseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpenseUseCase getAllExpeseUseCase)
        {
            var response = await getAllExpeseUseCase.Execute();

            if (response.Expenses.Count != 0)
                return Ok(response);

            return NoContent();

        }


     
        [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IGetByIdExpenseUseCase getByIdExpeseUseCase,
                                                 [FromRoute] long id)
        {
            var response = await getByIdExpeseUseCase.Execute(id);

            return Ok(response);

        }


        [ProducesResponseType( StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] IDeleteExpenseUseCase deleteExpenseUseCase,
                                                [FromRoute] long id)
        {
            await deleteExpenseUseCase.Execute(id);

            return NoContent();

        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromServices] IUpdateExpenseUseCase updateExpenseUseCase,
                                               [FromRoute] long id, [FromBody] RequestExpenseJson request)
        {
            await updateExpenseUseCase.Execute(id, request);

            return NoContent();

        }
    }
}
