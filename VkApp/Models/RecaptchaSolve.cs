using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace VkApp.Models
{
    public static class RecaptchaSolve
    {
        public static void SolveCaptcha(IWebDriver driver)
        {
            int framePos = 9;

            Actions builder = new Actions(driver);

            //driver.SwitchTo().Frame(DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"friend_status\"]/div[1]/button"), 3));
            //DriverWaitExtensions.FindElement(driver, By.ClassName("recaptcha-checkbox-checkmark"), 7).Click();
            #region AUDIO_BUTTON__CLICK
            IWebElement btn;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            Thread.Sleep(3000);
            try
            {
                driver.SwitchTo().DefaultContent();
                //driver.SwitchTo().Frame(driver.FindElements(By.XPath("iframe"))[framePos]);
                driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/div[13]/div[2]/iframe")));
                btn = driver.FindElement(By.XPath("//*[@id=\"recaptcha-audio-button\"]"));
                Thread.Sleep(3000);
                btn.Click();
            }
            catch (Exception)
            {
                for (int j = 0; j < 10; j++)
                {
                    bool check = false;
                    driver.SwitchTo().DefaultContent();
                    try
                    {
                        driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[j]);
                        btn = driver.FindElement(By.XPath("//*[@id=\"recaptcha-audio-button\"]"));
                        Thread.Sleep(1000);
                        btn.Click();
                        framePos = j;
                        check = true;
                    }
                    catch (Exception) { }
                    if (check)
                        break;
                }
            }
            #endregion
            try { driver.FindElement(By.ClassName("rc-audiochallenge-tdownload-link")).Click(); }
            catch
            {
                #region GO_SOLVE!
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(140);
                Thread.Sleep(127000);
                while (true)
                {
                    try
                    {
                        driver.SwitchTo().DefaultContent();
                        driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id=\"recaptcha0\"]/div/div/iframe")));
                        IWebElement captchaClick = driver.FindElement(By.ClassName("recaptcha-checkbox-checkmark"));
                        captchaClick.Click();
                        break;
                    }
                    catch { Thread.Sleep(5025); }
                }
                #endregion
                Thread.Sleep(5000);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                #region AUDIO_BUTTON__DOWNLOAD
                try
                {
                    driver.SwitchTo().DefaultContent();
                    driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[framePos]);
                    btn = driver.FindElement(By.ClassName("rc-audiochallenge-tdownload-link"));
                    btn.Click();
                }
                catch (Exception)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        bool check = false;
                        driver.SwitchTo().DefaultContent();
                        try
                        {
                            driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[j]);
                            btn = driver.FindElement(By.ClassName("rc-audiochallenge-tdownload-link"));
                            btn.Click();
                            framePos = j;
                            check = true;
                        }
                        catch (Exception) { }
                        if (check)
                            break;
                    }
                }
                #endregion
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            List<string> tabs = new List<string>(driver.WindowHandles);
            driver.SwitchTo().Window(tabs[tabs.Count - 1]);
            #region DOWNLOAD/GET_CODE
            DownloadRequest(driver.Url);
            GetCode(driver);
            #endregion

            driver.SwitchTo().Window(tabs[0]);

            #region CREATE_ANSWER
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[framePos]);

            IWebElement audioTB = DriverWaitExtensions.FindElement(driver, By.Id("audio-response"), 2);
            audioTB.Clear();
            DriverWaitExtensions.Type_Like_Human(audioTB, Clipboard.GetText());

            try
            {
                driver.FindElement(By.Id("recaptcha-verify-button")).Click();
                try
                {
                    driver.SwitchTo().DefaultContent();
                    driver.FindElement(By.XPath("/html/body/div[2]/div/div/div/div[1]/div/div/div[3]/div/button/span")).Click();
                }
                catch { RetryCaptcha(driver, framePos); }
            }
            catch { RetryCaptcha(driver, framePos); }

            driver.SwitchTo().DefaultContent();
            #endregion
        }

        private static void DownloadRequest(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = WebRequestMethods.Http.Get;
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream httpResponseStream = httpResponse.GetResponseStream();

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;

            FileStream fileStream = File.Create("audioRecaptcha.mp3");
            while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
        }

        private static void RetryCaptcha(IWebDriver driver, int framePos)
        {
            Thread.Sleep(127000);
            while (true)
            {
                try
                {
                    driver.SwitchTo().DefaultContent();
                    driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id=\"recaptcha0\"]/div/div/iframe")));
                    driver.FindElement(By.ClassName("recaptcha-checkbox-checkmark")).Click();

                    break;
                }
                catch { }
            }

            #region AUDIO_BUTTON__DOWNLOAD
            driver.SwitchTo().DefaultContent();
            #region AUDIO_BUTTON__DOWNLOAD
            try
            {
                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[9]);
                driver.FindElement(By.ClassName("rc-audiochallenge-tdownload-link")).Click();
            }
            catch (Exception)
            {
                for (int j = 0; j < 10; j++)
                {
                    bool check = false;
                    driver.SwitchTo().DefaultContent();
                    try
                    {
                        driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[j]);
                        driver.FindElement(By.ClassName("rc-audiochallenge-tdownload-link")).Click();
                        check = true;
                    }
                    catch (Exception) { }
                    if (check)
                        break;
                }
            }
            #endregion
            #endregion

            List<string> tabs = new List<string>(driver.WindowHandles);
            driver.SwitchTo().Window(tabs[tabs.Count - 1]);

            #region DOWNLOAD/GET_CODE
            DownloadRequest(driver.Url);
            GetCode(driver);
            #endregion

            driver.SwitchTo().Window(tabs[0]);

            #region CREATE_ANSWER
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[framePos]);

            IWebElement audioTB = DriverWaitExtensions.FindElement(driver, By.Id("audio-response"), 2);
            audioTB.Clear();
            DriverWaitExtensions.Type_Like_Human(audioTB, Clipboard.GetText());

            DriverWaitExtensions.FindElement(driver, By.Id("recaptcha-verify-button"), 2).Click();
            #endregion
        }

        private static void GetCode(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://realspeaker.net/");

            DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"app\"]/main/div/div/div/div/div[2]/div/button/div"), 7).Click();

            IWebElement uploadAudio = DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"uploader\"]/div/div/div[2]/input"), 5);
            uploadAudio.SendKeys($@"{Environment.CurrentDirectory}\audioRecaptcha.mp3");

            DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"app\"]/main/div/div/div/div/div[6]/div/button/div"), 10).Click();

            DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"app\"]/main/div/div[1]/div/div/div/ul/span/li[1]/div/div[2]/div/button[2]/div"), 5).Click();

            IWebElement topElem = DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"app\"]/main/div/div[1]/div/div/div/ul/span/li[1]/div/div[1]/div[1]"), 5);
            while (driver.Url == "https://realspeaker.net/media")
                topElem.Click();

            DriverWaitExtensions.FindElement(driver, By.XPath("//*[@id=\"app\"]/main/div/div[1]/div/div/span/div/nav/div/button/div"), 8).Click();
        }
    }
}