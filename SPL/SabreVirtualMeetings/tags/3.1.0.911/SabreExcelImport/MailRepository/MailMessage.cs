using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRepository {
    public class MailMessage {
        string _from;
        string _body;
        List<Attachment> _attachments;

        public MailMessage(string from, string body, List<Attachment> attachments) {
            this._from = from;
            this._body = body;
            this._attachments = attachments;
        }

        public string From {
            get { return _from; }
        }
        public string Body {
            get { return _body; }
        }
        public List<Attachment> Attachments {
            get { return _attachments; }
        }
    }
}
