using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2024_Popsicles.Repositories;
using UBB_SE_2024_Popsicles.Services;

using UBB_SE_2024_Popsicles.Models;
using System.Data.SqlClient;

namespace Test_UBB_SE_2024_Popsicles.TestService
{

    // /////////////////////// IMPORTANT /////////////////////////// //

    // when you run the tests please make sure you added this line of code in the main project in AssemblyInfo.cs   ->
    //         
    //                 [assembly: InternalsVisibleTo("Test-UBB-SE-2024-Popsicles")]
    //

    //  also, comment this line: " AddMemberToGroup(ownerId, groupId, "admin"); "  in 'CreateGroup' method which is in the GroupService,  so the tests can actually work,
    //  i found it so stupid, i hope you ll figure out why

    // ps. make PUBLIC interfaces

    // ////////////////////////////////////////////////////////////// //









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

        private GroupMemberRepository _groupMemberRepository;
        private GroupMembershipRepository _groupMembershipRepository;
        private GroupRepository _groupRepository;
        private RequestsRepository _requestsRepository;

        [SetUp]
        public void Setup()
        {
            
            var connection = "Server=DESKTOP-TBF404G\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection connectionStringProvider = new SqlConnection(connection);
            
            _groupRepository = new GroupRepository(connectionStringProvider);

            _groupMemberRepository = new GroupMemberRepository(connectionStringProvider);

            _groupMembershipRepository = new GroupMembershipRepository(connectionStringProvider);

            _requestsRepository = new RequestsRepository(connectionStringProvider);

            _groupService = new GroupService(
                _groupRepository,
                _groupMemberRepository,
                _groupMembershipRepository,
                _requestsRepository);


        }


        [Test]
        public void CreateGroup_ShouldAddGroupToRepository()
        {
            // Arrange
            Guid newGroupMemberId = _groupMemberRepository.GetGroupMembers().Last().Id;

            

            //_groupMemberRepository.AddGroupMember(new GroupMember(newGroupMemberId, "newGroupMember@example.com", "New Group Member"));

            Guid ownerId = newGroupMemberId;
            // Act
            _groupService.CreateGroup(ownerId);

            // Assert
            Assert.That(_groupRepository.GetGroups().Last().OwnerId, Is.EqualTo(ownerId));
            Assert.That(_groupRepository.GetGroups().Last().Name, Is.Not.Empty);
            Assert.That(_groupRepository.GetGroups().Last().Description, Is.Not.Empty);
            Assert.That(_groupRepository.GetGroups().Last().Icon, Is.Not.Empty);
            Assert.That(_groupRepository.GetGroups().Last().Banner, Is.Not.Empty);
            Assert.That(_groupRepository.GetGroups().Last().MaxPostsPerHourPerUser, Is.GreaterThan(0));
            Assert.That(_groupRepository.GetGroups().Last().IsPublic, Is.False);
            Assert.That(_groupRepository.GetGroups().Last().CanMakePostsByDefault, Is.False);
            Assert.That(_groupRepository.GetGroups().Last().GroupCode, Is.Not.Empty);
            //Assert.That(_groupRepository.GetGroups().Last().CreatedAt, Is.Not.Empty);

            
        //Assert.That(_groupMemberRepository.GetGroupMemberById(ownerId), Is.EqualTo());
        //Assert.That(_groupMemberRepository.GetGroupMembers(_groupRepository.GetGroups()[0].Id).Count, Is.EqualTo(1));
        //Assert.That(_groupMemberRepository.GetGroupMembers(_groupRepository.GetGroups()[0].Id)[0].UserId, Is.EqualTo(ownerId));
        //Assert.That(_groupMemberRepository.GetGroupMembers(_groupRepository.GetGroups()[0].Id)[0].Role, Is.EqualTo("admin"));
    }

        [Test]
        public void AddMemberToGroup_ShouldAddMemberToGroup()
        {
            // Arrange
            // Create a new group
            //Guid groupId = Guid.NewGuid();
            //Group group = new Group(groupId, Guid.NewGuid(), "Test Group", "Test Description", "Test Icon",
            //    "Test Banner", 5, true, true, "Test Code");
            //_groupRepository.AddGroup(group);

            _groupService.CreateGroup(_groupMemberRepository.GetGroupMembers().Last().Id);

            // Assert
            Guid groupId = _groupRepository.GetGroups().Last().Id;




            // Create a new group member
            Guid groupMemberId = Guid.NewGuid();
            GroupMember groupMember =
                new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0,10), "description");
            _groupMemberRepository.AddGroupMember(groupMember);

            string userRole = "admin";

            // Act
            _groupService.AddMemberToGroup(groupMemberId, groupId, userRole);

            // Assert
            // Check if the group now contains the new member
            Group updatedGroup = _groupRepository.GetGroupById(groupId);
            Assert.IsTrue(updatedGroup.Memberships.Any(m => m.GroupMemberId == groupMemberId && m.Role == userRole));

            // Check if the group member now contains the new membership
            GroupMember updatedGroupMember = _groupMemberRepository.GetGroupMemberById(groupMemberId);
            Assert.IsTrue(updatedGroupMember.Memberships.Any(m => m.GroupId == groupId && m.Role == userRole));

            // Since we don't have a direct method to retrieve the membership,
            // we can verify indirectly by ensuring that the count of group memberships has increased
            List<GroupMembership> memberships = _groupMembershipRepository.GetGroupMemberships();
            int initialCount = memberships.Count;

            // Act: Call AddMemberToGroup again to see if the count increases
            _groupService.AddMemberToGroup(groupMemberId, groupId, userRole);

            // Assert: Check if the count of group memberships has increased by one
            int finalCount = _groupMembershipRepository.GetGroupMemberships().Count;
            Assert.AreEqual(initialCount + 1, finalCount);
        }

        [Test]
        public void GetGroup_WhenGroupExists_ReturnsGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid ownerId = _groupMemberRepository.GetGroupMembers().Last().Id;

            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);

            // Act
            Group returnedGroup = _groupService.GetGroup(groupId);

            // Assert
            Assert.IsNotNull(returnedGroup);
            Assert.AreEqual(groupId, returnedGroup.Id);
            Assert.AreEqual(ownerId, returnedGroup.OwnerId);
            Assert.AreEqual("Test Group", returnedGroup.Name);
            Assert.AreEqual("Test Description", returnedGroup.Description);
            Assert.AreEqual("Test Icon", returnedGroup.Icon);
            Assert.AreEqual("Test Banner", returnedGroup.Banner);
            Assert.AreEqual(5, returnedGroup.MaxPostsPerHourPerUser);
            Assert.AreEqual(true, returnedGroup.IsPublic);
            Assert.AreEqual(true, returnedGroup.CanMakePostsByDefault);
            Assert.AreEqual("Test Code", returnedGroup.GroupCode);
        }


        [Test]
        public void GetMemberFromGroup_WhenGroupMemberExists_ReturnsGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId = Guid.NewGuid();
            string username = Guid.NewGuid().ToString();
            string password = Guid.NewGuid().ToString();
            string email = Guid.NewGuid().ToString();
            string phone = Guid.NewGuid().ToString().Substring(0, 10);

            GroupMember member = new GroupMember(memberId, username, password, email, phone, "Test User Description");
            _groupMemberRepository.AddGroupMember(member);

            Group group = new Group(groupId, memberId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            group.Memberships = new List<GroupMembership>
            {
                new GroupMembership(Guid.NewGuid(), memberId, "Test Membership", groupId, "Test Role", DateTime.Now, false, false, true)
            };
            _groupRepository.AddGroup(group);

            _groupService.AddMemberToGroup(memberId,groupId);

            // Act
            GroupMember returnedMember = _groupService.GetMemberFromGroup(groupId, memberId);

            // Assert
            Assert.IsNotNull(returnedMember);
            Assert.AreEqual(memberId, returnedMember.Id);
            Assert.AreEqual(username, returnedMember.Username);
            Assert.AreEqual(email, returnedMember.Email);
            Assert.AreEqual(phone, returnedMember.Phone);
            Assert.AreEqual(password, returnedMember.Password);
            Assert.AreEqual("Test User Description", returnedMember.Description);
        }


        [Test]
        public void GetMemberFromGroup_WhenGroupMemberDoesNotExist_ThrowsException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId = _groupMemberRepository.GetGroupMembers().Last().Id;

            Group group = new Group(groupId, memberId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);

            // Act & Assert
            Assert.Throws<Exception>(() => _groupService.GetMemberFromGroup(groupId, memberId));
        }


        [Test]
        public void GetMemberFromGroup_WhenGroupMemberIsFound_ReturnsGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();

            GroupMember groupMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            _groupMemberRepository.AddGroupMember(groupMember);

            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);

            GroupMembership membership = new GroupMembership(Guid.NewGuid(), groupMemberId, Guid.NewGuid().ToString(), groupId, Guid.NewGuid().ToString(), DateTime.Now, false, false, true);
            group.Memberships.Add(membership);

            // Act
            GroupMember result = _groupService.GetMemberFromGroup(groupId, groupMemberId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(groupMemberId, result.Id);
        }


        [Test]
        public void TestDeleteGroupService()
        {
            Guid Id = Guid.NewGuid();
            Guid OwnerId = _groupRepository.GetGroups()[0].OwnerId;
            string Name = "Test Group";
            string Description = "Test Description";
            string Icon = "test_icon.png";
            string Banner = "test_banner.png";
            int MaxPostsPerHourPerUser = 10;
            string GroupCode = "TEST";
            bool IsPublic = true;
            bool CanMakePostsByDefault = true;

            Group group = new Group(Id, OwnerId, Name, Description, Icon, Banner, MaxPostsPerHourPerUser, IsPublic, CanMakePostsByDefault, GroupCode);

            // Act
            _groupRepository.AddGroup(group);

            // Act
            _groupService.DeleteGroup(Id);
            Assert.IsFalse(_groupRepository.GetGroups().Any(g => g.Id == group.Id));
        }

        [Test]
        public void GetGroupPosts_WhenGroupExistsAndHasPosts_ReturnsPosts()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid postId1 = Guid.NewGuid();
            Guid postId2 = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            GroupMember owner = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "Owner Description");
            _groupMemberRepository.AddGroupMember(owner);

            GroupPost post1 = new GroupPost(postId1, ownerId, "Post 1", "Description 1",groupId);
            GroupPost post2 = new GroupPost(postId2, ownerId, "Post 2", "Description 2", groupId);



            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);

            group.AddPost(post1);
            group.AddPost(post2);

            // Act
            List<GroupPost> posts = _groupService.GetGroupPosts(groupId);

            // Assert
            Assert.IsNotNull(posts);
            Assert.AreEqual(2, posts.Count);
            Assert.IsTrue(posts.Any(p => p.Id == postId1));
            Assert.IsTrue(posts.Any(p => p.Id == postId2));
        }


        [Test]
        public void GetGroupPosts_WhenGroupDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            Guid ownerId = _groupMemberRepository.GetGroupMembers().Last().Id;


            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);
            // Act
            List<GroupPost> posts = _groupService.GetGroupPosts(groupId);

            // Assert
            Assert.IsNotNull(posts);
            Assert.AreEqual(0, posts.Count);
        }


        [Test]
        public void UpdateGroup_WhenGroupDoesNotExist_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            string newGroupName = "New Group Name";
            string newGroupDescription = "New Group Description";
            string newGroupIcon = "New Group Icon";
            string newGroupBanner = "New Group Banner";
            int maxPostsPerHourPerUser = 10;
            bool isTheGroupPublic = true;
            bool allowanceOfPostageByDefault = true;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.UpdateGroup(groupId, newGroupName, newGroupDescription, newGroupIcon, newGroupBanner, maxPostsPerHourPerUser, isTheGroupPublic, allowanceOfPostageByDefault));
        }

        [Test]
        public void UpdateGroup_WhenGroupExists_ShouldUpdateGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            string originalGroupName = "Original Group Name";
            string originalGroupDescription = "Original Group Description";
            string originalGroupIcon = "Original Group Icon";
            string originalGroupBanner = "Original Group Banner";
            int originalMaxPostsPerHourPerUser = 5;
            bool originalIsTheGroupPublic = true;
            bool originalAllowanceOfPostageByDefault = true;
            string groupCode = "TestCode";

            GroupMember owner = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0,10), "Owner Description");
            _groupMemberRepository.AddGroupMember(owner);

            Group group = new Group(groupId, ownerId, originalGroupName, originalGroupDescription, originalGroupIcon, originalGroupBanner, originalMaxPostsPerHourPerUser, originalIsTheGroupPublic, originalAllowanceOfPostageByDefault, groupCode);
            _groupRepository.AddGroup(group);

            string newGroupName = "New Group Name";
            string newGroupDescription = "New Group Description";
            string newGroupIcon = "New Group Icon";
            string newGroupBanner = "New Group Banner";
            int newMaxPostsPerHourPerUser = 10;
            bool newIsTheGroupPublic = false;
            bool newAllowanceOfPostageByDefault = false;

            // Act
            _groupService.UpdateGroup(groupId, newGroupName, newGroupDescription, newGroupIcon, newGroupBanner, newMaxPostsPerHourPerUser, newIsTheGroupPublic, newAllowanceOfPostageByDefault);

            // Assert
            Group updatedGroup = _groupRepository.GetGroupById(groupId);
            Assert.AreEqual(newGroupName, updatedGroup.Name);
            Assert.AreEqual(newGroupDescription, updatedGroup.Description);
            Assert.AreEqual(newGroupIcon, updatedGroup.Icon);
            Assert.AreEqual(newGroupBanner, updatedGroup.Banner);
            Assert.AreEqual(newMaxPostsPerHourPerUser, updatedGroup.MaxPostsPerHourPerUser);
            Assert.AreEqual(newIsTheGroupPublic, updatedGroup.IsPublic);
            Assert.AreEqual(newAllowanceOfPostageByDefault, updatedGroup.CanMakePostsByDefault);
        }


        [Test]
        public void UnbanMemberFromGroup_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.UnbanMemberFromGroup(memberId2, group1Id));
        }

        [Test]
        public void UnbanMemberFromGroup_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,true,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.UnbanMemberFromGroup(memberId1, group1Id);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.IsFalse(groupMembership.IsBanned);
        }

        [Test]
        public void BanMemberFromGroup_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.BanMemberFromGroup(memberId2, group1Id));
        }

        [Test]
        public void BanMemberFromGroup_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.BanMemberFromGroup(memberId1, group1Id);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.IsTrue(groupMembership.IsBanned);
        }


        [Test]
        public void TimeoutMemberFromGroup_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.TimeoutMemberFromGroup(memberId2, group1Id));
        }

        [Test]
        public void TimeoutMemberFromGroup_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.TimeoutMemberFromGroup(memberId1, group1Id);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.IsTrue(groupMembership.IsTimedOut);
        }




        [Test]
        public void EndTimeoutOfMemberFromGroup_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.EndTimeoutOfMemberFromGroup(memberId2, group1Id));
        }

        [Test]
        public void EndTimeoutOfMemberFromGroup_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,true,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.EndTimeoutOfMemberFromGroup(memberId1, group1Id);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.IsFalse(groupMembership.IsTimedOut);
        }




        [Test]
        public void ChangeMemberRoleInTheGroup_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();
            string newRole = "newRole";

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.ChangeMemberRoleInTheGroup(memberId2, group1Id, newRole));
        }

        [Test]
        public void ChangeMemberRoleInTheGroup_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            string newRole = "newRole";

            // Act
            _groupService.ChangeMemberRoleInTheGroup(memberId1, group1Id, newRole);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.AreEqual(newRole, groupMembership.Role);
        }

        [Test]
        public void AllowMemberToBypassPostageAllowance_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.AllowMemberToBypassPostageAllowance(memberId2, group1Id));
        }

        [Test]
        public void AllowMemberToBypassPostageAllowance_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.AllowMemberToBypassPostageAllowance(memberId1, group1Id);

            // Assert
            GroupMembership groupMembership = group1.GetMembership(memberId1);
            Assert.IsTrue(groupMembership.ByPassPostSettings);
        }

        [Test]
        public void DisallowMemberToBypassPostageAllowance_WhenGroupExistsButMemberIsNotInGroup_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
        Guid memberId1 = Guid.NewGuid();
        Guid memberId2 = Guid.NewGuid();

        GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
        GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

        _groupMemberRepository.AddGroupMember(member1);
    _groupMemberRepository.AddGroupMember(member2);

    Guid group1Id = Guid.NewGuid();

        Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

        _groupRepository.AddGroup(group1);

    List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

        _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => _groupService.DisallowMemberToBypassPostageAllowance(memberId2, group1Id));
}

    [Test]
    public void DisallowMemberToBypassPostageAllowance_WhenGroupExistsAndMemberIsInGroup_ShouldUpdateGroupMembership()
    {
        // Arrange
        Guid groupId = Guid.NewGuid();
        Guid memberId1 = Guid.NewGuid();

        GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

        _groupMemberRepository.AddGroupMember(member1);

        Guid group1Id = Guid.NewGuid();

        Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

        _groupRepository.AddGroup(group1);

        List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

        _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

        // Act
        _groupService.DisallowMemberToBypassPostageAllowance(memberId1, group1Id);

        // Assert
        GroupMembership groupMembership = group1.GetMembership(memberId1);
        Assert.IsFalse(groupMembership.ByPassPostSettings);
    }


    [Test]
        public void RejectRequestToJoinGroup_WhenRequestExists_ShouldRemoveRequestFromGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");


            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            Guid requestId = _groupService.GetRequestsToJoin(group1Id).Last().Id;

            // Act
            _groupService.RejectRequestToJoinGroup(requestId);

            // Assert
            Assert.IsFalse(group1.Requests.Any(r => r.Id == requestId));
        }

        [Test]
        public void RejectRequestToJoinGroup_WhenRequestExists_ShouldRemoveRequestFromGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");


            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);


            Guid requestId = _groupService.GetRequestsToJoin(group1Id).Last().Id;

            // Act
            _groupService.RejectRequestToJoinGroup(requestId);

            // Assert
            Assert.IsFalse(member2.OutgoingRequests.Any(r => r.Id == requestId));
        }



        [Test]
        public void AcceptRequestToJoinGroup_WhenRequestExists_ShouldAddMemberToGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            

            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            Guid requestId = _groupService.GetRequestsToJoin(group1Id).Last().Id;

            // Act
            _groupService.AcceptRequestToJoinGroup(requestId);

            // Assert
            Assert.IsTrue(group1.Memberships.Any(m => m.GroupMemberId == memberId2));
        }

        [Test]
        public void AcceptRequestToJoinGroup_WhenRequestExists_ShouldRemoveRequestFromGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            

            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            Guid requestId = _groupService.GetRequestsToJoin(group1Id).Last().Id;

            // Act
            _groupService.AcceptRequestToJoinGroup(requestId);

            // Assert
            Assert.IsFalse(group1.Requests.Any(r => r.Id == requestId));
        }

        [Test]
        public void AcceptRequestToJoinGroup_WhenRequestExists_ShouldRemoveRequestFromGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");


            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            Guid requestId = _groupService.GetRequestsToJoin(group1Id).Last().Id;

            // Act
            _groupService.AcceptRequestToJoinGroup(requestId);

            // Assert
            Assert.IsFalse(member2.OutgoingRequests.Any(r => r.Id == requestId));
        }



        [Test]
        public void AddNewRequestToJoinGroup_WhenGroupMemberIsNotInGroup_ShouldAddRequestToGroup()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            // Assert
            Assert.AreEqual(1, group1.Requests.Count);
            Assert.AreEqual(memberId2, group1.Requests[0].GroupMemberId);
            Assert.AreEqual(group1Id, group1.Requests[0].GroupId);
        }

        [Test]
        public void AddNewRequestToJoinGroup_WhenGroupMemberIsNotInGroup_ShouldAddRequestToGroupMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();

            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");

            _groupRepository.AddGroup(group1);

            List<GroupMembership> memberships = new List<GroupMembership>
    {
        new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
    };

            _groupService.AddMemberToGroup(memberId1, group1Id, "admin");

            // Act
            _groupService.AddNewRequestToJoinGroup(memberId2, group1Id);

            // Assert
            Assert.AreEqual(1, member2.OutgoingRequests.Count);
            Assert.AreEqual(group1Id, member2.OutgoingRequests[0].GroupId);
            Assert.AreEqual(memberId2, member2.OutgoingRequests[0].GroupMemberId);
        }


        [Test]
        public void GetGroupMembers_WhenGroupExistsAndHasMembers_ReturnsMembers()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid memberId1 = Guid.NewGuid();
            Guid memberId2 = Guid.NewGuid();
            
            
            GroupMember member1 = new GroupMember(memberId1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            GroupMember member2 = new GroupMember(memberId2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

            _groupMemberRepository.AddGroupMember(member1);
            _groupMemberRepository.AddGroupMember(member2);

            Guid group1Id = Guid.NewGuid();


            Group group1 = new Group(group1Id, memberId1, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            
            
            _groupRepository.AddGroup(group1);
            
            List<GroupMembership> memberships = new List<GroupMembership>
            {
                new GroupMembership(Guid.NewGuid(), memberId1, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
                new GroupMembership(Guid.NewGuid(), memberId2, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
             };

            _groupService.AddMemberToGroup(memberId2,group1Id);

            _groupService.AddMemberToGroup(memberId1, group1Id , "admin");

            // Act
            List<GroupMember> members = _groupService.GetGroupMembers(group1Id);

            // Assert
            Assert.IsNotNull(members);
            Assert.AreEqual(2, members.Count);
            Assert.IsTrue(members.Any(m => m.Id == memberId1));
            Assert.IsTrue(members.Any(m => m.Id == memberId2));
        }

        [Test]
        public void GetGroupMembers_WhenGroupExistsButHasNoMembers_ReturnsEmptyList()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            Group group = new Group(groupId, _groupMemberRepository.GetGroupMembers().Last().Id, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            group.Memberships = new List<GroupMembership>();

            _groupRepository.AddGroup(group);

            //_groupService.AddMemberToGroup(_groupMemberRepository.GetGroupMembers().Last().Id,groupId);

            // Act
            List<GroupMember> members = _groupService.GetGroupMembers(groupId);

            // Assert
            Assert.IsNotNull(members);
            Assert.AreEqual(0, members.Count);
        }

        [Test]
        public void GetGroupMembers_WhenGroupDoesNotExist_ThrowsException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _groupService.GetGroupMembers(groupId));
        }


        [Test]
        public void GetMemberFromGroup_ShouldReturnCorrectMember()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();

            // Create a group and add memberships
            Group group = new Group(groupId, _groupMemberRepository.GetGroupMembers().Last().Id, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);
            GroupMembership membership = new GroupMembership(Guid.NewGuid(), groupMemberId, Guid.NewGuid().ToString(),groupId, Guid.NewGuid().ToString(),  DateTime.Now, false, false, true);
            group.Memberships.Add(membership);

            // Add member to group member repository
            GroupMember groupMember = new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            _groupMemberRepository.AddGroupMember(groupMember);

            // Act
            GroupMember retrievedMember = _groupService.GetMemberFromGroup(groupId, groupMemberId);

            // Assert
            Assert.IsNotNull(retrievedMember);
            Assert.AreEqual(groupMemberId, retrievedMember.Id);
        }

        [Test]
        public void GetMemberFromGroup_WhenMemberNotFound_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = Guid.NewGuid();


            Group group = new Group(groupId, _groupMemberRepository.GetGroupMembers().Last().Id, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);

            // Act & Assert
            Assert.Throws<Exception>(() => _groupService.GetMemberFromGroup(groupId, _groupMemberRepository.GetGroupMembers().Last().Id));
        }
    

        [Test]
        public void GetAllGroups_WhenGroupMemberHasMemberships_ReturnsListOfGroups()
        {
            // Arrange
                //Guid groupMemberId = _groupMemberRepository.GetGroupMembers().Last().Id;
            GroupMember groupMember = new GroupMember(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0,10), "description");
            _groupMemberRepository.AddGroupMember(groupMember);

            Guid newMemberGuid = _groupMemberRepository.GetGroupMembers().Last().Id;

            // Create some memberships for the group member


            Guid group1Id = Guid.NewGuid(); 
            Guid group2Id = Guid.NewGuid();
            Guid group3Id = Guid.NewGuid();

            Group group1 = new Group(group1Id, newMemberGuid, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Group group2 = new Group(group2Id, newMemberGuid, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            Group group3 = new Group(group3Id, newMemberGuid, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group1);
            _groupRepository.AddGroup(group2);
            _groupRepository.AddGroup(group3);


            List<GroupMembership> memberships = new List<GroupMembership>
                {
                    new GroupMembership(Guid.NewGuid(), newMemberGuid, Guid.NewGuid().ToString(),group1Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
                    new GroupMembership(Guid.NewGuid(), newMemberGuid, Guid.NewGuid().ToString(),group2Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true),
                    new GroupMembership(Guid.NewGuid(), newMemberGuid, Guid.NewGuid().ToString(),group3Id,Guid.NewGuid().ToString(), DateTime.Now,false,false,true)
                };
            groupMember.Memberships.AddRange(memberships);

                // Create groups corresponding to the memberships
                List<Group> expectedGroups = new List<Group>();
                foreach (var membership in memberships)
                {
                    Group group = new Group(membership.GroupId, newMemberGuid, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
                    expectedGroups.Add(group);
                }

                // Act
                List<Group> actualGroups = _groupService.GetAllGroups(newMemberGuid);

                // Assert
                Assert.AreEqual(expectedGroups.Count, actualGroups.Count);
                for (int i = 0; i < expectedGroups.Count; i++)
                {
                    Assert.AreEqual(expectedGroups[i].Id, actualGroups[i].Id);
                }
            

        }




        [Test]
        public void CreateNewPostOnGroupChat_ByPassPostSettings_ShouldAddPost()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = _groupMemberRepository.GetGroupMembers().Last().Id;
            string postContent = "Test Post Content";
            string postImage = "test.jpg";
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, true, "Test Code");
            _groupRepository.AddGroup(group);
            _groupService.AddMemberToGroup(groupMemberId, groupId, "admin");

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);

            // Assert
            Assert.AreEqual(1, group.Posts.Count);
        }

        [Test]
        public void CreateNewPostOnGroupChat_WithinPostLimit_ShouldAddPost()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = _groupMemberRepository.GetGroupMembers().Last().Id;
            string postContent = "Test Post Content";
            string postImage = "test.jpg";
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon", "Test Banner", 5, true, false, "Test Code");
            _groupRepository.AddGroup(group);
            _groupService.AddMemberToGroup(groupMemberId, groupId, "admin");

            // Create and add posts to reach post limit
            for (int i = 0; i < group.MaxPostsPerHourPerUser-2; i++)
            {
                Guid postId = Guid.NewGuid();
                GroupPost post = new GroupPost(postId, groupMemberId, "Post Content", "image.jpg", groupId);
                group.Posts.Add(post);
            }

            // Act
            _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);

            // Assert
            Assert.AreEqual(group.MaxPostsPerHourPerUser -1, group.Posts.Count);
        }

        [Test]
        public void CreateNewPostOnGroupChat_ExceedPostLimit_ShouldThrowException()
        {
            // Arrange
            Guid groupId = Guid.NewGuid();
            Guid groupMemberId = _groupMemberRepository.GetGroupMembers().Last().Id;
            string postContent = "Test Post Content";
            string postImage = "test.jpg";
            Group group = new Group(groupId, groupMemberId, "Test Group", "Test Description", "Test Icon",
                "Test Banner", 5, false, false, "Test Code");
            _groupRepository.AddGroup(group);
            _groupService.AddMemberToGroup(groupMemberId, groupId, "admin");

            Guid postId_1 = Guid.NewGuid();
            GroupPost post_1 = new GroupPost(postId_1, groupMemberId, "Post Content", "image.jpg", groupId);
            group.Posts.Add(post_1);


            Assert.AreEqual(1,group.Posts.Count);

            for (int i = 0; i < group.MaxPostsPerHourPerUser-1; i++)
            {
                Guid postId = Guid.NewGuid();
                GroupPost post = new GroupPost(postId, groupMemberId, "Post Content", "image.jpg", groupId);
                group.Posts.Add(post);
            }

            //Guid postId_1 = Guid.NewGuid();
            //GroupPost post_1 = new GroupPost(postId_1, groupMemberId, "Post Content", "image.jpg", groupId);
            //group.Posts.Add(post_1);

            // postId_1 = Guid.NewGuid();
            // post_1 = new GroupPost(postId_1, groupMemberId, "Post Content", "image.jpg", groupId);
            //group.Posts.Add(post_1);

            //postId_1 = Guid.NewGuid();
            //post_1 = new GroupPost(postId_1, groupMemberId, "Post Content", "image.jpg", groupId);
            //group.Posts.Add(post_1);

            Console.WriteLine($"Number of posts: {group.Posts.Count}");

            Console.WriteLine($"Max posts : {group.MaxPostsPerHourPerUser}");
            

            // Act & Assert
            try
            {
                _groupService.CreateNewPostOnGroupChat(groupId, groupMemberId, postContent, postImage);
                Assert.Fail("Expected an exception to be thrown");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown: {ex.Message}");
                Assert.IsNotNull(ex);
            }
        }


        [Test]
        public void AddNewOptionToAPoll_ShouldAddOptionToPoll()
        {
            Guid ownerId = Guid.NewGuid();
            //Guid pollId = Guid.Parse("EE632931-EEB0-4048-92E3-0BB1E1C186F2");
            string newPollOption = "New Option";

            Guid groupMemberId = ownerId;
            GroupMember groupMember =
                new GroupMember(groupMemberId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");
            _groupMemberRepository.AddGroupMember(groupMember);


            // Create a group
            _groupService.CreateGroup(ownerId);

            Guid groupId = _groupRepository.GetGroups().Last().Id;

            //new Poll(pollId, ownerId, Guid.NewGuid().ToString(), groupId)

            _groupService.CreateNewPoll(groupId, ownerId, "Test Poll");
            //_groupRepository.GetGroups().Last().GetPoll());
            Guid pollID = _groupService.GetGroupPolls(groupId).Last().Id;
            // Act
            _groupService.AddNewOptionToAPoll(pollID, groupId, newPollOption);

            // Assert
            // Retrieve the updated poll from the repository
            var updatedPoll = _groupService.GetSpecificGroupPoll(groupId, pollID);
            // Check if the new option is present in the poll's options
            Assert.IsTrue(updatedPoll.Options.Contains(newPollOption),
                $"The new poll option '{newPollOption}' was not added to the poll with ID '{pollID}' in group '{groupId}'.");

            Assert.Throws
                <InvalidOperationException>(() =>
            {
                _groupService.AddNewOptionToAPoll(Guid.NewGuid(), _groupRepository.GetGroups()[10].Id, "");
            });
        }



    }


}



