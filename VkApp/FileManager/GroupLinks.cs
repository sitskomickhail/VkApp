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
            else
                return null;
            return games;
        }

        public List<string> GetLinks(string fileName)
        {
            if (fileName.IndexOf(' ') != 0)
                fileName = fileName.Replace(" ", "");

            string filePath = fileName + ".txt";
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

        public string CreateLinksFile(string filePath)
        {
            string[] fileManage = filePath.Split('\\');
            using (FileStream file = new FileStream($"{fileManage[fileManage.Count() - 1]}", FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (File.Exists(filePath))
                {
                    string[] linkssss = File.ReadAllLines(filePath);
                    List<string> links = new List<string>();

                    for (int i = 0; i < linkssss.Count(); i++)
                        if (!String.IsNullOrEmpty(linkssss[i]))
                            links.Add(linkssss[i]);

                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        links.ForEach(str => writer.WriteLine(str));
                    }
                    file.Close();
                    return AddToGames(fileManage[fileManage.Count() - 1].Remove(fileManage[fileManage.Count() - 1].IndexOf('.')));
                }
            }
            return null;
        }

        private string AddToGames(string game)
        {
            File.ReadAllLines(_gamesPath);

            //using (FileStream file = new FileStream(_gamesPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //{
            using (StreamWriter writer = File.AppendText(_gamesPath))
            {
                writer.WriteLine(game);
            }
            //}
            return game;
        }
    }
}