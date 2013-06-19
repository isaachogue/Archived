using System.Collections.Generic;
using AviSpl.Vnoc.Symphony.Services.Api;
using NUnit.Framework;
using Spec.Mocks;
using TechTalk.SpecFlow;

namespace Spec.StepDefinitions
{
    [Binding]
    public class SymphonySyncApiStepDefinition
    {
        SymphonySyncApi _api;
        MockSymphonyApiRepository _repository;
        SpaceSyncPoint _roomSync;

        string _thirdPartyId;
        string _enterpriseSystem;

        [Given]
        public void Given_I_have_a_set_of_N_rooms_that_are_synchronized(int N)
        {
            _repository = new MockSymphonyApiRepository();
            _repository.InitializeRooms(N);
        }

        [Given(@"I am a sync agent for (.*)")]
        public void Given_I_am_a_sync_agent_for_System(string System)
        {
            this._enterpriseSystem = System;
            _api = new SymphonySyncApi(_repository);
        }

        [When(@"I request the sync point from the Symphony Repository Service with (.*)")]
        public void When_I_request_the_sync_point_from_the_Symphony_Repository_Service_with_ThirdPartyId(string ThirdPartyId)
        {
            _thirdPartyId = ThirdPartyId;
            _roomSync = _api.GetRoomSyncPoint(ThirdPartyId, _enterpriseSystem);
        }

        [Then]
        public void Then_the_response_should_provide_the_third_party_sync_point_for_the_Id()
        {
            Assert.AreEqual(_roomSync.ThirdPartyId, _thirdPartyId);   
        }
    }
}
