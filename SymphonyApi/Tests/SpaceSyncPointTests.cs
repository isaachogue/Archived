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
    public class SpaceSyncPointTests
    {
        SpaceSyncPoint sut = new SpaceSyncPoint();

        [Test]
        public void assert_that_space_sync_point_is_an_object()
        {
            Assert.IsInstanceOf<SpaceSyncPoint>(sut);
        }

        [Test]
        public void assert_that_space_sync_point_has_watermark()
        {
            Assert.That(sut, Has.Property("Watermark"));
        }

        [Test]
        public void assert_that_space_sync_point_has_thirdpartyid()
        {
            Assert.That(sut, Has.Property("ThirdPartyId"));
        }

        [Test]
        public void assert_that_space_sync_point_has_name()
        {
            Assert.That(sut, Has.Property("Name"));
        }

        [Test]
        public void assert_that_space_sync_point_has_spaceid()
        {
            Assert.That(sut, Has.Property("SpaceId"));
        }

        [Test]
        public void assert_that_space_sync_point_has_subscriptionid()
        {
            Assert.That(sut, Has.Property("SubscriptionId"));
        }
    }
}
