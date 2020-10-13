using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TF2TextToSpeech
{
    public class ClassConnector
    {
        public UserSettings userSettings;
        public TextHandler textHandler;
        public BanList banList;
        public LogFile logFile;
        public TextToSpeech textToSpeech;
        public SaveLoadSystem saveLoadSystem;

        public ClassConnector()
        {

            textHandler = new TextHandler(this);
            banList = new BanList(this);
            logFile = new LogFile(this);
            textToSpeech = new TextToSpeech(this);
            saveLoadSystem = new SaveLoadSystem(this);
            saveLoadSystem.LoadSettings();
            textToSpeech.MicSpam();


        }
    }
}
