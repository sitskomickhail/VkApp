using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using VkApp.FileManager;
using System.IO;
using System.Windows;
using System.Text;
using VkNet.Model;
using VkNet;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Utils;
using static LogInfo.LogIO;
using LogInfo;

namespace VkApp.Models
{
    class VkNavigate
    {
        private FirefoxDriver _driver;
        private FirefoxOptions _options;
        private FriendsClass _friendList;
        private Users _userClass;
        private Logging _logger = new Logging(WriteLog);
        private MainWindow _tbLog = System.Windows.Application.Current.Windows[0] as MainWindow;


        private List<string> _friendLinks;
        List<string> _savedNames;

        public VkNavigate()
        {
            _options = new FirefoxOptions();
            _friendList = new FriendsClass();
            _userClass = new Users();
            _friendLinks = new List<string>();
            _savedNames = new List<string>();

            _logger += _tbLog.ShowLog;
        }

        internal void CloseDrivers()
        {
            if (_driver != null)
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                _driver.Quit();
                //_driver.Dispose();
            }
        }

        private void GetGroups(string choosedGame)
        {
            string strGame;
            if (choosedGame == "Metro 2033")
                strGame = "Метро 2033";
            else
                strGame = "Легенда о вампире";

            List<string> linksIds = new List<string>();

            VkApi api = new VkApi();
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GetGroups", LogMessage = "Подключение прокси...", UserName = String.Empty });
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = 6866405,
                Login = "375336732616",
                Password = "хорошенькая19",
                Settings = Settings.Offline,
                Host = "5.188.75.148",
                Port = 5285,
                ProxyLogin = "user2775",
                ProxyPassword = "9dq7rz"
            });
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = "Прокси подключено!", UserName = String.Empty });

            var grs = api.Groups.Search(new GroupsSearchParams() { Query = $"{strGame}", Count = Randomer.Next(17, 21), Sort = GroupSort.Growth });
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = $"Найдено групп по игре {strGame}: {grs.Count}", UserName = String.Empty });

            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = $"Сбор аккаунтов...", UserName = String.Empty });
            foreach (var group in grs)
            {
                VkCollection<User> uss = new VkCollection<User>(1000, new List<User>());
                try
                {
                    uss = api.Groups.GetMembers(new GroupsGetMembersParams() { Count = Randomer.Next(950, 1000), GroupId = group.Id.ToString(), Fields = UsersFields.All });
                }
                catch
                {
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = $"Ошибка при сборе информации о группе: {group.Name}, {group.Id}", UserName = String.Empty });
                    continue;
                }
                foreach (User user in uss)
                {
                    if (user.FirstName != "DELETED")
                    {
                        linksIds.Add("https://vk.com/" + user.Domain);
                        _savedNames.Add(user.FirstName + " " + user.LastName);
                    }
                }
            }
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = $"Всего собрано: {linksIds.Count} аккаунтов", UserName = String.Empty });

            for (int i = 0; i < 500; i++)
                _friendLinks.Add(linksIds[Randomer.Next(0, linksIds.Count)]);
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "GG", LogMessage = $"Выбрано 500 случайных аккаунтов", UserName = String.Empty });
        }

        public bool StartAdding(string choosedGame, string addMessage)
        {
            GetGroups(choosedGame);

            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Инициализация...", UserName = String.Empty });

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(Environment.CurrentDirectory);
            service.HideCommandPromptWindow = true;

            decimal dictSwitch = _userClass.GetUsers.Count / 10; // 10 - количество firefox профилей
            dictSwitch = Math.Floor(dictSwitch);

            int dictPos = 0;
            int tempCount = 0;
            int tempDictUsing = 0;
            int hrefsListed = 0;
            bool checkWhile = true;
            DateTime date = new DateTime();
            while (tempCount < 200 && hrefsListed < _friendLinks.Count)
            {
                foreach (var user in _userClass.GetUsers)
                {
                    if (tempDictUsing % dictSwitch == 0 && tempDictUsing != 0)
                        dictPos++;

                    FirefoxProfile profile = new FirefoxProfile(Environment.CurrentDirectory + $@"\Fires\fireuser{dictPos}");
                    _options = new FirefoxOptions();
                    _options.Profile = profile;
                    _options.AddArgument("--headless");
                    string[] logPass = user.Split(':');

                    _driver = new FirefoxDriver(service, _options);
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Открытие драйвера", UserName = logPass[0] });

                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    try
                    {
                        _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");
                    }
                    catch { }
                    _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2");

                    #region LOGIN
                    _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                    _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                    _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();
                    #endregion


                    if (IsRecaptchaExist())
                    {
                        tempDictUsing++;
                        _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Ошибка! Найдена рекапча... Идет решение проблемы...", UserName = logPass[0] });
                        Thread.Sleep(40000);
                        continue;
                    }
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Пользователь успешно залогинен", UserName = logPass[0] });
                    #region ADD_FRIEND
                    List<string> localFriends = new List<string>();
                    bool check = true;
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Добавление друзей...", UserName = logPass[0] });
                    if (DateTime.Now.Hour - date.Hour == 1)
                    {
                        _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = $"Добавлено друзей: {tempCount} за время работы", UserName = String.Empty });
                        date = DateTime.Now;
                    }


                    for (int i = hrefsListed; i < _friendLinks.Count; i++)
                    {
                        if (!_friendList.IsFriendExist(_savedNames[i]))
                        {
                            _driver.Navigate().GoToUrl(_friendLinks[i]); //second user no enabling
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
                        else
                            hrefsListed++;
                    }
                    if (localFriends.Count == 0)
                    {
                        _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = "Нет друзей для добавления. Просьба обратиться к программисту за помощью...", UserName = String.Empty });
                        check = false;
                    }
                    #endregion
                    if (localFriends.Count > 0)
                        _friendList.AddFriendsToFile(logPass[0], choosedGame, localFriends);
                    tempDictUsing++;
                    _options = null;
                    _driver.Dispose();
                    _driver.Quit();
                    if (!check)
                    {
                        checkWhile = false;
                        break;
                    }
                }
                if (!checkWhile)
                    break;
                dictPos = 0;
                tempDictUsing = 0;
            }
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "AF", LogMessage = $"Заявки успешно отосланы! Общее количество разосланных заявок: {tempCount}", UserName = String.Empty });
            return true;
        }

        public bool CheckAndMail(string choosedGame, string message)
        {
            _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Инициализация рассылки", UserName = String.Empty });

            message = message.Replace("\r", String.Empty);
            string[] temps = choosedGame.Split(' ');
            string gameNames = null;
            for (int i = 0; i < temps.Length; i++)
            {
                gameNames += temps[i];
            }
            string pathCheck = Environment.CurrentDirectory + @"\BotFriendsFolder\" + gameNames;
            if (!Directory.Exists(pathCheck))
            {
                MessageBox.Show("Ошибка! Нет информации о друзьях для данной игры\nВозможно не существует необходимых файлов", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Не найденно данных о добавленных друзьях", UserName = String.Empty });
                return false;
            }


            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(Environment.CurrentDirectory);
            service.HideCommandPromptWindow = true;


            decimal dictSwitch = _userClass.GetUsers.Count / 10; // 10 - количество firefox профилей
            dictSwitch = Math.Floor(dictSwitch);

            int dictPos = 0;
            int tempDictUsing = 0;

            foreach (string user in _userClass.GetUsers)
            {
                string[] logPass = user.Split(':');

                if (tempDictUsing % dictSwitch == 0 && tempDictUsing != 0)
                    dictPos++;

                FirefoxProfile profile = new FirefoxProfile(Environment.CurrentDirectory + $@"\Fires\fireuser{dictPos}");
                _options = new FirefoxOptions();
                _options.Profile = profile;
                _options.AddArgument("--headless");

                _driver = new FirefoxDriver(service, _options);

                try { _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2"); }
                catch { _driver.Navigate().GoToUrl("https://vk.com/login?to=aW0%2FYWN0PQ--&u=2"); }


                #region LOGIN
                _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Логин пользователя", UserName = logPass[0] });
                _driver.FindElementByXPath("//*[@id=\"email\"]").SendKeys(logPass[0]);
                _driver.FindElementByXPath("//*[@id=\"pass\"]").SendKeys(logPass[1]);
                _driver.FindElementByXPath("//*[@id=\"login_button\"]").Click();

                if (IsRecaptchaExist())
                {
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Логин пользователя", UserName = logPass[0] });
                    MessageBox.Show("Ошибка в работе программы. Перезапустите приложение в ближайшее время", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _driver.Close();
                    _driver.Quit();
                    return false;
                }
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

                int addFriends = 0;
                _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Сбор пользователей для рассылки", UserName = logPass[0] });
                if (File.Exists(path))
                    foreach (string friend in _friendList.GetAllFriendsFromFile(logPass[0], choosedGame))
                    {
                        friendTextBox.Clear();
                        friendTextBox.SendKeys(friend);
                        try { _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div").Click(); addFriends++; check = true; } catch { }
                        Thread.Sleep(1000);
                    }

                #endregion
                if (check)
                {
                    #region SENDMESSAGE
                    _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[1]/div/div[4]/div/button").Click();
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = $"Пользователи добавлены ({addFriends}), чат создан", UserName = logPass[0] });
                    _driver.FindElementByXPath("//*[@id=\"im_editable0\"]").SendKeys(message);

                    _driver.FindElementByXPath("//*[@id=\"content\"]/div/div[1]/div[3]/div[2]/div[4]/div[3]/div[4]/div[1]/button").Click();
                    Thread.Sleep(2000);
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Сообщения разосланы!", UserName = logPass[0] });
                    #endregion
                }
                else
                {
                    _logger.Invoke(LogIO.path, new Log { Date = DateTime.Now, Method = "SM", LogMessage = "Нет добавленных пользователей", UserName = logPass[0] });
                }
                tempDictUsing++;
                _driver.Close();
                _driver.Quit();
            }
            Directory.Delete(pathCheck);
            return true;
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
