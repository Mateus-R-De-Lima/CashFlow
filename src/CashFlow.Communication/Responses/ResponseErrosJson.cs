using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Communication.Responses
{
    public class ResponseErrosJson
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public ResponseErrosJson(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
