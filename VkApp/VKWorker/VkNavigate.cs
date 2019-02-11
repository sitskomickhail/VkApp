using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using VkApp.FileManager;
using System.IO;
using System.Windows;

namespace VkApp.Models
{
    class VkNavigate
    {
        private ChromeDriver _driver;
        private ChromeOptions _options;
        private GroupLinks _links;
        private FriendsClass _friendList;
        private Users _userClass;
        private ProxyUsing _proxy;

        private List<string> _friendLinks;
        List<string> _savedNames;

        public VkNavigate()
        {
            _options = new ChromeOptions();

            _links = new GroupLinks();
            _friendList = new FriendsClass();
            _userClass = new Users();
            _proxy = new ProxyUsing();

            _friendLinks = new List<string>();
            _savedNames = new List<string>();
        }

        public void ReOption(Dictionary<string, object> proxy)
        {
            _options = new ChromeOptions();
            //_options.AddArgument($"load-extension={Environment.CurrentDirectory}\\ProxyAutoAuth.crx");
            _options.AddArgument("--disable-software-rasterizer");
            _options.AddArgument("--disable-gpu\\" + Environment.CurrentDirectory);
            _options.AddArgument("--safebrowsing-disable-download-protection");
            _options.AddArgument("ignore-certificate-errors");
            //_options.AddArgument("--remote-debugging-port=9222");
            _options.AddArgument("headless");
            _options.AddArgument("start-maximized");
            _options.AddArgument("disable-infobars");
            _options.AddExtension(Environment.CurrentDirectory + "\\ProxyAutoAuth.crx");
            _options.ToCapabilities();

            Proxy prox = new Proxy();
            prox.IsAutoDetect = false;
            prox.Kind = ProxyKind.Manual;
            prox.HttpProxy = prox.SslProxy = $"{proxy["ip"]}:{proxy["port"]}";
            prox.SocksUserName = proxy["login"].ToString();
            prox.SocksPassword = proxy["password"].ToString();
            _options.Proxy = prox;
        }

        internal void CloseDrivers()
        {
            if (_driver != null)
            {
                _driver.Close();
            }
        }

        private void SecretStart(string choosedGame)
        {
            List<string> links = _links.GetLinks(choosedGame);
            if (links.Count == 0)
            {
                MessageBox.Show("Ошибка! Не найдены ссылки на группы", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Environment.CurrentDirectory);
            //service.HideCommandPromptWindow = true;
            ChromeOptions opts = new ChromeOptions();
            opts.AddArgument("--remote-debugging-port=9222");
            opts.AddArgument("--disable-software-rasterizer");
            opts.AddArgument("--disable-gpu");
            opts.AddArgument("ignore-certificate-errors");
            opts.AddArgument("--safebrowsing-disable-download-protection");
            opts.AddArgument("headless");

            ChromeDriver drv = new ChromeDriver(service, opts);
            //drv.Manage().Window.Maximize();
            drv.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            drv.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");
            string[] logPass = _userClass.GetUsers[0].Split(':');
            #region LOGIN
            drv.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
            drv.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
            drv.FindElementByXPath("//*[@id=\"login_button\"]").Click();
            #endregion

            foreach (var group in _links.GetLinks(choosedGame))
            {
                List<IWebElement> elems = new List<IWebElement>();
                Thread.Sleep(1500);
                drv.Navigate().GoToUrl(group);
                Thread.Sleep(1000);
                try
                {
                    drv.FindElementByXPath("//*[@id=\"group_followers\"]/a/div/span[1]").Click();
                }
                catch
                {
                    try { drv.FindElementByXPath("//*[@id=\"public_followers\"]/a/div/span[2]").Click(); }
                    catch { continue; }
                }

                foreach (IWebElement item in drv.FindElements(By.ClassName("fans_fan_lnk")))
                {
                    elems.Add(item);
                    if (elems.Count == 100)
                        break;
                }

                foreach (IWebElement link in elems)
                {
                    if (!_friendList.IsFriendExist(link.Text))
                    {
                        _friendLinks.Add(link.GetAttribute("href"));
                        string friend = link.Text;
                        friend = friend.Replace("\r\n", " ");
                        _savedNames.Add(friend);
                    }
                }
            }
            drv.Dispose();
            drv.Quit();
        }

        public void StartAdding(string choosedGame, string addMessage)
        {
            SecretStart(choosedGame);
            Thread.Sleep(2000);

            List<string> links = _links.GetLinks(choosedGame);

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Environment.CurrentDirectory);
            //service.HideCommandPromptWindow = true;

            List<Dictionary<string, object>> proxy = _proxy.Proxy;
            decimal dictSwitch = _userClass.GetUsers.Count / _proxy.Count;
            dictSwitch = Math.Floor(dictSwitch);

            int dictPos = 0;
            int tempCount = 0;
            int tempDictUsing = 0;
            int hrefsListed = 0;

            while (tempCount < 200 && hrefsListed < _friendLinks.Count)
            {
                foreach (var user in _userClass.GetUsers)
                {
                    if (tempDictUsing % dictSwitch == 0 && tempDictUsing != 0)
                        dictPos++;
                    
                    Dictionary<string, object> curProxy = proxy[dictPos];
                    ReOption(curProxy);
                    string[] logPass = user.Split(':');

                    #region CRX_PROXY_INIT
                     _driver = new ChromeDriver(service, _options);
                    //_driver.Manage().Window.Maximize();
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    try
                    {
                        _driver.FindElementById("login").SendKeys(curProxy["login"].ToString());
                        _driver.FindElementById("password").SendKeys(curProxy["password"].ToString());
                        _driver.FindElementById("save").Click();
                        Thread.Sleep(100);
                    }
                    catch
                    {
                        _options = null;
                        _driver.Close();
                        continue;
                    }
                    #endregion

                    _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

                    #region LOGIN
                    _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                    _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                    _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
                    #endregion
                    if (IsRecaptchaExist())
                    {
                        tempDictUsing++;
                        continue;
                    }

                    #region ADD_FRIEND
                    List<string> localFriends = new List<string>();
                    bool check = true;
                    for (int i = hrefsListed; i < _friendLinks.Count; i++)
                    {
                        if (!_friendList.IsFriendExist(_savedNames[i]))
                        {
                            _driver.Navigate().GoToUrl(_friendLinks[i]);
                            try
                            {
                                _driver.FindElementByXPath("//*[@id=\"friend_status\"]/div[1]/button").Click();

                                if (IsRecaptchaExist())
                                {
                                    break;
                                }
                                hrefsListed++;
                                _driver.FindElementByClassName("page_actions_dd_label").Click();
                                _driver.FindElementByXPath("//*[@id=\"page_actions_wrap\"]/div[2]/a[2]").Click();
                                _driver.FindElementById("preq_input").SendKeys(addMessage);
                                _driver.FindElementByClassName("flat_button").Click();

                                localFriends.Add(_savedNames[i]);
                                tempCount++;
                                Thread.Sleep(Randomer.Next(120000 * 10 / _userClass.GetUsers.Count, 180000 * 10 / _userClass.GetUsers.Count));
                            }
                            catch { }
                            if (hrefsListed == _friendLinks.Count)
                            {
                                check = false;
                                break;
                            }
                        }
                    }
                    #endregion
                    _friendList.AddFriendsToFile(logPass[0], choosedGame, localFriends);
                    tempDictUsing++;
                    _options = null;
                    _driver.Dispose();
                    //_driver.Quit();
                    if (!check)
                        break;
                }
                dictPos = 0;
                tempDictUsing = 0;
            }
        }

        public void CheckAndMail(string choosedGame, string message)
        {
            string[] temps = choosedGame.Split(' ');
            string gameNames = null;
            for (int i = 0; i < temps.Length; i++)
            {
                gameNames += temps[i];
            }
            string pathCheck = Environment.CurrentDirectory + @"\BotFriendsFolder\" + gameNames;
            if (!File.Exists(pathCheck))
            {
                MessageBox.Show("Ошибка! Нет информации о друзьях для данной игры", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Environment.CurrentDirectory);
            service.HideCommandPromptWindow = true;


            List<Dictionary<string, object>> proxy = _proxy.Proxy;
            decimal dictSwitch = _userClass.GetUsers.Count / _proxy.Count;
            dictSwitch = Math.Floor(dictSwitch);

            int dictPos = 0;
            int tempDictUsing = 0;

            foreach (string user in _userClass.GetUsers)
            {
                string[] logPass = user.Split(':');

                if (tempDictUsing % dictSwitch == 0 && tempDictUsing != 0)
                    dictPos++;

                Dictionary<string, object> curProxy = proxy[dictPos];
                ReOption(curProxy);

                #region CRX_PROXY_INIT
                _driver = new ChromeDriver(service, _options);
                _driver.Manage().Window.Maximize();
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                _driver.FindElementById("login").SendKeys(curProxy["login"].ToString());
                _driver.FindElementById("password").SendKeys(curProxy["password"].ToString());
                _driver.FindElementById("save").Click();
                Thread.Sleep(100);
                #endregion

                _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

                #region LOGIN
                _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
                IsRecaptchaExist();
                #endregion

                _driver.Navigate().GoToUrl("https://vk.com/im?act=create");

                #region CREATE_WORKSPACE
                IWebElement friendTextBox = _driver.FindElementByXPath("//*[@id=\"im_dialogs_creation\"]");
                bool check = false;

                #region HELL
                string[] temp = choosedGame.Split(' ');
                string gameName = null;
                for (int i = 0; i < temp.Length; i++)
                {
                    gameName += temp[i];
                }
                string path = Environment.CurrentDirectory + @"\BotFriendsFolder\" + gameName + @"\" + logPass[0] + ".txt";
                #endregion

                if (File.Exists(path))
                    foreach (string friend in _friendList.GetAllFriendsFromFile(logPass[0], choosedGame))
                    {
                        friendTextBox.Clear();
                        friendTextBox.SendKeys(friend);
                        try { _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div").Click(); check = true; } catch { }
                        Thread.Sleep(1000);
                    }
                #endregion
                if (check)
                {
                    #region SENDMESSAGE
                    _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[4]/div/button").Click();
                    _driver.FindElementByXPath("//*[@id=\"im_editable0\"]").SendKeys(message);

                    _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[3]/div[2]/div[4]/div[3]/div[4]/div[1]/button").Click();
                    Thread.Sleep(2000);
                    #endregion
                }
                _driver.Close();
                _driver.Quit();
            }
        }

        private bool IsRecaptchaExist()
        {
            Thread.Sleep(3000);
            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                _driver.SwitchTo().Frame(_driver.FindElement(By.XPath("//*[@id=\"recaptcha0\"]/div/div/iframe")));
                IWebElement elem = _driver.FindElement(By.ClassName("recaptcha-checkbox-checkmark"));
                return true;
            }
            catch
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                return false;
            }
            finally
            {
                _driver.SwitchTo().DefaultContent();
            }
        }
    }
}
