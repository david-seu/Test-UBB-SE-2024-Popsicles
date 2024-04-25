using NUnit.Framework;
using System;
using System.Data.SqlClient;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.Repositories;

namespace UnitTests
{
    [TestFixture]
    public class RequestsRepositoryTests
    {
        private RequestsRepository repository;

        [SetUp]
        public void Setup()
        {
            string connection = "Server=MARCHOME\\SQLEXPRESS;Database=TestPopsicles;Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection conn = new SqlConnection(connection);

            repository = new RequestsRepository(conn);
        }

        [TearDown]
        public void TearDown()
        {
            repository = null;
        }

        [Test]
        public void AddRequest_Should_Add_New_Request()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = repository.GetAllRequests()[0].GroupMemberId;
            Guid groupId = repository.GetAllRequests()[0].GroupId;
            string groupMemberName = "Test Request";

            Request request = new Request(requestId, groupMemberId, groupMemberName, groupId);

            // Act
            repository.AddRequest(request);

            // Assert
            Assert.That(repository.GetAllRequests().Contains(request));
        }

        [Test]
        public void GetRequestById_Should_Return_Correct_Request()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = repository.GetAllRequests()[0].GroupMemberId;
            Guid groupId = repository.GetAllRequests()[0].GroupId;
            string groupMemberName = "Test Request";

            Request request = new Request(requestId, groupMemberId, groupMemberName, groupId);
            repository.AddRequest(request);

            // Act
            Request retrievedRequest = repository.GetRequestById(requestId);

            // Assert
            Assert.That(retrievedRequest == request);

        }

        [Test]
        public void UpdateRequest_Should_Update_Request_Correctly()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = repository.GetAllRequests()[0].GroupMemberId;
            Guid groupId = repository.GetAllRequests()[0].GroupId;
            string groupMemberName = "Test Request";

            Request request = new Request(requestId, groupMemberId, groupMemberName, groupId);
            repository.AddRequest(request);


            // Act
            request.GroupMemberName = "Updated Group Member Name";

            repository.UpdateRequest(request);
            Request updatedRequest = repository.GetRequestById(request.Id);

            // Assert
            Assert.That(updatedRequest == request);

        }

        [Test]
        public void RemoveRequestById_Should_Remove_Request_Correctly()
        {
            // Arrange
            Guid requestId = Guid.NewGuid();
            Guid groupMemberId = repository.GetAllRequests()[0].GroupMemberId;
            Guid groupId = repository.GetAllRequests()[0].GroupId;
            string groupMemberName = "Test Request";

            Request request = new Request(requestId, groupMemberId, groupMemberName, groupId);
            repository.AddRequest(request);

            // Act
            repository.RemoveRequestById(request.Id);

            // Assert
            Assert.That(repository.GetAllRequests().Contains(request) == false);
        }
    }
}