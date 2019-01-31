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
        private List<string> _friendList;

        public FriendsClass()
        {
            _hiddenFolder = Environment.CurrentDirectory + @"\BotFriendsFolder\";
            _friendList = new List<string>();
            if (!Directory.Exists(_hiddenFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(_hiddenFolder);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }

        public List<string> GetAllFriendsFromFile(string userLogin, string gameName)
        {
            string path = _hiddenFolder + userLogin + @"\" + gameName + ".txt";
            if (File.Exists(path))
            {
                string[] result = File.ReadAllLines(path);
                int needPos = result.ToList<string>().IndexOf(userLogin);

                while (result.ToList<string>()[needPos].IndexOf(" ") != 0)
                {
                    _friendList.Add(result[needPos]);
                    needPos++;
                }
                return _friendList;
            }
            else
                return null;
        }

        public void AddFriendsToFile(string userLogin, string gameName, List<string> friendRequests)
        {
            gameName = gameName.Remove(gameName.IndexOf(" "));
            string path = _hiddenFolder + gameName;// + ".txt";

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
                var res = File.ReadAllLines(path);
                foreach (string str in res) previousNames.Add(str);
            }
            foreach (var str in friendRequests) previousNames.Add(str);
            #endregion
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Default))
                {
                    foreach (string str in friendRequests) { writer.WriteLine(str); }
                }
            }

            #region COPYING_FROM_FILE
            if (File.Exists(_filePath))
            {
                var res = File.ReadAllLines(_filePath);
                foreach (string str in res) previousNames.Add(str);
            }
            foreach (var str in friendRequests) previousNames.Add(str);
            #endregion
            using (FileStream file = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Default))
                {
                    foreach (string str in friendRequests) { writer.WriteLine(str); }
                }
            }
        }

        public List<string> GetFriends { get { return _friendList; } }

        internal bool IsFriendExist(string text)
        {
            string friend = text;
            friend = friend.Replace("\r\n", " ");

            if (File.Exists(_filePath))
            {
                string[] allFriends = File.ReadAllLines(_filePath);
                for (int i = 0; i < allFriends.Count(); i++)
                    if (allFriends[i] == friend)
                        return true;
            }
            return false;
        }
    }
}