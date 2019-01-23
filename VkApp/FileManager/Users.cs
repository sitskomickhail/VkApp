using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VkApp.FileManager
{
    class Users
    {
        private List<string> _users;
        private const string _userPath = "userLogins.txt";

        public Users()
        {
            if (File.Exists(_userPath))
            {
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

        public List<string> GetUsers { get { return _users; } }
    }
}