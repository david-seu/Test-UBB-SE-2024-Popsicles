using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            private static readonly Random random = new Random();
            private const string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; 

          [SetUp]
            public void Setup()
            {

            string connectionString = "Server=DESKTOP-55UJ616\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
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
                var groupMember = new GroupMember(Guid.NewGuid(), GenerateRandomString(10), "testpassword", GenerateRandomString(10), GenerateRandomString(10), "Test Description");

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
                var groupMember = new GroupMember(groupMemberId, GenerateRandomString(10), "testpassword", GenerateRandomString(10), GenerateRandomString(10), "Test Description");
                repository.AddGroupMember(groupMember);
                // Act
                var retrievedMember = repository.GetGroupMemberById(groupMemberId);

                // Assert
                Assert.That(groupMember.Id==retrievedMember.Id);
                Assert.That(groupMember.Description == retrievedMember.Description);
                Assert.That(groupMember.Email == retrievedMember.Email);
                Assert.That(groupMember.Password == retrievedMember.Password);
                Assert.That(groupMember.Phone == retrievedMember.Phone);
                Assert.That(groupMember.Username == retrievedMember.Username);
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
                var groupMember = new GroupMember(groupMemberId, GenerateRandomString(10), "testpassword", GenerateRandomString(10), GenerateRandomString(10), "Test Description");
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
                var groupMember = new GroupMember(groupMemberId, GenerateRandomString(10), "testpassword", GenerateRandomString(10), GenerateRandomString(10), "Test Description");
                repository.AddGroupMember(groupMember);

                // Act
                repository.RemoveGroupMemberById(groupMemberId);

                // Assert
                Assert.That(MemberExistsInDatabase(groupMemberId)==false);
                Assert.That(repository.GroupMembers.Contains(groupMember)==false);
            }

            [Test]
            public void RemoveGroupMemberById_NonExistingId_ThrowsException()
            {
                // Arrange - Create a non-existing group member ID
                var nonExistingId = Guid.NewGuid();

                // Act & Assert
                Assert.Throws<System.InvalidOperationException>(() => repository.RemoveGroupMemberById(nonExistingId));
            }
           

            public static string GenerateRandomString(int length)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    int index = random.Next(CharSet.Length);
                    builder.Append(CharSet[index]);
                }
                return builder.ToString();
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


