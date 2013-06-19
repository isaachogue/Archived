using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviSpl.Vnoc.Symphony.Services.Sync;
using NUnit.Framework;

namespace ServiceSampleTests {
    [TestFixture]
    public class SvmServiceAgentTests {

        AutoMoq mocker = new AutoMoq();

        SvmAgent _sut;
        [TestFixtureSetUp]
        public void setup() {
            _sut = ; 
        }
        [Test]
        public void SvmServiceAgent_has_a_property_called_svm_email_domain()
        {
            Assert.That(_sut, Has.Property("EmailDomain"));
        }
        [Test]
        public void SvmServiceAgent_has_a_property_called_results()
        {
            Assert.That(_sut, Has.Property("Results"));
        }
    }
}
