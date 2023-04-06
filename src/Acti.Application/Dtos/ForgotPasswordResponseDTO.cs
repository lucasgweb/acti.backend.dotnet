using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acti.Application.Dtos
{
    public class ForgotPasswordResponseDTO
    {
        public string Message { get; set; }

        public ForgotPasswordResponseDTO(string message)
        {
            Message = message;
        }
    }
}
