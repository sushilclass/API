using Outreach_FRMS_Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class AuthenticateResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; } 
        public LoginResponse User { get; set; }       
        /// <summary>
        /// Return the Authentication Response with Data and token to Login API
        /// </summary>
        /// <param name="_viewModel"></param>
        /// <param name="token"></param>
        public AuthenticateResponse(ViewModel _viewModel, string token)
        {

            StatusCode = (int)HttpStatusCode.OK;
            Message = Resources.LoginResponseMessage; 
            Token = token;
            if (_viewModel != null)
            {
                User = new LoginResponse
                {
                    UserType = _viewModel.UserType,
                    UserId = _viewModel.UserId,
                    FirstName = _viewModel.FirstName,
                    LastName = _viewModel.LastName,
                    EmailId = _viewModel.EmailId,
                    MobileNo = _viewModel.MobileNo,
                    ProfileImage = "",
                    ReferralCode = _viewModel.ReferralCode,
                    Address = new Address()
                    {
                        Street = _viewModel.Street,
                        City = _viewModel.City,
                        State = _viewModel.State,
                        Country = _viewModel.Country,
                        ZipCode = _viewModel.ZipCode
                    },
                    //UserDocumentMapping = new UserDocumentMapping()
                    //{
                    //    DocumentId = _viewModel.DocumentId,
                    //    UserId = _viewModel.UserId,
                    //    DocumentType = _viewModel.DocumentType,
                    //    DocumentImage = _viewModel.DocumentImage
                    //},
                    Businessdetails = new BusinessDetails()
                    {
                        BusinessName = _viewModel.BusinessName,
                        BusinessCategory = _viewModel.BusinessCategory,
                        BusinessPhoneNo = _viewModel.BusinessPhoneNo,
                        BusinessRegistrationNo = _viewModel.BusinessRegistrationNo,
                        Address = new Address()
                        {
                            Street = _viewModel.BusinessStreet,
                            City = _viewModel.BusinessCity,
                            State = _viewModel.BusinessState,
                            Country = _viewModel.BusinessCountry,
                            ZipCode = _viewModel.BusinessZipCode
                        }
                    }
                };
            }

        }
    }
}
