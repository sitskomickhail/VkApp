using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VkApp.FileManager
{
    public class FriendsClass
    {
        private const string _filePath = "Friends.txt";
        private string _hiddenFolder;

        public FriendsClass()
        {
            _hiddenFolder = Environment.CurrentDirectory + @"\BotFriendsFolder\";
            if (!Directory.Exists(_hiddenFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(_hiddenFolder);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }

        public List<string> GetAllFriendsFromFile(string userLogin, string checkedGame)
        {
            List<string> friendList = new List<string>();
            string[] temp = checkedGame.Split(' ');
            string gameName = null;
            for (int i = 0; i < temp.Count(); i++)
            {
                gameName += temp[i];
            }
            string path = _hiddenFolder + gameName + @"\" + userLogin + ".txt";
            if (File.Exists(path))
            {
                string[] result = File.ReadAllLines(path, Encoding.Default);
                for (int i = 0; i < result.Count(); i++)
                {
                    friendList.Add(result[i]);
                }
                return friendList;
            }
            else
                return null;
        }

        public void AddFriendsToFile(string userLogin, string checkedGame, List<string> friendRequests)
        {
            string[] temp = checkedGame.Split(' ');
            string gameName = null;
            for (int i = 0; i < temp.Count(); i++)
            {
                gameName += temp[i];
            }
            string path = _hiddenFolder + gameName;

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory;
            }

            path = _hiddenFolder + gameName + @"\" + userLogin + ".txt";

            #region COPYING_FROM_FILE
            List<string> previousNames = new List<string>();
            if (File.Exists(path))
            {
                string[] res = File.ReadAllLines(path, Encoding.Default);
                foreach (string str in res) if (!String.IsNullOrEmpty(str)) previousNames.Add(str);
            }
            foreach (string str in friendRequests) if (!String.IsNullOrEmpty(str)) previousNames.Add(str);
            #endregion
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Default))
                {
                    foreach (string str in previousNames) { writer.WriteLine(str); }
                }
            }

            previousNames.Clear();
            #region COPYING_FROM_FILE
            if (File.Exists(_filePath))
            {
                string[] res = File.ReadAllLines(_filePath, Encoding.Default);
                foreach (string str in res) if (!String.IsNullOrEmpty(str)) previousNames.Add(str);
            }
            foreach (string str in friendRequests) if (!String.IsNullOrEmpty(str)) previousNames.Add(str);
            #endregion
            using (FileStream file = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Default))
                {
                    foreach (string str in previousNames) { writer.WriteLine(str); }
                }
            }
        }

        internal bool IsFriendExist(string text)
        {
            if (File.Exists(_filePath))
            {
                string[] allFriends = File.ReadAllLines(_filePath, Encoding.Default);
                for (int i = 0; i < allFriends.Count(); i++)
                    if (allFriends[i] == text)
                        return true;
            }
            return false;
        }
    }
}