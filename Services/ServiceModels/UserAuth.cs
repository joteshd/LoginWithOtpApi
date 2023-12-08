using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceModels
{
    public class UserAuthServiceModel
    {
        public long AuthId { get; set; }
        public long UserId { get; set; }
        public bool IsLogedIn { get; set; }
        public bool IsLoggedInWithOtp { get; set; }
        public DateTime LastLoggedInDateTime { get; set; }
        public string LastLoggedInLocation { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
