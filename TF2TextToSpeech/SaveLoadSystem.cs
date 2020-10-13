using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

namespace TF2TextToSpeech
{
    public class SaveLoadSystem
    {
        private ClassConnector classConnector;
        string settingsFilePath = "settings\\UserSettings.cfg";

        public SaveLoadSystem(ClassConnector classConnector)
        {
            this.classConnector = classConnector;
        }

        public void SaveSettings()
        {
            using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
                jsonOptions.WriteIndented = true;
                string jsonString = JsonSerializer.Serialize(classConnector.userSettings, jsonOptions);
                streamWriter.Write(jsonString);
            }
        }

        public void LoadSettings()
        {
            // Check if settings file is not empty and if user wants to load settings
            bool shouldLoadSettings = ChoiceLoadSettings();
            bool settingsExist = CheckIfSettingsExist();
            if (settingsExist && shouldLoadSettings)
            {
                // Get settings file and deserialise it into the usersettings object
                using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string jsonString = streamReader.ReadToEnd();

                    try
                    {
                        classConnector.userSettings = JsonSerializer.Deserialize<UserSettings>(jsonString);
                        classConnector.userSettings.classConnector = classConnector;
                        classConnector.userSettings.TryGetLogFileLoop();
                        SaveSettings();
                    }
                    catch (Exception e)
                    {
                        //Log any and all errors.
                        Console.WriteLine(e);
                        Console.WriteLine("!!! ERROR !!! Something went wrong when loading settings.");
                        Console.WriteLine("Please check the .cfg file for errors. Initializing new settings...");
                        Console.WriteLine("Please exit the program if you don't want to reset your saved settings.");
                        InitializeNewSettings();
                    }

                }
            }
            else
            {
                InitializeNewSettings();
            }



        }

        private bool ChoiceLoadSettings()
        {
            ConsoleKey loadSettingsKey = ConsoleKey.A;
            while ((loadSettingsKey != ConsoleKey.Y) && (loadSettingsKey != ConsoleKey.N))
            {
                Console.WriteLine("Do you want to load your previous settings? [Y/N]");
                loadSettingsKey = Console.ReadKey().Key;
                Console.WriteLine("\n");
            }
            if (loadSettingsKey == ConsoleKey.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfSettingsExist()
        {
            using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                string fileContent = streamReader.ReadToEnd();
                if (fileContent != "")
                {
                    return true;
                }
                else { return false; }
            }
        }

        private void InitializeNewSettings()
        {
            classConnector.userSettings = new UserSettings();
            classConnector.userSettings.classConnector = classConnector;
            classConnector.userSettings.InitializeNewSettings();
            classConnector.userSettings.TryGetLogFileLoop();
            SaveSettings();
        }

    }
}
