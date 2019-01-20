using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        private ChromeOptions _options;
        private Users _userClass;
        private Dictionary<string, string> _usersDict;

        public VK_Navigate()
        {
            _options = new ChromeOptions();
            //_options.AddArgument("-headless");
            _driver = new ChromeDriver(_options);
            
            _userClass = new Users();
            _links = new GroupLinks();
            _usersDict = _userClass.GetUsersDictionary;
        }

        public void TestWork(string message, string xpathForFile = null)
        {
            List<string> links = new List<string>();
            List<string> savedNames = new List<string>();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

            _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(_usersDict["login"]);
            _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(_usersDict["password"]);
            _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();


            _driver.Navigate().GoToUrl("https://vk.com/club176816086");
            _driver.FindElementByXPath("//*[@id=\"group_followers\"]/a/div/span[1]").Click();

            
            List<IWebElement> elems = new List<IWebElement>();
            foreach (IWebElement item in _driver.FindElements(By.ClassName("fans_fan_lnk")))
            {
                elems.Add(item);
            }

            foreach (IWebElement item in elems)
            {
                links.Add(item.GetAttribute("href"));
                savedNames.Add(item.Text);
            }

            //links.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row158117675\"]/div[2]/a").GetAttribute("href"));
            //savedNames.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row158117675\"]/div[2]/a").Text);

            //links.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row512032023\"]/div[2]/a").GetAttribute("href"));
            //savedNames.Add(_driver.FindElementByXPath("//*[@id=\"fans_fan_row512032023\"]/div[2]/a").Text);

            AddFriends(links);



            _driver.FindElementByXPath("//*[@id=\"l_msg\"]/a/span/span[3]").Click();
            _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[2]/div[1]/div/div[1]/div[2]/div").Click();
            foreach (string user in savedNames)
            {
                user.Replace('\r', ' ');
                user.Replace('\n', new char());
                _driver.FindElementByXPath("//*[@id=\"im_dialogs_creation\"]").SendKeys(user);
                _driver.FindElementByXPath("//*[@id=\"olist_item_wrap158117675\"]/div/div[3]").Click();
                Thread.Sleep(1000);
            }

            _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[4]/div/button").Click();
            _driver.FindElementByXPath("//*[@id=\"im_editable0\"]").SendKeys(message);
            _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[3]/div[2]/div[4]/div[3]/div[4]/div[1]/button").Click();
        }

        private void AddFriends(IEnumerable<string> userList)
        {
            foreach (string addUser in userList)
            {
                _driver.Navigate().GoToUrl(addUser);
                _driver.FindElementByXPath("//*[@id=\"friend_status\"]/div[1]/button").Click();

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
                Thread.Sleep(1000);
            }
        }
    }
}
