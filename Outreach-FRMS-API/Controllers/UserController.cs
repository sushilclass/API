using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Outreach_FRMS_BL;
using Outreach_FRMS_LogManager;
using Outreach_FRMS_Model;
using Outreach_FRMS_Utility;

namespace Outreach_FRMS_API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService mailService;

        public UserController(IUserService userService, LogManagers logger, IMailService mailService)
        {
            _userService = userService;
            this.mailService = mailService;
        }
       
        /// <summary>
        /// Create API for User Signup
        /// Developer Name: Sushil Kumar
        /// Date: 24/9/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UserSignUp")]
        public IActionResult UserSignUp(Users model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.UserSignUp));
            // ResponseModel responseModel = new ResponseModel();
            dynamic responseModel = new ExpandoObject();
            try
            {
                if (ModelState.IsValid)
                {
                    var responseAPI = _userService.SingUpResturentResearch(model);
                    string[] r1 = responseAPI.Split("/");
                    var response = r1[0];
                    int UserId = Convert.ToInt32(r1[1]);  // var UserId = r1[1]; 

                    if (model.UserType == "2" && response == Resources.DataSaved)
                    {
                        mailService.SendEmailToUserAsync(model);
                        mailService.SendEmailToAdminAsync(model);
                    }
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if (response == Resources.UserExist)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(response);
                        return BadRequest(responseModel);
                    }
                    else if (response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.UserId = UserId;
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserSignUp));
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);

        }

        /// <summary>
        /// This API used to Upload multiple files
        /// Developer Name: Sushil Kumar
        /// Date: 05/10/2020
        /// </summary>
        /// <param name="files"></param>
        /// <returns>User Id</returns>        
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.UploadFiles));
            CommonUtility commonUtility = new CommonUtility();
            ResponseModel responseModel = new ResponseModel();
            var response = "";
            try
            {
                for(int i=0; i<files.Count; i++) 
                {
                    if (files != null)
                    {
                        string docId = HttpContext.Request.Form["DocumentId"+i];
                        string uid = HttpContext.Request.Form["UserId"];
                        string docType = HttpContext.Request.Form["DocumentType" + i];
                        //Getting FileName
                        var fileName = Path.GetFileName(files[i].FileName);
                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        var objfiles = new UserDocumentMapping()
                        {
                            DocumentId = docId,
                            UserId = Convert.ToInt32(uid),
                            DocumentType = docType
                        };

                        using (var target = new MemoryStream())
                        {
                            byte[] p1 = null;
                            files[i].CopyTo(target);
                            p1 = target.ToArray();
                            objfiles.DocumentImage = Convert.ToBase64String(p1, Base64FormattingOptions.None);
                            response = _userService.UploadImage(objfiles);
                        }
                    }
                }
                if(response == Resources.DocumentUploadSuccess)
                {
                    log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UploadFiles));
                    responseModel.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = response;
                    return Ok(responseModel);
                }
                else if(response == Resources.UserDocExist)
                {
                    log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UploadFiles));
                    responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Message = response;
                    return Ok(responseModel);
                }
                else
                {
                    log.Append(response);
                    responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Message = response;
                    return Ok(responseModel);
                }
               
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }            
          
        }


        /// <summary>
        /// Validate the user by Email and Password
        /// Developer Name: Sushil Kumar
        /// Date: 28/9/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Return data with JWT Token</returns>
        [HttpPost("UserValidate")]
        public IActionResult UserValidate(AuthenticateRequest model)
        {
            ResponseModel responseModel = new ResponseModel();
            StringBuilder log = new StringBuilder();
            var authenticateResponse = "";           
            var authenticateResponse1 = "";
            string requestParameter = "Email = " + model.EmailId + ", " + "Password = " + model.Password;
            log.Append(Resources.LoginStartMessage.Replace("{MethodName}", Resources.UserValidate).Replace("{RequestParameter}", requestParameter));
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.Validate(model);                    
                    if (!string.IsNullOrWhiteSpace(response.Message))
                    {
                        log.Append(Resources.LoginError);
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = response.Message;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserValidate));                        
                        return Ok(response);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel); 
            
        }

        /// <summary>
        /// This API is use to delete the user document
        /// Developer Name: Sushil Kumar
        /// Date: 9/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UserDeleteDocument")]
        public IActionResult UserDeleteDocument(Users model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.UserDeleteDocument));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.DeleteDocument(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DeleteDocumentResponse)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserDeleteDocument));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else if(response == Resources.DocumentNotExist)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserDeleteDocument));
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        log.Append(responseModel.Message);
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is used to search the near by Restaurant data
        /// Developer Name: Sushil Kumar
        /// Date: 14/10/2020
        /// </summary>
        /// <param name="request"></param>
        /// <returns>List of search result</returns>
        [HttpPost("SearchAPI")]
        public async Task <IActionResult> SearchAPI(SearchAPIRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            SearchAPIResponse datalist = new SearchAPIResponse();
            string apiResponse = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
#pragma warning disable CA2234 // Pass system uri objects instead of strings
                        using (var response = await httpClient.GetAsync("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + request.QuerySearch + "&key=" + request.Key + "&pagetoken=" + request.PageToken))
#pragma warning restore CA2234 // Pass system uri objects instead of strings
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                            datalist = JsonConvert.DeserializeObject<SearchAPIResponse>(apiResponse);
                            _userService.SaveSearchResponse(datalist,request.BusinessCategory);
                        }
                    }
                    responseModel.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = "";
                    responseModel.Data = datalist;
                    return Ok(responseModel);
                }
                catch(Exception ex)
                {
                    responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Message = Resources.ErrorMessage;
                    responseModel.Description = ex.Message;
                    responseModel.Data = null;
                    return BadRequest(responseModel);
                }
                finally
                {

                }
            }
            return BadRequest();
        }

        /// <summary>
        /// This API is use for forgot password functionality
        /// Developer Name: Sushil Kumar
        /// Date: 13/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(Users model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.ForgotPassword));
            ResponseModel responseModel = new ResponseModel();
            var path = Request.HttpContext;
            var scheme = Request.Scheme; //https
            var hostvalue = Request.Host.Value; //localhost:44365
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.ForgotPasscode(model);
                    if(response == Resources.CheckEmailResetPassword)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.ForgotPassword));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else if (response == Resources.UserNotExist)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.ForgotPassword));
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }                  
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to reset the user password
        /// Developer Name: Sushil Kumar
        /// Date: 13/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(Users model)
        {
            var response = string.Empty;
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.ResetPassword));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _userService.ResetPasscode(model);
                    if(response == Resources.PasswordUpdateSuccess)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.ResetPassword));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is used to get the details of individual user
        /// Developer Name: Sushil Kumar
        /// Date: 02/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User Information</returns>
        [HttpPost("GetUserDetails")]
        public IActionResult GetUserDetails(Users model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetUserDetails));
            var response = new List<UserDetail>();          
            dynamic responseModel = new ExpandoObject();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _userService.GetUserDetails(model);
                    if(response == null )
                    {
                        responseModel.statusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.message = Resources.UserNotExist;
                        responseModel.data = null;
                        log.Append(responseModel.message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.statusCode = (int)HttpStatusCode.OK;
                        responseModel.message = null;
                        responseModel.user = response;
                        log.Append(responseModel.statusCode);
                        return Ok(responseModel);
                    }
                  
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.statusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.message = Resources.ErrorMessage;
                responseModel.description = ex.Message;
                responseModel.data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the Restaurant List
        /// Developer Name: Sushil Kumar
        /// Date: 16/10/2020
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetRestaurantData")]
        public IActionResult GetRestaurantData(SearchRequest searchdata)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetRestaurantData));
            ResponseModel responseModel = new ResponseModel();           
            try
            {
                if (ModelState.IsValid)
                {
                   var response = _userService.GetRestaurantList(searchdata);
                    if(response == null)
                    {                        
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.GetRestaurantData));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        return Ok(responseModel);
                    }                    
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save the contact data
        /// Developer Name: Sushil Kumar
        /// Date: 20/10/2020
        /// </summary>
        /// <param name="contactsMapping"></param>
        /// <returns></returns>
        [HttpPost("SaveContact")]
        public IActionResult SaveContact(ContactsMappingModel contactsMapping)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.SaveContact));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.SaveContact(contactsMapping);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ContactAlreadySaved;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.SaveContact));
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the contact list
        /// Developer Name: Sushil Kumar
        /// Date: 26/10/2020
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetContactsList")]
        public IActionResult GetContactsList(InvitationRequest model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetContactsList));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                   var response = _userService.GetContactList(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserNotSaveContactMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to delete the contact details
        /// Developer Name: Sushil Kumar
        /// Date: 23/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("DeleteContact")]
        public IActionResult DeleteContact(ContactsMappingModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.DeleteContact));
            var response = string.Empty;
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _userService.DeleteContact(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserNotSaveContactMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.ContactDeleteMessage)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                log.Append(responseModel.Description);
                return BadRequest(responseModel);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel); 
        }

        /// <summary>
        /// This API is use to update my profile based on UserId
        /// Developer Name: Sushil Kumar
        /// Date: 29/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("MyProfileUpdate")]
        public IActionResult MyProfileUpdate(ViewModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.MyProfileUpdate));
            var response = string.Empty;
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _userService.UpdateMyProfile(model);
                    if(response == Resources.UserNotExist)
                    {                        
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.UserProfileUpdate)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
                log.Append(responseModel.Description);
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }
       

        /// <summary>
        /// This API is use to save the invitee request
        /// Developer Name: Sushil Kumar
        /// Date: 20/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Invitee")]
        public IActionResult Invitee(Invite model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.Invitee));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.SendInvitee(model);
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if (response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to send the application invitee
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <returns></returns>
        [HttpPost("ApplicationInvitee")]
        public IActionResult ApplicationInvitee(ApplicationInvite model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.ApplicationInviteeAPI));
            ResponseModel responseModel = new ResponseModel();
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.ApplicationInvite(model);
                    if(response == null)
                    {
                        // send email to invitee                       
                        mailService.SendEmailToInvitee(model, message);
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.UserExist;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.ApplicationInviteMessage || response == Resources.InvitationMessage)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else if(response == Resources.InvitationAlreadySentMessage)
                    {
                        // send email to invitee
                        mailService.SendEmailToInvitee(model, message);
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }                    
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }

                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to update the Invitee status
        /// Developer Name: Sushil Kumar
        /// Date: 28/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateInviteeStatus")]
        public IActionResult UpdateInviteeStatus(Invite model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.Invitee));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.StatusUpdateInvitee(model);
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.InvotorOrInviteeNotExistMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if (response == Resources.InviteeStatusUpdateMessage)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else if(response == Resources.ContactExist)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the List of Invitee
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("InviteeList")]
        public IActionResult InviteeList(Invite model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.InviteeList));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetInviteeList(model);
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.InviteeDoesNotExistMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }

                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the Invitor list
        /// Developer Name: Sushil Kumar
        /// Date: 21/10/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("InvitorList")]
        public IActionResult InvitorList(Invite model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetInviteeList));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetInvitorList(model);
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.InvitorDoesNotExistMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save my Favs Restaurant
        /// Developer Name: Sushil Kumar
        /// Date: 03/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveMyFavs")]
        public IActionResult SaveMyFavs(MyFavs model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.MyFavsSave));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.MyFavsSave(model);
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserAlreadyFavRestrRestaurant;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }
                    else
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }

                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get My Fav Restaurant List
        /// Developer Name: Sushil Kumar
        /// Date: 06/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("MyFavList")]      
        public IActionResult MyFavList(MyFavs model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetMyFavsList));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetMyFavsList(model);
                    if (response == null)
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserNotExist;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserDeleteDocument));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to delete my fav Restaurant
        /// Developer Name: Sushil Kumar
        /// Date: 05/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("DeleteMyFav")]
        public IActionResult DeleteMyFav(MyFavs model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.DeleteMyFav));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.DeleteMyFav(model);
                    if (response == null)
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserIdOrRestraIdNotExist;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DeleteMyFavMessage)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserDeleteDocument));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;                        
                    }

                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);

        }

        /// <summary>
        /// This API is use to save my contact favs
        /// Developer Name: Sushil Kumar
        /// Date: 10/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveMyContactFavs")]
        public IActionResult SaveMyContactFavs(MyContactFavs model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.MyContactFavSave));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.MyContactFavSave(model);
                    if (response == null)
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.RestraurantAlreadyFavsByYourContactId;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else if (response == Resources.DataSaved)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserSignUp));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    } 
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get my contact fav list
        /// Developer Name: Sushil Kumar
        /// Date: 12/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetMyContactFavList")]
        public IActionResult GetMyContactFavList(MyContactFavs model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetMyContactFavList));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetMyContactFavs(model);
                    if (response == null)
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = Resources.UserNotExist;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.GetMyContactFavList));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save the Save BusinessProfile
        /// Developer Name: Sushil Kumar
        /// Date: 13/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveBusinessProfile")]
        public IActionResult SaveBusinessProfile(ServiceDataModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.SaveRestaurantAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.SaveBusinessProfileData(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to update the Business profile and keywords
        /// Developer Name: Sushil Kumar
        /// Date: 16/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateBusinessProfile")]
        public IActionResult UpdateBusinessProfile(BusinessResearch model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.UpdateBusinessProfileAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.BusinessProfileUpdate(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.RestaurantNotExist;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.UserProfileUpdate)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        log.Append(responseModel.Message);
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the Business profile based on RestaurantId
        /// Developer Name: Sushil Kumar
        /// Date: 17/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetBusinessProfile")]
        public IActionResult GetBusinessProfile(ServiceDataModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetBusinessProfileAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetBusinessProfileData(model);
                    {
                        if(response == null)
                        {
                            responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                            responseModel.Message = Resources.RestaurantNotExist;
                            responseModel.Data = null;
                            log.Append(responseModel.Message);
                            return BadRequest(responseModel);
                        }
                        else
                        {
                            responseModel.StatusCode = (int)HttpStatusCode.OK;
                            responseModel.Message = null;
                            responseModel.Data = response;
                            log.Append(responseModel.Message);
                            return Ok(responseModel);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save the business claim
        /// Developer Name: Sushil Kumar
        /// Date: 18/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveClaimBusiness")]
        public IActionResult SaveClaimBusiness(BusinessClaimModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.SaveClaimBusinessAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.BusinessClaimSave(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.UserClaimedBusiness;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    } 
                    else if(response == Resources.DataSaved)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the list of claimed Business
        /// Developer Name: Sushil Kumar
        ///  Date: 19/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetBusinessClaim")]
        public IActionResult GetBusinessClaim(BusinessClaimModel model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetBusinessClaimAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.BusinessClaimGet(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.UserNotClaimBusiness;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.GetBusinessClaimAPI));
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get the list of MyFavs, MyContactsFavs and others
        /// Developer Name: Sushil Kumar
        /// Date: 23/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LeaderBoard")]
        public IActionResult LeaderBoard(LeaderBoardData model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.LeaderBoardAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetLeaderBoard(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save the User/BusinessOwner feedback
        /// Developer Name: Sushil Kumar
        /// Date: 25/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveFeedback")]
        public IActionResult SaveFeedback(Feedback model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.SaveFeedbackAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.FeedbackSave(model);
                    if(response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DataSaved)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.UserSignUp));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to save the User/BusinessOwner Suggestion
        /// Developer Name: Sushil Kumar
        /// Date: 26/11/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveSuggestion")]
        public IActionResult SaveSuggestion(Suggestion model)
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.SaveSuggestionAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.SuggestionSave(model);
                    if(response == null)
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Data = null;
                        return BadRequest(responseModel);
                    }
                    else if(response == Resources.DataSaved)
                    {
                        log.Append(Resources.ExecutedSuccessfully.Replace("{MethodName}", Resources.SaveSuggestionAPI));
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = response;
                        responseModel.Data = null;
                        return Ok(responseModel);
                    }
                    else
                    {
                        log.Append(responseModel.Message);
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = Resources.ErrorMessage;
                        responseModel.Description = null;
                        responseModel.Data = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get country, state and city list
        /// Developer Name: Sushil Kumar
        /// Date: 09/12/2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetCountryList")]
        public ActionResult GetCountryList()
        {
            ResponseModel responseModel = new ResponseModel();
            StringBuilder log = new StringBuilder();
            try
            {                
                log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.GetCountryListAPI));               
                List<Country> country = new List<Country>();
                country = _userService.GetCountry();

                if (country == null)
                {
                    responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Message = null;
                    responseModel.Data = null;
                    log.Append(responseModel.Message);
                    return BadRequest(responseModel);
                }
                else
                {
                    responseModel.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = null;
                    responseModel.Data = country;
                    log.Append(responseModel.StatusCode);
                    return Ok(responseModel);
                }
            }
            catch (Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

        /// <summary>
        /// This API is use to get Business Category and SubCategory List
        /// Developer Name: Sushil Kumar
        /// Date: 11/12/2020 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("BusinessCategoryList")]
        public IActionResult BusinessCategoryList()
        {
            StringBuilder log = new StringBuilder();
            log.Append(Resources.LogStartMessage.Replace("{MethodName}", Resources.BusinessCategoryListAPI));
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _userService.GetCategoryList();
                    if (response == null)
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = null;
                        responseModel.Data = null;
                        log.Append(responseModel.Message);
                        return BadRequest(responseModel);
                    }
                    else
                    {
                        responseModel.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.Message = null;
                        responseModel.Data = response;
                        log.Append(responseModel.StatusCode);
                        return Ok(responseModel);
                    }

                }
            }
            catch(Exception ex)
            {
                LogManagers.WriteErrorLog(ex);
                responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = Resources.ErrorMessage;
                responseModel.Description = ex.Message;
                responseModel.Data = null;
            }
            finally
            {
                LogManagers.WriteTraceLog(log);
            }
            return BadRequest(responseModel);
        }

    }
}
