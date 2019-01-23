using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using VkApp.Models;
using VkApp.FileManager;

namespace VkApp.VKWorker
{
    class VK_Navigate
    {
        private ChromeDriver _driver;
        private GroupLinks _links;
        private FriendsClass _friendList;
        private ChromeOptions _options;
        private Users _userClass;
        private ProxyUsing _proxy;

        public VK_Navigate()
        {
            _proxy = new ProxyUsing();
            _friendList = new FriendsClass();
            _userClass = new Users();
            _links = new GroupLinks();
        }

        public void ReOption(Dictionary<string, object> proxy)
        {
            _options = new ChromeOptions();
            //_options.AddArgument("-headless");
            _options.AddArgument("--incognito");
            _options.AddArgument("--safebrowsing-disable-download-protection");
            //_options.AddArguments("--proxy-server=http://user:password@yourProxyServer.com:8080");
            _options.Proxy = new Proxy(proxy);
        }


        public void StartWork(string choosedGame)
        {
            List<string> links = _links.GetLinks(choosedGame);
            int choosedLink = 0;

            List<IWebElement> elems = new List<IWebElement>(); int curentPosition = 0;
            ReOption(_proxy.Proxy);

            List<string> savedNames = new List<string>();
            int i = 1;
            foreach (string user in _userClass.GetUsers)
            {
                string[] logPass = user.Split(':');

                if ((_userClass.GetUsers.Count / _proxy.Count) == i && _userClass.GetUsers.Count > _proxy.Count)
                {
                    ReOption(_proxy.Proxy);
                    i = 1;
                }
                else if (_userClass.GetUsers.Count <= _proxy.Count)
                    ReOption(_proxy.Proxy);

                _driver = new ChromeDriver(_options);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

                #region LOGIN
                _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
                IsRecaptchaExist();
                #endregion
                //CHECKED REQUIRED :)
                #region GROUP_NAVIGATE
                if (elems.Count == 0)
                {
                    _driver.Navigate().GoToUrl(links[choosedLink]);
                    _driver.FindElementByXPath("//*[@id=\"group_followers\"]/a/div/span[1]").Click();
                    choosedLink++;
                }


                foreach (IWebElement item in _driver.FindElements(By.ClassName("fans_fan_lnk")))
                {
                    elems.Add(item);
                    if (elems.Count >= 250)
                        break;
                }

                for (int pos = curentPosition; pos < elems.Count; pos++)
                {
                    if (!_friendList.IsFriendExist(elems[pos].Text))
                    {
                        links.Add(elems[pos].GetAttribute("href"));
                        string friend = elems[pos].Text;
                        friend.Replace('\n', new char());
                        friend.Replace('\r', ' ');
                        savedNames.Add(friend);
                        if (savedNames.Count == 25)
                            break;
                    }
                    curentPosition++;
                }

                if (curentPosition >= 250)
                {
                    elems.RemoveRange(0, elems.Count);
                    curentPosition = 0;
                }

                _friendList.AddFriendsToFile(logPass[0], choosedGame ,savedNames);
                savedNames.RemoveRange(0, savedNames.Count);
                #endregion

                //links.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row158117675\"]/div[2]/a").GetAttribute("href"));
                //savedNames.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row158117675\"]/div[2]/a").Text);

                //links.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row512032023\"]/div[2]/a").GetAttribute("href"));
                //savedNames.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row512032023\"]/div[2]/a").Text);

                AddFriends(links);

                _driver.Close();
                i++;
            }
        }

        public void CheckAndMail(string message, string choosedGame, string xpathForFile = null)
        {
            ReOption(_proxy.Proxy);
            int i = 0;
            foreach (string user in _userClass.GetUsers)
            {
                string[] logPass = user.Split(':');

                if ((_userClass.GetUsers.Count / _proxy.Count) == i && _userClass.GetUsers.Count > _proxy.Count)
                {
                    ReOption(_proxy.Proxy);
                    i = 0;
                }
                else if (_userClass.GetUsers.Count <= _proxy.Count)
                    ReOption(_proxy.Proxy);

                ReOption(_proxy.Proxy);
                _driver = new ChromeDriver(_options);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

                #region LOGIN
                _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
                IsRecaptchaExist();
                #endregion

                _driver.FindElementByXPath("//*[@id=\"l_msg\"]/a/span/span[3]").Click();
                _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[2]/div[1]/div/div[1]/div[2]/div").Click();

                #region CREATE_WORKSPACE
                IWebElement friendTextBox = _driver.FindElementByXPath("//*[@id=\"im_dialogs_creation\"]");
                foreach (string friend in _friendList.GetAllFriendsFromFile(logPass[0], choosedGame))
                {
                    friendTextBox.Clear();
                    friendTextBox.SendKeys(friend);
                    try { _driver.FindElementByXPath("//*[@id=\"olist_item_wrap158117675\"]/div/div[3]").Click(); } catch { }
                    Thread.Sleep(1000);
                }
                #endregion

                #region SENDMESSAGE
                _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[4]/div/button").Click();
                _driver.FindElementByXPath("//*[@id=\"im_editable0\"]").SendKeys(message);
                _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[3]/div[2]/div[4]/div[3]/div[4]/div[1]/button").Click();
                #endregion
                _driver.Close();
                i++;
            }
        }


        private void AddFriends(IEnumerable<string> userList)
        {
            foreach (string addUser in userList)
            {
                _driver.Navigate().GoToUrl(addUser);
                _driver.FindElementByXPath("//*[@id=\"friend_status\"]/div[1]/button").Click();
                IsRecaptchaExist();
                Thread.Sleep(1000);
            }
        }

        private void IsRecaptchaExist()
        {
            Thread.Sleep(3000);
            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                _driver.SwitchTo().Frame(_driver.FindElement(By.XPath("//*[@id=\"recaptcha-element-container\"]/div/div/iframe")));
                _driver.FindElement(By.ClassName("recaptcha-checkbox-checkmark")).Click();

                RecaptchaSolve.SolveCaptcha(_driver);
            }
            catch
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
        }
    }
}
