using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace EasyCodeClass
{
    public class WindowsTheme
    {
        const string keyname = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";

        public static bool isAppsUseDarkTheme()
        {
            int tInteger = (int)Registry.GetValue(keyname, "AppsUseLightTheme", -1);
            if(tInteger == 0)
            {
                return true;
            }
            if(tInteger == 1)
            {
                return false;
            }
            throw new Exception("Error by reading app-theme!");
        }

        public static bool isSystemUseDarkTheme()
        {
            int tInteger = (int)Registry.GetValue(keyname, "SystemUsesLightTheme", -1);
            if (tInteger == 0)
            {
                return true;
            }
            if (tInteger == 1)
            {
                return false;
            }
            throw new Exception("Error by reading system-theme!");
        }

        public static void SetDarkAppTheme(bool darktheme)
        {
            if (darktheme)
            {
                Registry.SetValue(keyname, "AppsUseLightTheme", 0, RegistryValueKind.DWord);
            }
            else
            {
                Registry.SetValue(keyname, "AppsUseLightTheme", 1, RegistryValueKind.DWord);
            }
        }

        public static void SetDarkSystemTheme(bool darktheme)
        {
            if (darktheme)
            {
                Registry.SetValue(keyname, "SystemUsesLightTheme", 0, RegistryValueKind.DWord);
            }
            else
            {
                Registry.SetValue(keyname, "SystemUsesLightTheme", 1, RegistryValueKind.DWord);
            }
        }
    }
}
