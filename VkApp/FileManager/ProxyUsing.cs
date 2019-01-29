using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VkApp.FileManager
{
    public class ProxyUsing
    {
        private const string _path = "Proxy.txt";
        private int _curPos = 0;
        private List<Dictionary<string, object>> _proxy;

        public Dictionary<string, object> Proxy
        {
            get { return _proxy[_curPos++]; }
        }

        public ProxyUsing()
        {
            _proxy = new List<Dictionary<string, object>>();
            ResetProxy();
        }

        private void ResetProxy()
        {
            if (File.Exists(_path))
            {
                string[] result = File.ReadAllLines(_path);
                for (int i = 0; i < result.Count(); i++)
                {
                    string[] splited = result[i].Split('/');
                    _proxy.Add(new Dictionary<string, object>());

                    _proxy[i].Add("ip", splited[0]);
                    _proxy[i].Add("port", Int32.Parse(splited[1]));
                    _proxy[i].Add("login", splited[3]);
                    _proxy[i].Add("password", splited[4]);
                }
            }
        }

        public int Count { get { return _proxy.Count; } }
    }
}
