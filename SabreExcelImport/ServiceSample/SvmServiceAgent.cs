using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using SabreExcelImport;

namespace ServiceSample {
    public class SvmServiceAgent {
        private List<Conference> _svmConferences;

        public SvmServiceAgent(List<Conference> conferences)
        {
            this._svmConferences = conferences;
        }

        public string EmailDomain { get; set; }
    }
}
