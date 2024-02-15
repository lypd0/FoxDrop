using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxDrop
{
    internal class RegistryHelper
    {
        public static void CreateRegistryFolder(string folderPath)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(folderPath))
            {
                
            }
        }

        public static void CreateRegistryKeys(string folderPath, string keyName, object value)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(folderPath, true))
            {
                key.SetValue(keyName, value);
            }
        }
    }
}
