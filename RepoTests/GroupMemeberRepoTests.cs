using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Repositories;

namespace Test_UBB_SE_2024_Popsicles
{
    [TestFixture]
    public class GroupMemberTests
    {
        
            private SqlConnection connection;
            private GroupMemberRepository repository;

            [SetUp]
            public void Setup()
            {
            
                string connectionString = "Server = MARCHOME\\SQLEXPRESS; Database = TestPopsicles; Integrated Security = true; TrustServerCertificate = true;";
                connection = new SqlConnection(connectionString);
                repository = new GroupMemberRepository(connection);
            }

            [TearDown]
            public void TearDown()
            {
            repository = null;
            connection.Close();
            }

            [Test]
            public void AddGroupMember_ValidGroupMember_AddsMemberToDatabaseAndRepository()
            {
                // Arrange
                var groupMember = new GroupMember(Guid.NewGuid(), "testuser", "testpassword", "testemail@example.com", "123456789", "Test Description");

                // Act
                repository.AddGroupMember(groupMember);

                // Assert
                Assert.That(repository.GroupMembers.Contains(groupMember));
                Assert.That(MemberExistsInDatabase(groupMember.Id));
            }

            [Test]
            public void GetGroupMemberById_ExistingId_ReturnsCorrectMember()
            {
                // Arrange
                var groupMemberId = Guid.NewGuid();
                var groupMember = new GroupMember(groupMemberId, "testuser", "testpassword", "testemail@example.com", "123456789", "Test Description");

                // Act
                var retrievedMember = repository.GetGroupMemberById(groupMemberId);

                // Assert
                Assert.That(groupMember==retrievedMember);
            }

            [Test]
            public void GetGroupMemberById_NonExistingId_ThrowsException()
            {
                // Arrange - Create a non-existing group member ID
                var nonExistingId = Guid.NewGuid();

                // Act & Assert
                Assert.Throws<System.InvalidOperationException>(() => repository.GetGroupMemberById(nonExistingId));
            }

        [Test]
            public void UpdateGroupMember_ValidGroupMember_UpdatesMemberInDatabaseAndRepository()
            {
                // Arrange
                var groupMemberId = Guid.NewGuid();
                var groupMember = new GroupMember(groupMemberId, "testuser", "testpassword", "testemail@example.com", "123456789", "Test Description");
                repository.AddGroupMember(groupMember);

                // Modify member details
                groupMember.Description = "Updated Description";

                // Act
                repository.UpdateGroupMember(groupMember);

                // Assert
                var updatedMember = repository.GetGroupMemberById(groupMemberId);
                Assert.That("Updated Description"==updatedMember.Description);
            }

            [Test]
            public void RemoveGroupMemberById_ExistingId_RemovesMemberFromDatabaseAndRepository()
            {
                // Arrange
                var groupMemberId = Guid.NewGuid();
                var groupMember = new GroupMember(groupMemberId, "testuser", "testpassword", "testemail@example.com", "123456789", "Test Description");
                repository.AddGroupMember(groupMember);

                // Act
                repository.RemoveGroupMemberById(groupMemberId);

                // Assert
                Assert.That(MemberExistsInDatabase(groupMemberId));
                Assert.That(repository.GroupMembers.Contains(groupMember));
            }

            [Test]
            public void RemoveGroupMemberById_NonExistingId_ThrowsException()
            {
                // Arrange - Create a group member with a known ID
                var nonExistingId = Guid.NewGuid();

                // Act & Assert
                Assert.Throws<Exception>(() => repository.RemoveGroupMemberById(nonExistingId));
            }

            private bool MemberExistsInDatabase(Guid memberId)
            {
                // Check if the member exists in the database
                connection.Open();
                string query = "SELECT COUNT(*) FROM GroupMembers WHERE GroupMemberId = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", memberId);
                int count = (int)command.ExecuteScalar();
                connection.Close();
                return count > 0;
            }
        
    }
}


