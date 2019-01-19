using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VkApp.FileManager
{
    class Users
    {
        private string[][] _dictUsers;
        private const string _userPath = "userLogins.txt";

        public Users()
        {
            if (File.Exists(_userPath))
            {
                List<string> users = new List<string>();
                string[] settedUsers = File.ReadAllLines(_userPath);
                for (int i = 0; i < settedUsers.Count(); i++)
                {
                    users.Add(settedUsers[i]);
                }

                int d = 0;
                foreach (string user in users)
                {
                    string[] loginPass = user.Split(':');
                    //_dictUsers.Add(loginPass[0], loginPass[1]);
                    _dictUsers[d][0] = loginPass[0];
                    _dictUsers[d][1] = loginPass[1];
                    d++;
                }
            }
            //else
            //    throw new ArgumentNullException("Файл с пользователями не найден");
        }

        public Dictionary<string, string> GetUsersDictionary { get { return _dictUsers; } }
    }
}
