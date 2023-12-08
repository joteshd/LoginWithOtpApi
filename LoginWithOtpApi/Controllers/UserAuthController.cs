using LoginWithOtpApi.AuthUtility;
using Microsoft.AspNetCore.Mvc;
using Services.Iservices;
using Services.ServiceModels;
using Services.SMTPService;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace LoginWithOtpApi.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class UserAuthController : ControllerBase
    {
        IUserService _userService;
        IConfiguration _configuration;
        Services.SMTPService.IEmailService _emalService;
        public UserAuthController(IUserService userService,IConfiguration configuration, Services.SMTPService.IEmailService emalService)
        {
            _userService = userService;
            _configuration = configuration;
            _emalService = emalService;
        }
        [HttpGet(Name = "UseregisterWithUs")]
        public ActionResult ValidateUser(string emails)
            {
            var user = _userService.GetUser(emails);
            if (user == null || user.Email is null)
                return StatusCode(200,new {sucess = false,Message = "Please provode valid email"});
            else
            {
                string OneTimePassword = AuthHelper.GenerateOneTimePassword(_configuration);
                if (!string.IsNullOrEmpty(OneTimePassword))
                {
                    Services.ServiceModels.OtpDetailsServiceModel otpDetails = new Services.ServiceModels.OtpDetailsServiceModel()
                    {
                        UserId = user.UserId,
                        OTP = OneTimePassword,
                        CreatedDate = DateTime.Now,
                        IsExpired = false,
                        IsCurrentOtp = true,
                    };
                    _userService.SaveUserOtp(otpDetails);
                    List<string> sendTo = new List<string>();
                    sendTo.Add(emails);
                    string content = $"<h3> Thie one time password is valid for 1 minute</h3> </br> <h1>{OneTimePassword}</h1>";
                    Email message = new Email(sendTo, "Your One Time Password", content);
                   _emalService.SendEmail(message);
                }
               
                return StatusCode(200, new { sucess = true, Message = "Valid email" }); ;
            }
        }

        [HttpGet(Name = "ValidateUserOtp")]
        public IActionResult validateOtp(string otp, string email, string location)
        {
            var user = _userService.GetUser(email);
            bool isValidUser = user.Email.Trim().Equals(email.Trim(), StringComparison.OrdinalIgnoreCase);
            if (isValidUser)
            {
              bool isUsersOtpVerified =  _userService.VerifyUsersOtp(otp, user);

                if (isUsersOtpVerified)
                {
                    List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    string toke = AuthHelper.GetToken(authClaims,false,_configuration);

                    if (AuthHelper.validateAuthToke(toke,_configuration))
                    {
                        UserAuthServiceModel userAuth = new UserAuthServiceModel()
                        { 
                           UserId = user.UserId,
                           IsLogedIn = true,
                           CreatedDate = DateTime.UtcNow,
                           IsLoggedInWithOtp= true,
                           LastLoggedInLocation = location
                        };
                        _userService.saveLoginDetails(userAuth);
                    }
                    return Ok(new
                    {
                        Authtoken = toke,
                    });
                }
            }

            return StatusCode(401, "Please Provoide valid otp");
        }

        [HttpGet(Name = "MarkOtpEpirePostTimeOut")]
        public bool ExpireOtp(string otp, string email)
        {
            bool isOtpExpired = false;
            var user = _userService.GetUser(email);
            bool isValidUser = user.Email.Equals(otp, StringComparison.OrdinalIgnoreCase);
            if (isValidUser) 
            {
                Services.ServiceModels.OtpDetailsServiceModel otpDetails = new Services.ServiceModels.OtpDetailsServiceModel()
                {
                    UserId = user.UserId,
                    OTP = otp,
                    IsExpired = true,
                    IsCurrentOtp = false,
                };
                _userService.SaveUserOtp(otpDetails);
            }

            return isOtpExpired;
        }
    }
}
