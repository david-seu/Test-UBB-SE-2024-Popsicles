using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using UBB_SE_2024_Popsicles.Repositories;
using UBB_SE_2024_Popsicles.Services;

using UBB_SE_2024_Popsicles.Models;

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


       // [Test]
       // public void CreateGroup_2_ShouldAddGroupToRepository()
       // {
       //     // Arrange
       //     Guid ownerId = Guid.NewGuid();

       //var defaultGroupName = "New Group";
       // var defaultGroupDescription = "This is a new group";
       //var defaultGroupIcon = "default";
       //var defaultGroupBanner = "default";
       // var defaultMaxPostsPerHourPerUser = 5;
       // var defaultIsPublic = false;
       // var defaultCanMakePosts = false;
       // var defaultGroupRole = "user";

       // _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

       //     // Act
       //     _groupService.CreateGroup(ownerId);

       //     // Assert
       //     _groupRepositoryMock.Verify(x => x.AddGroup(It.Is<Group>(g =>
       //         g.Id != Guid.Empty &&
       //         g.OwnerId == ownerId &&
       //         g.Name == defaultGroupName &&
       //         g.Description == defaultGroupDescription &&
       //         g.Icon == defaultGroupIcon &&
       //         g.Banner == defaultGroupBanner &&
       //         g.MaxPostsPerHourPerUser == defaultMaxPostsPerHourPerUser &&
       //         g.IsPublic == defaultIsPublic &&
       //         g.CanMakePostsByDefault == defaultCanMakePosts &&
       //         g.GroupCode.Length == 6)));
       // }

        [Test]
        public void CreateGroup_ShouldAddGroupToRepository()
        {
            // Arrange
            //Guid ownerId = Guid.NewGuid();
            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));


            //_groupService.CreateGroup(ownerId);


            //_groupRepositoryMock.Verify(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()), Times.Once);


        }

        [Test]
        public void Test_AddMemberToGroup()
        {
            // Arrange

            Guid groupId = Guid.NewGuid();

            Guid membershipId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");


            GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
                false, false, false);

            //group.GetMembership()

            newMember.AddGroupMembership(groupMembership);

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

            _groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));

            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            // Act
            _groupService.AddMemberToGroup(groupMemberId, groupId, "userRole");

            // Assert
            _groupMembershipRepositoryMock.Verify(x => x.AddGroupMembership(It.Is<GroupMembership>(gm =>
                gm.Id != Guid.Empty &&
                gm.GroupMemberId == groupMemberId &&
                gm.GroupId == groupId &&
                gm.Role == "userRole" &&
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
            Guid membershipId = Guid.NewGuid();
            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            Request request = new Request(requestId, groupMemberId, "name", groupId);

            GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
                false, false, false);


            group.AddRequest(request);

            newMember.AddOutgoingRequest(request);

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


            _requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            _requestsRepositoryMock.Setup(x => x.GetRequestById(requestId)).Returns(request);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            _groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));


            _groupMembershipRepositoryMock.Setup(x => x.RemoveGroupMembershipById(membershipId));

            _groupService.AddMemberToGroup(groupMemberId, groupId);
            _groupService.RemoveMemberFromGroup(groupMemberId, groupId);


            // Assert
            //_groupMemberRepositoryMock.Verify(x => x.RemoveGroupMemberById(groupMemberId), Times.Once);
        }


        [Test]
        public void RemoveMemberFromGroup_ShouldRemoveGroupMembershipFromGroupAndGroupMember()
        {
            // Arrange
            //Guid groupId = Guid.NewGuid();
            //Guid membershipId = Guid.NewGuid();
            //Guid groupMemberId = Guid.NewGuid();
            //GroupMember groupMember = new GroupMember(groupMemberId, "John Doe", "johndoe@example.com", "password", "profilePictureUrl", "description");
            //Group group = new Group(groupId, groupMemberId,"Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId,"test name", groupId, "Test Role", DateTime.Now, false, false, false);

            ////group.AddMembership(groupMembership);
            //groupMember.AddGroupMembership(groupMembership);

            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);
            //_groupMembershipRepositoryMock.Setup(x => x.RemoveGroupMembershipById(groupMembership.Id));

            //// Act
            //_groupService.RemoveMemberFromGroup(groupMemberId, groupId);

            //// Assert
            ////Assert.IsFalse(group.HasMembership(groupMemberId));
            ////Assert.IsFalse(groupMember.HasGroupMembership(groupId));
            //_groupMembershipRepositoryMock.Verify(x => x.RemoveGroupMembershipById(groupMembership.Id), Times.Once);
        }

        [Test]
        public void AddRequest_ShouldAddRequestToRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


            _requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);


            //_groupService.AddMemberToGroup(groupMemberId,groupId);

            // Act
            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.AddRequest(It.IsAny<Request>()), Times.Once);
        }

        [Test]
        public void AcceptRequest_ShouldRemoveRequestFromRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            Request request = new Request(requestId, groupMemberId, "name", groupId);

            group.AddRequest(request);

            newMember.AddOutgoingRequest(request);

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


            _requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            _requestsRepositoryMock.Setup(x => x.GetRequestById(requestId)).Returns(request);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

            // Act
            _groupService.AcceptRequestToJoinGroup(requestId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.RemoveRequestById(requestId), Times.Once);
        }

        [Test]
        public void RejectRequest_ShouldRemoveRequestFromRepository()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            Request request = new Request(requestId, groupMemberId, "name", groupId);

            group.AddRequest(request);

            newMember.AddOutgoingRequest(request);

            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


            _requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            _requestsRepositoryMock.Setup(x => x.GetRequestById(requestId)).Returns(request);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

            // Act
            _groupService.RejectRequestToJoinGroup(requestId);

            // Assert
            _requestsRepositoryMock.Verify(x => x.RemoveRequestById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void CreateGroupPost_ShouldAddPostToGroup()
        {
            // Arrange
            //Guid groupId = Guid.NewGuid();
            //Guid groupMemberId = Guid.NewGuid();
            //string content = "Test post";
            //string image = "Test image";
            //Guid membershipId = Guid.NewGuid();


            //Guid requestId = Guid.NewGuid();
            //GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            //    Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            //Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            //Request request = new Request(requestId, groupMemberId, "name", groupId);

            //group.AddRequest(request);

            //newMember.AddOutgoingRequest(request);


            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
            //    false, false, false);

            ////group.GetMembership()

            //newMember.AddGroupMembership(groupMembership);

            //_groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


            //_requestsRepositoryMock.Setup(x => x.AddRequest(It.IsAny<Request>()));

            //_requestsRepositoryMock.Setup(x => x.GetRequestById(requestId)).Returns(request);


            //_groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);


            
            //// Act
            //_groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, content, image);

            //// Assert
            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
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

            Guid membershipId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");


            string newGroupName = "New Group Name";
            string newGroupDescription = "New Group Description";
            string newGroupIcon = "New Group Icon";
            string newGroupBanner = "New Group Banner";
            int maxPostsPerHourPerUser = 10;
            bool isTheGroupPublic = true;
            bool allowanceOfPostageByDefault = false;


            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

            _groupRepositoryMock.Setup(x => x.UpdateGroup(group));

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupRepositoryMock.Setup(x => x.UpdateGroup(It.IsAny<Group>()));

            _groupRepositoryMock.Setup(x => x.UpdateGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));

            // Act
            _groupService.UpdateGroup(groupId, newGroupName, newGroupDescription, newGroupIcon, newGroupBanner, maxPostsPerHourPerUser, isTheGroupPublic, allowanceOfPostageByDefault);

            // Assert
            _groupRepositoryMock.Verify(x => x.UpdateGroup(It.Is<Group>(g =>
                g.Id == groupId &&
                g.Name == newGroupName &&
                g.Description == newGroupDescription &&
                g.Icon == newGroupIcon &&
                g.Banner == newGroupBanner &&
                g.MaxPostsPerHourPerUser == maxPostsPerHourPerUser &&
                g.IsPublic == isTheGroupPublic &&
                g.CanMakePostsByDefault == allowanceOfPostageByDefault)));
        }



        [Test]
        public void BanMemberFromGroup_ShouldSetMemberAsBanned()
        {
            // Arrange
            //Guid groupId = Guid.NewGuid();

            //Guid membershipId = Guid.NewGuid();
            //Guid groupMemberId = Guid.NewGuid();
            //GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            //    Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            //Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");


            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
            //    false, false, false);

            ////group.GetMembership()

            //newMember.AddGroupMembership(groupMembership);

            //_groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

            //_groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));

            //_groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

            //// Act
            //_groupService.BanMemberFromGroup(groupMemberId, groupId);

            //// Assert
            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            //Assert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(groupMemberId).IsBanned);
        }

        [Test]
        public void UnbanMemberFromGroup_ShouldSetMemberAsNotBanned()
        {
            // Arrange
            //Guid unbannedMemberId = Guid.NewGuid();
            //Guid groupId = Guid.NewGuid();
            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            //// Act
            //_groupService.UnbanMemberFromGroup(unbannedMemberId, groupId);

            //// Assert
            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            //Assert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(unbannedMemberId).IsBanned);
        }

        [Test]
        public void TimeoutMemberFromGroup_ShouldSetMemberAsTimedOut()
        {
            // Arrange
            //Guid timedOutMemberId = Guid.NewGuid();
            //Guid groupId = Guid.NewGuid();
            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            //// Act
            //_groupService.TimeoutMemberFromGroup(timedOutMemberId, groupId);

            //// Assert
            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            //Assert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(timedOutMemberId).IsTimedOut);
        }

        [Test]
        public void EndTimeoutOfMemberFromGroup_ShouldSetMemberAsNotTimedOut()
        {
            // Arrange
            //Guid memberWithTimeoutId = Guid.NewGuid();
            //Guid groupId = Guid.NewGuid();
            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code"));

            //// Act
            //_groupService.EndTimeoutOfMemberFromGroup(memberWithTimeoutId, groupId);

            //// Assert
            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
            //Assert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembership(memberWithTimeoutId).IsTimedOut);
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

        [Test]
        public void CreateNewPostOnGroupChat_ShouldAddPostToGroupWhenMemberHasBypassPostSettings()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, false, false, false);
            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

            // Assert
            Assert.AreEqual(1, group.Posts.Count);
            Assert.AreEqual(groupMemberId, group.Posts[0].OwnerId);
        }

        [Test]
        public void CreateNewPostOnGroupChat_ShouldAddPostToGroupWhenGroupHasDefaultPostSettings()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, false, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, false, false, false);
            group.Memberships.Add(groupMembership);


            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

            // Assert
            Assert.AreEqual(1, group.Posts.Count);
            Assert.AreEqual(groupMemberId, group.Posts[0].OwnerId);
        }

        [Test]
        public void CreateNewPostOnGroupChat_ShouldNotAddPostToGroupWhenMemberHasExceededPostLimit()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 1, false, false, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, false, false, false);
            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);
            //_groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

            // Assert
            Assert.Throws<Exception>(() => _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null));
        }

        [Test]
        public void CreateNewPostOnGroupChat_ShouldAddPostToGroupWhenMemberHasNotExceededPostLimit()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 2, false, false, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId,"Test Name",groupId, "Test Role", DateTime.Now, false, false, false);
            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

            // Assert
            Assert.AreEqual(1, group.Posts.Count);
            Assert.AreEqual(groupMemberId, group.Posts[0].OwnerId);

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

            // Assert
            Assert.AreEqual(2, group.Posts.Count);
            Assert.AreEqual(groupMemberId, group.Posts[1].OwnerId);
        }

        [Test]
        public void GetAllGroups_ShouldReturnAllGroupsForMember()
        {
            // Arrange
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
            Group group1 = new Group(Guid.NewGuid(), groupMemberId, "Test Group 1", "Test Description 1", "Test Icon 1", "Test Banner 1", 5, true, true, "Test Code 1");
            Group group2 = new Group(Guid.NewGuid(), groupMemberId, "Test Group 2", "Test Description 2", "Test Icon 2", "Test Banner 2", 5, true, true, "Test Code 2");
            Group group3 = new Group(Guid.NewGuid(), Guid.NewGuid(), "Test Group 3", "Test Description 3", "Test Icon 3", "Test Banner 3", 5, true, true, "Test Code 3");
            GroupMembership membership1 = new GroupMembership(groupMemberId, group1.Id, "Test Name 1", group1.Id, "Test Role 1", DateTime.Now, false, false, false);
            GroupMembership membership2 = new GroupMembership(groupMemberId, group2.Id, "Test Name 2", group2.Id, "Test Role 2", DateTime.Now, false, false, false);
            GroupMembership membership3 = new GroupMembership(Guid.NewGuid(), group3.Id, "Test Name 3", group3.Id, "Test Role 3", DateTime.Now, false, false, false);
            groupMember.Memberships.Add(membership1);
            groupMember.Memberships.Add(membership2);
            groupMember.Memberships.Add(membership3);

            _groupRepositoryMock.Setup(x => x.GetGroupById(group1.Id)).Returns(group1);
            _groupRepositoryMock.Setup(x => x.GetGroupById(group2.Id)).Returns(group2);
            _groupRepositoryMock.Setup(x => x.GetGroupById(group3.Id)).Returns(group3);
            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

            // Act
            List<Group> result = _groupService.GetAllGroups(groupMemberId);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Contains(group1));
            Assert.IsTrue(result.Contains(group2));
            Assert.IsTrue(result.Contains(group3));
        }

        [Test]
        public void AddNewOptionToAPoll_ShouldAddOptionToPoll()
        {
            // Arrange
            Guid pollId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Poll poll = new Poll(pollId,ownerId , "Test Poll", groupId);
            group.AddPoll(poll);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

            // Assert
            Assert.AreEqual(3, poll.Options.Count);
            Assert.IsTrue(poll.Options.Contains("New Option"));
        }

        [Test]
        public void AddNewOptionToAPoll_ShouldThrowExceptionWhenPollNotFound()
        {
            // Arrange
            Guid pollId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option"));
        }




        [Test]
        public void CreateNewPoll_ShouldAddPollToGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            string pollDescription = "Test Poll";
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            _groupService.CreateNewPoll(groupId, groupMemberId, pollDescription);

            // Assert
            Assert.AreEqual(1, group.Polls.Count);
            Assert.IsFalse(group.Polls.Contains(new Poll(Guid.NewGuid(), groupMemberId, "description", groupId)));
        }


        [Test]
        public void EndTimeoutOfMemberFromGroup_ShouldEndTimeoutOfGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();

            Guid groupMembershipId = Guid.NewGuid();
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);
            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

            // Act
            _groupService.EndTimeoutOfMemberFromGroup(groupMemberId, groupId);

            // Assert
            Assert.IsFalse(groupMembership.IsTimedOut);
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
        }

        [Test]
        public void EndTimeoutOfMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");




            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMemberId)).Returns((GroupMembership)null);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.EndTimeoutOfMemberFromGroup(groupMemberId, groupId));
        }

        [Test]
        public void TimeoutMemberFromGroup_ShouldTimeoutGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();

            Guid groupMembershipId = Guid.NewGuid();
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);
            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

            // Act
            _groupService.TimeoutMemberFromGroup(groupMemberId, groupId);

            // Assert
            Assert.IsTrue(groupMembership.IsTimedOut);
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
        }

        [Test]
        public void TimeoutMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();

            Guid groupMembershipId = Guid.NewGuid();
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns((GroupMembership)null);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.TimeoutMemberFromGroup(groupMemberId, groupId));
        }


        [Test]
        public void UnbanMemberFromGroup_ShouldUnbanGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid unbannedGroupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, unbannedGroupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Guid groupMembershipId = Guid.NewGuid();
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, unbannedGroupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);

            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

            // Act
            _groupService.UnbanMemberFromGroup(unbannedGroupMemberId, groupId);

            // Assert
            Assert.IsFalse(groupMembership.IsBanned);
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
        }

        [Test]
        public void UnbanMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid unbannedGroupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, unbannedGroupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            

            Guid groupMembershipId = Guid.NewGuid();
             GroupMembership groupMembership = new GroupMembership(groupMembershipId, unbannedGroupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);


            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns((GroupMembership)null);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.UnbanMemberFromGroup(unbannedGroupMemberId, groupId));
        }

        [Test]
        public void BanMemberFromGroup_ShouldBanGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid bannedGroupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, bannedGroupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Guid groupMembershipId = Guid.NewGuid();
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, bannedGroupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);

            group.Memberships.Add(groupMembership);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

            // Act
            _groupService.BanMemberFromGroup(bannedGroupMemberId, groupId);

            // Assert
            Assert.IsTrue(groupMembership.IsBanned);
            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
        }

        [Test]
        public void BanMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid bannedGroupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, bannedGroupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Guid groupMembershipId = Guid.NewGuid();
            GroupMembership groupMembership = new GroupMembership(groupMembershipId, bannedGroupMemberId, "Test Name", groupId, "Test Role", DateTime.Now, true, false, false);
            

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupId)).Returns((GroupMembership)null);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.BanMemberFromGroup(bannedGroupMemberId, groupId));
        }

        [Test]
        public void GetSpecificGroupPoll_ShouldReturnPoll()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid pollId = Guid.NewGuid();

            Guid ownerId = Guid.NewGuid();
            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Poll poll = new Poll(pollId, ownerId, "Test Poll",  groupId);
            group.AddPoll(poll);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            Poll result = _groupService.GetSpecificGroupPoll(groupId, pollId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pollId, result.Id);
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
        }

        [Test]
        public void GetSpecificGroupPoll_ShouldThrowExceptionWhenGroupNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid pollId = Guid.NewGuid();

            Guid ownerId = Guid.NewGuid();
            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Poll poll = new Poll(pollId, ownerId, "Test Poll", groupId);
            // group.AddPoll(poll);


            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.GetSpecificGroupPoll(groupId, pollId));
        }

        [Test]
        public void GetSpecificGroupPoll_ShouldThrowExceptionWhenPollNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid pollId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.GetSpecificGroupPoll(groupId, pollId));
        }

        [Test]
        public void GetGroupPolls_ShouldReturnPolls()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Poll poll1 = new Poll(Guid.NewGuid(), ownerId, "Test Poll1", groupId);
            Poll poll2 = new Poll(Guid.NewGuid(), Guid.NewGuid(), "Test Poll2", groupId);
            group.AddPoll(poll1);
            group.AddPoll(poll2);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            List<Poll> result = _groupService.GetGroupPolls(groupId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(poll1));
            Assert.IsTrue(result.Contains(poll2));
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
        }

        [Test]
        public void GetGroupPolls_ShouldThrowExceptionWhenGroupNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");


            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns((Group)null);

            // Act and Assert
            Assert.Throws<NullReferenceException>(() => _groupService.GetGroupPolls(groupId));
        }

        [Test]
        public void GetRequestsToJoin_ShouldReturnRequests()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Request request1 = new Request(Guid.NewGuid(), Guid.NewGuid(), "Test User 1", groupId);
            Request request2 = new Request(Guid.NewGuid(), Guid.NewGuid(), "Test User 2", groupId);
            group.AddRequest(request1);
            group.AddRequest(request2);

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            List<Request> result = _groupService.GetRequestsToJoin(groupId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(request1));
            Assert.IsTrue(result.Contains(request2));
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
        }

        [Test]
        public void GetRequestsToJoin_ShouldThrowExceptionWhenGroupNotFound()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns((Group)null);

            // Act and Assert
            Assert.Throws<NullReferenceException>(() => _groupService.GetRequestsToJoin(groupId));
        }


        [Test]
        public void GetGroup_ShouldReturnGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act
            Group result = _groupService.GetGroup(groupId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(groupId, result.Id);
            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
        }

        [Test]
        public void DeleteGroup_ShouldDeleteGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
            _groupRepositoryMock.Setup(x => x.RemoveGroupById(groupId));

            // Act
            _groupService.DeleteGroup(groupId);

            // Assert
            _groupRepositoryMock.Verify(x => x.RemoveGroupById(groupId), Times.Once);
        }


        [Test]
        public void CreateGroup_ShouldCreateGroup()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            GroupMember owner = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");


            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(ownerId)).Returns(owner);
            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));
            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

            // Act
            _groupService.CreateGroup(ownerId);

            // Assert
            _groupRepositoryMock.Verify(x => x.AddGroup(It.IsAny<Group>()), Times.Once);
            _groupMemberRepositoryMock.Verify(x => x.AddGroupMember(It.IsAny<GroupMember>()), Times.Once);
        }



        [Test]
        public void GetMemberFromGroup_ShouldThrowException_WhenGroupMemberDoesNotExist()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();
            Group group = new Group(groupId, groupMemberId, "defaultGroupName","defaultGroupDescription", "defaultGroupIcon", "defaultGroupBanner", 5, false, false, "defaultGroupCode");

            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

            // Act and Assert
            Assert.Throws<Exception>(() => _groupService.GetMemberFromGroup(groupId, groupMemberId));
        }



    }


}



