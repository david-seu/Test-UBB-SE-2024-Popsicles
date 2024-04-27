//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Moq;
//using UBB_SE_2024_Popsicles.Models;
//using UBB_SE_2024_Popsicles.Services;
//using UBB_SE_2024_Popsicles.Repositories;
//using System.Data.SqlClient;


//namespace Test_UBB_SE_2024_Popsicles.TestService
//{
   
//    internal class TestFeedService
//    {
//        //    private Mock<IFeedService> _feedServiceMock;

//        //    [SetUp]
//        //    public void Setup()
//        //    {
//        //        _feedServiceMock = new Mock<IFeedService>();
//        //    }

//        //    [Test]
//        //    public void FilterPostsByTags_WithMatchingTags_ReturnsFilteredPosts()
//        //    {
//        //        // Arrange
//        //        var posts = new List<GroupPost>
//        //        {
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
//        //        };

//        //        var tags = new List<string> { "tag1", "tag4" };

//        //        _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(posts.Take(2).ToList());

//        //        // Act
//        //        var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

//        //        // Assert
//        //        Assert.AreEqual(2, result.Count);
//        //    }
//        //    [Test]
//        //    public void FilterPostsByTags_WithNoMatchingTags_ReturnsEmptyList()
//        //    {
//        //        // Arrange
//        //        var posts = new List<GroupPost>
//        //{
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
//        //};

//        //        var tags = new List<string> { "tag5", "tag6" };

//        //        _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(new List<GroupPost>());

//        //        // Act
//        //        var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

//        //        // Assert
//        //        Assert.AreEqual(0, result.Count);
//        //    }

//        //    [Test]
//        //    public void FilterPostsByTags_WithPinnedPosts_ReturnsPinnedPostsFirst()
//        //    {
//        //        // Arrange
//        //        var posts = new List<GroupPost>
//        //{
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
//        //    new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
//        //};
//        //        posts[0].IsPinned = true;
//        //        posts[2].IsPinned = true;

//        //        var tags = new List<string> { "tag1", "tag4" };

//        //        _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(posts);

//        //        // Act
//        //        var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

//        //        // Assert
//        //        Assert.AreEqual(3, result.Count);
//        //        Assert.IsTrue(result[0].IsPinned);
//        //        Assert.IsFalse(result[1].IsPinned);
//        //        Assert.IsTrue(result[2].IsPinned);
//        //    }

//        //}




//        private IFeedService _feedService;
//        private GroupMemberRepository _groupMemberRepository;
//        private GroupMembershipRepository _groupMembershipRepository;
//        private GroupRepository _groupRepository;
//        private RequestsRepository _requestsRepository;
//        private IGroupService _groupService;

//        [SetUp]
//        public void Setup()
//        {
//            _feedService = new FeedService();

//            var connection = "Server=DESKTOP-TBF404G\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
//            SqlConnection connectionStringProvider = new SqlConnection(connection);

//            _groupRepository = new GroupRepository(connectionStringProvider);

//            _groupMemberRepository = new GroupMemberRepository(connectionStringProvider);

//            _groupMembershipRepository = new GroupMembershipRepository(connectionStringProvider);

//            _requestsRepository = new RequestsRepository(connectionStringProvider);

//            _groupService = new GroupService(
//                _groupRepository,
//                _groupMemberRepository,
//                _groupMembershipRepository,
//                _requestsRepository);


//        }

//        //[Test]
//        //    public void FilterPostsByTags_WhenPostsHaveMatchingTags_ReturnsFilteredPosts()
//        //    {

//        //        Guid ownerId = Guid.NewGuid();

//        //        GroupMember member1 = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//        //        _groupMemberRepository.AddGroupMember(member1);

//        //        Guid groupId = Guid.NewGuid();

//        //        Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon",
//        //            "Test Banner", 5, true, true, "Test Code");
//        //        _groupRepository.AddGroup(group);
//        //        // Arrange
//        //        List<GroupPost> posts = new List<GroupPost>
//        //        {
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 1", "Description 1",groupId),
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 2", "Description 2", groupId),
//        //            new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 3", "Description 3",groupId),
//        //        };

//        //        List<string> tags = new List<string> { "tag2", "tag4" };

//        //        // Act
//        //        List<GroupPost> filteredPosts = _feedService.FilterPostsByTags(posts, tags);

//        //        // Assert
//        //        Assert.AreEqual(2, filteredPosts.Count);
//        //        Assert.IsTrue(filteredPosts.Any(post => post.Id == posts[0].Id));
//        //        Assert.IsTrue(filteredPosts.Any(post => post.Id == posts[1].Id));
//        //        Assert.IsFalse(filteredPosts.Any(post => post.Id == posts[2].Id));
//        //    }


//        [Test]
//        public void FilterPostsByTags_ReturnsPinnedPostsFirst_WhenTagsMatch()
//        {
//            // Arrange



//            Guid ownerId = Guid.NewGuid();

//            GroupMember member1 = new GroupMember(ownerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString().Substring(0, 10), "description");

//            _groupMemberRepository.AddGroupMember(member1);

//            Guid groupId = Guid.NewGuid();

//            Group group = new Group(groupId, ownerId, "Test Group", "Test Description", "Test Icon",
//                "Test Banner", 5, true, true, "Test Code");
//            _groupRepository.AddGroup(group);




//            GroupPost post1 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 1", "Description 1", groupId)
//            {
//                Tags = new List<string> { "tag1", "tag2" }
//            };
//            GroupPost post2 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 2", "Description 2", groupId)
//            {
//                Tags = new List<string> { "tag3", "tag4" }
//            };
//            GroupPost post3 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 3", "Description 3", groupId)
//            {
//                Tags = new List<string> { "tag2", "tag5" }
//            };

//            group.AddPost(post1);
//            group.AddPost(post2);
//            group.AddPost(post3);

//            var posts = new List<GroupPost> { post1, post2, post3 };

//            var tags = new List<string> { "tag2", "tag4" };

//            // Act
//            var filteredPosts = _feedService.FilterPostsByTags(posts, tags);

//            Console.WriteLine(posts.Count);
//            Console.WriteLine(filteredPosts.Count);

//            // Assert
//            Assert.AreEqual(posts[0].Id, filteredPosts[0].Id);
//            Assert.AreEqual(posts[0].Description, filteredPosts[0].Description);
//            Assert.AreEqual(posts[0].GroupId, filteredPosts[0].GroupId);
//            Assert.IsFalse(filteredPosts[0].IsPinned);

//            Assert.AreEqual(posts[1].Id, filteredPosts[1].Id);
//            Assert.AreEqual(posts[1].Description, filteredPosts[1].Description);
//            Assert.AreEqual(posts[1].GroupId, filteredPosts[1].GroupId);
//            Assert.IsFalse(filteredPosts[1].IsPinned);

//            Assert.AreEqual(posts[2].Id, filteredPosts[2].Id);
//            Assert.AreEqual(posts[2].Description, filteredPosts[2].Description);
//            Assert.AreEqual(posts[2].GroupId, filteredPosts[2].GroupId);
//            Assert.IsFalse(filteredPosts[2].IsPinned); // Post 2 is not pinned, should be last
//        }


//        [Test]
//        public void FilterPostsByTags_WithNoMatchingTags_ReturnsEmptyList()
//        {
//            // Arrange
//            var posts = new List<GroupPost>
//            {
//                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
//                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
//                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
//            };

//            var tags = new List<string> { "tag5", "tag6" };

//            // Act
//            var result = _feedService.FilterPostsByTags(posts, tags);

//            // Assert
//            Assert.AreEqual(0, result.Count);
//        }

//        //[Test]
//        //public void FilterPostsByTags_WithPinnedPosts_ReturnsPinnedPostsFirst()
//        //{
//        //    // Arrange
//        //    var posts = new List<GroupPost>
//        //    {
//        //        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
//        //        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
//        //        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
//        //    };
//        //    posts[0].IsPinned = true;
//        //    posts[2].IsPinned = true;

//        //    var tags = new List<string> { "tag1", "tag4" };

//        //    // Act
//        //    var result = _feedService.FilterPostsByTags(posts, tags);

//        //    // Assert
//        //    Assert.AreEqual(3, result.Count);
//        //    Assert.IsTrue(result[0].IsPinned);
//        //    Assert.IsFalse(result[1].IsPinned);
//        //    Assert.IsTrue(result[2].IsPinned);
//        //}
//    }


//}