using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace TF2TextToSpeech
{
    public class BanList
    {
        private ClassConnector classConnector;
        public List<String> usernameBanList = new List<string>();
        
        public BanList(ClassConnector classConnector)
        {
            this.classConnector = classConnector;
        }

        // Need to test
        public bool UserIsBanned(string username)
        {
            if (usernameBanList.Contains(username))
            {
                return true;
            }
            else { return false; }
        }

        public void BanUser(string username)
        {
            usernameBanList.Add(username);
        }

        public void UnbanUser(string username)
        {
            //int keyOfUserToUnban = UsernameBanList.Find(bannedUser => bannedUser.Contains(username)).Key;
            if (UserIsBanned(username))
            {
                usernameBanList.Remove(username);
            }
        }
    }
}
