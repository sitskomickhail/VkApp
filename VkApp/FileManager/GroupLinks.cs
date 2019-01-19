using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VkApp.FileManager
{
    class GroupLinks
    {
        private List<string> _links;        
        private const string _gamesPath = "SavedGames.txt";

        public GroupLinks()
        {
            _links = new List<string>();
        }
        
        public List<string> GetUsefullGames()
        {
            List<string> games = new List<string>();
            if (File.Exists(_gamesPath))
            {
                string[] returnedGamesValue = File.ReadAllLines(_gamesPath);
                for (int i = 0; i < returnedGamesValue.Count(); i++)
                {
                    games.Add(returnedGamesValue[i]);
                }
            }
            return games;
        }

        public List<string> GetLinks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] fileInfo = File.ReadAllLines(filePath);
                for (int i = 0; i < fileInfo.Count(); i++)
                {
                    _links.Add(fileInfo[i]);
                }
            }
            return _links;
        }

        public bool CreateLinksFile(string filePath)
        {
            using (FileStream file = new FileStream(Environment.CurrentDirectory, FileMode.OpenOrCreate))
            {
                if (File.Exists(filePath))
                {
                    string[] links = File.ReadAllLines(filePath);

                }
            }
            return false;
        }
    }
}