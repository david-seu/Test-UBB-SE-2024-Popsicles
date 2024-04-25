﻿using NUnit.Framework;
using System;
using System.Data.SqlClient;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Repositories;

namespace UnitTests
{
    [TestFixture]
    public class GroupRepositoryTests
    {
        
        private GroupRepository repository;

        [SetUp]
        public void Setup()
        {
            string connection = "Server=MARCHOME\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection conn = new SqlConnection(connection);
            repository = new GroupRepository(conn);
        }

        [TearDown]
        public void TearDown()
        {
           
            repository = null;
        }

        [Test]
        public void AddGroup_Should_Add_New_Group()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid OwnerId = Guid.NewGuid();
            string Name = "Test Group";
            string Description = "Test Description";
            string Icon = "test_icon.png";
            string Banner = "test_banner.png";
            int MaxPostsPerHourPerUser = 10;
            string GroupCode = "TEST";
            bool IsPublic = true;
            bool CanMakePostsByDefault = true;
         
            Group group = new Group(Id, OwnerId, Name, Description, Icon, Banner, MaxPostsPerHourPerUser,IsPublic, CanMakePostsByDefault, GroupCode);
            
            // Act
            repository.AddGroup(group);

            // Assert
            Assert.Equals(repository.GetGroups()[0], group);
        }

        [Test]
        public void GetGroupById_Should_Return_Correct_Group()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid OwnerId = Guid.NewGuid();
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
            repository.AddGroup(group);
            // Act
            Group retrievedGroup = repository.GetGroupById(group.Id);

            // Assert
            Assert.Equals(group, retrievedGroup);
        }

        [Test]
        public void UpdateGroup_Should_Update_Group_Correctly()
        {
         
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid OwnerId = Guid.NewGuid();
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
            repository.AddGroup(group);

            group.Name = "Updated Group Name";
            group.Description = "Updated Description";
            group.Icon = "updated_icon.png";
            group.Banner = "updated_banner.png";
            group.MaxPostsPerHourPerUser = 20;
            group.GroupCode = "UPDATED";
            group.IsPublic = false;
            group.CanMakePostsByDefault = false;
            group.CreatedAt = DateTime.Now.AddDays(-1);

            // Act
            repository.UpdateGroup(group);
            Group updatedGroup = repository.GetGroupById(group.Id);

            // Assert
            Assert.Equals(group, updatedGroup);
        }

        [Test]
        public void RemoveGroupById_Should_Remove_Group_Correctly()
        {
            // Arrange
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid OwnerId = Guid.NewGuid();
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
            repository.AddGroup(group);

            // Act
            repository.RemoveGroupById(group.Id);

            // Assert
            Assert.Equals(repository.GetGroups().Count,0);
        }
    }
}
