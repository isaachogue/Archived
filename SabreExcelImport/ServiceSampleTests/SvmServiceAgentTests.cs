using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceSample;

namespace ServiceSampleTests {
    [TestFixture]
    public class SvmServiceAgentTests {

        SvmServiceAgent _sut;
        [TestFixtureSetUp]
        public void setup() {
            _sut = new SvmServiceAgent(); 
        }
        [Test]
        public void SvmServiceAgent_has_a_property_called_svm_email_domain() {
            Assert.That(_sut, Has.Property("EmailDomain"));
        }
    }
}
