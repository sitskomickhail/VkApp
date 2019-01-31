using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using VkApp.Models;
using VkApp.FileManager;
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
            //_options.AddArgument("-headless");
            //_options.AddArgument("--incognito");
            _options.AddArgument("--safebrowsing-disable-download-protection");
            _options.AddExtension("ProxyAutoAuth.crx");
            _options.ToCapabilities();

            Proxy prox = new Proxy();
            prox.IsAutoDetect = false;
            prox.Kind = ProxyKind.Manual;
            prox.HttpProxy = prox.SslProxy = $"{proxy["ip"]}:{proxy["port"]}";
            prox.SocksUserName = proxy["login"].ToString();
            prox.SocksPassword = proxy["password"].ToString();
            _options.Proxy = prox;
        }

        private void SecretStart(string choosedGame)
        {
            List<string> links = _links.GetLinks(choosedGame);

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Environment.CurrentDirectory);
            service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(service);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");
            string[] logPass = _userClass.GetUsers[0].Split(':');
            #region LOGIN
            _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
            _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
            _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
            #endregion

            foreach (var group in _links.GetLinks(choosedGame))
            {
                List<IWebElement> elems = new List<IWebElement>();
                _driver.Navigate().GoToUrl(group);
                _driver.FindElementByXPath("//*[@id=\"group_followers\"]/a/div/span[1]").Click();

                foreach (IWebElement item in _driver.FindElements(By.ClassName("fans_fan_lnk")))
                {
                    elems.Add(item);
                    if (elems.Count == 30)
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
            _driver.Close();
        }

        public void StartAdding(string choosedGame)
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

            while (tempCount <= 200 || tempCount == _friendLinks.Count)
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
                    #endregion
                    if (IsRecaptchaExist())
                    {
                        tempDictUsing++;
                        continue;
                    }

                    #region ADD_FRIEND
                    List<string> localFriends = new List<string>();
                    for (int i = tempCount; i < _friendLinks.Count; i++)
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


                                _driver.FindElementByClassName("page_actions_dd_label").Click();
                                _driver.FindElementByXPath("//*[@id=\"page_actions_wrap\"]/div[2]/a[2]").Click();
                                _driver.FindElementById("preq_input").SendKeys($"Привет. Добваляю для игры {choosedGame}");
                                _driver.FindElementByClassName("flat_button").Click();

                                localFriends.Add(_savedNames[i]);
                                tempCount++;
                                Thread.Sleep(Randomer.Next(40000, 65000));
                            }
                            catch { }
                        }
                    }
                    #endregion

                    _friendList.AddFriendsToFile(logPass[0], choosedGame, localFriends);
                    tempDictUsing++;
                    _options = null;
                    _driver.Close();
                }

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
        }
    }
}
