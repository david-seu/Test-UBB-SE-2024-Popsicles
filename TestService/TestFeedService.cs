using NUnit.Framework.Legacy;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Services;

namespace Test_UBB_SE_2024_Popsicles.TestService
{
    internal class TestFeedService
    {
        private IFeedService feedService;

        [SetUp]
        public void Setup()
        {
            feedService = new FeedService();
        }

        private List<GroupPost> FilterPostsByTagsFactory()
        {
            GroupPost groupPost1 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 1", "Content 1", Guid.NewGuid());
            GroupPost groupPost2 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 2", "Content 2", Guid.NewGuid());
            GroupPost groupPost3 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 3", "Content 3", Guid.NewGuid());
            GroupPost groupPost4 = new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Post 4", "Content 4", Guid.NewGuid());
            List<GroupPost> posts = new List<GroupPost>
                { groupPost1, groupPost2, groupPost3, groupPost4 };

            groupPost1.AddPostTag("tag1");
            groupPost1.AddPostTag("tag2");
            groupPost1.AddPostTag("tag3");

            groupPost2.AddPostTag("tag2");
            groupPost2.AddPostTag("tag3");

            groupPost3.AddPostTag("tag1");

            groupPost4.AddPostTag("tag4");

            return posts;
        }

        [Test]
        public void FilterPostsByTags_NoMatchingTags_ShouldReturnFilteredPosts()
        {
            // Arrange
            List<GroupPost> posts = FilterPostsByTagsFactory();
            List<string> tags = new List<string> { "tag5", "tag6" };

            // Act
            List<GroupPost> filteredPosts = feedService.FilterGroupPostsByTags(posts, tags);

            // Assert
            ClassicAssert.AreEqual(0, filteredPosts.Count);
        }

        [Test]
        public void FilterPostsByTags_ShouldReturnEmptyList_WhenNoPostsMatchTags()
        {
            // Arrange
            List<GroupPost> posts = FilterPostsByTagsFactory();
            List<string> tags = new List<string> { "tag4" };

            // Act
            List<GroupPost> filteredPosts = feedService.FilterGroupPostsByTags(posts, tags);

            // Assert
            ClassicAssert.AreEqual(1, filteredPosts.Count);
        }
    }
}
