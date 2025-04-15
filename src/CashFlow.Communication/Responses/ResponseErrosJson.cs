using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Communication.Responses
{
    public class ResponseErrosJson
    {
        public List<string> ErrorMessages { get; set; } 

        public ResponseErrosJson(string errorMessage)
        {
            ErrorMessages = [errorMessage];
        }

        public ResponseErrosJson(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
