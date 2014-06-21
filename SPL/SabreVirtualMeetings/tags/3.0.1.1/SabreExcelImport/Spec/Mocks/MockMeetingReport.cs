using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SabreExcelImport;

namespace Spec.Mocks
{
    public class MockMeetingReport : IMeetingReport
    {
        List<Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes.Conference> IMeetingReport.Meetings
        {
            get { throw new NotImplementedException(); }
        }
    }
}
