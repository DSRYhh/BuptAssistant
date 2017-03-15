using System;
using System.IO;
using BuptAssistant.Droid.NativeCode;
using BuptAssistant.Toolkit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BuptAssistant.Droid.NativeCode
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}