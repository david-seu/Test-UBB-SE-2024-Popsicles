using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework.Legacy;
using UBB_SE_2024_Popsicles.Services;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Repositories;

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
            groupRepositoryMock = new ();
            groupMemberRepositoryMock = new ();
            groupMembershipRepositoryMock = new ();
            requestsRepositoryMock = new ();

            groupService = new GroupService(groupRepositoryMock.Object,
                groupMemberRepositoryMock.Object,
                groupMembershipRepositoryMock.Object,
                requestsRepositoryMock.Object);
        }

        public Group CreateGroupWithMemberFactory(bool bypassesPostageRestriction = false, bool allowanceOfPostage = false, int maximumNumberOfPostsPerHour = 5)
        {
            groupId = Guid.NewGuid();
            groupMemberId = Guid.NewGuid();
            Guid groupMembershipId = Guid.NewGuid();

            Group group = new Group(groupId, groupMemberId, "Test Name", "Test Description", "Test Icon", "Test Banner",
                maximumNumberOfPostsPerHour, true, allowanceOfPostage, "Test Code");

            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "Test Password", "Test Email",
                "Test Phone number", "Test Description");

            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId,
                groupMember.UserName, groupId, "Test Group Member Role", DateTime.Now, false, false, bypassesPostageRestriction);

            group.AddMember(groupMembership);

            groupRepositoryMock.Setup(repository => repository.GetGroupById(It.IsAny<Guid>())).Returns(group);

            return group;
        }

        [Test]
        public void CreateNewPostOnGroupChat_BypassesPostageRestriction_AddsNewGroupPosts()
        {
            // Arrange
            Group group = CreateGroupWithMemberFactory(bypassesPostageRestriction: true);
            Guid groupId = this.groupId;
            Guid groupMemberId = this.groupMemberId;
            string postContent = "Test post";
            string postImage = "Test image";

            // Act
            groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);

            // Assert
            ClassicAssert.That(group.ListOfGroupPosts.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreateNewPostOnGroupChat_PostageIsAllowed_AddsNewGroupPosts()
        {
            // Arange
            Group group = CreateGroupWithMemberFactory(allowanceOfPostage: true);
            Guid groupId = this.groupId;
            Guid groupMemberId = this.groupMemberId;
            string postContent = "Test post";
            string postImage = "Test image";

            // Act
            groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);

            // Assert
            ClassicAssert.That(group.ListOfGroupPosts.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreateNewPostOnGroupChat_LimitPostNotReached_AddsNewGroupPosts()
        {
            // Arrange
            Group group = CreateGroupWithMemberFactory();
            Guid groupId = this.groupId;
            Guid groupMemberId = this.groupMemberId;

            // Act
            groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test Post", "Test image");

            // Assert
            ClassicAssert.That(group.ListOfGroupPosts.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreateNewPostOnGroupChat_LimitOfPostsReached_ExceptionThrown()
        {
            // Arrange
            Group group = CreateGroupWithMemberFactory(maximumNumberOfPostsPerHour: 0);
            Guid groupMemberId = this.groupMemberId;
            Guid groupId = this.groupId;

            try
            {
                // Act
                groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test Post", "Test image");
            }
            catch (Exception expectedException)
            {
                ClassicAssert.That(expectedException.Message, Is.EqualTo("Post limit exceeded"));
            }
        }
    }
}
