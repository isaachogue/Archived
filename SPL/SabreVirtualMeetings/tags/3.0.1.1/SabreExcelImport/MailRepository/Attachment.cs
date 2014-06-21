using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRepository {
    public class Attachment : IDisposable {
        string _location;
        bool disposed;

        public Attachment(string fileLocation) {
            this._location = fileLocation;
        }

        public string FolderLocation {
            get {
                return _location;
            }
        }

        public void Dispose() {

            if (disposed == false) {
                if (System.IO.File.Exists(_location)) {
                    System.IO.File.Delete(_location);
                }
                disposed = true;
                _location = null;
            }
        }
    }
}
