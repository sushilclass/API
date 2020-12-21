using Outreach_FRMS_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_BL
{
    public interface IUserService
    {
        string SingUp(Users user);
        string UploadImage(UserDocumentMapping obj);
        AuthenticateResponse Validate(AuthenticateRequest model);
        ViewModel GetByEmailId(string email);
        ViewModel GetById(int id);
        string DeleteDocument(Users user);
        string SaveSearchResponse(SearchAPIResponse response, int BusinessCategory);
        string ForgotPasscode(Users users);
        string ResetPasscode(Users users);
        List<UserDetail> GetUserDetails(Users users);
        List<FavsSearchData> GetRestaurantList(SearchRequest searchdata);
        string SaveContact(ContactsMappingModel model);
        // List<ContactsMappingModel> GetContactList(ContactsMappingModel model);
        InvitationRequest GetContactList(InvitationRequest model);
        string DeleteContact(ContactsMappingModel model);
        string UpdateMyProfile(ViewModel model);
        //  string SaveInvitor(Invite model);
        string ApplicationInvite(ApplicationInvite model);
        string SendInvitee(Invite model);
        string StatusUpdateInvitee(Invite model);
        List<InviteList> GetInviteeList(Invite model);
        List<InviteList> GetInvitorList(Invite model);
        string MyFavsSave(MyFavs model);
        List<MyFavsList> GetMyFavsList(MyFavs model);
        string DeleteMyFav(MyFavs model);
        string MyContactFavSave(MyContactFavs model);
        List<MyFavsList> GetMyContactFavs(MyContactFavs model);
        List<ContactModel> GetContacts();
        string SaveBusinessProfileData(ServiceDataModel model);
        string BusinessProfileUpdate(BusinessResearch model);
        List<ServiceDataModel> GetBusinessProfileData(ServiceDataModel model);
        string BusinessClaimSave(BusinessClaimModel model);
        List<ServiceDataModel> BusinessClaimGet(BusinessClaimModel model);
        LeaderBoardData GetLeaderBoard(LeaderBoardData model);
        string FeedbackSave(Feedback model);
        string SuggestionSave(Suggestion model);
        string SingUpResturentResearch(Users user);
        List<Country> GetCountry();
        List<CategorySubCategory> GetCategoryList();


    }
}
