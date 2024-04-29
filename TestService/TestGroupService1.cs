using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using UBB_SE_2024_Popsicles.Services;

namespace Test_UBB_SE_2024_Popsicles.TestService
{

    [TestFixture]
    internal class TestGroupService1
    {
        private Mock<IGroupRepository> groupRepositoryMock;
        private Mock<IGroupMemberRepository> groupMemberRepositoryMock;
        private Mock<IGroupMembershipRepository> groupMembershipRepositoryMock;
        private Mock<IJoinRequestRepository> requestsRepositoryMock;
        private GroupService groupService;
        private Guid groupId;
        private Guid groupMemberId;


        [SetUp]
        public void SetUp()
        {
            groupRepositoryMock = new();
            groupMemberRepositoryMock = new();
            groupMembershipRepositoryMock = new();
            requestsRepositoryMock = new();

            groupService = new GroupService(groupRepositoryMock.Object,
                groupMemberRepositoryMock.Object,
                groupMembershipRepositoryMock.Object,
                requestsRepositoryMock.Object);

        }

        public void CreateGroupWithMemberthatBypasesPostageRestriction()
        {
            
        }

        [TearDown] public void TearDown() { }


        [Test]
        public void CreateNewPostOnGroupChat_BypassesPostageRestriction_AddsNewGroupPosts()
        {
            //Arrange
            Guid groupId = this.groupId;
            Guid groupMemberId = this.groupMemberId;
            string postContent = "Test post";
            string postImage = "Test image";
            
            //Act
            groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);
    }

    }
}
