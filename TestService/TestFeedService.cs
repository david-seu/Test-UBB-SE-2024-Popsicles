using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Services;
using UBB_SE_2024_Popsicles.Repositories;


namespace Test_UBB_SE_2024_Popsicles.TestService
{
   
    internal class TestFeedService
    {
        private Mock<IFeedService> _feedServiceMock;

        [SetUp]
        public void Setup()
        {
            _feedServiceMock = new Mock<IFeedService>();
        }

        [Test]
        public void FilterPostsByTags_WithMatchingTags_ReturnsFilteredPosts()
        {
            // Arrange
            var posts = new List<GroupPost>
            {
                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
                new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
            };

            var tags = new List<string> { "tag1", "tag4" };

            _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(posts.Take(2).ToList());

            // Act
            var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

            // Assert
            Assert.AreEqual(2, result.Count);
        }
        [Test]
        public void FilterPostsByTags_WithNoMatchingTags_ReturnsEmptyList()
        {
            // Arrange
            var posts = new List<GroupPost>
    {
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
    };

            var tags = new List<string> { "tag5", "tag6" };

            _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(new List<GroupPost>());

            // Act
            var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void FilterPostsByTags_WithPinnedPosts_ReturnsPinnedPostsFirst()
        {
            // Arrange
            var posts = new List<GroupPost>
    {
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 1", "Image 1", Guid.NewGuid()),
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 2", "Image 2", Guid.NewGuid()),
        new GroupPost(Guid.NewGuid(), Guid.NewGuid(), "Description 3", "Image 3", Guid.NewGuid())
    };
            posts[0].IsPinned = true;
            posts[2].IsPinned = true;

            var tags = new List<string> { "tag1", "tag4" };

            _feedServiceMock.Setup(service => service.FilterPostsByTags(posts, tags)).Returns(posts);

            // Act
            var result = _feedServiceMock.Object.FilterPostsByTags(posts, tags);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result[0].IsPinned);
            Assert.IsFalse(result[1].IsPinned);
            Assert.IsTrue(result[2].IsPinned);
        }

    }
}