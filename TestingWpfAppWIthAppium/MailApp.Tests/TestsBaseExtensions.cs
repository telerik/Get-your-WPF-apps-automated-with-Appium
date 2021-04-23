using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace MapApp.Tests
{
    public static class TestsBaseExtensions
    {
        private const string ButtonReplydId = "buttonReply";
        private const string ButtonReplyAllId = "buttonReplyAll";
        private const string ButtonForwardId = "buttonForward";
        private const string RichTextBoxReplyId = "editableRichTextBox";
        private const string TabAllEmailsId = "tabAllEmails";
        private const string TabUnreadEmailsId = "tabUnreadEmails";
        private const string ButtonSendEmailId = "buttonSendEmail";

        // All the three fields have the same XPath
        private const string fieldsReplyToCcSubjectXPath = "/Window[@Name=\"Inbox - Mark@telerikdomain.com - My Application\"][@AutomationId=\"MyApplication\"]/Custom[@ClassName=\"MailView\"]/Custom[@ClassName=\"RadDocking\"][@Name=\"Rad Docking\"]/Custom[@ClassName=\"RadSplitContainer\"][@Name=\"Rad Split Container\"]/Tab[@ClassName=\"RadPaneGroup\"][@Name=\"Pane Group Base\"]/Edit[@ClassName=\"TextBox\"]";

        public static WindowsElement GetButtonReply(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(ButtonReplydId);
        }

        public static WindowsElement GetButtonReplyAll(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(ButtonReplyAllId);
        }

        public static WindowsElement GetButtonForward(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(ButtonForwardId);
        }

        public static WindowsElement GetRichTextBoxReply(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(RichTextBoxReplyId);

        }

        public static WindowsElement GetTabAllEmails(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(TabAllEmailsId);
        }

        public static WindowsElement GetTabUnreadEmail(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementByAccessibilityId(TabUnreadEmailsId);
        }

        public static WindowsElement GetFieldReplyTo(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementsByXPath(fieldsReplyToCcSubjectXPath)[0];
        }

        public static WindowsElement GetFieldReplyCc(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementsByXPath(fieldsReplyToCcSubjectXPath)[1];
        }

        public static WindowsElement GetFieldSubject(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementsByXPath(fieldsReplyToCcSubjectXPath)[2];
        }

        public static WindowsElement GetUnreadEmailCellByFromAddress(this TestsBase testsBase, string fromAddress)
        {

            return testsBase.AppSession.FindElementByName(fromAddress);
        }

        public static AppiumWebElement GetUInboxUnreadMessagesCount(this TestsBase testsBase)
        {
            return testsBase.AppSession.FindElementsByClassName("RadTreeViewItem")[0].FindElementsByClassName("TextBlock")[2];
        }

        public static WindowsElement GetButtonSendEmail(this TestsBase testBase)
        {
            return testBase.AppSession.FindElementByAccessibilityId(ButtonSendEmailId);
        }

        public static WindowsElement GetElementByName(this TestsBase testsBase, string elementName)
        {
            try
            {
                return testsBase.AppSession.FindElementByName(elementName);
            }
            catch
            {
                Logger.LogMessage("Element was not found using the AppSession. Trying to locate the element using the DesktopSession.");
            }

            return testsBase.DesktopSession.FindElementByName(elementName);
        }

        public static WindowsElement GetElementByAutomationId(this TestsBase testsBase, string automationId)
        {
            try
            {
                return testsBase.AppSession.FindElementByName(automationId);
            }
            catch
            {
                Logger.LogMessage("Element was not found using the AppSession. Trying to locate the element using the DesktopSession.");
            }

            return testsBase.DesktopSession.FindElementByName(automationId);
        }
    }

}
