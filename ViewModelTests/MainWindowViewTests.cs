using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using NUnit.Framework;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.ViewModels;
using Moq;
using NUnit.Framework.Legacy;

namespace Test_UBB_SE_2024_Popsicles.MainWindowViewTests
{
    public class MainWindowViewModelTests
    {
        private Mock<Group> mockGroup1, mockGroup2, mockGroup3, mockGroup4, mockGroup5;
        private Mock<GroupMember> mockUser;
        private MainWindowViewModel testView=new();
        private Mock<GroupMember> testMember;
        private ObservableCollection<Group> mockGroups = new ObservableCollection<Group>();

        [SetUp]
        public void Setup()
        {
            mockGroup1 = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", "test", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockGroup2 = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockGroup3 = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockGroup4 = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockGroup5 = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockUser = new Mock<GroupMember>(It.IsAny<Guid>(),"testName","testDescription","testMail","testPhone","test");
            mockGroups.Add(mockGroup1.Object);
            mockGroups.Add(mockGroup2.Object);
            mockGroups.Add(mockGroup3.Object);
            mockGroups.Add(mockGroup4.Object);
            mockGroups.Add(mockGroup5.Object);
            testView.CollectionOfActiveGroups=mockGroups;
            testView.CurrentlySelectedGroup = mockGroups[0];
            testView.CurrentActiveUser = mockUser.Object;

        }

        [Test]
        public void CollectionOfActiveGroups_Getter_ShouldReturnTheCollection()
        {
             ObservableCollection<Group> actualGroups = testView.CollectionOfActiveGroups;

            // Assert
            ClassicAssert.AreSame(mockGroups, actualGroups); // Assert they are the same object reference
            ClassicAssert.AreEqual(5, actualGroups.Count); // Assert the number of groups
            ClassicAssert.AreEqual("Group Name", testView.CollectionOfActiveGroups[0].Name);
        }


        [Test]
        public void CollectionOfActiveGroups_Setter_ShouldUpdateTheCollection()
        {
            // Arrange
          
            Mock<Group> newMockGroup = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "New group", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            

            // Act
            testView.CollectionOfActiveGroups = new ObservableCollection<Group>() { newMockGroup.Object };

            // Assert
            ClassicAssert.AreEqual(1, testView.CollectionOfActiveGroups.Count);
            ClassicAssert.AreEqual("New group", testView.CollectionOfActiveGroups[0].Name);
        }
        [Test]
        public void CurrentActiveUser_Getter_ShouldReturnUser()
        {
            GroupMember actualUser = testView.CurrentActiveUser;

            ClassicAssert.AreEqual(actualUser,mockUser.Object);
        }

        [Test]
        public void CurrentActiveUser_Setter_ShouldChangeUser()
        {
            Mock<GroupMember> newUser = new Mock<GroupMember>(It.IsAny<Guid>(), "test", "test", "test", "test", "test");

            testView.CurrentActiveUser= newUser.Object;

            ClassicAssert.AreSame(testView.CurrentActiveUser,newUser.Object);
        }

        [Test]
        public void CurrentlySelectedGroupGetterTest()
        {
            Group actualGroup = testView.CurrentlySelectedGroup;
            ClassicAssert.AreSame(actualGroup, mockGroups[0]);
            ClassicAssert.AreEqual("Group Name", testView.CurrentlySelectedGroup.Name);
            ClassicAssert.AreEqual("test", testView.CurrentlySelectedGroup.Description);
        }

        [Test]
        public void CurrentlySelectedGroupSetterTest()
        {
            Mock<Group> newMockGroup = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "New group", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());

            testView.CurrentlySelectedGroup= newMockGroup.Object;

            ClassicAssert.AreEqual("New group", testView.CurrentlySelectedGroup.Name);
        }
    }


}
