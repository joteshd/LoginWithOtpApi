using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SMTPService
{
    public interface IEmailService
    {
        void SendEmail(Email message);
    }
}
