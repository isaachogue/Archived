using System;
using ActiveUp.Net.Mail;

namespace imap
{
    class Test
    {

        public static void Main()
        {
            MailRepository rep = new MailRepository("imap.gmail.com", 993, true, @"vnocsymphony@gmail.com", "xml003ontop");
            foreach (Message email in rep.GetUnreadMails("Inbox"))
            {
                Console.Write(string.Format("<p>{0}: {1}</p><p>{2}</p>", email.From, email.Subject, email.BodyHtml.Text));
                if (email.Attachments.Count > 0)
                {
                    foreach (MimePart attachment in email.Attachments)
                    {
                        Console.Write(string.Format("<p>Attachment: {0} {1}</p>", attachment.ContentName, attachment.ContentType.MimeType));
                    }
                }
            }
            Console.Read();
        }
    }
}