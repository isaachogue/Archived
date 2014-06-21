using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviSpl.Vnoc.Symphony.Services.Api;
using NUnit.Framework;

namespace SymphonyApiTests
{
    [TestFixture]
    public class ConferenceSyncPointTests
    {
        ConferenceSyncPoint sut = new ConferenceSyncPoint();

        [Test]
        public void assert_that_conference_sync_point_is_an_object()
        {
            Assert.IsInstanceOf<ConferenceSyncPoint>(sut);
        }

        [Test]
        public void assert_that_conference_sync_point_has_confirmation_number()
        {
            Assert.That(sut, Has.Property("ConfirmationNumber"));
        }

        [Test]
        public void assert_that_conference_sync_point_has_third_party_id()
        {
            Assert.That(sut, Has.Property("ThirdPartyId"));
        }
        
        [Test]
        public void assert_that_conference_sync_point_has_third_party_name()
        {
            Assert.That(sut, Has.Property("ThirdPartyName"));
        }

        [Test]
        public void assert_that_conference_sync_point_has_utc_timestamp()
        {
            Assert.That(sut, Has.Property("UtcTimestamp"));
        }
        [Test]
        public void assert_that_conference_sync_point_has_md5()
        {
            Assert.That(sut, Has.Property("MD5"));
        }
        [Test]
        public void assert_that_conference_sync_point_has_origin()
        {
            Assert.That(sut, Has.Property("Origin"));
        }
    }
}
