using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.DbModels
{
    [Table("UserAuth")]
    public class UserAuth
    {
        public long AuthId { get; set; }
        public long UserId { get; set; }
        public bool IsLogedIn { get; set; }
        public string IsLoggedInWithOtp { get; set; }
        public DateTime LastLoggedInDateTime { get; set; }
        public string LastLoggedInLocation { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
