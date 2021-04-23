using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Data;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using Telerik.Windows.Documents.Model;

namespace MailApp
{
    public static class SampleContentService
    {
        private static List<Folder> _folders;
        private static readonly List<string> content = new List<string>
        {
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
            "At Telerik Corporation, we realize that customer feedback is becoming more and more critical to doing business efficiently. That's why I'm writing to inform you that we are creating a new community web site for the following control packages: Silverlight controls WPF controls WinForms There will be several support officers who will gather feedback about the products during the 6 month trial of the site. Your assigned support officer is Matti Karttunen. Feel free to send him mail with any questions or suggestions. Your feedback will really give our business a clear idea of how to best serve our customers and become more reliable. Our support officer Matti Karttunen will send a mail to talk over the matter further and see if you accept our test site account and will participate in this clients program. Thank you for your attention. ",
            "RadRichTextBox is a control that offers Microsoft Word-like authoring and editing in your applications using a familiar interface for end users. The experience is enhanced by the support of multilevel bullet and numbered lists, tables, inline and floating images. More advanced options include external and in-document hyperlinks, bookmarks and comments.  The control can preview and edit text in various languages thanks to the Input Method Editor and the Right-to-Left support, which makes it an appropriate choice in a wide range of applications. Even more sweetness is added by the integrated spell-checker and image editor allowing immediate detection and correction of spelling mistakes and in-place tweaking of images. ",
            "RadRichTextBox provides two convenient ways to insert additional information about content in the document without inserting it in the main body directly. These are the footnotes and endnotes, which provide a natural way for introducing supplementary information without interrupting the flow of thought.",
            "In English, however, the meaning is rather different. It means a word that has been created by combining the morphemes of two other words, just how 'entrance' and 'coat' have been combined to form the 'porte-manteau' term. Some of the examples include chocoholic (from 'chocolate' and 'holic', as in 'alcoholic'), smog (from 'smoke'and 'fog') and wurly (from 'wavy' and 'curly', used to describe hair)4. The popularity of each new developed term gains determines if the word will be used only by a closed circle of people, or will become part of the speech, and subsequently – of the language.",
            "RadRichTextBox has a concept of styles which allows you to use a set of predefined styling rules for document elements. One of the types of styles which you can use in the control is Table styles. They can be applied and manipulated using the table styles gallery which is part of the predefined RadRichTextBoxRibbonUI. Once a style is applied to a particular table every change in its properties will result in a change in the instance in the current document as well. All properties related to the formatting can be viewed and modified through the Modify style dialog. You can also change the Table Style Options from the Design ribbon tab.",
        };

        private static readonly List<string> emailSubjects = new List<string>
        {
            "Let's have a party for new years eve",
            "New application brainstorming",
            "Happy Birthday",
            "Don't miss this month's discounts",
            "Action required"
        };

        private static readonly List<string> firstNames = new List<string>
        {
            "Ricky",
            "Alan",
            "Seth",
            "John",
            "Daniel",
            "Jimmie",
            "Tom",
            "Cruz",
            "Vince",
            "Colin"
        };

        private static readonly List<string> lastNames = new List<string>
        {
            "Barley",
            "Fields",
            "Hillsman",
            "Hosking",
            "Gabel",
            "Zander",
            "Cavins",
            "Benz",
            "Trudell",
            "Brim"
        };

        private static readonly List<string> domains = new List<string>
        {
            "telerikdomain.com",
            "telerikdomain.net",
            "telerikdomain.eu",
            "telerikdomain.de",
            "telerikdomain.es",
            "telerikdomain.bg",
            "telerikdomain.ru",
            "telerikdomain.uk",
            "telerikdomain.fr",
            "telerikdomain.it"
        };

        public static QueryableCollectionView GetEmails(int size, string keyword)
        {
            var now = DateTime.Now;
            var result = new ObservableCollection<Email>();
            int dateIncrement = 0;
            for (int i = 1; i <= size; i++)
            {
                var subject = SampleContentService.GetRandomEmailSubject(i);
                var status = i % 3 == 0 ? Enums.EmailStatus.Unread : Enums.EmailStatus.Read;
                var sender = SampleContentService.GetRandomEmailAddress(i);
                var recipient = SampleContentService.GetRandomEmailAddress(i + 1);
                var carbonCopy = SampleContentService.GetRandomEmailAddress(i + 2);
                if (i % 10 == 0)
                {
                    dateIncrement++;
                }

                var receivedDate = now.AddDays(-dateIncrement);
                XamlFormatProvider xamlFormatProvider = new XamlFormatProvider();
                var content = xamlFormatProvider.Export(CreateDocument(i));
                result.Add(new Email(sender, recipient, subject, receivedDate)
                {
                    Content = content,
                    Status = status,
                    CarbonCopy = carbonCopy
                });
            }

            return new QueryableCollectionView(result);
        }

        public static List<Folder> GetRootEmailFolders()
        {
            if (_folders == null)
            {
                _folders = new List<Folder>();
                var personalFolders = new List<Folder>
                {
                    new Folder
                    {
                        Name = "Inbox",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(100, "Inbox")
                    },
                    new Folder
                    {
                        Name = "Sent Items",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(20, "Sent Items")
                    },
                    new Folder
                    {
                        Name = "Deleted Items",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(10, "Deleted Items")
                    }
                };
                var publicFolders = new List<Folder>
                {
                    new Folder
                    {
                        Name = "Sports News",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(200, "Sports News")
                    },
                    new Folder
                    {
                        Name = "Global News",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(300, "Global News")
                    },
                    new Folder
                    {
                        Name = "IT News",
                        Folders = null,
                        Emails = SampleContentService.GetEmails(100, "IT News")
                    }
                };

                _folders.Add(new Folder()
                {
                    Name = "Mark@telerikdomain.com",
                    Folders = personalFolders
                });
                _folders.Add(new Folder()
                {
                    Name = "Public Folders - Mark@telerikdomain.com",
                    Folders = publicFolders
                });
            }
            return _folders;
        }

        public static ObservableCollection<OutlookSection> GetOutlookSections()
        {
            var result = new ObservableCollection<OutlookSection>();
            var rootEmailFolders = SampleContentService.GetRootEmailFolders();
            var selectedItem = rootEmailFolders[0].Folders.OfType<Folder>().ToList<Folder>().FirstOrDefault(f => f.Name == "Inbox");
            result.Add(new OutlookSection
            {
                Name = "Mail",
                Content = rootEmailFolders,
                SelectedItem = selectedItem,
                IconPath = "../Images/mail_32x32.png",
                MinimizedIconPath = "../Images/email_16x16.png"
            });

            return result;
        }

        private static RadDocument CreateDocument(int seed)
        {
            var document = new RadDocument();
            var section = new Section();
            var paragraph1 = new Paragraph();
            section.Blocks.Add(paragraph1);
            var paragraph2 = new Paragraph();
            paragraph2.TextAlignment = Telerik.Windows.Documents.Layout.RadTextAlignment.Center;
            var span1 = new Span("Thank you for choosing Telerik");
            paragraph2.Inlines.Add(span1);
            var span2 = new Span();
            span2.Text = " RadRichTextBox!";
            span2.FontWeight = FontWeights.Bold;
            paragraph2.Inlines.Add(span2);
            section.Blocks.Add(paragraph2);
            var rand = new Random(seed);
            var paragraph3 = new Paragraph();
            var span3 = new Span(content[rand.Next(0, 6)]);
            paragraph3.Inlines.Add(span3);
            section.Blocks.Add(paragraph3);
            section.Blocks.Add(new Paragraph());
            document.Sections.Add(section);

            return document;
        }

        private static string GetRandomEmailSubject(int seed)
        {
            var rand = new Random(seed);

            return emailSubjects[rand.Next(0, emailSubjects.Count - 1)];
        }

        private static string GetRandomEmailAddress(int seed)
        {
            var rand = new Random(seed);
            var firstName = SampleContentService.firstNames[rand.Next(0, firstNames.Count - 1)];
            var lastName = SampleContentService.lastNames[rand.Next(0, lastNames.Count - 1)];
            var domain = SampleContentService.domains[rand.Next(0, domains.Count - 1)];

            return string.Format("{0}{1}@{2}", firstName, lastName, domain);
        }
    }
}