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

        public (Group, GroupMember) CreateGroupFactory(bool bypassesPostageRestriction = false, bool allowanceOfPostage = false, int maximumNuberOfPostPerHour = 5)
        {
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            Guid groupMembershipId = Guid.NewGuid();

            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "Test Password", "Test Email",
                "Test Phone number", "Test Description");

            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId,
                groupMember.UserName, groupId, "Test Group Member Role", DateTime.Now, false, false, bypassesPostageRestriction);

            Group group = new Group(groupId, groupMemberId, "Test Name", "Test Description", "Test Icon", "Test Banner",
                maximumNuberOfPostPerHour, true, allowanceOfPostage, "Test Code");

            group.AddMembership(groupMembership);

            groupRepositoryMock.Setup(repository => repository.GetGroupById(groupId)).Returns(group);

            return (group, groupMember);
        }

        public GroupMember CreateGroupMemberFactory()
        {
            Guid groupMemberId = Guid.NewGuid();

            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "Test Password", "Test Email",
                "Test Phone number", "Test Description");

            return groupMember;
        }

        public GroupMembership CreateGroupMembershipFactory(GroupMember groupMember, Group group, bool bypassesPostageRestriction = false)
        {
            Guid groupMembershipId = Guid.NewGuid();

            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMember.UserId,
                "Test userName", group.GroupId, "Test Group Member Role", DateTime.Now, false, false, bypassesPostageRestriction);
            return groupMembership;
        }

        [Test]
        public void CreateNewPostOnGroupChat_BypassesPostageRestriction_AddsNewGroupPosts()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            (group, groupMember) = CreateGroupFactory(bypassesPostageRestriction: true);
            Guid groupId = group.GroupId;
            Guid groupMemberId = groupMember.UserId;
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
            Group group;
            GroupMember groupMember;
            (group, groupMember) = CreateGroupFactory(allowanceOfPostage: true);
            Guid groupId = group.GroupId;
            Guid groupMemberId = groupMember.UserId;
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
            Group group;
            GroupMember groupMember;
            (group, groupMember) = CreateGroupFactory();
            Guid groupId = group.GroupId;
            Guid groupMemberId = groupMember.UserId;

            // Act
            groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test Post", "Test image");

            // Assert
            ClassicAssert.That(group.ListOfGroupPosts.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreateNewPostOnGroupChat_LimitOfPostsReached_ExceptionThrown()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            (group, groupMember) = CreateGroupFactory(maximumNuberOfPostPerHour: 0);
            Guid groupId = group.GroupId;
            Guid groupMemberId = groupMember.UserId;

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

        public (Group, GroupMember) FactoryForAddMemberToGroup()
        {
            Group group;
            GroupMember groupOwner;
            (group, groupOwner) = CreateGroupFactory();
            GroupMember groupMember = CreateGroupMemberFactory();
            return (group, groupMember);
        }

        [Test]
        public void CreateNewPostOnGroupChat_InvalidGroupId_ExceptionThrown()
        {
            Guid groupId = Guid.NewGuid();
            groupRepositoryMock.Setup(mockRepository => mockRepository.GetGroupById(groupId)).Throws(new Exception("Group not found"));

            try
            {
                groupService.CreateNewPostOnGroupChat(groupId, Guid.NewGuid(), "Test Post", "Test image");
            }
            catch (Exception expectedException)
            {
                Assert.That(expectedException.Message, Is.EqualTo("Group not found"));
            }
        }

        [Test]
        public void CreateNewPostOnGroupChat_MemberNotInGroup_ExceptionThrown()
        {
            Group group;
            GroupMember groupOwner;
            (group, groupOwner) = CreateGroupFactory();
            GroupMember memberNotPartOfGroup = CreateGroupMemberFactory();

            try
            {
                groupService.CreateNewPostOnGroupChat(group.GroupId, memberNotPartOfGroup.UserId, "Test post", "Test image");
            }
            catch (Exception expectedException)
            {
                Assert.That(expectedException.Message, Is.EqualTo("Membership not found"));
            }
        }

        [Test]
        public void AddMemberToGroup_ValidGroupIdAndGroupMemberId_AddsMemberToGroup()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            (group, groupMember) = FactoryForAddMemberToGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(It.IsAny<Guid>()))
                .Returns(groupMember);

            // Act
            groupService.AddMemberToGroup(groupMember.UserId, group.GroupId);

            // Assert
            ClassicAssert.AreEqual(group.ListOfGroupMemberships.Count, 2);
        }

        [Test]
        public void AddMemberToGroup_ValidGroupIdInvalidGroupMemberId_ThrowsException()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            (group, groupMember) = FactoryForAddMemberToGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(groupMember.UserId))
                .Throws(new Exception("Group member not found"));

            // Act & Assert
            Assert.Throws<Exception>(() => groupService.AddMemberToGroup(groupMember.UserId, group.GroupId));
        }

        [Test]
        public void AddMemberToGroup_InValidGroupIdValidGroupMemberId_ThrowsException()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            (group, groupMember) = FactoryForAddMemberToGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(groupMember.UserId))
                .Returns(groupMember);
            groupRepositoryMock.Setup(repository => repository.GetGroupById(group.GroupId))
                .Throws(new Exception("Group not found"));

            // Act & Assert
            Assert.Throws<Exception>(() => groupService.AddMemberToGroup(groupMember.UserId, group.GroupId));
        }

        public (Group, GroupMember, GroupMembership) FactoryRemoveMemberFromGroup()
        {
            Group group;
            (group, _) = CreateGroupFactory();
            GroupMember groupMember = CreateGroupMemberFactory();
            GroupMembership groupMembership = CreateGroupMembershipFactory(groupMember, group);
            groupMember.AddGroupMembership(groupMembership);
            group.AddMembership(groupMembership);
            return (group, groupMember, groupMembership);
        }

        [Test]
        public void RemoveMemberFromGroup_ValidGroupIdAndGroupMemberId_RemovesMemberFromGroup()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            GroupMembership groupMembership;
            (group, groupMember, groupMembership) = FactoryRemoveMemberFromGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(groupMember.UserId))
                .Returns(groupMember);
            groupMembershipRepositoryMock.Setup(repository =>
                repository.RemoveGroupMembershipById(groupMembership.GroupMembershipId));

            // Act
            groupService.RemoveMemberFromGroup(groupMember.UserId, group.GroupId);

            // Assert
            ClassicAssert.AreEqual(1, group.ListOfGroupMemberships.Count);
        }

        [Test]
        public void RemoveMemberFromGroup_InValidGroupIdAndGroupMemberId_RemovesMemberFromGroup()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            GroupMembership groupMembership;
            (group, groupMember, groupMembership) = FactoryRemoveMemberFromGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(groupMember.UserId))
                .Returns(groupMember);
            groupRepositoryMock.Setup(repository => repository.GetGroupById(group.GroupId)).Throws<Exception>();
            groupMembershipRepositoryMock.Setup(repository =>
                repository.RemoveGroupMembershipById(groupMembership.GroupMembershipId));

            // Act & Assert
            Assert.Throws<Exception>(() => groupService.RemoveMemberFromGroup(groupMember.UserId, group.GroupId));
        }

        [Test]
        public void RemoveMemberFromGroup_ValidGroupIdAndInvalidGroupMemberId_RemovesMemberFromGroup()
        {
            // Arrange
            Group group;
            GroupMember groupMember;
            GroupMembership groupMembership;
            (group, groupMember, groupMembership) = FactoryRemoveMemberFromGroup();
            groupMemberRepositoryMock.Setup(repository => repository.GetGroupMemberById(groupMember.UserId))
                .Throws<Exception>();
            groupMembershipRepositoryMock.Setup(repository =>
                repository.RemoveGroupMembershipById(groupMembership.GroupMembershipId));

            // Act & Assert
            Assert.Throws<Exception>(() => groupService.RemoveMemberFromGroup(groupMember.UserId, group.GroupId));
        }
    }
}
