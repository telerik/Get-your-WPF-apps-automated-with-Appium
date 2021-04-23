using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace MapApp.Tests
{
    [TestClass]
    public class MailAppTests : TestsBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.Initialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.Cleanup();
        }

        [ClassCleanup]
        public static void ClassCleanusp()
        {
            StopWinappDriver();
        }

        [TestMethod]
        public void AssureEmailFieldsAreFilledInTest()
        {
            string replyTo = "SethBarley@telerikdomain.es";
            string replyCC = "SethCavins@telerikdomain.uk";
            string subject = "RE: Let's have a party for new years eve";

            this.GetButtonReply().Click();
            Thread.Sleep(1000);

            Assert.AreEqual(replyTo, this.GetFieldReplyTo().Text);
            Assert.AreEqual(replyCC, this.GetFieldReplyCc().Text);
            Assert.AreEqual(subject, this.GetFieldSubject().Text);
        }

        [TestMethod]
        public void ReplyToAnEmailTest()
        {
            string textToWrite = "Writing some text here...";

            this.GetButtonReply().Click();
            this.GetRichTextBoxReply().Click();
            this.SelectAllText();
            this.PerformDelete();
            this.WriteText(textToWrite);

            Assert.AreEqual(textToWrite, this.GetRichTextBoxReply().Text);

            this.GetButtonSendEmail().Click();

            Assert.AreEqual(@"Send's command executed.", this.GetElementByName(@"Send's command executed.").Text);

            // Close information dialog
            this.GetElementByName("OK").Click();
            
        }

        [TestMethod]
        public void MarkMessageAsReadTest()
        {
            this.GetTabUnreadEmail().Click();
            this.GetUnreadEmailCellByFromAddress("SethCavins@telerikdomain.uk").Click();
            this.GetUnreadEmailCellByFromAddress("JimmieFields@telerikdomain.eu").Click();

            Assert.AreEqual("[31]", this.GetUInboxUnreadMessagesCount().Text);
        }
    }
}
