using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VkApp.VKWorker
{
    public class ThreadWorker
    {
        private Thread _testThread;
        private VK_Navigate _vk;

        public ThreadWorker()
        {
            _vk = new VK_Navigate();
        }

        [STAThread]
        public void StartDriver_Work(string choosedGame)
        {
            _testThread = new Thread(() => _vk.StartWork(choosedGame));
            _testThread.SetApartmentState(ApartmentState.STA);
            _testThread.Start();
        }

        [STAThread]
        public void StartDriver_Send(string message, string choosedGame, string pathFile = null)
        {
            if (pathFile == null)
                _testThread = new Thread(() => _vk.CheckAndMail(message, choosedGame));
            else
                _testThread = new Thread(() => _vk.CheckAndMail(message, choosedGame, pathFile));
            _testThread.SetApartmentState(ApartmentState.STA);
            _testThread.Start();
        }

        public void Kill_Threads()
        {
            if (_testThread != null)
                _testThread.Abort();
        }
    }
}
