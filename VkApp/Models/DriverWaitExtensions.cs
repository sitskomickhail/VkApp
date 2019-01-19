using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace VkApp.Models
{
    public static class DriverWaitExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by) ?? null);
            }
            return driver.FindElement(by);
        }


        public static void Type_Like_Human(IWebElement element, string str)
        {
            foreach (char c in str)
            {
                element.SendKeys(c.ToString());
                Thread.Sleep(Randomer.Next(100, 400));
            }
        }
    }
}