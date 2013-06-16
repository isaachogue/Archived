using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using Office.Framework.Excel;
using System.Data;
using AviSpl.Vnoc.Symphony.Services.Api;

namespace SabreExcelImport.Tests
{
    [TestFixture]
    public class MeetingReportUnit
    {
        MeetingReport _report;
        DataSet _dsReport; 

        [SetUp]
        public void InitializeDataSet()
        {
            string _path = @"Z:\Downloads\SPL\SabreExcelImport\Spec\Samples\out.xls";
            ExcelAdapter ea = new ExcelAdapter();
            ea.Load(_path);
            _dsReport = ea.DataSource;
        }

        [Test]
        public void meeting_report_has_a_constructor_with_stream_input()
        {
            _report = new MeetingReport(_dsReport, new SymphonySyncApi(null));
            NUnit.Framework.Assert.IsInstanceOf(typeof(MeetingReport), _report);
        }

        [Test]
        public void meeting_report_has_a_meetings_property()
        {
            if (_report == null)
                _report = new MeetingReport(_dsReport, new SymphonySyncApi(null));
            Assert.That(_report, Has.Property("Meetings"));
        }

    }
}
