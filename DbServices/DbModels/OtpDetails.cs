using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.DbModels
{
    [Table("OtpDetails")]
    public class OtpDetails
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OTP { get; set; }
        public bool IsExpired { get; set; }
        public bool IsCurrentOtp { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
