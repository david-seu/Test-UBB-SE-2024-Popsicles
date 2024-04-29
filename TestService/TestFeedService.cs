using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Services;
using NUnit.Framework.Legacy;

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

            gp1.AddPostTag("tag1");
            gp1.AddPostTag("tag2");

            gp2.AddPostTag("tag2");
            gp2.AddPostTag("tag3");

            gp3.AddPostTag("tag1");
            gp1.AddPostTag("tag3");

            gp4.AddPostTag("tag4");


            List<string> tags = new List<string> { "tag5", "tag6" };

            // Act
            List<GroupPost> filteredPosts = _feedService.FilterGroupPostsByTags(posts, tags);

            // ClassicAssert
            ClassicAssert.AreEqual(0, filteredPosts.Count);
            ClassicAssert.IsFalse(filteredPosts.Any(post => post.PostId == posts[0].PostId));
            ClassicAssert.IsFalse(filteredPosts.Any(post => post.PostId == posts[2].PostId));
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

            gp1.AddPostTag("tag1");
            gp1.AddPostTag("tag2");

            gp2.AddPostTag("tag2");
            gp2.AddPostTag("tag3");

            gp3.AddPostTag("tag1");
            gp1.AddPostTag("tag3");

            gp4.AddPostTag("tag4");

            List<string> tags = new List<string> { "tag5", "tag6" };




            // Act
            List<GroupPost> filteredPosts = _feedService.FilterGroupPostsByTags(posts, tags);

            // ClassicAssert
            ClassicAssert.IsEmpty(filteredPosts);
        }



    }
}
