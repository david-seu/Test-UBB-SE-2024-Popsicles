//using NUnit.Framework;
//using System;
//using System.Linq;
//using UBB_SE_2024_Popsicles.Models;

//namespace UBB_SE_2024_Popsicles.Tests.Models
//{
//    [TestFixture]
//    public class GroupMemberTests
//    {
//        [Test]
//        public void GetMembership_ExistingMembership_ReturnsMembership()
//        {
//            // Arrange
//            var memberId = Guid.NewGuid();
//            var groupId = Guid.NewGuid();
//            var groupMembership = new GroupMembership(groupId, "Role", DateTime.Now,);
//            var groupMember = new GroupMember(memberId, "username", "password", "email@example.com", "123456789", "description");
//            groupMember.AddGroupMembership(groupMembership);

//            // Act
//            var retrievedMembership = groupMember.GetMembership(groupId);

//            // Assert
//            Assert.Equals(groupMembership, retrievedMembership);
//        }

//        [Test]
//        public void GetMembership_NonExistingMembership_ThrowsException()
//        {
//            // Arrange
//            var memberId = Guid.NewGuid();
//            var groupMember = new GroupMember(memberId, "username", "password", "email@example.com", "123456789", "description");

//            // Act & Assert
//            Assert.Throws<Exception>(() => groupMember.GetMembership(Guid.NewGuid()));
//        }

//        [Test]
//        public void AddGroupMembership_ValidMembership_AddsMembershipToList()
//        {
//            // Arrange
//            var memberId = Guid.NewGuid();
//            var groupId = Guid.NewGuid();
//            var groupMembership = new GroupMembership(groupId, "Role", DateTime.Now);
//            var groupMember = new GroupMember(memberId, "username", "password", "email@example.com", "123456789", "description");

//            // Act
//            groupMember.AddGroupMembership(groupMembership);

//            // Assert
//            Assert.Equals(1, groupMember.Memberships.Count);
//            Assert.That(groupMember.Memberships.Contains(groupMembership));
//        }

//        [Test]
//        public void RemoveGroupMembership_ExistingMembership_RemovesMembershipFromList()
//        {
//            // Arrange
//            var memberId = Guid.NewGuid();
//            var groupId = Guid.NewGuid();
//            var groupMembership = new GroupMembership(groupId, "Role", DateTime.Now);
//            var groupMember = new GroupMember(memberId, "username", "password", "email@example.com", "123456789", "description");
//            groupMember.AddGroupMembership(groupMembership);

//            // Act
//            groupMember.RemoveGroupMembership(groupMembership.Id);

//            // Assert
//            Assert.Equals(0, groupMember.Memberships.Count);
//            Assert.That(groupMember.Memberships.Contains(groupMembership));
//        }

//        [Test]
//        public void RemoveGroupMembership_NonExistingMembership_ThrowsException()
//        {
//            // Arrange
//            var memberId = Guid.NewGuid();
//            var groupMember = new GroupMember(memberId, "username", "password", "email@example.com", "123456789", "description");

//            // Act & Assert
//            Assert.Throws<Exception>(() => groupMember.RemoveGroupMembership(Guid.NewGuid()));
//        }
//    }
//}

