using NUnit.Framework;
using System;
using System.Data.SqlClient;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Repositories;

namespace UnitTests
{
    [TestFixture]
    public class GroupMembershipTests
    {
        private GroupMembershipRepository repository;

        [SetUp]
        public void Setup()
        {
            string connection = "Server=MARCHOME\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection conn = new SqlConnection(connection);

            repository = new GroupMembershipRepository(conn);
        }

        [TearDown]
        public void TearDown()
        {
            repository = null;
        }

        [Test]
        public void AddGroupMemberships_Should_Add_New_Request()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid GroupId = repository.GetGroupMemberships()[0].GroupId;
            Guid GroupMemberId = repository.GetGroupMemberships()[0].GroupMemberId;
            string GroupMemberName = "Test Group Member Name";
            string Role = "Test Role";
            DateTime JoinDate = DateTime.Now;
            bool IsBanned = false;
            bool IsTimedOut = false;
            bool ByPassPostSettings = false;


            GroupMembership groupMembership = new GroupMembership(Id, GroupMemberId, GroupMemberName, GroupId, Role, JoinDate, IsBanned, IsTimedOut, ByPassPostSettings);
            // Act
            repository.AddGroupMembership(groupMembership);

            // Assert
            Assert.That(repository.GetGroupMemberships().Contains(groupMembership));
        }

        [Test]
        public void GetGroupMembershipsById_Should_Return_Correct_Request()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid GroupId = repository.GetGroupMemberships()[0].GroupId;
            Guid GroupMemberId = repository.GetGroupMemberships()[0].GroupMemberId;
            string GroupMemberName = "Test Group Member Name";
            string Role = "Test Role";
            DateTime JoinDate = DateTime.Now;
            bool IsBanned = false;
            bool IsTimedOut = false;
            bool ByPassPostSettings = false;


            GroupMembership groupMembership = new GroupMembership(Id, GroupMemberId, GroupMemberName, GroupId, Role, JoinDate, IsBanned, IsTimedOut, ByPassPostSettings);
            // Act
            repository.AddGroupMembership(groupMembership);
            GroupMembership retrievedGroupMembership = repository.GetGroupMembershipById(groupMembership.Id);

            // Assert
            Assert.That(retrievedGroupMembership == groupMembership);

        }

        [Test]
        public void UpdateGroupMembership_Should_Update_Request_Correctly()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid GroupId = repository.GetGroupMemberships()[0].GroupId;
            Guid GroupMemberId = repository.GetGroupMemberships()[0].GroupMemberId;
            string GroupMemberName = "Test Group Member Name";
            string Role = "Test Role";
            DateTime JoinDate = DateTime.Now;
            bool IsBanned = false;
            bool IsTimedOut = false;
            bool ByPassPostSettings = false;


            GroupMembership groupMembership = new GroupMembership(Id, GroupMemberId, GroupMemberName, GroupId, Role, JoinDate, IsBanned, IsTimedOut, ByPassPostSettings);
            // Act
            repository.AddGroupMembership(groupMembership);

            groupMembership.GroupMemberName = "Updated Name";
            groupMembership.Role = "Updated Role";
            groupMembership.IsBanned = true;
            groupMembership.IsTimedOut = true;
            groupMembership.ByPassPostSettings = true;

            repository.UpdateGroupMembership(groupMembership);
            GroupMembership updatedGroupMembership = repository.GetGroupMembershipById(groupMembership.Id);
            // Assert

            Assert.That(updatedGroupMembership == groupMembership);
        }

        [Test]
        public void RemoveGroupMembershipById_Should_Remove_Request_Correctly()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            Guid GroupId = repository.GetGroupMemberships()[0].GroupId;
            Guid GroupMemberId = repository.GetGroupMemberships()[0].GroupMemberId;
            string GroupMemberName = "Test Group Member Name";
            string Role = "Test Role";
            DateTime JoinDate = DateTime.Now;
            bool IsBanned = false;
            bool IsTimedOut = false;
            bool ByPassPostSettings = false;


            GroupMembership groupMembership = new GroupMembership(Id, GroupMemberId, GroupMemberName, GroupId, Role, JoinDate, IsBanned, IsTimedOut, ByPassPostSettings);

            // Act
            repository.AddGroupMembership(groupMembership);

            repository.RemoveGroupMembershipById(groupMembership.Id);
            // Asser
            Assert.That(repository.GetGroupMemberships().Contains(groupMembership) == false);
        }
    }
}