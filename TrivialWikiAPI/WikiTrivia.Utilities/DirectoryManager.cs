using System;
using System.IO;

namespace WikiTrivia.Utilities
{
    public static class DirectoryManager
    {
        public static string GetLocalPath()
        {
            var exeLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var exeDir = Path.GetDirectoryName(exeLocation) + "\\..";
            var localPath = new Uri(exeDir).LocalPath;
            return localPath;
        }
    }
}
