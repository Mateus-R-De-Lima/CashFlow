﻿using CashFlow.Application.UseCases.Reports.Excel;
using CashFlow.Application.UseCases.Reports.PDF;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel([FromServices] IGenerateExpensesReportExcelUseCase generateExpensesReportExcelUseCase,
                                                  [FromHeader] DateOnly month)
        {
            byte[] file = await generateExpensesReportExcelUseCase.Execute(month);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

            return NoContent();
        }


        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPDF([FromServices] IGenerateExpensesReportPDFUseCase expensesReportPDFUseCase,
                                                [FromQuery] DateOnly month)
        {
            byte[] file = await expensesReportPDFUseCase.Execute(month);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

            return NoContent();
        }

    }
}
