using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Services;

namespace Test_UBB_SE_2024_Popsicles.TestService
{
    [TestFixture]
    internal class TestFeedService
    {   
        private IFeedService _feedService;

        [SetUp]
        public void Setup()
        {
            _feedService = new FeedService();
        }

        [Test]
        public void FilterPostsByTags_ShouldReturnFilteredPosts()
        {
            // Arrange
            GroupPost gp1 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 1", "Content 1", Guid.NewGuid());
            GroupPost gp2 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 2", "Content 2", Guid.NewGuid());
            GroupPost gp3 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 3", "Content 3", Guid.NewGuid());
            GroupPost gp4 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 4", "Content 4", Guid.NewGuid());
            List<GroupPost> posts = new List<GroupPost>
                {gp1,gp2,gp3,gp4};

            gp1.AddTag("tag1");
            gp1.AddTag("tag2");

            gp2.AddTag("tag2");
            gp2.AddTag("tag3");

            gp3.AddTag("tag1");
            gp1.AddTag("tag3");

            gp4.AddTag("tag4");


            List<string> tags = new List<string> { "tag5", "tag6" };

            // Act
            List<GroupPost> filteredPosts = _feedService.FilterPostsByTags(posts, tags);

            // Assert
            Assert.AreEqual(0, filteredPosts.Count);
            Assert.IsFalse(filteredPosts.Any(post => post.Id == posts[0].Id));
            Assert.IsFalse(filteredPosts.Any(post => post.Id == posts[2].Id));
        }

        [Test]
        public void FilterPostsByTags_ShouldReturnEmptyList_WhenNoPostsMatchTags()
        {
            // Arrange

            GroupPost gp1 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 1", "Content 1", Guid.NewGuid());
            GroupPost gp2 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 2", "Content 2", Guid.NewGuid());
            GroupPost gp3 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 3", "Content 3", Guid.NewGuid());
            GroupPost gp4 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 4", "Content 4", Guid.NewGuid());
            List<GroupPost> posts = new List<GroupPost>
            {gp1,gp2,gp3,gp4};

            gp1.AddTag("tag1");
            gp1.AddTag("tag2");

            gp2.AddTag("tag2");
            gp2.AddTag("tag3");

            gp3.AddTag("tag1");
            gp1.AddTag("tag3");

            gp4.AddTag("tag4");

            List<string> tags = new List<string> { "tag5", "tag6" };




            // Act
            List<GroupPost> filteredPosts = _feedService.FilterPostsByTags(posts, tags);

            // Assert
            Assert.IsEmpty(filteredPosts);
        }



    }
}
