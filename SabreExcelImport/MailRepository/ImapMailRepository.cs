using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveUp.Net.Mail;
namespace MailRepository {
    public class ImapMailRepository : IMailRepository {
        private Imap4Client _client = null;
        private string _attachmentPath;

        public string AttachmentPath {
            get { return _attachmentPath; }
        }

        public ImapMailRepository(string mailServer, int port, bool ssl, string login, string password, string path)
        {
            if (ssl)
                Client.ConnectSsl(mailServer, port);
            else
                Client.Connect(mailServer, port);
            Client.Login(login, password);
            _attachmentPath = path;
        }

        public IList<MailMessage> GetUnreadMails(string mailBox)
        {
            List<MailMessage> result = new List<MailMessage>();

            IEnumerable<Message> messages = GetMails(mailBox, "UNSEEN").Cast<Message>();
            
            foreach (Message email in messages) {
                List<Attachment> attachments = LoadAttachments(_attachmentPath, email.Attachments);
                string from = email.From.Email;
                string text = email.BodyText.TextStripped;
                result.Add(new MailMessage(from, text, attachments));
            }
            return result;
        }

        private List<Attachment> LoadAttachments(string path, AttachmentCollection attachments) {
            List<Attachment> results = new List<Attachment>();
            foreach (MimePart mimePart in attachments) {
                try {

                    string filePath = path.TrimEnd('\\') + "\\" + mimePart.Filename;
                   
                    mimePart.StoreToFile(filePath);
                    results.Add(new Attachment(filePath));
                
                } catch (Exception e) {
                    throw e;
                }
            }
            return results;
        }

        protected Imap4Client Client
        {
            get
            {
                if (_client == null)
                    _client = new Imap4Client();
                return _client;
            }
        }

        private MessageCollection GetMails(string mailBox, string searchPhrase)
        {
            Mailbox mails = Client.SelectMailbox(mailBox);
            MessageCollection messages = mails.SearchParse(searchPhrase);
            return messages;
        }

    }
}
