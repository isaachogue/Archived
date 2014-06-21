using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveUp.Net.Mail;

namespace MailRepository {
    public interface IMailRepository {
        string AttachmentPath { get; }
        IList<MailMessage> GetUnreadMails(string mailBox);
    }
}
