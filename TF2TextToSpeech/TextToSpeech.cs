using System;
using System.Collections.Generic;
using System.Text;
using System.Speech;
using System.Speech.Synthesis;
using System.Threading;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Text.RegularExpressions;

namespace TF2TextToSpeech
{
    public class TextToSpeech
    {
        private ClassConnector classConnector;
        public List<string> installedVoiceNames = new List<string>();
        
        public TextToSpeech(ClassConnector classConnector)
        {
            this.classConnector = classConnector;
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    installedVoiceNames.Add(voice.VoiceInfo.Name);
                }
            }
        }


        public void MicSpam()
        {
            while (true)
            {
                // Checks if last  line from log fits parameter, if so, lineToSay is updated
                string unfilteredLineToSay = classConnector.textHandler.GetUnfilteredLineToSay();
                if (unfilteredLineToSay != "")
                {
                    Thread thread1 = new Thread(() => Speak(unfilteredLineToSay));
                    thread1.Start();
                }
                else { Thread.Sleep(50);  }

            }

        }

        private void Speak(string unfilteredLineToSay)
        {
            using (WaveOut waveOut = new WaveOut())
            using (MemoryStream stream = new MemoryStream())
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                SetSynthOptions(synth, unfilteredLineToSay);

                // Remove all previous commands from the line so they won't be spoken
                // by the synth.
                string lineToSpeak = classConnector.textHandler.RemoveCommandsFromLine(unfilteredLineToSay);

                // Converts speech to a wave, inserts wave in stream
                synth.SetOutputToWaveStream(stream);
                synth.Speak(lineToSpeak);

                // Goes to begin of speech 
                stream.Seek(0, SeekOrigin.Begin);

                // Reads the speech inserted in stream
                var reader = new WaveFileReader(stream);


                // Set device to output to. Number of connected devices,
                // 0-based index, starts from bottom
                waveOut.DeviceNumber = classConnector.userSettings.audioOutputDeviceNumber;
                waveOut.Init(reader);

                waveOut.Play();

                // Makes sure to let the wave finish speaking
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                }
            }
        }

        void SetSynthOptions(SpeechSynthesizer synth, string unfilteredLineToSay)
        {
            // Selects voice based on input. If none is given, default synth voice is chosen
            string synthVoiceToUse = classConnector.textHandler.GetVoice(unfilteredLineToSay);
            synth.SelectVoice(synthVoiceToUse);

            // Select rate (Speak speed) Default is 0
            synth.Rate = classConnector.textHandler.GetRate(unfilteredLineToSay);
        }

        


    }
}
