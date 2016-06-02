using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement.Avatar
{
    public sealed class AvatarManager
    {
        public void ChangeUserAvatar(byte[] image, string username)
        {
            var imageForDisk = ImageManager.ConvertBytesToImage(image);
            SaveImageToDisk(imageForDisk, "Avatars", username);
        }

        public void ChangeUserAvatar(string image, string username)
        {
            var imageForDisk = ImageManager.ConvertBase64ToImage(image);
            SaveImageToDisk(imageForDisk, "Avatars", username);
        }

        public void SaveImageToDisk(Image image, string folderName, string username)
        {
            var userDirectory = CreateDirectoryForUser(folderName, username);
            SaveImageToDirectory(image, userDirectory, username);
            SaveImageForChat(image, userDirectory, username);
        }

        private static void SaveImageForChat(Image image, string userDirectory, string username)
        {
            var bitmapImage = new Bitmap(image);
            var resizedImage = ImageManager.ResizeImage(bitmapImage, 50, 50);
            var imageToSave = ImageManager.ConvertBytesToImage(resizedImage);
            using (var temp = new Bitmap(imageToSave))
            {
                SaveImageToDirectory(temp, userDirectory, username, true);
            }
        }

        private static string CreateDirectoryForUser(string folderName, string username)
        {
            var localPath = DirectoryManager.GetLocalPath() + "\\" + folderName;
            var avatarsDirectoryExists = Directory.Exists(localPath);
            if (!avatarsDirectoryExists)
            {
                CreateDirectory(localPath, folderName);
            }
            var userDirectory = localPath + "\\" + username;
            var userDirectoryExists = Directory.Exists(localPath + "\\" + username);
            if (!userDirectoryExists)
            {
                CreateDirectory(localPath, username);
            }
            return userDirectory;
        }

        private static void CreateDirectory(string dirPath, string dirName)
        {
            Directory.CreateDirectory(dirPath + "\\" + dirName);
        }

        private static void SaveImageToDirectory(Image img, string directory, string username, bool imageIsForChat = false)
        {
            var path = imageIsForChat ? directory + "\\" + username + "_chat.jpg" : directory + "\\" + username + ".jpg";
            img.Save(path, ImageFormat.Bmp);
        }

        public MemoryStream GetAvatar(string avatarPath)
        {
            if (!File.Exists(avatarPath))
            {
                return null;
            }
            var avatar = Image.FromFile(avatarPath);
            return ImageManager.ImageToStream(avatar);
        }
    }
}