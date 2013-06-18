using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveUp.Net.Mail;
namespace MailRepository {
    public class ImapMailRepository : IMailRepository {
        
        private Imap4Client _client = null;
        private string _username = null;
        private string _password = null;

        public string AttachmentPath { get; set; }

        public ImapMailRepository(string mailServer, int port, bool ssl)
        {
            if (ssl)
                Client.ConnectSsl(mailServer, port);
            else
                Client.Connect(mailServer, port);
        }

        public string Authenticate(string username, string password)
        {
            this._username = username;
            this._password = password;
            return Client.Login(username, password);
        }

        public IList<MailMessage> GetUnreadMails(string mailBox)
        {
            CheckAuthentication();
            List<MailMessage> result = new List<MailMessage>();

            IEnumerable<Message> messages = GetMails(mailBox, "UNSEEN").Cast<Message>();
            
            foreach (Message email in messages) {
                List<Attachment> attachments = LoadAttachments(email.Attachments);
                string from = email.From.Email;
                string text = email.BodyText.TextStripped;
                result.Add(new MailMessage(from, text, attachments));
            }
            return result;
        }

        private List<Attachment> LoadAttachments(AttachmentCollection attachments) {
            List<Attachment> results = new List<Attachment>();
            foreach (MimePart mimePart in attachments) {
                try {

                    string filePath = this.AttachmentPath.TrimEnd('\\') + "\\" + mimePart.Filename;
                   
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
            CheckAuthentication();
            Mailbox mails = Client.SelectMailbox(mailBox);
            MessageCollection messages = mails.SearchParse(searchPhrase);
            return messages;
        }

        private void CheckAuthentication()
        {
            if (this._username == null || this._password == null)
            {
                throw new Exception("Authenticate must be called with valid credentials");
            }
        }

    }
}
