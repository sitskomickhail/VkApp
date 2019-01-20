using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VkApp.FileManager
{
    class Users
    {
        private Dictionary<string, string> _dictUsers;
        private List<string> _users;
        private const string _userPath = "userLogins.txt";
        private int _curPos;

        public Users()
        {
            if (File.Exists(_userPath))
            {
                _curPos = 0;
                _dictUsers = new Dictionary<string, string>();
                _users = new List<string>();

                string[] settedUsers = File.ReadAllLines(_userPath, Encoding.Default);
                for (int i = 0; i < settedUsers.Count(); i++)
                {
                    _users.Add(settedUsers[i]);
                }
            }
            //else
            //    throw new ArgumentNullException("Файл с пользователями не найден");
        }

        public Dictionary<string, string> GetUsersDictionary
        {
            get
            {
                string[] loginPass = _users[_curPos++].Split(':');

                _dictUsers["login"] = loginPass[0];
                _dictUsers["password"] = loginPass[1];
                return _dictUsers;
            }
        }
    }
}