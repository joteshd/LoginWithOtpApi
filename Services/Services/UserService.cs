using DbServices.DbModels;
using DbServices.Repos;
using DbServices.UnitOfWork;
using Services.Iservices;
using Services.ServiceModels;
using Services.ServiceUtilities;
using Services.SMTPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Services.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork _userRepo;
        public UserService(IUnitOfWork userRepo) {
            _userRepo = userRepo;
        }
        public UserServiceModel GetUser(string username)
        {
            UserServiceModel user = new UserServiceModel();
            DbServices.DbModels.User? userDbModel;
            userDbModel = _userRepo.userRepo.getWithCondition(new { Email = username })
                .FirstOrDefault();
            if (!(userDbModel == null)) 
            user = Mapper.Clone<DbServices.DbModels.User, UserServiceModel>(userDbModel, user);
            return user;
        }

        public UserServiceModel GetUserByEmail(string email)
        {
            UserServiceModel user = new UserServiceModel();
            DbServices.DbModels.User? userDbModel = new DbServices.DbModels.User();
            userDbModel = _userRepo.userRepo.getWithCondition(new { Email = email })
                ?.FirstOrDefault();
            if (!(userDbModel == null))
                user = Mapper.Clone<DbServices.DbModels.User, UserServiceModel>(userDbModel,user);
            return user;
        }

        public OtpDetailsServiceModel GetUserOtp(string UserId)
        {
            OtpDetailsServiceModel userOtpDetails = new OtpDetailsServiceModel();
            DbServices.DbModels.OtpDetails? userOtpDbModel = new DbServices.DbModels.OtpDetails();
            userOtpDbModel = _userRepo.userOtpDetalRepo.getWithCondition(new { UserId = UserId })
                .FirstOrDefault();
            if (!(userOtpDbModel == null))
            userOtpDetails = Mapper.Clone<DbServices.DbModels.OtpDetails, OtpDetailsServiceModel>(userOtpDbModel, userOtpDetails);
            return userOtpDetails;
        }

        public bool saveLoginDetails(UserAuthServiceModel userAuth)
        {
            bool saved = false;
            DbServices.DbModels.UserAuth userAuthDbModel = new DbServices.DbModels.UserAuth();
            userAuthDbModel = Mapper.Clone<UserAuthServiceModel, DbServices.DbModels.UserAuth>(userAuth, userAuthDbModel);
            try
            {
                saved = _userRepo.userAuthRepo.Create(userAuthDbModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return saved;
        }

        public bool SaveUserOtp(OtpDetailsServiceModel userOtp)
        {
            bool saved = false;
            DbServices.DbModels.OtpDetails userOtpDeatilsDbModel = new DbServices.DbModels.OtpDetails();
            userOtpDeatilsDbModel = Mapper.Clone<OtpDetailsServiceModel, DbServices.DbModels.OtpDetails>(userOtp, userOtpDeatilsDbModel);
            try
            {
              saved = _userRepo.userOtpDetalRepo.Create(userOtpDeatilsDbModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return saved;
        }

        public bool VerifyUsersOtp(string Otp, UserServiceModel user)
        {
            bool isValidOtp = false;
            try
            {
                IQueryable<OtpDetails> userOtpDeatilsDbModel;
                userOtpDeatilsDbModel = _userRepo.userOtpDetalRepo.getWithCondition(new { UserId = user.UserId, OTP = Otp });
                if (!(userOtpDeatilsDbModel is null) && userOtpDeatilsDbModel.Count() > 0)
                {
                   var validOtp = userOtpDeatilsDbModel.FirstOrDefault(w => w.IsCurrentOtp == true && w.IsExpired == false);
                    if (!(validOtp is null) && validOtp.OTP.Equals(Otp, StringComparison.OrdinalIgnoreCase))
                    {
                        isValidOtp = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isValidOtp;
        }
    }
}
