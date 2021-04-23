using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Diagnostics;

namespace MapApp.Tests
{
    public class TestsBase
    {

        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string ApplicationPath = @"..\..\..\..\MailApp\bin\Debug\net5.0-windows\MailApp.exe";
        private const string DeviceName = "WindowsPC";
        private const int WaitForAppLaunch = 5;
        private const string WinAppDriverPath = @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";
        private static Process winAppDriverProcess;

        public WindowsDriver<WindowsElement> AppSession { get; private set; }
        public WindowsDriver<WindowsElement> DesktopSession { get; private set; }

        public void Initialize()
        {
            StartWinAppDriver();

            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", ApplicationPath);
            appiumOptions.AddAdditionalCapability("deviceName", DeviceName);
            appiumOptions.AddAdditionalCapability("ms:waitForAppLaunch", WaitForAppLaunch);

            this.AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
            Assert.IsNotNull(AppSession);
            Assert.IsNotNull(AppSession.SessionId);

            AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

            AppiumOptions optionsDesktop = new AppiumOptions();
            optionsDesktop.AddAdditionalCapability("app", "Root");
            optionsDesktop.AddAdditionalCapability("deviceName", DeviceName);
            DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), optionsDesktop);

            CloseTrialDialog();
        }

        public void Cleanup()
        {
            // Close the session
            if (AppSession != null)
            {
                AppSession.Close();
                AppSession.Quit();
            }

            // Close the desktopSession
            if (DesktopSession != null)
            {
                DesktopSession.Close();
                DesktopSession.Quit();
            }
        }

        public static void StopWinappDriver()
        {
            // Stop the WinAppDriverProcess
            if (winAppDriverProcess != null)
            {
                foreach (var process in Process.GetProcessesByName("WinAppDriver"))
                {
                    process.Kill();
                }
            }
        }

        private static void StartWinAppDriver()
        {
            ProcessStartInfo psi = new ProcessStartInfo(WinAppDriverPath);
            psi.UseShellExecute = true;
            psi.Verb = "runas"; // run as administrator
            winAppDriverProcess = Process.Start(psi);
        }

        protected void SelectAllText()
        {
            Actions action = new Actions(AppSession);
            action.KeyDown(Keys.Control).SendKeys("a");
            action.KeyUp(Keys.Control);
            action.Perform();
        }

        protected void PerformDelete()
        {
            Actions action = new Actions(AppSession);
            action.SendKeys(Keys.Delete);
            action.Perform();
        }

        protected void PerformEnter()
        {
            Actions action = new Actions(AppSession);
            action.SendKeys(Keys.Enter);
            action.Perform();
        }

        protected void WriteText(string text)
        {
            Actions action = new Actions(AppSession);
            action.SendKeys(text);
            action.Perform();
        }

        protected void CloseTrialDialog()
        {
            this.GetElementByName("Telerik UI for WPF Trial").FindElementByName("Cancel").Click();
        }
    }
}