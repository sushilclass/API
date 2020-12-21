using Microsoft.EntityFrameworkCore;
using Outreach_FRMS_DataLayer;
using Outreach_FRMS_Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Outreach_FRMS_Utility;
using Outreach_FRMS_LogManager;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;

namespace Outreach_FRMS_BL
{
    public class UserService : IUserService
    {
        private readonly OutreachFRMSDBContext _dbContext;
        private readonly IMailService mailService;


        public UserService(OutreachFRMSDBContext dbContext, IMailService mailService)
        {
            _dbContext = dbContext;
            this.mailService = mailService;
        }

        /// <summary>
        /// Get List of all users
        /// Developer Name: Sushil Kumar
        /// Date: 24/9/2020
        /// </summary>
        /// <returns></returns>
        public List<ViewModel> GetUserList()
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetUserList));
            List<ViewModel> userList = new List<ViewModel>();
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetListSP;

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetListSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ViewModel users = new ViewModel();
                    users.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                    users.UserType = dt.Rows[i]["UserType"].ToString();
                    users.FirstName = dt.Rows[i]["FirstName"].ToString();
                    users.LastName = dt.Rows[i]["LastName"].ToString();
                    users.Password = dt.Rows[i]["Password"].ToString();
                    users.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    users.EmailId = dt.Rows[i]["EmailId"].ToString();
                    users.UserApprovalStatus = dt.Rows[i]["UserApprovalStatus"].ToString();
                    users.UserStatus = dt.Rows[i]["UserStatus"].ToString();
                    users.ReferralCode = dt.Rows[i]["ReferralCode"].ToString();

                    users.Street = dt.Rows[i]["Street"].ToString();
                    users.City = Convert.ToInt32(dt.Rows[i]["City"]);
                    users.State = Convert.ToInt32(dt.Rows[i]["State"]);
                    users.Country = Convert.ToInt32(dt.Rows[i]["Country"]);
                    users.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                    userList.Add(users);
                }

                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetListSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetUserList));
                return userList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }



        /// <summary>
        /// Get the List of users
        /// Developer Name: Sushil Kumar
        /// Date: 25/9/2020
        /// </summary>
        /// <returns></returns>
        public List<UserDetail> GetUsers()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<UserDetail> listUserDetails = new List<UserDetail>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetUsers));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetUsersMethod;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listUserDetails = JsonConvert.DeserializeObject<List<UserDetail>>(response);
                return listUserDetails;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get list of all my contact
        /// Developer Name: Sushil Kumar
        /// Date: 22/10/2020
        /// </summary>
        /// <returns></returns>
        public List<ContactModel> GetContacts()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<ContactModel> contactList = new List<ContactModel>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetContactsList));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetContactListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                contactList = JsonConvert.DeserializeObject<List<ContactModel>>(response);

                return contactList;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get list of Application Invitee
        /// Developer Name: Sushil Kumar
        /// Date: 20/10/2020
        /// </summary>
        /// <returns></returns>
        public List<ApplicationInvite> GetApplicationInvite()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<ApplicationInvite> listAppInvitee = new List<ApplicationInvite>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetApplicationInviteMethod));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetAppInviteListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listAppInvitee = JsonConvert.DeserializeObject<List<ApplicationInvite>>(response);
                return listAppInvitee;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the List of Invitee
        /// Developer Name: Sushil Kumar
        /// Date: 26/10/2020
        /// </summary>
        /// <returns></returns>
        public List<Invite> GetInviteeList()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<Invite> listInvitee = new List<Invite>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetInviteeList));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetInviteeListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listInvitee = JsonConvert.DeserializeObject<List<Invite>>(response);
                return listInvitee;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the List of My Favs
        /// Developer Name: Sushil Kumar
        /// Date: 3/11/2020
        /// </summary>
        /// <returns></returns>
        public List<MyFavs> GetMYFavsList()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<MyFavs> listMyFav = new List<MyFavs>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetMyFavsList));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetMyFavsListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listMyFav = JsonConvert.DeserializeObject<List<MyFavs>>(response);
                return listMyFav;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the list of my contact favs
        /// Developer Name: Sushil Kumar
        /// Date: 10/11/2020
        /// </summary>
        /// <returns></returns>
        public List<MyContactFavs> GetMyContactFavList()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<MyContactFavs> listMyFav = new List<MyContactFavs>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetMyContactFavList));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetMyContactFavListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listMyFav = JsonConvert.DeserializeObject<List<MyContactFavs>>(response);
                return listMyFav;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the User document list
        /// Developer Name: Sushil Kumar
        /// Date: 05/10/2020
        /// </summary>
        /// <returns></returns>
        public List<UserDocumentMapping> GetUserDocumentList()
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetUserList));
            List<UserDocumentMapping> userList = new List<UserDocumentMapping>();
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetUserDocDataSP;

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetUserDocDataSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserDocumentMapping users = new UserDocumentMapping();
                    users.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                    users.DocumentId = dt.Rows[i]["DocumentId"].ToString();
                    users.DocumentType = dt.Rows[i]["DocumentType"].ToString();
                    users.DocumentImage = dt.Rows[i]["DocumentImage"].ToString();
                    userList.Add(users);
                }

                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetListSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetUserList));
                return userList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the list of service data
        /// Developer Name: Sushil Kumar
        /// Date: 14/10/2020
        /// </summary>
        /// <returns></returns>
        public List<ServiceDataModel> GetServiceDataList()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<ServiceDataModel> dataList = new List<ServiceDataModel>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetServiceDataListMethod));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetServiceDataListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                dataList = JsonConvert.DeserializeObject<List<ServiceDataModel>>(response);

                return dataList;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Get the Business claim list
        /// Developer Name: Sushil Kumar
        /// Date: 18/11/2020
        /// </summary>
        /// <returns></returns>
        public List<BusinessClaimModel> GetBusinessClaimList()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<BusinessClaimModel> claimList = new List<BusinessClaimModel>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetClaimListMethod));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetClaimListSP;

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                claimList = JsonConvert.DeserializeObject<List<BusinessClaimModel>>(response);

                return claimList;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Validate the user with EmailId and Password
        /// Developer Name: Sushil Kumar
        /// Date: 28/9/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthenticateResponse Validate(AuthenticateRequest model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.ServiceExecuteStart.Replace("{MethodName}", Resources.Validate));
            try
            {
                CommonUtility commonUtility = new CommonUtility();
                ViewModel user = new ViewModel();
                string errormesage = string.Empty;
                var isvalid = false;
                string token = string.Empty;
                if (GetUserList().Where(x => x.EmailId == model.EmailId.ToLower()).ToList().Count > 0)
                {
                    user = GetUserList().SingleOrDefault(x => x.EmailId == model.EmailId.ToLower() && commonUtility.Decrypt(x.Password) == model.Password && x.UserApprovalStatus == "Approved");
                    if (user is null)
                    {
                        errormesage = Resources.LoginError;
                    }
                    else
                    {
                        isvalid = true; ;
                    }
                }
                else
                {
                    errormesage = Resources.UserNotExist;                   
                }
                if (isvalid)
                {
                    token = GenerateJwtToken(user);
                }
                AuthenticateResponse AuthenticateResponse = new AuthenticateResponse(user, token);
                AuthenticateResponse.Message = errormesage;
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.Validate));
                return AuthenticateResponse;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }

        }

        /// <summary>
        /// Generate JWT Token
        /// Developer Name: Sushil Kumar
        /// Date: 30/9/2020
        /// </summary>
        /// <param name="user"></param>
        /// <returns>JWT Token</returns>
        public string GenerateJwtToken(ViewModel user)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.ServiceExecuteStart.Replace("{MethodName}", Resources.GenerateJwtToken));
            try
            {
                var someSecret = Resources.SecretKey;
                List<Claim> claims = new List<Claim>() {
                    new Claim("EmailId", user.EmailId.ToString())
                };
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(someSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken SecurityToken = new JwtSecurityToken(
                    issuer: "myapi.com",
                    audience: "myapi.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials
                    );

                return jwtTokenHandler.WriteToken(SecurityToken);

            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }

        }

        /// <summary>
        /// Signup user or business user
        /// Developer Name: Sushil Kumar
        /// Date: 24/09/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SingUp(Users model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserType = " + model.UserType + ", " + "FirstName = " + model.FirstName + ", " + "LastName = " + model.LastName + ", " + "Password = " + model.Password + ", " + "MobileNo = " + model.MobileNo + ", " + "EmailId = " + model.EmailId + ", " + "UserApprovalStatus = " + model.UserApprovalStatus
                                    + ", " + "UserStatus = " + model.UserStatus + ", " + "Street = " + model.Address.Street + ", " + "City = " + model.Address.City + ", " + "State = " + model.Address.State + ", " + "Country = " + model.Address.Country + ", " + "ZipCode = " + model.Address.ZipCode + ", " + "BusinessName = " + model.Businessdetails.BusinessName
                                    + ", " + "BusinessCategory = " + model.Businessdetails.BusinessCategory + ", " + "BusinessStreet = " + model.Address.Street + ", " + "BusinessCity = " + model.Address.City + ", " + "BusinessState = " + model.Address.State + ", " + "BusinessCountry = " + model.Address.Country + ", " + "BusinessZipCode = " + model.Address.ZipCode;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.SingUp).Replace("{RequestParameter}", requestParameter));

            string response = string.Empty;
            CommonUtility commonUtility = new CommonUtility();
            IList<IFormFile> files = null;
            try
            {
                var user = GetUserList().SingleOrDefault(x => x.EmailId == model.EmailId.ToLower());
                if (user != null)
                    return response = "User Already Exist." + "/ 0";

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SignUpSP;

                DbParameter UserType = cmd.CreateParameter();
                UserType.DbType = DbType.String;
                UserType.ParameterName = "@UserType";
                UserType.Value = model.UserType;
                cmd.Parameters.Add(UserType);

                DbParameter FirstName = cmd.CreateParameter();
                FirstName.DbType = DbType.String;
                FirstName.ParameterName = "@FirstName";
                FirstName.Value = model.FirstName;
                cmd.Parameters.Add(FirstName);

                DbParameter LastName = cmd.CreateParameter();
                LastName.DbType = DbType.String;
                LastName.ParameterName = "@LastName";
                LastName.Value = model.LastName;
                cmd.Parameters.Add(LastName);

                DbParameter Password = cmd.CreateParameter();
                Password.DbType = DbType.String;
                Password.ParameterName = "@Password";
                Password.Value = commonUtility.Encrypt(model.Password);
                cmd.Parameters.Add(Password);

                DbParameter MobileNo = cmd.CreateParameter();
                MobileNo.DbType = DbType.String;
                MobileNo.ParameterName = "@MobileNo";
                MobileNo.Value = model.MobileNo;
                cmd.Parameters.Add(MobileNo);

                DbParameter EmailId = cmd.CreateParameter();
                EmailId.DbType = DbType.String;
                EmailId.ParameterName = "@EmailId";
                EmailId.Value = model.EmailId.ToLower();
                cmd.Parameters.Add(EmailId);

                DbParameter UserApprovalStatus = cmd.CreateParameter();
                UserApprovalStatus.DbType = DbType.String;
                UserApprovalStatus.ParameterName = "@UserApprovalStatus";
                UserApprovalStatus.Value = model.UserApprovalStatus;
                cmd.Parameters.Add(UserApprovalStatus);

                DbParameter UserStatus = cmd.CreateParameter();
                UserStatus.DbType = DbType.String;
                UserStatus.ParameterName = "@UserStatus";
                UserStatus.Value = model.UserStatus;
                cmd.Parameters.Add(UserStatus);

                DbParameter ReferralCode = cmd.CreateParameter();
                ReferralCode.DbType = DbType.String;
                ReferralCode.ParameterName = "@ReferralCode";
                ReferralCode.Value = model.ReferralCode;
                cmd.Parameters.Add(ReferralCode);

                DbParameter Street = cmd.CreateParameter();
                Street.DbType = DbType.String;
                Street.ParameterName = "@Street";
                Street.Value = model.Address.Street;
                cmd.Parameters.Add(Street);

                DbParameter City = cmd.CreateParameter();
                City.DbType = DbType.Int64;
                City.ParameterName = "@City";
                City.Value = model.Address.City;
                cmd.Parameters.Add(City);

                DbParameter State = cmd.CreateParameter();
                State.DbType = DbType.Int64;
                State.ParameterName = "@State";
                State.Value = model.Address.State;
                cmd.Parameters.Add(State);

                DbParameter Country = cmd.CreateParameter();
                Country.DbType = DbType.Int64;
                Country.ParameterName = "@Country";
                Country.Value = model.Address.Country;
                cmd.Parameters.Add(Country);

                DbParameter ZipCode = cmd.CreateParameter();
                ZipCode.DbType = DbType.String;
                ZipCode.ParameterName = "@ZipCode";
                ZipCode.Value = model.Address.ZipCode;
                cmd.Parameters.Add(ZipCode);

                DbParameter BusinessName = cmd.CreateParameter();
                BusinessName.DbType = DbType.String;
                BusinessName.ParameterName = "@BusinessName";
                BusinessName.Value = model.Businessdetails.BusinessName;
                cmd.Parameters.Add(BusinessName);

                DbParameter BusinessCategory = cmd.CreateParameter();
                BusinessCategory.DbType = DbType.String;
                BusinessCategory.ParameterName = "@BusinessCategory";
                BusinessCategory.Value = model.Businessdetails.BusinessCategory;
                cmd.Parameters.Add(BusinessCategory);

                DbParameter BusinessStreet = cmd.CreateParameter();
                BusinessStreet.DbType = DbType.String;
                BusinessStreet.ParameterName = "@BusinessStreet";
                BusinessStreet.Value = model.Businessdetails.Address.Street;
                cmd.Parameters.Add(BusinessStreet);

                DbParameter BusinessCity = cmd.CreateParameter();
                BusinessCity.DbType = DbType.Int64;
                BusinessCity.ParameterName = "@BusinessCity";
                BusinessCity.Value = model.Businessdetails.Address.City;
                cmd.Parameters.Add(BusinessCity);

                DbParameter BusinessState = cmd.CreateParameter();
                BusinessState.DbType = DbType.Int64;
                BusinessState.ParameterName = "@BusinessState";
                BusinessState.Value = model.Businessdetails.Address.State;
                cmd.Parameters.Add(BusinessState);

                DbParameter BusinessCountry = cmd.CreateParameter();
                BusinessCountry.DbType = DbType.Int64;
                BusinessCountry.ParameterName = "@BusinessCountry";
                BusinessCountry.Value = model.Businessdetails.Address.Country;
                cmd.Parameters.Add(BusinessCountry);

                DbParameter BusinessZipCode = cmd.CreateParameter();
                BusinessZipCode.DbType = DbType.String;
                BusinessZipCode.ParameterName = "@BusinessZipCode";
                BusinessZipCode.Value = model.Businessdetails.Address.ZipCode;
                cmd.Parameters.Add(BusinessZipCode);

                DbParameter BusinessPhoneNo = cmd.CreateParameter();
                BusinessPhoneNo.DbType = DbType.String;
                BusinessPhoneNo.ParameterName = "@BusinessPhoneNo";
                BusinessPhoneNo.Value = model.Businessdetails.BusinessPhoneNo;
                cmd.Parameters.Add(BusinessPhoneNo);

                DbParameter BusinessRegistrationNo = cmd.CreateParameter();
                BusinessRegistrationNo.DbType = DbType.String;
                BusinessRegistrationNo.ParameterName = "@BusinessRegistrationNo";
                BusinessRegistrationNo.Value = model.Businessdetails.BusinessRegistrationNo;
                cmd.Parameters.Add(BusinessRegistrationNo);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SignUpSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SignUpSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.SingUp));
                response = Resources.DataSaved + "/" + reader;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is used to upload the files
        /// Developer Name: Sushil Kumar
        /// Date: 05/10/2020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UploadImage(UserDocumentMapping obj)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.UploadFiles));
            CommonUtility commonUtility = new CommonUtility();
            string response = string.Empty;
            var user = GetUserDocumentList().SingleOrDefault(x => x.DocumentId == obj.DocumentId && x.UserId == obj.UserId);
            if (user != null)
                return response = Resources.UserDocExist;
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveUserDocSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int64;
                UserId.ParameterName = "@UserId";
                UserId.Value = obj.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter DocumentId = cmd.CreateParameter();
                DocumentId.DbType = DbType.String;
                DocumentId.ParameterName = "@DocumentId";
                DocumentId.Value = obj.DocumentId;
                cmd.Parameters.Add(DocumentId);

                DbParameter DocumentImage = cmd.CreateParameter();
                DocumentImage.DbType = DbType.String;
                DocumentImage.ParameterName = "@DocumentImage";
                DocumentImage.Value = obj.DocumentImage;
                cmd.Parameters.Add(DocumentImage);

                DbParameter DocumentType = cmd.CreateParameter();
                DocumentType.DbType = DbType.String;
                DocumentType.ParameterName = "@DocumentType";
                DocumentType.Value = obj.DocumentType;
                cmd.Parameters.Add(DocumentType);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveUserDocSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveUserDocSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.SaveUserDocSP));
                response = Resources.DocumentUploadSuccess;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// Get user or business user by EmailId
        /// Developer Name: Sushil Kumar
        /// Date: 06/10/2020
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ViewModel GetByEmailId(string email)
        {
            return GetUserList().FirstOrDefault(x => x.EmailId == email);
        }

        /// <summary>
        /// Get user details by UserId
        /// Developer Name: Sushil Kumar
        /// Date: 08/10/2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewModel GetById(int id)
        {

            return GetUserList().FirstOrDefault(x => x.UserId == id);
        }

        /// <summary>
        /// This method is use to delete the user document
        /// Developer Name: Sushil Kumar
        /// Date: 9/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string DeleteDocument(Users model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "DocumentId = " + model.UserDocumentMapping.DocumentId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.DeleteDocument).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetUserDocumentList().SingleOrDefault(x => x.DocumentId == model.UserDocumentMapping.DocumentId && x.UserId == model.UserDocumentMapping.UserId);
                if (user == null)
                    return response = Resources.UserDocumentNotExist;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.DeleteDocumentSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserDocumentMapping.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter DocumentId = cmd.CreateParameter();
                DocumentId.DbType = DbType.String;
                DocumentId.ParameterName = "@DocumentId";
                DocumentId.Value = model.UserDocumentMapping.DocumentId;
                cmd.Parameters.Add(DocumentId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.DeleteDocumentSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.DeleteDocumentSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.DeleteDocument));
                response = "Document delete Successfully.";
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to save the google api search result
        /// Developer Name: Sushil Kumar
        /// Date: 14/10/2020
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public string SaveSearchResponse(SearchAPIResponse response, int businessCategory)
        {
            string type = string.Empty;
            string typearray = string.Empty;
            string openstatus = string.Empty;
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveSearchSP;

                int resultCount = response.results.Count;

                for (int i = 0; i < resultCount; i++)
                {
                    cmd.Parameters.Clear();

                    DbParameter BusinessCategoryId = cmd.CreateParameter();
                    BusinessCategoryId.DbType = DbType.Int32;
                    BusinessCategoryId.ParameterName = "@BusinessCategoryId";
                    BusinessCategoryId.Value = businessCategory;
                    cmd.Parameters.Add(BusinessCategoryId);

                    DbParameter Name = cmd.CreateParameter();
                    Name.DbType = DbType.String;
                    Name.ParameterName = "@Name";
                    Name.Value = response.results[i].name;
                    cmd.Parameters.Add(Name);

                    string formatedaddress = response.results[i].formatted_address;
                    string[] splitAddress = formatedaddress.Split(',');

                    DbParameter address = cmd.CreateParameter();
                    address.DbType = DbType.String;
                    address.ParameterName = "@address";
                    address.Value = splitAddress[0];
                    cmd.Parameters.Add(address);

                    DbParameter city = cmd.CreateParameter();
                    city.DbType = DbType.String;
                    city.ParameterName = "@city";
                    city.Value = splitAddress[1];
                    cmd.Parameters.Add(city);

                    string statepin = splitAddress[2];
                    string[] splitstatepin = statepin.Split(' ');

                    DbParameter state = cmd.CreateParameter();
                    state.DbType = DbType.String;
                    state.ParameterName = "@state";
                    state.Value = splitstatepin[1];
                    cmd.Parameters.Add(state);

                    DbParameter zipcode = cmd.CreateParameter();
                    zipcode.DbType = DbType.String;
                    zipcode.ParameterName = "@zipcode";
                    zipcode.Value = splitstatepin[2];
                    cmd.Parameters.Add(zipcode);

                    DbParameter country = cmd.CreateParameter();
                    country.DbType = DbType.String;
                    country.ParameterName = "@country";
                    country.Value = splitAddress[3];
                    cmd.Parameters.Add(country);

                    DbParameter rating = cmd.CreateParameter();
                    rating.DbType = DbType.Decimal;
                    rating.ParameterName = "@rating";
                    rating.Value = response.results[i].rating;
                    cmd.Parameters.Add(rating);

                    DbParameter OpenNow = cmd.CreateParameter();
                    OpenNow.DbType = DbType.String;
                    OpenNow.ParameterName = "@OpenNow";
                    OpenNow.Value = openstatus;
                    cmd.Parameters.Add(OpenNow);

                    string icon = response.results[i].icon;
                    if (response.results[i].photos == null)
                    {
                        DbParameter PhotoReference = cmd.CreateParameter();
                        PhotoReference.DbType = DbType.String;
                        PhotoReference.ParameterName = "@PhotoReference";
                        PhotoReference.Value = "";
                        cmd.Parameters.Add(PhotoReference);
                    }
                    else
                    {
                        string photoreff = response.results[i].photos[0].photo_reference;
                        DbParameter PhotoReference = cmd.CreateParameter();
                        PhotoReference.DbType = DbType.String;
                        PhotoReference.ParameterName = "@PhotoReference";
                        PhotoReference.Value = photoreff;
                        cmd.Parameters.Add(PhotoReference);
                    }
                    if (response.results[i].photos == null)
                    {
                        DbParameter Link = cmd.CreateParameter();
                        Link.DbType = DbType.String;
                        Link.ParameterName = "@Link";
                        Link.Value = "";
                        cmd.Parameters.Add(Link);
                    }
                    else
                    {
                        string linkurl = response.results[i].photos[0].html_attributions[0];
                        string[] splitlink = linkurl.Split('"');
                        string link = splitlink[1];
                        DbParameter Link = cmd.CreateParameter();
                        Link.DbType = DbType.String;
                        Link.ParameterName = "@Link";
                        Link.Value = link;
                        cmd.Parameters.Add(Link);

                    }

                    int typecount = response.results[i].types.Count;
                    StringBuilder builder = new StringBuilder();
                    for (int j = 0; j < typecount; j++)
                    {
                        type = response.results[i].types[j];
                        builder.Append(type);
                        builder.Append(',');
                    }
                    typearray = builder.ToString();
                    typearray = typearray.Remove(typearray.Length - 1, 1);
                    DbParameter Types = cmd.CreateParameter();
                    Types.DbType = DbType.String;
                    Types.ParameterName = "@Types";
                    Types.Value = typearray;
                    cmd.Parameters.Add(Types);

                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteScalar();
                }
            }
            finally
            {
                this._dbContext.Database.CloseConnection();
            }

            return null;
        }

        /// <summary>
        /// This method is use for forgot the password
        /// Developer Name: Sushil Kumar
        /// Date: 13/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ForgotPasscode(Users model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "EmaiId = " + model.EmailId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.ForgotPassword).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            CommonUtility commonUtility = new CommonUtility();
            string pass = DateTime.Now.ToString("MMddmmssff"); //Guid.NewGuid().ToString();
            try
            {
                var user = GetUserList().SingleOrDefault(x => x.EmailId == model.EmailId);
                if (user == null)
                {
                    response = Resources.UserNotExist;
                    log.Append(Resources.UserNotExist);
                }
                else
                {
                    this._dbContext.Database.OpenConnection();
                    DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd.CommandText = Resources.ResetPasswordSP;

                    DbParameter EmailId = cmd.CreateParameter();
                    EmailId.DbType = DbType.String;
                    EmailId.ParameterName = "@EmailId";
                    EmailId.Value = model.EmailId.ToLower();
                    cmd.Parameters.Add(EmailId);

                    DbParameter Password = cmd.CreateParameter();
                    Password.DbType = DbType.String;
                    Password.ParameterName = "@Password";
                    Password.Value = commonUtility.Encrypt(pass); //commonUtility.Encrypt(model.Password);
                    cmd.Parameters.Add(Password);

                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteScalar();

                    string body = "<b>Your updated Password. </b><br/>" + pass;
                    mailService.SendEmailForResetPassword(model, body);

                    response = Resources.CheckEmailResetPassword;
                    log.Append(Resources.CheckEmailResetPassword);
                }

                return response;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use for reset the password
        /// Developer Name: Sushil Kumar
        /// Date: 15/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ResetPasscode(Users model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "EmaiId = " + model.EmailId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.ResetPassword).Replace("{RequestParameter}", requestParameter));
            CommonUtility commonUtility = new CommonUtility();
            var response = string.Empty;
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.ResetPasswordSP;

                DbParameter EmailId = cmd.CreateParameter();
                EmailId.DbType = DbType.String;
                EmailId.ParameterName = "@EmailId";
                EmailId.Value = model.EmailId.ToLower();
                cmd.Parameters.Add(EmailId);

                DbParameter Password = cmd.CreateParameter();
                Password.DbType = DbType.String;
                Password.ParameterName = "@Password";
                Password.Value = commonUtility.Encrypt(model.Password);
                cmd.Parameters.Add(Password);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.ResetPasswordSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.ResetPasswordSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.ResetPassword));
                response = Resources.PasswordUpdateSuccess;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get the user details
        /// Developer Name: Sushil Kumar
        /// Date: 02/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<UserDetail> GetUserDetails(Users model)
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<UserDetail> ud = new List<UserDetail>();
            ud.Add(new UserDetail
            {
                AddressDetails = new AddressDetails(),
                UserDocumentMapping = new List<UserDocumentMapping>()
            });

            string requestParameter = "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetUserDetails).Replace("{RequestParameter}", requestParameter));
            try
            {
                var user = GetUsers().SingleOrDefault(x => x.UserId == model.UserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetUserDataSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetUserDataSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetUserDataSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetUserDetails));
                response = JsonConvert.SerializeObject(dt);
                ud = JsonConvert.DeserializeObject<List<UserDetail>>(response);
                ud[0].UserDocumentMapping = new List<UserDocumentMapping>();
                for (int i = 0; i < ud.Count; i++)
                {
                    ud[0].UserDocumentMapping.Add(new UserDocumentMapping
                    {
                        UserId = dt.Rows[i].Field<int>("UserId"),
                        DocumentId = dt.Rows[i].Field<string>("DocumentId"),
                        DocumentImage = dt.Rows[i].Field<string>("DocumentImage"),
                        DocumentType = dt.Rows[i].Field<string>("DocumentType")
                    });
                    ud[0].AddressDetails = new AddressDetails()
                    {
                        Street = dt.Rows[i].Field<string>("Street"),
                        City = dt.Rows[i].Field<string>("City"),
                        State = dt.Rows[i].Field<string>("State"),
                        Country = (dt.Rows[i].Field<string>("Country")).Trim(),
                        ZipCode = dt.Rows[i].Field<string>("ZipCode")
                    };                    

                }
                int len = ud.Count - 1;
                ud.RemoveRange(1, len);

                return ud;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }

        }
        /// <summary>
        /// This method is use to get the Restaurant Data
        /// Developer Name: Sushil Kumar
        /// Date: 16/10/2020
        /// </summary>
        /// <returns></returns>
        public List<FavsSearchData> GetRestaurantList(SearchRequest searchdata)
        {
            StringBuilder log = new StringBuilder();

            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetRestaurantList));
            var responseMyfavs = string.Empty;
            var responseMyContactfavs = string.Empty;
            var responseOthers = string.Empty;
            List<FavsSearchData> myfavslist = new List<FavsSearchData>();
            List<FavsSearchData> mycontactfavslist = new List<FavsSearchData>();
            List<FavsSearchData> othersfavslist = new List<FavsSearchData>();

            List<FavsSearchData> listdata = new List<FavsSearchData>();
            // searchlist = null;
            int a = 0, myWord = 1;
            string str = searchdata.searchString;
            string param1 = null, param2 = null, param3 = null, param4 = null, param5 = null,
                   param6 = null, param7 = null, param8 = null, param9 = null, param10 = null;
            try
            {
                if (str != null)
                {
                    while (a <= str.Length - 1)
                    {
                        if (str[a] == ' ' || str[a] == '\n' || str[a] == '\t')
                        {
                            myWord++;
                        }
                        a++;
                    }
                }
                if (myWord == 1)
                {
                    param1 = str;
                }
                if (myWord == 2)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1];
                }
                if (myWord == 3)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2];
                }
                if (myWord == 4)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3];
                }
                if (myWord == 5)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4];
                }
                if (myWord == 6)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4]; param6 = strsplit[5];
                }
                if (myWord == 7)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4]; param6 = strsplit[5]; param7 = strsplit[6];
                }
                if (myWord == 8)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4]; param6 = strsplit[5]; param7 = strsplit[6]; param8 = strsplit[7];
                }
                if (myWord == 9)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4]; param6 = strsplit[5]; param7 = strsplit[6]; param8 = strsplit[7]; param9 = strsplit[8];
                }
                if (myWord == 10)
                {
                    string[] strsplit = str.Split(' ');
                    param1 = strsplit[0]; param2 = strsplit[1]; param3 = strsplit[2]; param4 = strsplit[3]; param5 = strsplit[4]; param6 = strsplit[5]; param7 = strsplit[6]; param8 = strsplit[7]; param9 = strsplit[8]; param10 = strsplit[9];
                }
                // my favs search
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetRestaurantDataMyfavsSP;

                DbParameter Type1 = cmd.CreateParameter();
                Type1.DbType = DbType.String;
                Type1.ParameterName = "@Type1";
                Type1.Value = param1;
                cmd.Parameters.Add(Type1);

                DbParameter Type2 = cmd.CreateParameter();
                Type2.DbType = DbType.String;
                Type2.ParameterName = "@Type2";
                Type2.Value = param2;
                cmd.Parameters.Add(Type2);

                DbParameter Type3 = cmd.CreateParameter();
                Type3.DbType = DbType.String;
                Type3.ParameterName = "@Type3";
                Type3.Value = param3;
                cmd.Parameters.Add(Type3);

                DbParameter Type4 = cmd.CreateParameter();
                Type4.DbType = DbType.String;
                Type4.ParameterName = "@Type4";
                Type4.Value = param4;
                cmd.Parameters.Add(Type4);

                DbParameter Type5 = cmd.CreateParameter();
                Type5.DbType = DbType.String;
                Type5.ParameterName = "@Type5";
                Type5.Value = param5;
                cmd.Parameters.Add(Type5);

                DbParameter Type6 = cmd.CreateParameter();
                Type6.DbType = DbType.String;
                Type6.ParameterName = "@Type6";
                Type6.Value = param6;
                cmd.Parameters.Add(Type6);

                DbParameter Type7 = cmd.CreateParameter();
                Type7.DbType = DbType.String;
                Type7.ParameterName = "@Type7";
                Type7.Value = param7;
                cmd.Parameters.Add(Type7);

                DbParameter Type8 = cmd.CreateParameter();
                Type8.DbType = DbType.String;
                Type8.ParameterName = "@Type8";
                Type8.Value = param8;
                cmd.Parameters.Add(Type8);

                DbParameter Type9 = cmd.CreateParameter();
                Type9.DbType = DbType.String;
                Type9.ParameterName = "@Type9";
                Type9.Value = param9;
                cmd.Parameters.Add(Type9);

                DbParameter Type10 = cmd.CreateParameter();
                Type10.DbType = DbType.String;
                Type10.ParameterName = "@Type10";
                Type10.Value = param10;
                cmd.Parameters.Add(Type10);

                DbParameter pageno = cmd.CreateParameter();
                pageno.DbType = DbType.Int32;
                pageno.ParameterName = "@pageno";
                pageno.Value = searchdata.PageNumber;
                cmd.Parameters.Add(pageno);

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = searchdata.UserId;
                cmd.Parameters.Add(UserId);

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(DBDR);
                DBDR.Close();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetRestaurantDataOthersfavsSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetRestaurantList));
                responseMyfavs = JsonConvert.SerializeObject(dt);              
                myfavslist = JsonConvert.DeserializeObject<List<FavsSearchData>>(responseMyfavs);
                listdata.AddRange(myfavslist);


                // my contact favs search
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetRestaurantDataSP));
                DbCommand cmd1 = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd1.CommandText = Resources.GetRestaurantDataMyContactfavsSP;

                DbParameter Type11 = cmd1.CreateParameter();
                Type11.DbType = DbType.String;
                Type11.ParameterName = "@Type1";
                Type11.Value = param1;
                cmd1.Parameters.Add(Type11);

                DbParameter Type12 = cmd1.CreateParameter();
                Type12.DbType = DbType.String;
                Type12.ParameterName = "@Type2";
                Type12.Value = param2;
                cmd1.Parameters.Add(Type12);

                DbParameter Type13 = cmd1.CreateParameter();
                Type13.DbType = DbType.String;
                Type13.ParameterName = "@Type3";
                Type13.Value = param3;
                cmd1.Parameters.Add(Type13);

                DbParameter Type14 = cmd1.CreateParameter();
                Type14.DbType = DbType.String;
                Type14.ParameterName = "@Type4";
                Type14.Value = param4;
                cmd1.Parameters.Add(Type14);

                DbParameter Type15 = cmd1.CreateParameter();
                Type15.DbType = DbType.String;
                Type15.ParameterName = "@Type5";
                Type15.Value = param5;
                cmd1.Parameters.Add(Type15);

                DbParameter Type16 = cmd1.CreateParameter();
                Type16.DbType = DbType.String;
                Type16.ParameterName = "@Type6";
                Type16.Value = param6;
                cmd1.Parameters.Add(Type16);

                DbParameter Type17 = cmd1.CreateParameter();
                Type17.DbType = DbType.String;
                Type17.ParameterName = "@Type7";
                Type17.Value = param7;
                cmd1.Parameters.Add(Type17);

                DbParameter Type18 = cmd1.CreateParameter();
                Type18.DbType = DbType.String;
                Type18.ParameterName = "@Type8";
                Type18.Value = param8;
                cmd1.Parameters.Add(Type18);

                DbParameter Type19 = cmd1.CreateParameter();
                Type19.DbType = DbType.String;
                Type19.ParameterName = "@Type9";
                Type19.Value = param9;
                cmd1.Parameters.Add(Type19);

                DbParameter Type110 = cmd1.CreateParameter();
                Type110.DbType = DbType.String;
                Type110.ParameterName = "@Type10";
                Type110.Value = param10;
                cmd1.Parameters.Add(Type110);

                DbParameter pageno1 = cmd1.CreateParameter();
                pageno1.DbType = DbType.Int32;
                pageno1.ParameterName = "@pageno";
                pageno1.Value = searchdata.PageNumber;
                cmd1.Parameters.Add(pageno1);

                DbParameter UserId1 = cmd1.CreateParameter();
                UserId1.DbType = DbType.Int32;
                UserId1.ParameterName = "@UserId";
                UserId1.Value = searchdata.UserId;
                cmd1.Parameters.Add(UserId1);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetMyContactFavSP));
                cmd1.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR1 = cmd1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(DBDR1);
                DBDR1.Close();
                responseMyContactfavs = JsonConvert.SerializeObject(dt1);
                mycontactfavslist = JsonConvert.DeserializeObject<List<FavsSearchData>>(responseMyContactfavs);
                listdata.AddRange(mycontactfavslist);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetMyContactFavSP));


                // others search
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetRestaurantDataSP));
                DbCommand cmd2 = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd2.CommandText = Resources.GetRestaurantDataOthersfavsSP;

                DbParameter Type21 = cmd2.CreateParameter();
                Type21.DbType = DbType.String;
                Type21.ParameterName = "@Type1";
                Type21.Value = param1;
                cmd2.Parameters.Add(Type21);

                DbParameter Type22 = cmd2.CreateParameter();
                Type22.DbType = DbType.String;
                Type22.ParameterName = "@Type2";
                Type22.Value = param2;
                cmd2.Parameters.Add(Type22);

                DbParameter Type23 = cmd2.CreateParameter();
                Type23.DbType = DbType.String;
                Type23.ParameterName = "@Type3";
                Type23.Value = param3;
                cmd2.Parameters.Add(Type23);

                DbParameter Type24 = cmd2.CreateParameter();
                Type24.DbType = DbType.String;
                Type24.ParameterName = "@Type4";
                Type24.Value = param4;
                cmd2.Parameters.Add(Type24);

                DbParameter Type25 = cmd2.CreateParameter();
                Type25.DbType = DbType.String;
                Type25.ParameterName = "@Type5";
                Type25.Value = param5;
                cmd2.Parameters.Add(Type25);

                DbParameter Type26 = cmd2.CreateParameter();
                Type26.DbType = DbType.String;
                Type26.ParameterName = "@Type6";
                Type26.Value = param6;
                cmd2.Parameters.Add(Type26);

                DbParameter Type27 = cmd2.CreateParameter();
                Type27.DbType = DbType.String;
                Type27.ParameterName = "@Type7";
                Type27.Value = param7;
                cmd2.Parameters.Add(Type27);

                DbParameter Type28 = cmd2.CreateParameter();
                Type28.DbType = DbType.String;
                Type28.ParameterName = "@Type8";
                Type28.Value = param8;
                cmd2.Parameters.Add(Type28);

                DbParameter Type29 = cmd2.CreateParameter();
                Type29.DbType = DbType.String;
                Type29.ParameterName = "@Type9";
                Type29.Value = param9;
                cmd2.Parameters.Add(Type29);

                DbParameter Type210 = cmd2.CreateParameter();
                Type210.DbType = DbType.String;
                Type210.ParameterName = "@Type10";
                Type210.Value = param10;
                cmd2.Parameters.Add(Type210);

                DbParameter pageno2 = cmd2.CreateParameter();
                pageno2.DbType = DbType.Int32;
                pageno2.ParameterName = "@pageno";
                pageno2.Value = searchdata.PageNumber;
                cmd2.Parameters.Add(pageno2);

                DbParameter UserId2 = cmd2.CreateParameter();
                UserId2.DbType = DbType.Int32;
                UserId2.ParameterName = "@UserId";
                UserId2.Value = searchdata.UserId;
                cmd2.Parameters.Add(UserId2);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetMyContactFavSP));
                cmd2.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR2 = cmd2.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Load(DBDR2);
                DBDR2.Close();
                responseMyContactfavs = JsonConvert.SerializeObject(dt2);
                othersfavslist = JsonConvert.DeserializeObject<List<FavsSearchData>>(responseMyContactfavs);
                listdata.AddRange(othersfavslist);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetMyContactFavSP));
                return listdata;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }

        }

        /// <summary>
        /// This method is use to save the contacts data
        /// Developer Name: Sushil Kumar
        /// Date: 20/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveContact(ContactsMappingModel model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "ConnectedFriendId = " + model.ConnectedFriendsId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.SaveContact).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetContacts().FirstOrDefault(x => x.UserId == model.UserId && x.ConnectedFriendsId == model.ConnectedFriendsId);
                if (user != null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveContactSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter ConnectedFriendsId = cmd.CreateParameter();
                ConnectedFriendsId.DbType = DbType.Int32;
                ConnectedFriendsId.ParameterName = "@ConnectedFriendsId";
                ConnectedFriendsId.Value = model.ConnectedFriendsId;
                cmd.Parameters.Add(ConnectedFriendsId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveContactSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveContactSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.SaveContact));
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }       

        /// <summary>
        /// This method is use to get the list of contacts
        ///  Developer Name: Sushil Kumar
        ///  Date: 26/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvitationRequest GetContactList(InvitationRequest model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.MyContactFavSave).Replace("{RequestParameter}", requestParameter));
            var responseMyContact = string.Empty;
            var responseInvitor = string.Empty;
            var responseInvitee = string.Empty;
            List<ContactsMappingModel> MyContact = new List<ContactsMappingModel>();
            List<RequestInvites> Invitor = new List<RequestInvites>();
            List<RequestInvites> Invitee = new List<RequestInvites>();
            InvitationRequest invitationRequest = new InvitationRequest();
            try
            {
                // My contacts List
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetContactDataSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetContactDataSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                responseMyContact = JsonConvert.SerializeObject(dt);


                invitationRequest.MyContact = JsonConvert.DeserializeObject<List<ContactsMappingModel>>(responseMyContact);
                invitationRequest.UserId = model.UserId;
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetContactDataSP));

                // Invitor List
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetInvitorSP));
                DbCommand cmd1 = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd1.CommandText = Resources.GetInvitorSP;

                DbParameter InvitorUserId = cmd1.CreateParameter();
                InvitorUserId.DbType = DbType.Int32;
                InvitorUserId.ParameterName = "@InvitorUserId";
                InvitorUserId.Value = model.UserId;
                cmd1.Parameters.Add(InvitorUserId);

                cmd1.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR1 = cmd1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(DBDR1);
                responseInvitor = JsonConvert.SerializeObject(dt1);
                invitationRequest.Invitor = JsonConvert.DeserializeObject<List<RequestInvites>>(responseInvitor);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetInvitorSP));
                invitationRequest.UserId = model.UserId;

                //Invitee List
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetInviteeSP));
                DbCommand cmd2 = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd2.CommandText = Resources.GetInviteeSP;

                DbParameter InviteeUserId = cmd.CreateParameter();
                InviteeUserId.DbType = DbType.Int32;
                InviteeUserId.ParameterName = "@InviteeUserId";
                InviteeUserId.Value = model.UserId;
                cmd2.Parameters.Add(InviteeUserId);

                cmd2.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR2 = cmd2.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Load(DBDR2);
                responseInvitee = JsonConvert.SerializeObject(dt2);
                invitationRequest.Invitee = JsonConvert.DeserializeObject<List<RequestInvites>>(responseInvitee);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetInviteeSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetContactsList));
                invitationRequest.UserId = model.UserId;

                return invitationRequest;
            }

            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to delete the contact details
        ///  Developer Name: Sushil Kumar
        ///  Date: 23/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string DeleteContact(ContactsMappingModel model)
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            string requestParameter = "UserId = " + model.UserId + "ConnectedFriendsId = " + model.ConnectedFriendsId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetUserDetails).Replace("{RequestParameter}", requestParameter));
            try
            {
                var user = GetContacts().FirstOrDefault(x => x.UserId == model.UserId && x.ConnectedFriendsId == model.ConnectedFriendsId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.DeleteContactSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter ConnectedFriendsId = cmd.CreateParameter();
                ConnectedFriendsId.DbType = DbType.Int32;
                ConnectedFriendsId.ParameterName = "@ConnectedFriendsId";
                ConnectedFriendsId.Value = model.ConnectedFriendsId;
                cmd.Parameters.Add(ConnectedFriendsId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.DeleteContactSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.DeleteContactSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.DeleteContact));
                response = Resources.ContactDeleteMessage;
                return response;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }

        }

        /// <summary>
        /// This method is use to update my profile
        /// Developer Name: Sushil Kumar
        /// Date: 29/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UpdateMyProfile(ViewModel model)
        {
            var response = string.Empty;
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "First Name = " + model.FirstName + ", " + "Last Name = " + model.LastName + ", " + "Mobile No = " + model.MobileNo + ", " + "EmailId = " + model.EmailId + ", " + "ReferralCode = " + model.ReferralCode + ", " +
                                      "Street = " + model.Street + ", " + "City = " + model.City + ", " + "State = " + model.State + ", " + "Country = " + model.Country + ", " + "Zipcode = " + model.ZipCode;           
            
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.MyProfileUpdate).Replace("{RequestParameter}", requestParameter));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var user = GetUserList().SingleOrDefault(x => x.UserId == model.UserId);
                if (user == null)
                    return response = Resources.UserNotExist;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.UpdateProfileSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter FirstName = cmd.CreateParameter();
                FirstName.DbType = DbType.String;
                FirstName.ParameterName = "@FirstName";
                FirstName.Value = model.FirstName;
                cmd.Parameters.Add(FirstName);

                DbParameter LastName = cmd.CreateParameter();
                LastName.DbType = DbType.String;
                LastName.ParameterName = "@LastName";
                LastName.Value = model.LastName;
                cmd.Parameters.Add(LastName);

                DbParameter MobileNo = cmd.CreateParameter();
                MobileNo.DbType = DbType.String;
                MobileNo.ParameterName = "@MobileNo";
                MobileNo.Value = model.MobileNo;
                cmd.Parameters.Add(MobileNo);

                DbParameter EmailId = cmd.CreateParameter();
                EmailId.DbType = DbType.String;
                EmailId.ParameterName = "@EmailId";
                EmailId.Value = model.EmailId.ToLower();
                cmd.Parameters.Add(EmailId);

                DbParameter ReferralCode = cmd.CreateParameter();
                ReferralCode.DbType = DbType.String;
                ReferralCode.ParameterName = "@ReferralCode";
                ReferralCode.Value = model.ReferralCode;
                cmd.Parameters.Add(ReferralCode);

                DbParameter Street = cmd.CreateParameter();
                Street.DbType = DbType.String;
                Street.ParameterName = "@Street";
                Street.Value = model.Street;
                cmd.Parameters.Add(Street);

                DbParameter City = cmd.CreateParameter();
                City.DbType = DbType.Int64;
                City.ParameterName = "@City";
                City.Value = model.City;
                cmd.Parameters.Add(City);

                DbParameter State = cmd.CreateParameter();
                State.DbType = DbType.Int64;
                State.ParameterName = "@State";
                State.Value = model.State;
                cmd.Parameters.Add(State);

                DbParameter Country = cmd.CreateParameter();
                Country.DbType = DbType.Int64;
                Country.ParameterName = "@Country";
                Country.Value = model.Country;
                cmd.Parameters.Add(Country);

                DbParameter ZipCode = cmd.CreateParameter();
                ZipCode.DbType = DbType.String;
                ZipCode.ParameterName = "@ZipCode";
                ZipCode.Value = model.ZipCode;
                cmd.Parameters.Add(ZipCode);

                DbParameter UserImage = cmd.CreateParameter();
                UserImage.DbType = DbType.String;
                UserImage.ParameterName = "@UserImage";
                UserImage.Value = model.UserImage;
                cmd.Parameters.Add(UserImage);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.UpdateProfileSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.UpdateProfileSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.MyProfileUpdate));
                response = Resources.UserProfileUpdate;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }
        
        /// <summary>
        /// This method is use to save the Invitee request
        /// Developer Name: Sushil Kumar
        /// Date: 20/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SendInvitee(Invite model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "InvitorUser Id = " + model.InvitorUserId + ", " + "InviteeUser Id = " + model.InviteeUserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.Invitee).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveInviteeSP;

                DbParameter InvitorUserId = cmd.CreateParameter();
                InvitorUserId.DbType = DbType.Int32;
                InvitorUserId.ParameterName = "@InvitorUserId";
                InvitorUserId.Value = model.InvitorUserId;
                cmd.Parameters.Add(InvitorUserId);

                DbParameter InviteeUserId = cmd.CreateParameter();
                InviteeUserId.DbType = DbType.Int32;
                InviteeUserId.ParameterName = "@InviteeUserId";
                InviteeUserId.Value = model.InviteeUserId;
                cmd.Parameters.Add(InviteeUserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveInviteeSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveInviteeSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.Invitee));
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to send the application invitation
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ApplicationInvite(ApplicationInvite model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "InvitorUserId = " + model.InvitorUserId + ", " + "FirstName = " + model.FirstName + ", " + "LastName = " + model.LastName + ", " + "ReturnToken = " + model.RefrenceToken + ", " +
                                      "MobileNumber = " + model.MobileNumber + ", " + "EmailId = " + model.EmailId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.ApplicationInviteMethod).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetUserList().SingleOrDefault(x => x.EmailId == model.EmailId);
                if (user != null)
                {
                    var invite = GetInviteeList().FirstOrDefault(x => x.InvitorUserId == model.InvitorUserId && x.InviteeUserId == user.UserId);
                    if (invite != null)
                    {
                        return null;
                    }
                    else
                    {
                        this._dbContext.Database.OpenConnection();
                        DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                        cmd.CommandText = Resources.SaveInviteeSP;

                        DbParameter InvitorUserId = cmd.CreateParameter();
                        InvitorUserId.DbType = DbType.Int32;
                        InvitorUserId.ParameterName = "@InvitorUserId";
                        InvitorUserId.Value = model.InvitorUserId;
                        cmd.Parameters.Add(InvitorUserId);

                        DbParameter InviteeUserId = cmd.CreateParameter();
                        InviteeUserId.DbType = DbType.Int32;
                        InviteeUserId.ParameterName = "@InviteeUserId";
                        InviteeUserId.Value = user.UserId;
                        cmd.Parameters.Add(InviteeUserId);

                        cmd.CommandType = CommandType.StoredProcedure;
                        var reader = cmd.ExecuteScalar();
                        response = Resources.InvitationMessage;
                    }                   
                }
                else
                {
                    var InviteApp = GetApplicationInvite().FirstOrDefault(x => x.EmailId == model.EmailId);
                    if (InviteApp != null)
                    {
                        return response = Resources.InvitationAlreadySentMessage;
                    }
                    else
                    {
                        string reffNumber = DateTime.Now.ToString("MMddmmssff");
                        this._dbContext.Database.OpenConnection();
                        DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                        cmd.CommandText = Resources.ApplicationInviteSP;

                        DbParameter InvitorUserId = cmd.CreateParameter();
                        InvitorUserId.DbType = DbType.Int32;
                        InvitorUserId.ParameterName = "@InvitorUserId";
                        InvitorUserId.Value = model.InvitorUserId;
                        cmd.Parameters.Add(InvitorUserId);

                        DbParameter FirstName = cmd.CreateParameter();
                        FirstName.DbType = DbType.String;
                        FirstName.ParameterName = "@FirstName";
                        FirstName.Value = model.FirstName;
                        cmd.Parameters.Add(FirstName);

                        DbParameter LastName = cmd.CreateParameter();
                        LastName.DbType = DbType.String;
                        LastName.ParameterName = "@LastName";
                        LastName.Value = model.LastName;
                        cmd.Parameters.Add(LastName);

                        DbParameter RefrenceToken = cmd.CreateParameter();
                        RefrenceToken.DbType = DbType.String;
                        RefrenceToken.ParameterName = "@RefrenceToken";
                        RefrenceToken.Value = reffNumber;   // Guid.NewGuid().ToString(); // model.ReturnToken;
                        cmd.Parameters.Add(RefrenceToken);

                        DbParameter MobileNumber = cmd.CreateParameter();
                        MobileNumber.DbType = DbType.String;
                        MobileNumber.ParameterName = "@MobileNumber";
                        MobileNumber.Value = model.MobileNumber;
                        cmd.Parameters.Add(MobileNumber);

                        DbParameter EmailId = cmd.CreateParameter();
                        EmailId.DbType = DbType.String;
                        EmailId.ParameterName = "@EmailId";
                        EmailId.Value = model.EmailId;
                        cmd.Parameters.Add(EmailId);

                        cmd.CommandType = CommandType.StoredProcedure;
                        var reader = cmd.ExecuteScalar();
                        response = Resources.ApplicationInviteMessage;
                     
                        string body = "<b>Your Reference Number: " + reffNumber;
                        mailService.SendEmailToInvitee(model, body);
                    }
                }
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to update the Invitee status
        /// Developer Name: Sushil Kumar
        /// Date: 28/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string StatusUpdateInvitee(Invite model)
        {
            string response = string.Empty;
            StringBuilder log = new StringBuilder();
            string requestParameter = "InviteeUser Id = " + model.InviteeUserId + ", " + "RequestStatus = " + model.RequestStatus + ", " + "IsRequestAccepted = " + model.IsRequestAccepted + ", " + "ApprovedBy = " + model.ApprovedBy;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.StatusUpdateInvitee).Replace("{RequestParameter}", requestParameter));
            try
            {
                var user = GetInviteeList().FirstOrDefault(x => x.InviteeUserId == model.InviteeUserId && x.InvitorUserId == model.InvitorUserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.UpdateInviteeStatusSP;

                DbParameter InviteeUserId = cmd.CreateParameter();
                InviteeUserId.DbType = DbType.Int32;
                InviteeUserId.ParameterName = "@InviteeUserId";
                InviteeUserId.Value = model.InviteeUserId;
                cmd.Parameters.Add(InviteeUserId);

                DbParameter InvitorUserId = cmd.CreateParameter();
                InvitorUserId.DbType = DbType.Int32;
                InvitorUserId.ParameterName = "@InvitorUserId";
                InvitorUserId.Value = model.InvitorUserId;
                cmd.Parameters.Add(InvitorUserId);

                DbParameter RequestStatus = cmd.CreateParameter();
                RequestStatus.DbType = DbType.String;
                RequestStatus.ParameterName = "@RequestStatus";
                RequestStatus.Value = model.RequestStatus;
                cmd.Parameters.Add(RequestStatus);

                DbParameter IsRequestAccepted = cmd.CreateParameter();
                IsRequestAccepted.DbType = DbType.String;
                IsRequestAccepted.ParameterName = "@IsRequestAccepted";
                IsRequestAccepted.Value = model.IsRequestAccepted;
                cmd.Parameters.Add(IsRequestAccepted);

                DbParameter ApprovedBy = cmd.CreateParameter();
                ApprovedBy.DbType = DbType.String;
                ApprovedBy.ParameterName = "@ApprovedBy";
                ApprovedBy.Value = model.ApprovedBy;
                cmd.Parameters.Add(ApprovedBy);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.UpdateInviteeStatusSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.UpdateInviteeStatusSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.StatusUpdateInvitee));

                // Add friend id in contact list for Invitor
                if (model.RequestStatus == "Accept" && model.IsRequestAccepted == "Yes")
                {
                    var invitor = GetContacts().FirstOrDefault(x => x.UserId == model.InvitorUserId && x.ConnectedFriendsId == model.InviteeUserId);
                    if (invitor != null)
                        return Resources.ContactExist;

                    // Add friend id in contact list for Invitor
                    DbCommand cmd1 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd1.CommandText = Resources.SaveContactSP;

                    DbParameter UserId = cmd1.CreateParameter();
                    UserId.DbType = DbType.Int32;
                    UserId.ParameterName = "@UserId";
                    UserId.Value = model.InvitorUserId;
                    cmd1.Parameters.Add(UserId);

                    DbParameter ConnectedFriendsId = cmd1.CreateParameter();
                    ConnectedFriendsId.DbType = DbType.Int32;
                    ConnectedFriendsId.ParameterName = "@ConnectedFriendsId";
                    ConnectedFriendsId.Value = model.InviteeUserId; //user.UserId;
                    cmd1.Parameters.Add(ConnectedFriendsId);

                    cmd1.CommandType = CommandType.StoredProcedure;
                    var invitorUpdate = cmd1.ExecuteScalar();
                }
                else
                {
                    // delete contact invitee
                    DbCommand cmd3 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd3.CommandText = Resources.DeleteContactInviteeSP;

                    DbParameter InvitorUserId1 = cmd3.CreateParameter();
                    InvitorUserId1.DbType = DbType.Int32;
                    InvitorUserId1.ParameterName = "@InvitorUserId";
                    InvitorUserId1.Value = model.InvitorUserId;
                    cmd3.Parameters.Add(InvitorUserId1);

                    DbParameter InviteeUserId1 = cmd3.CreateParameter();
                    InviteeUserId1.DbType = DbType.Int32;
                    InviteeUserId1.ParameterName = "@InviteeUserId";
                    InviteeUserId1.Value = model.InviteeUserId; //user.UserId;
                    cmd3.Parameters.Add(InviteeUserId1);

                    cmd3.CommandType = CommandType.StoredProcedure;
                    var delinvitee = cmd3.ExecuteScalar();

                }
                // Add friend id in contact list for Invitee
                if (model.RequestStatus == "Accept" && model.IsRequestAccepted == "Yes")
                {
                    var invitee = GetContacts().FirstOrDefault(x => x.UserId == model.InviteeUserId && x.ConnectedFriendsId == model.InvitorUserId);
                    if (invitee != null)
                        return Resources.ContactExist;

                    DbCommand cmd2 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd2.CommandText = Resources.SaveContactSP;

                    DbParameter UserId = cmd2.CreateParameter();
                    UserId.DbType = DbType.Int32;
                    UserId.ParameterName = "@UserId";
                    UserId.Value = model.InviteeUserId;
                    cmd2.Parameters.Add(UserId);

                    DbParameter ConnectedFriendsId = cmd2.CreateParameter();
                    ConnectedFriendsId.DbType = DbType.Int32;
                    ConnectedFriendsId.ParameterName = "@ConnectedFriendsId";
                    ConnectedFriendsId.Value = model.InvitorUserId; //user.UserId;
                    cmd2.Parameters.Add(ConnectedFriendsId);

                    cmd2.CommandType = CommandType.StoredProcedure;
                    var inviteeUpdate = cmd2.ExecuteScalar();
                }
                else
                {
                    // delete contact invitee
                    DbCommand cmd4 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd4.CommandText = Resources.DeleteContactInviteeSP;

                    DbParameter InvitorUserId1 = cmd4.CreateParameter();
                    InvitorUserId1.DbType = DbType.Int32;
                    InvitorUserId1.ParameterName = "@InvitorUserId";
                    InvitorUserId1.Value = model.InvitorUserId;
                    cmd4.Parameters.Add(InvitorUserId1);

                    DbParameter InviteeUserId1 = cmd4.CreateParameter();
                    InviteeUserId1.DbType = DbType.Int32;
                    InviteeUserId1.ParameterName = "@InviteeUserId";
                    InviteeUserId1.Value = model.InviteeUserId; //user.UserId;
                    cmd4.Parameters.Add(InviteeUserId1);

                    cmd4.CommandType = CommandType.StoredProcedure;
                    var delinvitee = cmd4.ExecuteScalar();

                }

                response = Resources.InviteeStatusUpdateMessage;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get the List of Invitee
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<InviteList> GetInviteeList(Invite model)
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<InviteList> listInvitee = new List<InviteList>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetInviteeList));
            try
            {
                var user = GetInviteeList().FirstOrDefault(x => x.InviteeUserId == model.InviteeUserId); //.SingleOrDefault(x => x.InviteeUserId == model.InviteeUserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetInviteeSP;

                DbParameter InviteeUserId = cmd.CreateParameter();
                InviteeUserId.DbType = DbType.Int32;
                InviteeUserId.ParameterName = "@InviteeUserId";
                InviteeUserId.Value = model.InviteeUserId;
                cmd.Parameters.Add(InviteeUserId);

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listInvitee = JsonConvert.DeserializeObject<List<InviteList>>(response);
                return listInvitee;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to get the Invitor list
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<InviteList> GetInvitorList(Invite model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "InvitorUser Id = " + model.InvitorUserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetInvitorList).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            List<InviteList> listInvitee = new List<InviteList>();
            try
            {
                var user = GetInviteeList().FirstOrDefault(x => x.InvitorUserId == model.InvitorUserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetInvitorSP;

                DbParameter InvitorUserId = cmd.CreateParameter();
                InvitorUserId.DbType = DbType.Int32;
                InvitorUserId.ParameterName = "@InvitorUserId";
                InvitorUserId.Value = model.InvitorUserId;
                cmd.Parameters.Add(InvitorUserId);

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetInvitorSP));
                response = JsonConvert.SerializeObject(dt);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetInvitorSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetInvitorList));
                listInvitee = JsonConvert.DeserializeObject<List<InviteList>>(response);
                return listInvitee;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to save the my favs Restaurant
        /// Developer Name: Sushil Kumar
        /// Date: 03/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string MyFavsSave(MyFavs model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "RestaurantId = " + model.RestaurantId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.MyFavsSave).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetMYFavsList().FirstOrDefault(x => x.UserId == model.UserId && x.RestaurantId == model.RestaurantId);
                if (user != null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveMyFavsSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter RestaurantId = cmd.CreateParameter();
                RestaurantId.DbType = DbType.Int32;
                RestaurantId.ParameterName = "@RestaurantId";
                RestaurantId.Value = model.RestaurantId;
                cmd.Parameters.Add(RestaurantId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveMyFavsSP));
                cmd.CommandType = CommandType.StoredProcedure;
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveMyFavsSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.MyFavsSave));
                var reader = cmd.ExecuteScalar();
                response = Resources.DataSaved;

            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method id use to get the list of my favs Restaurant
        /// Developer Name: Sushil Kumar
        /// Date: 06/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MyFavsList> GetMyFavsList(MyFavs model)
        {
            var response = string.Empty;
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetMyFavsList).Replace("{RequestParameter}", requestParameter));
            List<MyFavsList> listMyFavs = new List<MyFavsList>();
            try
            {
                var user = GetMYFavsList().FirstOrDefault(x => x.UserId == model.UserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetListMyFavsSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetListMyFavsSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listMyFavs = JsonConvert.DeserializeObject<List<MyFavsList>>(response);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetListMyFavsSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetMyFavsList));
                return listMyFavs;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to delete my fav Restaurant
        /// Developer Name: Sushil Kumar
        /// Date: 05/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string DeleteMyFav(MyFavs model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "RestaurantId = " + model.RestaurantId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.DeleteMyFav).Replace("{RequestParameter}", requestParameter));
            var response = string.Empty;
            try
            {
                var user = GetMYFavsList().FirstOrDefault(x => x.UserId == model.UserId && x.RestaurantId == model.RestaurantId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.DeleteMyFavsSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter RestaurantId = cmd.CreateParameter();
                RestaurantId.DbType = DbType.Int32;
                RestaurantId.ParameterName = "@RestaurantId";
                RestaurantId.Value = model.RestaurantId;
                cmd.Parameters.Add(RestaurantId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.DeleteMyFavsSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.DeleteMyFavsSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.DeleteMyFav));
                response = Resources.DeleteMyFavMessage;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to save my contact favs
        /// Developer Name: Sushil Kumar
        /// Date: 10/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string MyContactFavSave(MyContactFavs model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "RestaurantId = " + model.RestaurantId + ", " + "MyContactId = " + model.MyContactId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.MyContactFavSave).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetMyContactFavList().FirstOrDefault(x => x.UserId == model.UserId && x.MyContactId == model.MyContactId && x.RestaurantId == model.RestaurantId);
                if (user != null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveMyContactFavSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter MyContactId = cmd.CreateParameter();
                MyContactId.DbType = DbType.Int32;
                MyContactId.ParameterName = "@MyContactId";
                MyContactId.Value = model.MyContactId;
                cmd.Parameters.Add(MyContactId);

                DbParameter RestaurantId = cmd.CreateParameter();
                RestaurantId.DbType = DbType.Int32;
                RestaurantId.ParameterName = "@RestaurantId";
                RestaurantId.Value = model.RestaurantId;
                cmd.Parameters.Add(RestaurantId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveMyContactFavSP));
                cmd.CommandType = CommandType.StoredProcedure;
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveMyContactFavSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.MyContactFavSave));
                var reader = cmd.ExecuteScalar();
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get my contact fav list
        /// Developer Name: Sushil Kumar
        /// Date: 12/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        public List<MyFavsList> GetMyContactFavs(MyContactFavs model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.MyContactFavSave).Replace("{RequestParameter}", requestParameter));
            var response = string.Empty;
            List<MyFavsList> listMyFavs = new List<MyFavsList>();
            try
            {
                var user = GetMyContactFavList().FirstOrDefault(x => x.UserId == model.UserId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetMyContactFavSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetMyContactFavSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                response = JsonConvert.SerializeObject(dt);
                listMyFavs = JsonConvert.DeserializeObject<List<MyFavsList>>(response);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetMyContactFavSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.MyContactFavSave));
                return listMyFavs;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }

        }

        /// <summary>
        /// This method is use to save external restaurant data
        /// Developer Name: Sushil Kumar
        /// Date: 13/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveBusinessProfileData(ServiceDataModel model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "BusinessCategoryId = " + model.BusinessCategoryId + ", " + "UserId = " + model.UserId + ", " + ", " + "Name = " + model.Name + ", " +
                "Address = " + model.Address + ", " + "City = " + model.City + ", " + "State = " + model.State + ", " + "Zipcode = " + model.Zipcode + ", " + "Country = " + model.Country + ", " +
                "Rating = " + model.Rating + ", " + "OpenNow = " + model.OpenNow + ", " + "PhotoReference = " + model.PhotoReference + ", " + "Keyword = " + model.KeyWords + ", " +
                "Link = " + model.Link + ", " + "BusinessType = " + model.BusinessType;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.RestaurantDataSaveMethod).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveRestaurantDataSP;

                DbParameter BusinessCategoryId = cmd.CreateParameter();
                BusinessCategoryId.DbType = DbType.Int32;
                BusinessCategoryId.ParameterName = "@BusinessCategoryId";
                BusinessCategoryId.Value = model.BusinessCategoryId;
                cmd.Parameters.Add(BusinessCategoryId);

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter Name = cmd.CreateParameter();
                Name.DbType = DbType.String;
                Name.ParameterName = "@Name";
                Name.Value = model.Name;
                cmd.Parameters.Add(Name);

                DbParameter Address = cmd.CreateParameter();
                Address.DbType = DbType.String;
                Address.ParameterName = "@Address";
                Address.Value = model.Address;
                cmd.Parameters.Add(Address);

                DbParameter City = cmd.CreateParameter();
                City.DbType = DbType.String;
                City.ParameterName = "@City";
                City.Value = model.City;
                cmd.Parameters.Add(City);

                DbParameter State = cmd.CreateParameter();
                State.DbType = DbType.String;
                State.ParameterName = "@State";
                State.Value = model.State;
                cmd.Parameters.Add(State);

                DbParameter Zipcode = cmd.CreateParameter();
                Zipcode.DbType = DbType.String;
                Zipcode.ParameterName = "@Zipcode";
                Zipcode.Value = model.Zipcode;
                cmd.Parameters.Add(Zipcode);

                DbParameter Country = cmd.CreateParameter();
                Country.DbType = DbType.String;
                Country.ParameterName = "@Country";
                Country.Value = model.Country;
                cmd.Parameters.Add(Country);

                DbParameter Rating = cmd.CreateParameter();
                Rating.DbType = DbType.Decimal;
                Rating.ParameterName = "@Rating";
                Rating.Value = model.Rating;
                cmd.Parameters.Add(Rating);

                DbParameter OpenNow = cmd.CreateParameter();
                OpenNow.DbType = DbType.String;
                OpenNow.ParameterName = "@OpenNow";
                OpenNow.Value = model.OpenNow;
                cmd.Parameters.Add(OpenNow);

                DbParameter PhotoReference = cmd.CreateParameter();
                PhotoReference.DbType = DbType.String;
                PhotoReference.ParameterName = "@PhotoReference";
                PhotoReference.Value = model.PhotoReference;
                cmd.Parameters.Add(PhotoReference);

                DbParameter KeyWords = cmd.CreateParameter();
                KeyWords.DbType = DbType.String;
                KeyWords.ParameterName = "@KeyWords";
                KeyWords.Value = model.KeyWords;
                cmd.Parameters.Add(KeyWords);

                DbParameter Link = cmd.CreateParameter();
                Link.DbType = DbType.String;
                Link.ParameterName = "@Link";
                Link.Value = model.Link;
                cmd.Parameters.Add(Link);

                DbParameter BusinessType = cmd.CreateParameter();
                BusinessType.DbType = DbType.String;
                BusinessType.ParameterName = "@BusinessType";
                BusinessType.Value = model.BusinessType;
                cmd.Parameters.Add(BusinessType);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveRestaurantDataSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveRestaurantDataSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.RestaurantDataSaveMethod));
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to update the Business profile and Keywords
        /// Developer Name: Sushil Kumar
        /// Date: 16/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BusinessProfileUpdate(BusinessResearch model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "Name = " + model.BusinessName + ", " + "Address = " + model.BusinessAddress + ", " + "City = " + model.BusinessCity + ", " + "State = " + model.BusinessState + ", " + "Zipcode = " + model.BusinessZipcode + ", " + "Country = " + model.BusinessCountry + ", " +
                                      "Rating = " + model.BusinessRating + ", " + "KeyWords = " + model.BusinessKeywords + ", " + "Link = " + model.BusinessLink + ", " + "BusinessType = " + model.BusinessTypes + ", " + "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.BusinessProfileUpdateMethod).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var restid = GetServiceDataList().FirstOrDefault(x => x.UserId == model.UserId);
                if (restid == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.UpdateBusinessProfileSP;

                DbParameter Name = cmd.CreateParameter();
                Name.DbType = DbType.String;
                Name.ParameterName = "@Name";
                Name.Value = model.BusinessName;
                cmd.Parameters.Add(Name);

                DbParameter Address = cmd.CreateParameter();
                Address.DbType = DbType.String;
                Address.ParameterName = "@Address";
                Address.Value = model.BusinessAddress;
                cmd.Parameters.Add(Address);

                DbParameter City = cmd.CreateParameter();
                City.DbType = DbType.Int64;
                City.ParameterName = "@City";
                City.Value = model.BusinessCity;
                cmd.Parameters.Add(City);

                DbParameter State = cmd.CreateParameter();
                State.DbType = DbType.Int64;
                State.ParameterName = "@State";
                State.Value = model.BusinessState;
                cmd.Parameters.Add(State);

                DbParameter Zipcode = cmd.CreateParameter();
                Zipcode.DbType = DbType.String;
                Zipcode.ParameterName = "@Zipcode";
                Zipcode.Value = model.BusinessZipcode;
                cmd.Parameters.Add(Zipcode);

                DbParameter Country = cmd.CreateParameter();
                Country.DbType = DbType.Int64;
                Country.ParameterName = "@Country";
                Country.Value = model.BusinessCountry;
                cmd.Parameters.Add(Country);

                DbParameter Rating = cmd.CreateParameter();
                Rating.DbType = DbType.Decimal;
                Rating.ParameterName = "@Rating";
                Rating.Value = model.BusinessRating;
                cmd.Parameters.Add(Rating);

                DbParameter KeyWords = cmd.CreateParameter();
                KeyWords.DbType = DbType.String;
                KeyWords.ParameterName = "@KeyWords";
                KeyWords.Value = model.BusinessKeywords;
                cmd.Parameters.Add(KeyWords);

                DbParameter BusinessPhotoReference = cmd.CreateParameter();
                BusinessPhotoReference.DbType = DbType.String;
                BusinessPhotoReference.ParameterName = "@BusinessPhotoReference";
                BusinessPhotoReference.Value = model.BusinessPhotoReference;
                cmd.Parameters.Add(BusinessPhotoReference);

                DbParameter Link = cmd.CreateParameter();
                Link.DbType = DbType.String;
                Link.ParameterName = "@Link";
                Link.Value = model.BusinessLink;
                cmd.Parameters.Add(Link);

                DbParameter BusinessType = cmd.CreateParameter();
                BusinessType.DbType = DbType.String;
                BusinessType.ParameterName = "@BusinessTypes";
                BusinessType.Value = model.BusinessTypes;
                cmd.Parameters.Add(BusinessType);

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter RestaurantId = cmd.CreateParameter();
                RestaurantId.DbType = DbType.Int32;
                RestaurantId.ParameterName = "@RestaurantId";
                RestaurantId.Value = model.RestaurantId;
                cmd.Parameters.Add(RestaurantId);

                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                response = Resources.UserProfileUpdate;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get the business profile data based on RestaurantId
        /// Developer Name: Sushil Kumar
        /// Date: 17/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ServiceDataModel> GetBusinessProfileData(ServiceDataModel model)
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.BusinessProfileDataMethod));
            List<ServiceDataModel> listBusinessProfile = new List<ServiceDataModel>();
            try
            {
                var restid = GetServiceDataList().FirstOrDefault(x => x.UserId == model.UserId);
                if (restid == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetBusinessProfileSP;

                DbParameter RestaurantId = cmd.CreateParameter();
                RestaurantId.DbType = DbType.Int32;
                RestaurantId.ParameterName = "@UserId";
                RestaurantId.Value = model.UserId;
                cmd.Parameters.Add(RestaurantId);

                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                response = JsonConvert.SerializeObject(dt);
                listBusinessProfile = JsonConvert.DeserializeObject<List<ServiceDataModel>>(response);
                return listBusinessProfile;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to save the business claim
        /// Developer Name: Sushil Kumar
        /// Date: 17/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BusinessClaimSave(BusinessClaimModel model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "ServiceTypeId = " + model.ServiceTypeId + ", " + "BusinessId = " + model.BusinessId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.BusinessClaimSaveMethod).Replace("{RequestParameter}", requestParameter));
            string response = string.Empty;
            try
            {
                var user = GetBusinessClaimList().FirstOrDefault(x => x.UserId == model.UserId && x.ServiceTypeId == model.ServiceTypeId && x.BusinessId == model.BusinessId);
                if (user != null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveBusinessClaimSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter ServiceTypeId = cmd.CreateParameter();
                ServiceTypeId.DbType = DbType.Int32;
                ServiceTypeId.ParameterName = "@ServiceTypeId";
                ServiceTypeId.Value = model.ServiceTypeId;
                cmd.Parameters.Add(ServiceTypeId);

                DbParameter BusinessId = cmd.CreateParameter();
                BusinessId.DbType = DbType.Int32;
                BusinessId.ParameterName = "@BusinessId";
                BusinessId.Value = model.BusinessId;
                cmd.Parameters.Add(BusinessId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveBusinessClaimSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveBusinessClaimSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.BusinessClaimSaveMethod));
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get the list of business claim
        /// Developer Name: Sushil Kumar
        /// Date: 18/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ServiceDataModel> BusinessClaimGet(BusinessClaimModel model)
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            List<ServiceDataModel> listClaimBusiness = new List<ServiceDataModel>();
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.BusinessClaimGetMethod));
            try
            {
                var user = GetBusinessClaimList().FirstOrDefault(x => x.UserId == model.UserId && x.ServiceTypeId == model.ServiceTypeId);
                if (user == null)
                    return null;

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetBusinessClaimedDataSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter ServiceTypeId = cmd.CreateParameter();
                ServiceTypeId.DbType = DbType.Int32;
                ServiceTypeId.ParameterName = "@ServiceTypeId";
                ServiceTypeId.Value = model.ServiceTypeId;
                cmd.Parameters.Add(ServiceTypeId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetBusinessClaimedDataSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetBusinessClaimedDataSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.BusinessClaimGetMethod));
                response = JsonConvert.SerializeObject(dt);
                listClaimBusiness = JsonConvert.DeserializeObject<List<ServiceDataModel>>(response);
                return listClaimBusiness;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to get the list of MyFavs, MyContactsFavs and others
        /// Developer Name: Sushil Kumar
        /// Date: 23/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LeaderBoardData GetLeaderBoard(LeaderBoardData model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetLeaderBoardMethod).Replace("{RequestParameter}", requestParameter));
            var responseMyfavs = string.Empty;
            var responseMyContactfavs = string.Empty;
            var responseOthers = string.Empty;
            List<MyFavsList> myFavs = new List<MyFavsList>();
            List<MyFavsList> myContactFavs = new List<MyFavsList>();
            List<MyFavsList> others = new List<MyFavsList>();
            LeaderBoardData leaderBoardobj = new LeaderBoardData();
            try
            {
                // MyContact favs
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetMyContactFavSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetMyContactFavSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                responseMyContactfavs = JsonConvert.SerializeObject(dt);
                leaderBoardobj.myContactFavs = JsonConvert.DeserializeObject<List<MyFavsList>>(responseMyContactfavs);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetMyContactFavSP));

                // MyFavs
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetListMyFavsSP));
                cmd.CommandText = Resources.GetListMyFavsSP;
                DbDataReader DBDR1 = cmd.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(DBDR1);
                responseMyfavs = JsonConvert.SerializeObject(dt1);
                leaderBoardobj.myFavs = JsonConvert.DeserializeObject<List<MyFavsList>>(responseMyfavs);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetListMyFavsSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetLeaderBoardMethod));
                leaderBoardobj.UserId = model.UserId;

                //Others
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetOtherLeaderBoardSP));
                cmd.CommandText = Resources.GetOtherLeaderBoardSP;
                DbDataReader DBDR2 = cmd.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Load(DBDR2);
                responseOthers = JsonConvert.SerializeObject(dt2);
                leaderBoardobj.others = JsonConvert.DeserializeObject<List<MyFavsList>>(responseOthers);
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetOtherLeaderBoardSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetOtherLeaderBoard));
                leaderBoardobj.UserId = model.UserId;

                return leaderBoardobj;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to save the feedback
        /// Developer Name: Sushil Kumar
        /// Date: 25/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string FeedbackSave(Feedback model)
        {
            string response = string.Empty;
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "Contents = " + model.Contents;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.FeedbackSaveMethod).Replace("{RequestParameter}", requestParameter));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveFeedbackSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter Contents = cmd.CreateParameter();
                Contents.DbType = DbType.String;
                Contents.ParameterName = "@Contents";
                Contents.Value = model.Contents;
                cmd.Parameters.Add(Contents);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveFeedbackSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveFeedbackSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.FeedbackSaveMethod));
                response = Resources.DataSaved;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to save the Suggestion
        /// Developer Name: Sushil Kumar
        /// Date: 26/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SuggestionSave(Suggestion model)
        {
            string response = string.Empty;
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserId = " + model.UserId + ", " + "Category = " + model.Category + ", " + "SubCategory = " + model.SubCategory + ", " + "Comments = " + model.Comments;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.SuggestionSaveMethod).Replace("{RequestParameter}", requestParameter));
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.SaveSuggestionSP;

                DbParameter UserId = cmd.CreateParameter();
                UserId.DbType = DbType.Int32;
                UserId.ParameterName = "@UserId";
                UserId.Value = model.UserId;
                cmd.Parameters.Add(UserId);

                DbParameter Category = cmd.CreateParameter();
                Category.DbType = DbType.String;
                Category.ParameterName = "@Category";
                Category.Value = model.Category;
                cmd.Parameters.Add(Category);

                DbParameter SubCategory = cmd.CreateParameter();
                SubCategory.DbType = DbType.String;
                SubCategory.ParameterName = "@SubCategory";
                SubCategory.Value = model.SubCategory;
                cmd.Parameters.Add(SubCategory);

                DbParameter Comments = cmd.CreateParameter();
                Comments.DbType = DbType.String;
                Comments.ParameterName = "@Comments";
                Comments.Value = model.Comments;
                cmd.Parameters.Add(Comments);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.SaveSuggestionSP));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.SaveSuggestionSP));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.SuggestionSaveMethod));
                response = Resources.DataSaved;

            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }
        /// <summary>
        /// This method is use for SignUp with Business details
        /// Developer Name: Sushil Kumar
        /// Date: 02/12/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SingUpResturentResearch(Users model)
        {
            StringBuilder log = new StringBuilder();
            string requestParameter = "UserType = " + model.UserType + ", " + "FirstName = " + model.FirstName + ", " + "LastName = " + model.LastName + ", " + "Password = " + model.Password + ", " + "MobileNo = " + model.MobileNo + ", " + "EmailId = " + model.EmailId + ", " + "UserApprovalStatus = " + model.UserApprovalStatus
                                    + ", " + "UserStatus = " + model.UserStatus + ", " + "Street = " + model.Address.Street + ", " + "City = " + model.Address.City + ", " + "State = " + model.Address.State + ", " + "Country = " + model.Address.Country + ", " + "ZipCode = " + model.Address.ZipCode;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.UserSignResturentSearch).Replace("{RequestParameter}", requestParameter));

            string response = string.Empty;
            CommonUtility commonUtility = new CommonUtility();
            IList<IFormFile> files = null;
            try
            {
                var user = GetUserList().SingleOrDefault(x => x.EmailId == model.EmailId.ToLower());
                if (user != null)
                    return response = Resources.UserExist + "/ 0";

                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.UserSignResturentSearch;

                DbParameter UserType = cmd.CreateParameter();
                UserType.DbType = DbType.String;
                UserType.ParameterName = "@UserType";
                UserType.Value = model.UserType;
                cmd.Parameters.Add(UserType);

                DbParameter FirstName = cmd.CreateParameter();
                FirstName.DbType = DbType.String;
                FirstName.ParameterName = "@FirstName";
                FirstName.Value = model.FirstName;
                cmd.Parameters.Add(FirstName);

                DbParameter LastName = cmd.CreateParameter();
                LastName.DbType = DbType.String;
                LastName.ParameterName = "@LastName";
                LastName.Value = model.LastName;
                cmd.Parameters.Add(LastName);

                DbParameter Password = cmd.CreateParameter();
                Password.DbType = DbType.String;
                Password.ParameterName = "@Password";
                Password.Value = commonUtility.Encrypt(model.Password);
                cmd.Parameters.Add(Password);

                DbParameter MobileNo = cmd.CreateParameter();
                MobileNo.DbType = DbType.String;
                MobileNo.ParameterName = "@MobileNo";
                MobileNo.Value = model.MobileNo;
                cmd.Parameters.Add(MobileNo);

                DbParameter EmailId = cmd.CreateParameter();
                EmailId.DbType = DbType.String;
                EmailId.ParameterName = "@EmailId";
                EmailId.Value = model.EmailId.ToLower();
                cmd.Parameters.Add(EmailId);

                DbParameter UserApprovalStatus = cmd.CreateParameter();
                UserApprovalStatus.DbType = DbType.String;
                UserApprovalStatus.ParameterName = "@UserApprovalStatus";
                UserApprovalStatus.Value = model.UserApprovalStatus;
                cmd.Parameters.Add(UserApprovalStatus);

                DbParameter UserStatus = cmd.CreateParameter();
                UserStatus.DbType = DbType.String;
                UserStatus.ParameterName = "@UserStatus";
                UserStatus.Value = model.UserStatus;
                cmd.Parameters.Add(UserStatus);

                DbParameter ReferralCode = cmd.CreateParameter();
                ReferralCode.DbType = DbType.String;
                ReferralCode.ParameterName = "@ReferralCode";
                ReferralCode.Value = model.ReferralCode;
                cmd.Parameters.Add(ReferralCode);

                DbParameter Street = cmd.CreateParameter();
                Street.DbType = DbType.String;
                Street.ParameterName = "@Street";
                Street.Value = model.Address.Street;
                cmd.Parameters.Add(Street);

                DbParameter City = cmd.CreateParameter();
                City.DbType = DbType.Int64;
                City.ParameterName = "@City";
                City.Value = model.Address.City;
                cmd.Parameters.Add(City);

                DbParameter State = cmd.CreateParameter();
                State.DbType = DbType.Int64;
                State.ParameterName = "@State";
                State.Value = model.Address.State;
                cmd.Parameters.Add(State);

                DbParameter Country = cmd.CreateParameter();
                Country.DbType = DbType.Int64;
                Country.ParameterName = "@Country";
                Country.Value = model.Address.Country;
                cmd.Parameters.Add(Country);

                DbParameter ZipCode = cmd.CreateParameter();
                ZipCode.DbType = DbType.String;
                ZipCode.ParameterName = "@ZipCode";
                ZipCode.Value = model.Address.ZipCode;
                cmd.Parameters.Add(ZipCode);

                DbParameter BusinessName = cmd.CreateParameter();
                BusinessName.DbType = DbType.String;
                BusinessName.ParameterName = "@BusinessName";
                BusinessName.Value = model.BusinessResearch.BusinessName;
                cmd.Parameters.Add(BusinessName);

                DbParameter BusinessAddress = cmd.CreateParameter();
                BusinessAddress.DbType = DbType.String;
                BusinessAddress.ParameterName = "@BusinessAddress";
                BusinessAddress.Value = model.BusinessResearch.BusinessAddress;
                cmd.Parameters.Add(BusinessAddress);

                DbParameter Business = cmd.CreateParameter();
                Business.DbType = DbType.Int64;
                Business.ParameterName = "@BusinessCity";
                Business.Value = model.BusinessResearch.BusinessCity;
                cmd.Parameters.Add(Business);

                DbParameter BusinessState = cmd.CreateParameter();
                BusinessState.DbType = DbType.Int64;
                BusinessState.ParameterName = "@BusinessState";
                BusinessState.Value = model.BusinessResearch.BusinessState;
                cmd.Parameters.Add(BusinessState);

                DbParameter BusinessCountry = cmd.CreateParameter();
                BusinessCountry.DbType = DbType.Int64;
                BusinessCountry.ParameterName = "@BusinessCountry";
                BusinessCountry.Value = model.BusinessResearch.BusinessCountry;
                cmd.Parameters.Add(BusinessCountry);

                DbParameter BusinessZipCode = cmd.CreateParameter();
                BusinessZipCode.DbType = DbType.String;
                BusinessZipCode.ParameterName = "@BusinessZipCode";
                BusinessZipCode.Value = model.BusinessResearch.BusinessZipcode;
                cmd.Parameters.Add(BusinessZipCode);

                DbParameter BusinessRating = cmd.CreateParameter();
                BusinessRating.DbType = DbType.Decimal;
                BusinessRating.ParameterName = "@BusinessRating";
                BusinessRating.Value = model.BusinessResearch.BusinessRating;
                cmd.Parameters.Add(BusinessRating);

                DbParameter BusinessOpenNow = cmd.CreateParameter();
                BusinessOpenNow.DbType = DbType.String;
                BusinessOpenNow.ParameterName = "@BusinessOpenNow";
                BusinessOpenNow.Value = model.BusinessResearch.BusinessOpenNow;
                cmd.Parameters.Add(BusinessOpenNow);

                DbParameter BusinessPhotoReference = cmd.CreateParameter();
                BusinessPhotoReference.DbType = DbType.String;
                BusinessPhotoReference.ParameterName = "@BusinessPhotoReference";
                BusinessPhotoReference.Value = model.BusinessResearch.BusinessPhotoReference;
                cmd.Parameters.Add(BusinessPhotoReference);

                DbParameter BusinessTypes = cmd.CreateParameter();
                BusinessTypes.DbType = DbType.String;
                BusinessTypes.ParameterName = "@BusinessTypes";
                BusinessTypes.Value = model.BusinessResearch.BusinessTypes;
                cmd.Parameters.Add(BusinessTypes);

                DbParameter BusinessKeywords = cmd.CreateParameter();
                BusinessKeywords.DbType = DbType.String;
                BusinessKeywords.ParameterName = "@BusinessKeywords";
                BusinessKeywords.Value = model.BusinessResearch.BusinessKeywords;
                cmd.Parameters.Add(BusinessKeywords);

                DbParameter BusinessLink = cmd.CreateParameter();
                BusinessLink.DbType = DbType.String;
                BusinessLink.ParameterName = "@BusinessLink";
                BusinessLink.Value = model.BusinessResearch.BusinessLink;
                cmd.Parameters.Add(BusinessLink);

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.UserSignResturentSearch));
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = cmd.ExecuteScalar();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.UserSignResturentSearch));
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.UserSignResturentSearch));
                response = Resources.DataSaved + "/" + reader;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
            return response;
        }

        /// <summary>
        /// This method is use to get the list of country, state, city
        /// Developer Name: Sushil Kumar
        /// Date: 09/12/2020
        /// </summary>
        /// <returns></returns>
        public List<Country> GetCountry()
        {
            StringBuilder log = new StringBuilder();
            var response = string.Empty;
            log.Append(Resources.LogServiceStartMessage.Replace("{MethodName}", Resources.GetCountryMethod));
            Country CountryList = new Country();
            try
            {
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.ContryListSP;

                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.ContryListSP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.ContryListSP));
                DataTable dt = new DataTable();
                dt.Load(DBDR);

                List<Country> country = new List<Country>();

                List<Country> country1 = new List<Country>();

                Country countr = new Country();
                List<State> state = new List<State>();
                State stat = new State();

                City city = new City();                

                country = (from r in dt.AsEnumerable()

                           select new Country
                           {
                               CountryId = Convert.ToInt32(r["CountryId"].ToString()),
                               CountryName = r["CountryName"].ToString()
                           }
                           ).Distinct().ToList<Country>();


                var groupedCountryList = country.GroupBy(u => u.CountryId).Select(grp => grp.ToList()).ToList();

                for (int Countrys = 0; Countrys < groupedCountryList.Count; Countrys++)
                {
                    countr.CountryName = "USA";
                    countr.CountryId = 1;



                    state = (from r in dt.AsEnumerable()
                             where r.Field<long>("CountryId") == groupedCountryList[Countrys][Countrys].CountryId
                             select new State
                             {
                                 StateId = Convert.ToInt32(r["StateId"].ToString()),
                                 StateName = r["StateName"].ToString()
                             }).ToList();



                    var groupedSatatList = state.GroupBy(u => u.StateId).Select(grp => grp.ToList()).ToList();
                    List<State> stt = new List<State>();
                    for (int i = 0; i < groupedSatatList.Count; i++)
                    {


                        State sttt = new State();

                        sttt.StateName = groupedSatatList[i][0].StateName;
                        sttt.StateId = groupedSatatList[i][0].StateId;

                        List<City> cities = new List<City>();
                        cities = (from r in dt.AsEnumerable()
                                  where r.Field<long>("StateId") == groupedSatatList[i][0].StateId
                                  select new City
                                  {
                                      CityId = Convert.ToInt32(r["CityId"].ToString()),
                                      CityName = r["CityName"].ToString()
                                  }).ToList<City>();


                        countr.State = stt;
                        sttt.City = cities;
                        stt.Add(sttt);

                    }
                    country1.Add(countr);                  

                }                

                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetCountryMethod));
                return country1;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }

        /// <summary>
        /// This method is use to get the list of category, subcategory of business types
        /// Developer Name: Sushil Kumar
        /// Date: 11/12/2020
        /// </summary>
        /// <returns></returns>
        public List<CategorySubCategory> GetCategoryList()
        {
            StringBuilder log = new StringBuilder();
            var responseCategory = string.Empty;
            var responseSubCategory = string.Empty;
            var responseType = string.Empty;
            List<SubCategory> subcat = new List<SubCategory>();
            List<Types> type = new List<Types>();
            List<CategorySubCategory> categorySubCategorys = new List<CategorySubCategory>();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetCategoryListMethod));
            try
            {
                // Get Category
                this._dbContext.Database.OpenConnection();
                DbCommand cmd = this._dbContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Resources.GetBusinessCategorySP;
                log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetBusinessCategorySP));
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader DBDR = cmd.ExecuteReader();
                log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetBusinessCategorySP));
                DataTable dt = new DataTable();
                dt.Load(DBDR);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // for SubCategory
                    CategorySubCategory categorySubCategory = new CategorySubCategory();
                    DbCommand cmd1 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd1.CommandText = Resources.GetBusinessSubCategorySP;

                    DbParameter SubCategoryId = cmd.CreateParameter();
                    SubCategoryId.DbType = DbType.Int32;
                    SubCategoryId.ParameterName = "@CategoryId";
                    SubCategoryId.Value = Convert.ToInt32(dt.Rows[i]["CategoryId"].ToString());
                    cmd1.Parameters.Add(SubCategoryId);

                    log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetBusinessSubCategorySP));
                    cmd1.CommandType = CommandType.StoredProcedure;
                    DbDataReader DBDR1 = cmd1.ExecuteReader();
                    log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetBusinessSubCategorySP));
                    DataTable dt1 = new DataTable();
                    dt1.Load(DBDR1);
                    if (dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            SubCategory sub = new SubCategory();
                            sub.SubCategoryName = dt1.Rows[j]["SubCategoryName"].ToString();
                            sub.SubCategoryId = Convert.ToInt32(dt1.Rows[j]["SubCategoryId"].ToString());
                            subcat.Add(sub);

                        }
                        categorySubCategory.subCategory = subcat;
                    }

                    // for Types
                    DbCommand cmd2 = this._dbContext.Database.GetDbConnection().CreateCommand();
                    cmd2.CommandText = Resources.GetBusinessTypeSP;

                    DbParameter typeId = cmd.CreateParameter();
                    typeId.DbType = DbType.Int32;
                    typeId.ParameterName = "@CategoryId";
                    typeId.Value = Convert.ToInt32(dt.Rows[i]["CategoryId"].ToString());
                    cmd2.Parameters.Add(typeId);

                    log.Append(Resources.SPExecutionStart.Replace("{SPName}", Resources.GetBusinessTypeSP));
                    cmd2.CommandType = CommandType.StoredProcedure;
                    DbDataReader DBDR2 = cmd2.ExecuteReader();
                    log.Append(Resources.SPExecutionEnd.Replace("{SPName}", Resources.GetBusinessTypeSP));
                    DataTable dt2 = new DataTable();
                    dt2.Load(DBDR2);
                    if (dt1.Rows.Count > 0)
                    {
                        for (int k = 0; k < dt2.Rows.Count; k++)
                        {
                            Types types = new Types();
                            types.type = dt2.Rows[k]["Type"].ToString();
                            types.typeId = Convert.ToInt32(dt2.Rows[k]["TypeId"].ToString());
                            type.Add(types);

                        }
                        categorySubCategory.types = type;
                    }

                    categorySubCategory.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                    categorySubCategory.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"].ToString());
                    categorySubCategorys.Add(categorySubCategory);
                }
                log.Append(Resources.ServiceExecutedSuccessfully.Replace("{MethodName}", Resources.GetCategoryListMethod));
                return categorySubCategorys;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
                this._dbContext.Database.CloseConnection();
            }
        }
    }
}



