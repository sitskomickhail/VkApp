using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VkApp.FileManager
{
    public class AddedNameFriends
    {
        private const string _filePath = "Friends.txt";
        private List<string> _friendList;

        public AddedNameFriends()
        {
            _friendList = new List<string>();
        }

        public List<string> GetFriendsFromFile(string userLogin)
        {
            if (File.Exists(_filePath))
            {
                string[] result = File.ReadAllLines(_filePath);
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

        public void AddFriendsToFile(string userLogin, List<string> friendRequests)
        {
            using (FileStream file = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Default))
                {
                    writer.WriteLine(userLogin);
                    foreach (string str in friendRequests) { writer.WriteLine(str); }
                }
            }
        }

        public List<string> GetFriends { get { return _friendList; } }
    }
}