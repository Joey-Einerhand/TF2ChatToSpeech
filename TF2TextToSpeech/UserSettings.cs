using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.IO;


namespace TF2TextToSpeech
{
    [Serializable]
    public enum PermissionList
    {
        Blacklist,
        Whitelist
    }
    public class UserSettings
    {
        // This gets set in the Load() or InitializeSettings() function of SaveLoadSystem
        public ClassConnector classConnector;

        // Settings
        public TypeOfConsoleLine typeToCheck { get; set; } = TypeOfConsoleLine.Talked;
        public PermissionList permissionListTypeToCheck { get; set; } = PermissionList.Blacklist;
        public int audioOutputDeviceNumber { get; set; }
        public int amountOfInstalledVoices { get; set; }
        public string pathToTF { get; set; }
        public string consoleLogFileName { get; set; }

        // End of settings

        public string pathToFile;

        public UserSettings()
        {
            //classConnector gets set in SaveLoadSystem

            // Determine max installed voices
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                amountOfInstalledVoices = synth.GetInstalledVoices().Count;
            }

            if (classConnector != null)
            {
                TryGetLogFileLoop();
            }


        }

        // Can get called by itself and SaveLoadSystem
        public void InitializeNewSettings()
        {
            // Different types not implemented
            //typeToCheck = TypeOfConsoleLine.Talked;
            // Not implemented yet
            //permissionListTypeToCheck = PermissionList.Blacklist;

            // Get Audio Output Number
            Console.WriteLine("Please write the device number of your output device (CABLE Input)");
            Console.WriteLine("Windows start menu > Sounds > Sound Configuration > Output devices");
            Console.WriteLine("The device on the bottom is number 0. Counts up from bottom to top.");
            audioOutputDeviceNumber = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the path to your Team Fortress 2/TF folder. Do not put spaces at the beginning or end of the path.");
            Console.WriteLine("Example path: C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf");
            pathToTF = Console.ReadLine();

            Console.WriteLine("Please enter the name of your console output file");
            Console.WriteLine("Default: tf2consoleoutput.log");
            consoleLogFileName = Console.ReadLine();

            Console.WriteLine("\n");

            TryGetLogFileLoop();
            

        }

        public void TryGetLogFileLoop()
        {
            while (!CheckLogFile())
            {
                Console.WriteLine("Please enter the path to your Team Fortress 2/TF folder. Do not put spaces at the beginning or end of the path.");
                Console.WriteLine("Example path: C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf");
                pathToTF = Console.ReadLine();


                Console.WriteLine("\nPlease enter the name of your console output file");
                Console.WriteLine("Default: tf2consoleoutputlog.log");
                consoleLogFileName = Console.ReadLine();
            }
        }

        public bool CheckLogFile()
        {
            // Path to file
            pathToFile = pathToTF + "\\" + consoleLogFileName;
            return classConnector.logFile.TryConnectLogFile();
        }

        // Switch between the two enum types based on the current enum type
        // TODO: Implement
        public void ChangePermissonListType()
        {  
            if (permissionListTypeToCheck == PermissionList.Blacklist)
            {
                permissionListTypeToCheck = PermissionList.Whitelist;
            }
            else { permissionListTypeToCheck = PermissionList.Blacklist; }
        }
    }
}
