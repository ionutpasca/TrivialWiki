using Nancy;
using Nancy.Extensions;
using System.Drawing;
using System.Web;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement.Avatar
{
    public class AvatarModule : NancyModule
    {
        private readonly AvatarManager avatarManager = new AvatarManager();

        public AvatarModule()
        {
            Get["/avatar/{username}"] = param => GetUserAvatar((string)param.username);
            Get["/chatAvatar/{username}"] = param => GetChatAvatar((string)param.username);

            Post["/changeAvatar"] = param => ChangeAvatar();
            Post["/changeAvatarAsBase64"] = param => ChangeAvatarAsBase64();
        }

        private object GetChatAvatar(string username)
        {
            const string ContentType = "image/jpg";
            var avatarPath = DirectoryManager.GetLocalPath() + $"Avatars\\{username}\\{username}_chat.jpg";
            var avatar = avatarManager.GetAvatar(avatarPath);
            return Response.FromStream(avatar, ContentType);
        }

        private Response GetUserAvatar(string username)
        {
            const string ContentType = "image/jpg";
            var avatarPath = DirectoryManager.GetLocalPath() + $"Avatars\\{username}\\{username}.jpg";
            var avatar = avatarManager.GetAvatar(avatarPath);
            return Response.FromStream(avatar, ContentType);
        }

        private Response ChangeAvatarAsBase64()
        {
            var base64Image = this.Request.Body.AsString();
            if (base64Image == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var currentUser = this.Context.CurrentUser;
            avatarManager.ChangeUserAvatar(base64Image, currentUser.UserName);
            return HttpStatusCode.OK;
        }

        private Response ChangeAvatar()
        {
            const int height = 200;
            var currentUser = this.Context.CurrentUser;
            var file = HttpContext.Current.Request.Files[0];
            var bitmapImage = new Bitmap(file.InputStream);
            var width = (200 * bitmapImage.Width) / bitmapImage.Height;
            var resizedImage = ImageManager.ResizeImage(bitmapImage, width, height, file.FileName);

            avatarManager.ChangeUserAvatar(resizedImage, currentUser.UserName);
            return HttpStatusCode.OK;
        }
    }
}