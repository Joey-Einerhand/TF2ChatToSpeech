using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TF2TextToSpeech
{

    public class TextHandler
    {
        private ClassConnector classConnector;
        string lineToSay = "Debug Text : Please Remove";
        string previousLineToSay = "";

        // regex pattern that gets used multiple times
        readonly Regex killPattern = new Regex("^(\\w| )+killed(\\w| )+with(\\w| )+");
        readonly Regex talkPattern = new Regex("( :)+");

        public TextHandler(ClassConnector classConnector)
        {
            this.classConnector = classConnector;
        }

        //Unfiltered meaning text hasn't been removed from the line, e.g commands, swears, etc
        public string GetUnfilteredLineToSay()
        {
            string lineToCheck = GetLastLogFileLine();

            // If line shouldn't be said, return an empty string instead.
            if (ShouldLineBeSaid(lineToCheck))
            {
                lineToSay = lineToCheck;
                previousLineToSay = lineToSay;
                return lineToSay;
            }
            else { return ""; }
        }

        public string GetLastLogFileLine()
        {
            return classConnector.logFile.ReadEndOfFile();
        }

        // This method has sole responsibility in checking prerequisites, e.g repeated line, proper type, blacklist, etc
        public bool ShouldLineBeSaid(string lineToCheck)
        {
            // TODO:
            // Check banlist
            // whitelist
            if (!IsRepeatedLine(lineToCheck) && IsLinePropertype(classConnector.userSettings.typeToCheck, lineToCheck))
            {
                return true;
            }
            else { return false; }

        }

        public bool IsRepeatedLine(string lineToCheck)
        {
            if (lineToCheck == previousLineToSay)
            {
                return true;
            }
            else { return false; }
        }

        bool IsLinePropertype(TypeOfConsoleLine typeToCheck, string lastLogLine)
        {
            // What type to check for?
            // In if statements: Is the string that type?
            if (typeToCheck == TypeOfConsoleLine.Killed)
            {
                if (isKilledString(lastLogLine))
                {
                    return true;
                }
                else { return false; }
            }
            else if (typeToCheck == TypeOfConsoleLine.Talked)
            {
                if (isTalkedString(lastLogLine))
                {
                    return true;
                }
                else { return false; }
            }
            else { throw new System.ArgumentException("TypeOfConsoleLine type not implemented in IsPorperLine()");  }
        }

        // Filters last events
        // Returns string to say depending on which event last happend
        public bool isKilledString(string stringToFilter)
        {
            Boolean isKilledMessage = killPattern.IsMatch(stringToFilter);
            // X killed Y with Z
            return isKilledMessage;
        }

        public bool isTalkedString(string stringToFilter)
        {
            Boolean isTalkedMessage = talkPattern.IsMatch(stringToFilter);
            // X killed Y with Z
            return isTalkedMessage;
        }

        public string RemoveCommandsFromLine(string lineToCheck)
        {
            string lineToReturn = lineToCheck;
            // Match "$x" where "x" can be multiple characters, but never a whitespace character.
            foreach (Match match in Regex.Matches(lineToCheck, "(\\$[^\\s]+)"))
            {
                Console.WriteLine(match.Value);
                lineToReturn = lineToReturn.Replace(match.Value, "");
            }
            return lineToReturn;
        }

        public string GetVoice(string lineToCheck)
        {
            List<string> installedVoiceNames = classConnector.textToSpeech.installedVoiceNames;
            int amountOfInstalledVoices = classConnector.userSettings.amountOfInstalledVoices;
            Match VoiceCommandMatch = Regex.Match(lineToCheck, "(\\$V)(\\d{1,5})");
            if (VoiceCommandMatch.Success)
            {
                // From the match, get the second group (\\d), which contains a digit, and return it
                // Example, if lineToCheck is $R5:
                // Groups[0] is "$R5", Groups[0] is "$R", Groups[2] is "5".
                int voiceNumber = Int32.Parse(VoiceCommandMatch.Groups[2].Value);

                // No else needed; smallest voicenumber is 0, smallest amountOfInstalledVoices is 1.
                if (voiceNumber > (amountOfInstalledVoices - 1))
                {
                    voiceNumber = 0;
                }
                

                // Get a voice from a list of voice names located at the index of voiceNumber and return it
                return installedVoiceNames[voiceNumber];
            }
            // return first installed voice (default)
            else { return installedVoiceNames[0]; }

        }

        public int GetRate(string lineToCheck)
        {
            // Match anything that has "$Rxx" where x can be digits. Accept a "-" before
            // the digits.
            Match RateCommandMatch = Regex.Match(lineToCheck, "(\\$R)(-?\\d{1,2})");
            if (RateCommandMatch.Success)
            {
                // From the match, get the second group (\\d), which contains a digit, and return it
                // Example, if lineToCheck is $R5:
                // Groups[0] is "$R5", Groups[0] is "$R", Groups[2] is "5".
                int rate = Int32.Parse(RateCommandMatch.Groups[2].Value);

                // make sure the rate doesn't surpass Synth boundaries
                if (rate < -10)
                { rate = -10; }
                else if (rate > 10)
                { rate = 10; }

                return rate;
            }
            else { return 0; }
        }


    }
}
