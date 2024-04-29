//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices.JavaScript;
//using System.Threading.Tasks;
//using UBB_SE_2024_Popsicles.Repositories;
//using UBB_SE_2024_Popsicles.Services;
//using NUnit.Framework.Legacy;
//using UBB_SE_2024_Popsicles.Models;

//namespace Test_UBB_SE_2024_Popsicles.TestService
//{
//    [TestFixture]
//    internal class TestGroupService
//    {
//        private Mock<IGroupRepository> _groupRepositoryMock;
//        private Mock<IGroupMemberRepository> _groupMemberRepositoryMock;
//        private Mock<IGroupMembershipRepository> _groupMembershipRepositoryMock;
//        private Mock<IJoinRequestRepository> _requestsRepositoryMock;
//        //private Mock<IGroupService> _groupServiceMock;
//        //private GroupService groupService;
//        private GroupService _groupService;

//        [SetUp]
//        public void Setup()
//        {
//            _groupRepositoryMock = new Mock<IGroupRepository>();
//            _groupMemberRepositoryMock = new Mock<IGroupMemberRepository>();
//            _groupMembershipRepositoryMock = new Mock<IGroupMembershipRepository>();
//            _requestsRepositoryMock = new Mock<IJoinRequestRepository>();

//            _groupService = new GroupService(
//                _groupRepositoryMock.Object,
//                _groupMemberRepositoryMock.Object,
//                _groupMembershipRepositoryMock.Object,
//                _requestsRepositoryMock.Object);


//            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(It.IsAny<Guid>()))
//            //                  .Returns(new GroupMember(Guid.NewGuid(), "TestUsername","password","email","phone","description"));

//            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));

//        }


//        //_groupServiceMock = new Mock<IGroupService>();
//        //_groupServiceMock.Setup(x => x.CreateGroup(It.IsAny<Guid>())).Callback<Guid>(groupService.CreateGroup);


//        //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>())).Verifiable();


//        //private GroupService _groupService;


//       // [Test]
//       // public void CreateGroup_2_ShouldAddGroupToRepository()
//       // {
//       //     // Arrange
//       //     Guid ownerId = Guid.NewGuid();

//       //var defaultGroupName = "New Group";
//       // var defaultGroupDescription = "This is a new group";
//       //var defaultGroupIcon = "default";
//       //var defaultGroupBanner = "default";
//       // var defaultMaxPostsPerHourPerUser = 5;
//       // var defaultIsPublic = false;
//       // var defaultCanMakePosts = false;
//       // var defaultGroupRole = "user";

//       // _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

//       //     // Act
//       //     _groupService.CreateGroup(ownerId);

//       //     // ClassicAssert
//       //     _groupRepositoryMock.Verify(x => x.AddGroup(It.Is<Group>(g =>
//       //         g.UserId != Guid.Empty &&
//       //         g.GroupOwnerId == ownerId &&
//       //         g.GroupName == defaultGroupName &&
//       //         g.GroupDescription == defaultGroupDescription &&
//       //         g.GroupIcon == defaultGroupIcon &&
//       //         g.GroupBanner == defaultGroupBanner &&
//       //         g.MaximumNumberOfPostsPerHourPerUser == defaultMaxPostsPerHourPerUser &&
//       //         g.IsGroupPublic == defaultIsPublic &&
//       //         g.AllowanceOfPostage == defaultCanMakePosts &&
//       //         g.GroupCode.Length == 6)));
//       // }

//        [Test]
//        public void CreateGroup_ShouldAddGroupToRepository()
//        {
//            // Arrange
//            //Guid ownerId = Guid.NewGuid();
//            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));


//            //_groupService.CreateGroup(ownerId);


//            //_groupRepositoryMock.Verify(x => x.AddGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()), Times.Once);


//        }

//        [Test]
//        public void Test_AddMemberToGroup()
//        {
//            // Arrange

//            Guid groupId = Guid.NewGuid();

//            Guid membershipId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");


//            GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
//                false, false, false);

//            //group.GetMembershipFromGroupMemberId()

//            newMember.AddGroupMembership(groupMembership);

//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

//            _groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));

//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            // Act
//            _groupService.AddMemberToGroup(groupMemberId, groupId, "userRole");

//            // ClassicAssert
//            _groupMembershipRepositoryMock.Verify(x => x.AddGroupMembership(It.Is<GroupMembership>(gm =>
//                gm.GroupMembershipId != Guid.Empty &&
//                gm.GroupMemberId == groupMemberId &&
//                gm.GroupId == groupId &&
//                gm.GroupMemberRole == "userRole" &&
//                gm.IsBannedFromGroup == false &&
//                gm.IsTimedOutFromGroup == false &&
//                gm.BypassPostageRestriction == false &&
//                gm.JoinDate != null
//            )), Times.Once);
//        }

//        [Test]
//        public void RemoveMember_ShouldRemoveGroupMembershipFromRepository()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid membershipId = Guid.NewGuid();
//            Guid requestId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            JoinRequest joinRequest = new JoinRequest(requestId, groupMemberId, "name", groupId);

//            GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", DateTime.Now,
//                false, false, false);


//            group.AddJoinRequest(joinRequest);

//            newMember.AddActiveJoinRequest(joinRequest);

//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


//            _requestsRepositoryMock.Setup(x => x.AddJoinRequest(It.IsAny<JoinRequest>()));

//            _requestsRepositoryMock.Setup(x => x.GetJoinRequestById(requestId)).Returns(joinRequest);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            _groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));


//            _groupMembershipRepositoryMock.Setup(x => x.RemoveGroupMembershipById(membershipId));

//            _groupService.AddMemberToGroup(groupMemberId, groupId);
//            _groupService.RemoveMemberFromGroup(groupMemberId, groupId);


//            // ClassicAssert
//            //_groupMemberRepositoryMock.Verify(x => x.RemoveGroupMemberById(groupMemberId), Times.Once);
//        }


//        [Test]
//        public void RemoveMemberFromGroup_ShouldRemoveGroupMembershipFromGroupAndGroupMember()
//        {
//            // Arrange
//            //Guid groupId = Guid.NewGuid();
//            //Guid membershipId = Guid.NewGuid();
//            //Guid groupMemberId = Guid.NewGuid();
//            //GroupMember groupMember = new GroupMember(groupMemberId, "John Doe", "johndoe@example.com", "password", "profilePictureUrl", "description");
//            //Group group = new Group(groupId, groupMemberId,"Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId,"test name", groupId, "Test GroupMemberRole", PostageDateTime.Now, false, false, false);

//            ////group.AddMembership(groupMembership);
//            //groupMember.AddGroupMembership(groupMembership);

//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);
//            //_groupMembershipRepositoryMock.Setup(x => x.RemoveGroupMembershipById(groupMembership.UserId));

//            //// Act
//            //_groupService.RemoveMemberFromGroup(groupMemberId, groupId);

//            //// ClassicAssert
//            ////ClassicAssert.IsFalse(group.HasMembership(groupMemberId));
//            ////ClassicAssert.IsFalse(groupMember.HasGroupMembership(groupId));
//            //_groupMembershipRepositoryMock.Verify(x => x.RemoveGroupMembershipById(groupMembership.UserId), Times.Once);
//        }

//        [Test]
//        public void AddRequest_ShouldAddRequestToRepository()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

            

//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


//            _requestsRepositoryMock.Setup(x => x.AddJoinRequest(It.IsAny<JoinRequest>()));

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);


//            //_groupService.AddMemberToGroup(groupMemberId,groupId);

//            // Act
//            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

//            // ClassicAssert
//            _requestsRepositoryMock.Verify(x => x.AddJoinRequest(It.IsAny<JoinRequest>()), Times.Once);
//        }

//        [Test]
//        public void AcceptRequest_ShouldRemoveRequestFromRepository()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();

//            Guid requestId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            JoinRequest joinRequest = new JoinRequest(requestId, groupMemberId, "name", groupId);

//            group.AddJoinRequest(joinRequest);

//            newMember.AddActiveJoinRequest(joinRequest);

//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


//            _requestsRepositoryMock.Setup(x => x.AddJoinRequest(It.IsAny<JoinRequest>()));

//            _requestsRepositoryMock.Setup(x => x.GetJoinRequestById(requestId)).Returns(joinRequest);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

//            // Act
//            _groupService.AcceptRequestToJoinGroup(requestId);

//            // ClassicAssert
//            _requestsRepositoryMock.Verify(x => x.RemoveJoinRequestById(requestId), Times.Once);
//        }

//        [Test]
//        public void RejectRequest_ShouldRemoveRequestFromRepository()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();

//            Guid requestId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            JoinRequest joinRequest = new JoinRequest(requestId, groupMemberId, "name", groupId);

//            group.AddJoinRequest(joinRequest);

//            newMember.AddActiveJoinRequest(joinRequest);

//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


//            _requestsRepositoryMock.Setup(x => x.AddJoinRequest(It.IsAny<JoinRequest>()));

//            _requestsRepositoryMock.Setup(x => x.GetJoinRequestById(requestId)).Returns(joinRequest);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            _groupService.AddNewRequestToJoinGroup(groupMemberId, groupId);

//            // Act
//            _groupService.RejectRequestToJoinGroup(requestId);

//            // ClassicAssert
//            _requestsRepositoryMock.Verify(x => x.RemoveJoinRequestById(It.IsAny<Guid>()), Times.Once);
//        }

//        [Test]
//        public void CreateGroupPost_ShouldAddPostToGroup()
//        {
//            // Arrange
//            //Guid groupId = Guid.NewGuid();
//            //Guid groupMemberId = Guid.NewGuid();
//            //string content = "Test post";
//            //string image = "Test image";
//            //Guid membershipId = Guid.NewGuid();


//            //Guid requestId = Guid.NewGuid();
//            //GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//            //    Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            //Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            //JoinRequest request = new JoinRequest(requestId, groupMemberId, "name", groupId);

//            //group.AddJoinRequest(request);

//            //newMember.AddActiveJoinRequest(request);


//            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", PostageDateTime.Now,
//            //    false, false, false);

//            ////group.GetMembershipFromGroupMemberId()

//            //newMember.AddGroupMembership(groupMembership);

//            //_groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));


//            //_requestsRepositoryMock.Setup(x => x.AddJoinRequest(It.IsAny<JoinRequest>()));

//            //_requestsRepositoryMock.Setup(x => x.GetJoinRequestById(requestId)).Returns(request);


//            //_groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);


            
//            //// Act
//            //_groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, content, image);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//        }

//        [Test]
//        public void GetGroupPosts_ShouldReturnGroupPosts()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            List<GroupPost> groupPosts = new List<GroupPost>
//                {
//                    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Test Post 1", "Test PostImage 1", groupId),
//                    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Test Post 2", "Test PostImage 2", groupId)
//                };
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupPosts = groupPosts });

//            // Act
//            List<GroupPost> result = _groupService.GetGroupPosts(groupId);

//            // ClassicAssert
//            ClassicAssert.AreEqual(groupPosts, result);
//        }

//        [Test]
//        public void UpdateGroup_ShouldUpdateGroupInRepository()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();

//            Guid membershipId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//                Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");


//            string newGroupName = "New Group GroupName";
//            string newGroupDescription = "New Group GroupDescription";
//            string newGroupIcon = "New Group GroupIcon";
//            string newGroupBanner = "New Group GroupBanner";
//            int maxPostsPerHourPerUser = 10;
//            bool isTheGroupPublic = true;
//            bool allowanceOfPostageByDefault = false;


//            _groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            _groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

//            _groupRepositoryMock.Setup(x => x.UpdateGroup(group));

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupRepositoryMock.Setup(x => x.UpdateGroup(It.IsAny<Group>()));

//            _groupRepositoryMock.Setup(x => x.UpdateGroup(It.IsAny<UBB_SE_2024_Popsicles.Models.Group>()));

//            // Act
//            _groupService.UpdateGroup(groupId, newGroupName, newGroupDescription, newGroupIcon, newGroupBanner, maxPostsPerHourPerUser, isTheGroupPublic, allowanceOfPostageByDefault);

//            // ClassicAssert
//            _groupRepositoryMock.Verify(x => x.UpdateGroup(It.Is<Group>(g =>
//                g.GroupId == groupId &&
//                g.GroupName == newGroupName &&
//                g.GroupDescription == newGroupDescription &&
//                g.GroupIcon == newGroupIcon &&
//                g.GroupBanner == newGroupBanner &&
//                g.MaximumNumberOfPostsPerHourPerUser == maxPostsPerHourPerUser &&
//                g.IsGroupPublic == isTheGroupPublic &&
//                g.AllowanceOfPostage == allowanceOfPostageByDefault)));
//        }



//        [Test]
//        public void BanMemberFromGroup_ShouldSetMemberAsBanned()
//        {
//            // Arrange
//            //Guid groupId = Guid.NewGuid();

//            //Guid membershipId = Guid.NewGuid();
//            //Guid groupMemberId = Guid.NewGuid();
//            //GroupMember newMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//            //    Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            //Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");


//            //GroupMembership groupMembership = new GroupMembership(membershipId, groupMemberId, "test", groupId, "test", PostageDateTime.Now,
//            //    false, false, false);

//            ////group.GetMembershipFromGroupMemberId()

//            //newMember.AddGroupMembership(groupMembership);

//            //_groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));

//            //_groupMembershipRepositoryMock.Setup(x => x.AddGroupMembership(It.IsAny<GroupMembership>()));

//            //_groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(membershipId)).Returns(groupMembership);

//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(newMember);

//            //// Act
//            //_groupService.BanMemberFromGroup(groupMemberId, groupId);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//            //ClassicAssert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembershipFromGroupMemberId(groupMemberId).IsBannedFromGroup);
//        }

//        [Test]
//        public void UnbanMemberFromGroup_ShouldSetMemberAsNotBanned()
//        {
//            // Arrange
//            //Guid unbannedMemberId = Guid.NewGuid();
//            //Guid groupId = Guid.NewGuid();
//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code"));

//            //// Act
//            //_groupService.UnbanMemberFromGroup(unbannedMemberId, groupId);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//            //ClassicAssert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembershipFromGroupMemberId(unbannedMemberId).IsBannedFromGroup);
//        }

//        [Test]
//        public void TimeoutMemberFromGroup_ShouldSetMemberAsTimedOut()
//        {
//            // Arrange
//            //Guid timedOutMemberId = Guid.NewGuid();
//            //Guid groupId = Guid.NewGuid();
//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code"));

//            //// Act
//            //_groupService.TimeoutMemberFromGroup(timedOutMemberId, groupId);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//            //ClassicAssert.IsTrue(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembershipFromGroupMemberId(timedOutMemberId).IsTimedOutFromGroup);
//        }

//        [Test]
//        public void EndTimeoutOfMemberFromGroup_ShouldSetMemberAsNotTimedOut()
//        {
//            // Arrange
//            //Guid memberWithTimeoutId = Guid.NewGuid();
//            //Guid groupId = Guid.NewGuid();
//            //_groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code"));

//            //// Act
//            //_groupService.EndTimeoutOfMemberFromGroup(memberWithTimeoutId, groupId);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//            //ClassicAssert.IsFalse(_groupRepositoryMock.Object.GetGroupById(groupId).GetMembershipFromGroupMemberId(memberWithTimeoutId).IsTimedOutFromGroup);
//        }


//        [Test]
//        public void GetGroupMembers_ShouldReturnGroupMembers()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            List<GroupMembership> memberships = new List<GroupMembership>
//    {
//        new GroupMembership(Guid.NewGuid(), Guid.NewGuid(), "Member1", groupId, "role1", DateTime.Now, false, false, false),
//        new GroupMembership(Guid.NewGuid(), Guid.NewGuid(), "Member2", groupId, "role2", DateTime.Now, false, false, false)
//    };
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupMemberships = memberships });

//            // Act
//            List<GroupMember> result = _groupService.GetGroupMembers(groupId);

//            // ClassicAssert
//            ClassicAssert.AreEqual(2, result.Count);
//        }

//        [Test]
//        public void GetMemberFromGroup_ShouldReturnGroupMember()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMembership membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "TestMember", groupId, "role", DateTime.Now, false, false, false);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupMemberships = new List<GroupMembership> { membership } });
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(new GroupMember(groupMemberId, "TestMember", "password", "email", "phone", "description"));

//            // Act
//            GroupMember result = _groupService.GetMemberFromGroup(groupId, groupMemberId);

//            // ClassicAssert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.AreEqual(groupMemberId, result.UserId);
//        }


//        [Test]
//        public void ChangeMemberRoleInTheGroup_ShouldChangeRole()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            string newGroupRole = "admin";
//            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, false);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupMemberships = new List<GroupMembership> { membership } });

//            // Act
//            _groupService.ChangeMemberRoleInTheGroup(groupMemberId, groupId, newGroupRole);

//            // ClassicAssert
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && gm.GroupMemberRole == newGroupRole)), Times.Once);
//        }

//        [Test]
//        public void AllowMemberToBypassPostageAllowance_ShouldSetBypassPostSettingsToTrue()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, false);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupMemberships = new List<GroupMembership> { membership } });

//            // Act
//            _groupService.AllowMemberToBypassPostageRestriction(groupMemberId, groupId);

//            // ClassicAssert
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && gm.BypassPostageRestriction)), Times.Once);
//        }

//        [Test]
//        public void DisallowMemberToBypassPostageAllowance_ShouldSetBypassPostSettingsToFalse()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            var membership = new GroupMembership(Guid.NewGuid(), groupMemberId, "Test Member", groupId, "user", DateTime.Now, false, false, true);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(new UBB_SE_2024_Popsicles.Models.Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code") { ListOfGroupMemberships = new List<GroupMembership> { membership } });

//            // Act
//            _groupService.DisallowMemberToBypassPostageRestriction(groupMemberId, groupId);

//            // ClassicAssert
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(It.Is<GroupMembership>(gm => gm.GroupId == groupId && gm.GroupMemberId == groupMemberId && !gm.BypassPostageRestriction)), Times.Once);
//        }

//        [Test]
//        public void CreateNewPostOnGroupChat_ShouldAddPostToGroupWhenMemberHasBypassPostSettings()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, false, false, false);
//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

//            // Act
//            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

//            // ClassicAssert
//            ClassicAssert.AreEqual(1, group.ListOfGroupPosts.Count);
//            ClassicAssert.AreEqual(groupMemberId, group.ListOfGroupPosts[0].PostOwnerId);
//        }

//        [Test]
//        public void CreateNewPostOnGroupChat_ShouldAddPostToGroupWhenGroupHasDefaultPostSettings()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, false, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, false, false, false);
//            group.ListOfGroupMemberships.Add(groupMembership);


//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

//            // Act
//            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

//            // ClassicAssert
//            ClassicAssert.AreEqual(1, group.ListOfGroupPosts.Count);
//            ClassicAssert.AreEqual(groupMemberId, group.ListOfGroupPosts[0].PostOwnerId);
//        }

//        [Test]
//        public void CreateNewPostOnGroupChat_ShouldNotAddPostToGroupWhenMemberHasExceededPostLimit()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 1, false, false, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMemberId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, false, false, false);
//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

//            // Act
//            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);
//            //_groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null);

//            // ClassicAssert
//            ClassicAssert.Throws<Exception>(() => _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, "Test post", null));
//        }

//        [Test]
//        public void CreateNewPostOnGroupChat_BypassPostageRestriction_CreateAndAddNewGroupPost()
//        {
//            // Arrange

            
//        }

//        [Test]
//        public void GetAllGroups_ShouldReturnAllGroupsForMember()
//        {
//            // Arrange
//            Guid groupMemberId = Guid.NewGuid();
//            GroupMember groupMember = new GroupMember(groupMemberId, "Test User", "test@example.com", "password", "profilePictureUrl", "description");
//            Group group1 = new Group(Guid.NewGuid(), groupMemberId, "Test Group 1", "Test GroupDescription 1", "Test GroupIcon 1", "Test GroupBanner 1", 5, true, true, "Test Code 1");
//            Group group2 = new Group(Guid.NewGuid(), groupMemberId, "Test Group 2", "Test GroupDescription 2", "Test GroupIcon 2", "Test GroupBanner 2", 5, true, true, "Test Code 2");
//            Group group3 = new Group(Guid.NewGuid(), Guid.NewGuid(), "Test Group 3", "Test GroupDescription 3", "Test GroupIcon 3", "Test GroupBanner 3", 5, true, true, "Test Code 3");
//            GroupMembership membership1 = new GroupMembership(groupMemberId, group1.GroupId, "Test GroupName 1", group1.GroupId, "Test GroupMemberRole 1", DateTime.Now, false, false, false);
//            GroupMembership membership2 = new GroupMembership(groupMemberId, group2.GroupId, "Test GroupName 2", group2.GroupId, "Test GroupMemberRole 2", DateTime.Now, false, false, false);
//            GroupMembership membership3 = new GroupMembership(Guid.NewGuid(), group3.GroupId, "Test GroupName 3", group3.GroupId, "Test GroupMemberRole 3", DateTime.Now, false, false, false);
//            groupMember.GroupMemberships.Add(membership1);
//            groupMember.GroupMemberships.Add(membership2);
//            groupMember.GroupMemberships.Add(membership3);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(group1.GroupId)).Returns(group1);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(group2.GroupId)).Returns(group2);
//            _groupRepositoryMock.Setup(x => x.GetGroupById(group3.GroupId)).Returns(group3);
//            _groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(groupMemberId)).Returns(groupMember);

//            // Act
//            List<Group> result = _groupService.GetAllGroups(groupMemberId);

//            // ClassicAssert
//            ClassicAssert.AreEqual(3, result.Count);
//            ClassicAssert.IsTrue(result.Contains(group1));
//            ClassicAssert.IsTrue(result.Contains(group2));
//            ClassicAssert.IsTrue(result.Contains(group3));
//        }

//        [Test]
//        public void AddNewOptionToAPoll_ShouldAddOptionToPoll()
//        {
//            // Arrange
//            Guid pollId = Guid.NewGuid();
//            Guid groupId = Guid.NewGuid();
//            Guid ownerId = Guid.NewGuid();
//            Group group = new Group(groupId, ownerId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupPoll groupPoll = new GroupPoll(pollId,ownerId , "Test GroupPoll", groupId);
//            group.AddGroupPoll(groupPoll);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

//            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

//            _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option");

//            // ClassicAssert
//            ClassicAssert.AreEqual(3, groupPoll.GroupPollOptions.Count);
//            ClassicAssert.IsTrue(groupPoll.GroupPollOptions.Contains("New Option"));
//        }

//        [Test]
//        public void AddNewOptionToAPoll_ShouldThrowExceptionWhenPollNotFound()
//        {
//            // Arrange
//            Guid pollId = Guid.NewGuid();
//            Guid groupId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.AddNewOptionToAPoll(pollId, groupId, "New Option"));
//        }




//        [Test]
//        public void CreateNewPoll_ShouldAddPollToGroup()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            string pollDescription = "Test GroupPoll";
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            _groupService.CreateNewPoll(groupId, groupMemberId, pollDescription);

//            // ClassicAssert
//            ClassicAssert.AreEqual(1, group.ListOfGroupPolls.Count);
//            ClassicAssert.IsFalse(group.ListOfGroupPolls.Contains(new GroupPoll(Guid.NewGuid(), groupMemberId, "description", groupId)));
//        }


//        [Test]
//        public void EndTimeoutOfMemberFromGroup_ShouldEndTimeoutOfGroupMembership()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();

//            Guid groupMembershipId = Guid.NewGuid();
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);
//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

//            // Act
//            _groupService.EndTimeoutOfMemberFromGroup(groupMemberId, groupId);

//            // ClassicAssert
//            ClassicAssert.IsFalse(groupMembership.IsTimedOutFromGroup);
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
//        }

//        [Test]
//        public void EndTimeoutOfMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");




//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMemberId)).Returns((GroupMembership)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.EndTimeoutOfMemberFromGroup(groupMemberId, groupId));
//        }

//        [Test]
//        public void TimeoutMemberFromGroup_ShouldTimeoutGroupMembership()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();

//            Guid groupMembershipId = Guid.NewGuid();
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);
//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

//            // Act
//            _groupService.TimeoutMemberFromGroup(groupMemberId, groupId);

//            // ClassicAssert
//            ClassicAssert.IsTrue(groupMembership.IsTimedOutFromGroup);
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
//        }

//        [Test]
//        public void TimeoutMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();

//            Guid groupMembershipId = Guid.NewGuid();
//            Group group = new Group(groupId, groupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, groupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns((GroupMembership)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.TimeoutMemberFromGroup(groupMemberId, groupId));
//        }


//        [Test]
//        public void UnbanMemberFromGroup_ShouldUnbanGroupMembership()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid unbannedGroupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, unbannedGroupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            Guid groupMembershipId = Guid.NewGuid();
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, unbannedGroupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);

//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

//            // Act
//            _groupService.UnbanMemberFromGroup(unbannedGroupMemberId, groupId);

//            // ClassicAssert
//            ClassicAssert.IsFalse(groupMembership.IsBannedFromGroup);
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
//        }

//        [Test]
//        public void UnbanMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid unbannedGroupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, unbannedGroupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
            

//            Guid groupMembershipId = Guid.NewGuid();
//             GroupMembership groupMembership = new GroupMembership(groupMembershipId, unbannedGroupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);


//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns((GroupMembership)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.UnbanMemberFromGroup(unbannedGroupMemberId, groupId));
//        }

//        [Test]
//        public void BanMemberFromGroup_ShouldBanGroupMembership()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid bannedGroupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, bannedGroupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            Guid groupMembershipId = Guid.NewGuid();
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, bannedGroupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);

//            group.ListOfGroupMemberships.Add(groupMembership);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupMembershipId)).Returns(groupMembership);

//            // Act
//            _groupService.BanMemberFromGroup(bannedGroupMemberId, groupId);

//            // ClassicAssert
//            ClassicAssert.IsTrue(groupMembership.IsBannedFromGroup);
//            _groupMembershipRepositoryMock.Verify(x => x.UpdateGroupMembership(groupMembership), Times.Once);
//        }

//        [Test]
//        public void BanMemberFromGroup_ShouldThrowExceptionWhenGroupMembershipNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid bannedGroupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, bannedGroupMemberId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            Guid groupMembershipId = Guid.NewGuid();
//            GroupMembership groupMembership = new GroupMembership(groupMembershipId, bannedGroupMemberId, "Test GroupName", groupId, "Test GroupMemberRole", DateTime.Now, true, false, false);
            

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupMembershipRepositoryMock.Setup(x => x.GetGroupMembershipById(groupId)).Returns((GroupMembership)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.BanMemberFromGroup(bannedGroupMemberId, groupId));
//        }

//        [Test]
//        public void GetSpecificGroupPoll_ShouldReturnPoll()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid pollId = Guid.NewGuid();

//            Guid ownerId = Guid.NewGuid();
//            Group group = new Group(groupId, ownerId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupPoll groupPoll = new GroupPoll(pollId, ownerId, "Test GroupPoll",  groupId);
//            group.AddGroupPoll(groupPoll);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            GroupPoll result = _groupService.GetSpecificGroupPoll(groupId, pollId);

//            // ClassicAssert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.AreEqual(pollId, result.PollId);
//            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//        }

//        [Test]
//        public void GetSpecificGroupPoll_ShouldThrowExceptionWhenGroupNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid pollId = Guid.NewGuid();

//            Guid ownerId = Guid.NewGuid();
//            Group group = new Group(groupId, ownerId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupPoll groupPoll = new GroupPoll(pollId, ownerId, "Test GroupPoll", groupId);
//            // group.AddGroupPoll(groupPoll);


//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.GetSpecificGroupPoll(groupId, pollId));
//        }

//        [Test]
//        public void GetSpecificGroupPoll_ShouldThrowExceptionWhenPollNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid pollId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<InvalidOperationException>(() => _groupService.GetSpecificGroupPoll(groupId, pollId));
//        }

//        [Test]
//        public void GetGroupPolls_ShouldReturnPolls()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid ownerId = Guid.NewGuid();
//            Group group = new Group(groupId, ownerId, "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            GroupPoll poll1 = new GroupPoll(Guid.NewGuid(), ownerId, "Test Poll1", groupId);
//            GroupPoll poll2 = new GroupPoll(Guid.NewGuid(), Guid.NewGuid(), "Test Poll2", groupId);
//            group.AddGroupPoll(poll1);
//            group.AddGroupPoll(poll2);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            List<GroupPoll> result = _groupService.GetGroupPolls(groupId);

//            // ClassicAssert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.AreEqual(2, result.Count);
//            ClassicAssert.IsTrue(result.Contains(poll1));
//            ClassicAssert.IsTrue(result.Contains(poll2));
//            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//        }

//        [Test]
//        public void GetGroupPolls_ShouldThrowExceptionWhenGroupNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");


//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns((Group)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<NullReferenceException>(() => _groupService.GetGroupPolls(groupId));
//        }

//        [Test]
//        public void GetRequestsToJoin_ShouldReturnRequests()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");
//            JoinRequest request1 = new JoinRequest(Guid.NewGuid(), Guid.NewGuid(), "Test User 1", groupId);
//            JoinRequest request2 = new JoinRequest(Guid.NewGuid(), Guid.NewGuid(), "Test User 2", groupId);
//            group.AddJoinRequest(request1);
//            group.AddJoinRequest(request2);

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            List<JoinRequest> result = _groupService.GetRequestsToJoin(groupId);

//            // ClassicAssert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.AreEqual(2, result.Count);
//            ClassicAssert.IsTrue(result.Contains(request1));
//            ClassicAssert.IsTrue(result.Contains(request2));
//            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//        }

//        [Test]
//        public void GetRequestsToJoin_ShouldThrowExceptionWhenGroupNotFound()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns((Group)null);

//            // Act and ClassicAssert
//            ClassicAssert.Throws<NullReferenceException>(() => _groupService.GetRequestsToJoin(groupId));
//        }


//        [Test]
//        public void GetGroup_ShouldReturnGroup()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act
//            Group result = _groupService.GetGroup(groupId);

//            // ClassicAssert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.AreEqual(groupId, result.GroupId);
//            _groupRepositoryMock.Verify(x => x.GetGroupById(groupId), Times.Once);
//        }

//        [Test]
//        public void DeleteGroup_ShouldDeleteGroup()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test GroupDescription", "Test GroupIcon", "Test GroupBanner", 5, true, true, "Test Code");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);
//            _groupRepositoryMock.Setup(x => x.RemoveGroupById(groupId));

//            // Act
//            _groupService.DeleteGroup(groupId);

//            // ClassicAssert
//            _groupRepositoryMock.Verify(x => x.RemoveGroupById(groupId), Times.Once);
//        }


//        [Test]
//        public void CreateGroup_ShouldCreateGroup()
//        {
//            //// Arrange
//            //Guid ownerId = Guid.NewGuid();
//            //GroupMember owner = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
//            //    Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");


//            //_groupMemberRepositoryMock.Setup(x => x.GetGroupMemberById(ownerId)).Returns(owner);
//            //_groupRepositoryMock.Setup(x => x.AddGroup(It.IsAny<Group>()));
//            //_groupMemberRepositoryMock.Setup(x => x.AddGroupMember(It.IsAny<GroupMember>()));

//            //// Act
//            //_groupService.CreateGroup(ownerId);

//            //// ClassicAssert
//            //_groupRepositoryMock.Verify(x => x.AddGroup(It.IsAny<Group>()), Times.Once);
//            //_groupMemberRepositoryMock.Verify(x => x.AddGroupMember(It.IsAny<GroupMember>()), Times.Once);
//        }



//        [Test]
//        public void GetMemberFromGroup_ShouldThrowException_WhenGroupMemberDoesNotExist()
//        {
//            // Arrange
//            Guid groupId = Guid.NewGuid();
//            Guid groupMemberId = Guid.NewGuid();
//            Group group = new Group(groupId, groupMemberId, "defaultGroupName","defaultGroupDescription", "defaultGroupIcon", "defaultGroupBanner", 5, false, false, "defaultGroupCode");

//            _groupRepositoryMock.Setup(x => x.GetGroupById(groupId)).Returns(group);

//            // Act and ClassicAssert
////            ClassicAssert.Throws<Exception>(() => _groupService.GetMemberFromGroup(groupId, groupMemberId));
//        }



//    }


//}



