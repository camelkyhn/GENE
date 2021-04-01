namespace Gene.Middleware.Constants
{
    public class Urls
    {
        public const string DefaultFormat = "[controller]/[action]/{id?}";
        public const string DefaultAreaFormat = "[area]/[controller]/[action]/{id?}";
        public const string ServerDomain = "https://www.genesoftware.com";

        #region Account

        public const string MailLinkAccountConfirmEmail = ServerDomain + "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.ConfirmEmail;
        public const string MailLinkAccountResetPassword = ServerDomain + "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.ResetPassword;
        public const string MailLinkAccountNotifications = ServerDomain + "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.Notifications;
        public const string AccountDeleteNotification = "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.DeleteNotification;
        public const string AccountGetNotification = "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.GetNotification;

        #endregion

        #region Delete

        public const string ActionDelete = "/" + Areas.Core + "/" + Controllers.Action + "/" + Actions.Delete;
        public const string AreaDelete = "/" + Areas.Core + "/" + Controllers.Area + "/" + Actions.Delete;
        public const string ControllerDelete = "/" + Areas.Core + "/" + Controllers.Controller + "/" + Actions.Delete;
        public const string ControllerActionDelete = "/" + Areas.Core + "/" + Controllers.ControllerAction + "/" + Actions.Delete;
        public const string ControllerActionRoleDelete = "/" + Areas.Core + "/" + Controllers.ControllerActionRole + "/" + Actions.Delete;
        public const string ResourceDelete = "/" + Areas.Core + "/" + Controllers.Resource + "/" + Actions.Delete;

        public const string NotificationDelete = "/" + Areas.Identity + "/" + Controllers.Notification + "/" + Actions.Delete;
        public const string RoleDelete = "/" + Areas.Identity + "/" + Controllers.Role + "/" + Actions.Delete;
        public const string UserDelete = "/" + Areas.Identity + "/" + Controllers.User + "/" + Actions.Delete;
        public const string UserRoleDelete = "/" + Areas.Identity + "/" + Controllers.UserRole + "/" + Actions.Delete;

        #endregion

        #region Detail

        public const string ActionDetail = "/" + Areas.Core + "/" + Controllers.Action + "/" + Actions.Detail;
        public const string AreaDetail = "/" + Areas.Core + "/" + Controllers.Area + "/" + Actions.Detail;
        public const string ControllerDetail = "/" + Areas.Core + "/" + Controllers.Controller + "/" + Actions.Detail;
        public const string ControllerActionDetail = "/" + Areas.Core + "/" + Controllers.ControllerAction + "/" + Actions.Detail;
        public const string ControllerActionRoleDetail = "/" + Areas.Core + "/" + Controllers.ControllerActionRole + "/" + Actions.Detail;
        public const string ResourceDetail = "/" + Areas.Core + "/" + Controllers.Resource + "/" + Actions.Detail;

        public const string NotificationDetail = "/" + Areas.Identity + "/" + Controllers.Notification + "/" + Actions.Detail;
        public const string RoleDetail = "/" + Areas.Identity + "/" + Controllers.Role + "/" + Actions.Detail;
        public const string UserDetail = "/" + Areas.Identity + "/" + Controllers.User + "/" + Actions.Detail;
        public const string UserRoleDetail = "/" + Areas.Identity + "/" + Controllers.UserRole + "/" + Actions.Detail;

        #endregion

        #region Edit

        public const string ActionEdit = "/" + Areas.Core + "/" + Controllers.Action + "/" + Actions.Edit;
        public const string AreaEdit = "/" + Areas.Core + "/" + Controllers.Area + "/" + Actions.Edit;
        public const string ControllerEdit = "/" + Areas.Core + "/" + Controllers.Controller + "/" + Actions.Edit;
        public const string ControllerActionEdit = "/" + Areas.Core + "/" + Controllers.ControllerAction + "/" + Actions.Edit;
        public const string ControllerActionRoleEdit = "/" + Areas.Core + "/" + Controllers.ControllerActionRole + "/" + Actions.Edit;
        public const string ResourceEdit = "/" + Areas.Core + "/" + Controllers.Resource + "/" + Actions.Edit;

        public const string NotificationEdit = "/" + Areas.Identity + "/" + Controllers.Notification + "/" + Actions.Edit;
        public const string RoleEdit = "/" + Areas.Identity + "/" + Controllers.Role + "/" + Actions.Edit;
        public const string UserEdit = "/" + Areas.Identity + "/" + Controllers.User + "/" + Actions.Edit;
        public const string UserRoleEdit = "/" + Areas.Identity + "/" + Controllers.UserRole + "/" + Actions.Edit;

        #endregion
    }
}