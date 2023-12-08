using Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Iservices
{
    public interface IUserService
    {
        UserServiceModel GetUser(string username);

        UserServiceModel GetUserByEmail(string email);

        OtpDetailsServiceModel GetUserOtp(string UserId);
      
        bool VerifyUsersOtp(string Otp, UserServiceModel user);
      
        bool SaveUserOtp(OtpDetailsServiceModel userOtp);

        bool saveLoginDetails(UserAuthServiceModel userAuth);
    }
}
