using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkApp.Models;

namespace VkApp.VKWorker
{
    public class ThreadWorker
    {
        private Thread _testThread;
        //private VK_Navigate _vk;
        private VkNavigate _vkTest;

        public ThreadWorker()
        {
            _vkTest = new VkNavigate();
            //_vk = new VK_Navigate();
        }

        [STAThread]
        public void StartDriver_Work(string choosedGame)
        {
            _testThread = new Thread(() => _vkTest.StartAdding(choosedGame));
            _testThread.SetApartmentState(ApartmentState.STA);
            _testThread.Start();
        }
        

        //[STAThread]
        //public void StartDriver_Send(string message, string choosedGame, string pathFile = null)
        //{
        //    if (pathFile == null)
        //        _testThread = new Thread(() => _vk.CheckAndMail(message, choosedGame));
        //    else
        //        _testThread = new Thread(() => _vk.CheckAndMail(message, choosedGame, pathFile));
        //    _testThread.SetApartmentState(ApartmentState.STA);
        //    _testThread.Start();
        //}

        internal void CloseThreads()
        {
            if (_testThread != null)
            {
                //_vk.CloseDrivers();
                _testThread.Abort();
            }
        }
    }
}
