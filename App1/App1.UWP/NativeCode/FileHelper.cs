using System;
using System.IO;
using Windows.Storage;
using BuptAssistant.Toolkit;
using BuptAssistant.UWP.NativeCode;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BuptAssistant.UWP.NativeCode
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            return localFolder.Path;
        }
    }
}