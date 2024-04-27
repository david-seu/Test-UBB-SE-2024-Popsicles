using NUnit.Framework;
using Moq;
using UBB_SE_2024_Popsicles.ViewModels;
using UBB_SE_2024_Popsicles.Models;
using System.Collections.ObjectModel;
using System;
using NUnit.Framework.Legacy;

namespace Test_UBB_SE_2024_Popsicles.GroupViewModelTests
{
    public class GroupViewModelTests
    {
        private Mock<Poll> mockPoll;
        private Mock<Group> mockGroup;
        private Mock<PollViewModel> mockPollView;
        private Mock<GroupMember> mockGroupMember;
        private Mock<Request> mockRequest;
        private Mock<GroupPost> mockPostInGroup;

        private GroupViewModel testViewModel;

        [SetUp]
        public void Setup()
        {

            mockGroup = new Mock<Group>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Group Name", "test", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>());
            mockPoll = new Mock<Poll>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Test description", It.IsAny<Guid>());
            mockPollView = new Mock<PollViewModel>(mockPoll.Object);
            mockGroupMember = new Mock<GroupMember>(It.IsAny<Guid>(), "testName", "testDescription", "testMail", "testPhone", "test");
            mockRequest = new Mock<Request>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Request Name", It.IsAny<Guid>());
            mockPostInGroup = new Mock<GroupPost>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Test desc", "Test img",It.IsAny<Guid>());

            testViewModel = new GroupViewModel(mockGroup.Object);
            testViewModel.GroupMembers = new ObservableCollection<GroupMember>() { mockGroupMember.Object };
            testViewModel.RequestsToJoinTheGroup = new ObservableCollection<Request>() { mockRequest.Object };
            testViewModel.PostsMadeInTheGroupChat = new ObservableCollection<GroupPost>() { mockPostInGroup.Object };
            testViewModel.CollectionOfPolls = new ObservableCollection<Poll>() { mockPoll.Object };
            testViewModel.CollectionOfViewModelsForEachIndividualPoll = new ObservableCollection<PollViewModel>() { mockPollView.Object };
            testViewModel.AllowanceOfPostageOnTheGroupChat = "true";
            


        }

        [Test]
        public void GroupMembersGetterTest()
        {
            ObservableCollection<GroupMember> actualMembers = new ObservableCollection<GroupMember>()
                { mockGroupMember.Object };
            ClassicAssert.AreEqual(actualMembers, testViewModel.GroupMembers);
        }

        [Test]
        public void RequestsGetterTest()
        {
            ObservableCollection<Request> actualRequests = new ObservableCollection<Request>()
            {
                mockRequest.Object
            };
            ClassicAssert.AreEqual(testViewModel.RequestsToJoinTheGroup, actualRequests);
        }

        [Test]
        public void PostsGetterTest()
        {
            ObservableCollection<GroupPost> actualPosts = new ObservableCollection<GroupPost>()
            {
                mockPostInGroup.Object
            };
            ClassicAssert.AreEqual(testViewModel.PostsMadeInTheGroupChat, actualPosts);
        }

        [Test]
        public void CollectionOfPollViewModelsGetter()
        {
            ObservableCollection<PollViewModel> actualPollViewModels = new ObservableCollection<PollViewModel>() { mockPollView.Object };
            ClassicAssert.AreEqual(testViewModel.CollectionOfViewModelsForEachIndividualPoll, actualPollViewModels);
        }
       


    }
}