using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuptAssistant.Toolkit
{
    public class CrossPlatformFeatures
    {
        public static async Task Toast(Page page,string title, string message, string cancel)
        {
            await page.DisplayAlert(strings.Alert, strings.LoginFailed, strings.OK);
        }
    }
}