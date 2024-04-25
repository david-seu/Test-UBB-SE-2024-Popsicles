using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2024_Popsicles.Repositories;
using UBB_SE_2024_Popsicles.Services;

using UBB_SE_2024_Popsicles.Models;
using System.Text.RegularExpressions;

namespace Test_UBB_SE_2024_Popsicles.TestService
{
    [TestFixture]
    internal class TestGroupService
    {
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IGroupMemberRepository> _groupMemberRepositoryMock;
        private Mock<IGroupMembershipRepository> _groupMembershipRepositoryMock;
        private Mock<IRequestRepository> _requestsRepositoryMock;
        //private Mock<IGroupService> _groupServiceMock;
        //private GroupService groupService;
        private GroupService _groupService;

        [SetUp]
        public void Setup()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _groupMemberRepositoryMock = new Mock<IGroupMemberRepository>();
            _groupMembershipRepositoryMock = new Mock<IGroupMembershipRepository>();
            _requestsRepositoryMock = new Mock<IRequestRepository>();

            _groupService = new GroupService(
                _groupRepositoryMock.Object,
                _groupMemberRepositoryMock.Object,
                _groupMembershipRepositoryMock.Object,
                _requestsRepositoryMock.Object);


            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(It.IsAny<Guid>()))
            //                  .Returns(new GroupMember(Guid.NewGuid(), "TestUsername","password","email","phone","description"));

            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));

        }


        //_groupServiceMock = new Mock<IGroupService>();
        //_groupServiceMock.Setup(x => x.CreateGroup(It.IsAny<Guid>())).Callback<Guid>(groupService.CreateGroup);


        //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>())).Verifiable();


        //private GroupService _groupService;



        [Test]
        public void CreateGroup_ShouldAddGroupToRepository()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));


            _groupService.CreateGroup(ownerId);


            _groupRepositoryMock.Verify(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()), Times.Once);


        }

        [Test]
        public void Test_AddMemberToGroup()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupMemberRepositoryMock = new Mock<IGroupMemberRepository>();
            var groupMembershipRepositoryMock = new Mock<IGroupMembershipRepository>();
            var requestsRepositoryMock = new Mock<IRequestRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object, groupMemberRepositoryMock.Object, groupMembershipRepositoryMock.Object, requestsRepositoryMock.Object);

            var groupId = Guid.NewGuid();
            var groupMemberId = Guid.NewGuid();
            var userRole = "admin";

            // Act
            groupService.AddMemberToGroup(groupMemberId, groupId, userRole);

            // Assert
            groupMembershipRepositoryMock.Verify(x => x.AddGroupMembership(It.Is<GroupMembership>(gm =>
                gm.Id != Guid.Empty &&
                gm.GroupMemberId == groupMemberId &&
                gm.GroupId == groupId &&
                gm.Role == userRole &&
                gm.IsBanned == false &&
                gm.IsTimedOut == false &&
                gm.ByPassPostSettings == false &&
                gm.JoinDate != null
            )), Times.Once);
        }

        [Test]
        public void RemoveMember_ShouldRemoveGroupMembershipFromRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));


            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(new GroupMember(groupMemberId, "TestUsername", "password", "email", "phone", "description"));


            //_groupMembershipRepositoryMock.Setup(x => x.RemoveGroupMembershipById(It.IsAny<Guid>()));

            // Act
            _groupService.CreateGroup(groupId);
            _groupService.AddMemberToGroup(groupMemberId, groupId);
            _groupService.RemoveMemberFromGroup(groupMemberId, groupId);


            // Assert
            _groupMembershipRepositoryMock.Verify(x => x.RemoveGroupMembershipById(groupMemberId), Times.Once);
        }

        [Test]
        public void AddRequest_ShouldAddRequestToRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            _requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            // Act
            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.AddRequest(It.IsAny<Request>()), Times.Once);
        }

        [Test]
        public void AcceptRequest_ShouldRemoveRequestFromRepository()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            _requestsRepositoryMock.Setup(x => x.RemoveRequestById(It.IsAny<Guid>()));

            // Act
            _groupService.AcceptRequestToJoinGroup(requestId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.RemoveRequestById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void RejectRequest_ShouldRemoveRequestFromRepository()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            _requestsRepositoryMock.Setup(x => x.RemoveRequestById(It.IsAny<Guid>()));

            // Act
            _groupService.RejectRequestToJoinGroup(requestId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.RemoveRequestById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void CreateGroupPost_ShouldAddPostToGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            string content = "Test post";
            string image = "Test image";
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, content, image);

            // Assert
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
        }

        [Test]
        public void GetGroupPosts_ShouldReturnGroupPosts()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            List<GroupPost> groupPosts = new List<GroupPost>
                {
                    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Test Post 1", "Test Image 1", groupId),
                    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Test Post 2", "Test Image 2", groupId)
                };
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Posts = groupPosts });

            // Act
            List<GroupPost> result = _groupService.GetGroupPosts(groupId);

            // Assert
            Assert.AreEqual(groupPosts, result);
        }

        [Test]
        public void UpdateGroup_ShouldUpdateGroupInRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            var group = new UBB_SE_2024_Popsicles.Models.Group(groupId, ownerId, "New Group Name", "New Group Description", "new_icon.png", "new_banner.png",
           20,
            false,
            false, "groupCode");

            _groupRepositoryMock.Setup(x => x.UpdateGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));

            // Act
            _groupService.UpdateGroup(group.Id, group.Name, group.Description, group.Icon, group.Banner, group.PostCount, group.IsPublic, group.CanMakePostsByDefault);

            // Assert
            _groupRepositoryMock.Verify(x => x.UpdateGroup(group), Times.Once());

        }



        [Test]
        public void BanMemberFromGroup_ShouldSetMemberAsBanned()
        {
            // Arrange
            Guid bannedMemberId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            // Act
            _groupService.BanMemberFromGroup(bannedMemberId, groupId);

            // Assert
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            Assert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(bannedMemberId).IsBanned);
        }

        [Test]
        public void UnbanMemberFromGroup_ShouldSetMemberAsNotBanned()
        {
            // Arrange
            Guid unbannedMemberId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            // Act
            _groupService.UnbanMemberFromGroup(unbannedMemberId, groupId);

            // Assert
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            Assert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(unbannedMemberId).IsBanned);
        }

        [Test]
        public void TimeoutMemberFromGroup_ShouldSetMemberAsTimedOut()
        {
            // Arrange
            Guid timedOutMemberId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            // Act
            _groupService.TimeoutMemberFromGroup(timedOutMemberId, groupId);

            // Assert
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            Assert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(timedOutMemberId).IsTimedOut);
        }

        [Test]
        public void EndTimeoutOfMemberFromGroup_ShouldSetMemberAsNotTimedOut()
        {
            // Arrange
            Guid memberWithTimeoutId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            // Act
            _groupService.EndTimeoutOfMemberFromGroup(memberWithTimeoutId, groupId);

            // Assert
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            Assert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(memberWithTimeoutId).IsTimedOut);
        }


        [Test]
        public void GetGroupMembers_ShouldReturnGroupMembers()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), Guid.NewGuid(), "Member1", groupId, "role1", DateTime.Now, false, false, false),
        new GroupMembership(Guid.NewGuid(), Guid.NewGuid(), "Member2", groupId, "role2", DateTime.Now, false, false, false)
    };
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Memberships = memberships });

            // Act
            List<GroupMember> result = _groupService.GetGroupMembers(groupId);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetMemberFromGroup_ShouldReturnGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMembership membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "TestMember", groupId, "role", DateTime.Now, false, false, false);
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Memberships = new List<GroupMembership> { membership } });
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(new GroupMember(groupMemberId, "TestMember", "password", "email", "phone", "description"));

            // Act
            GroupMember result = _groupService.GetMemberFromGroup(groupId, groupMemberId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(groupMemberId, result.Id);
        }


        [Test]
        public void ChangeMemberRoleInTheGroup_ShouldChangeRole()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            string newGroupRole = "admin";
            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, false);
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Memberships = new List<GroupMembership> { membership } });

            // Act
            _groupService.ChangeMemberRoleInTheGroup(groupMemberId, groupId, newGroupRole);

            // Assert
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && gm.Role == newGroupRole)), Times.Once);
        }

        [Test]
        public void AllowMemberToBypassPostageAllowance_ShouldSetBypassPostSettingsToTrue()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, false);
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Memberships = new List<GroupMembership> { membership } });

            // Act
            _groupService.AllowMemberToBypassPostageAllowance(groupMemberId, groupId);

            // Assert
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && gm.ByPassPostSettings)), Times.Once);
        }

        [Test]
        public void DisallowMemberToBypassPostageAllowance_ShouldSetBypassPostSettingsToFalse()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, true);
            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code") { Memberships = new List<GroupMembership> { membership } });

            // Act
            _groupService.DisallowMemberToBypassPostageAllowance(groupMemberId, groupId);

            // Assert
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && !gm.ByPassPostSettings)), Times.Once);
        }





    }


}



