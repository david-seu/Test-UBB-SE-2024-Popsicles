using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using NUnit.Framework;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.ViewModels;
using Moq;
using NUnit.Framework.Legacy;

namespace Test_UBB_SE_2024_Popsicles.PollViewModelTests
{
    public class PollViewModelTests
    {
        public Mock<GroupPoll> MockPoll;
        public List<string> MockList;
        public PollViewModel TestViewModel;

        [SetUp]
        public void Setup()
        {
            MockList = new List<string>() { "Option 1", "Option 2", "Option 3", "Option 4" };
            MockPoll = new Mock<GroupPoll>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Test description", It.IsAny<Guid>());

            MockPoll.Object.GroupPollOptions = MockList;

            TestViewModel = new PollViewModel(MockPoll.Object);
        }

        //----------------------------------------Get/Set Functions--------------------------------
        [Test]
        public void PollThatIsEncapsulatedByThisInstanceOnViewModelTest()
        {
            GroupPoll actualGroupPoll = TestViewModel.GroupPollThatIsEncapsulatedByThisInstanceOnViewModel;
            ClassicAssert.AreEqual(MockPoll.Object, actualGroupPoll);
        }

        [Test]
        public void DescriptionOfThePollTest()
        {
            string newDescription = "New Test description";
            TestViewModel.DescriptionOfThePoll = newDescription;

            ClassicAssert.AreEqual(newDescription, TestViewModel.DescriptionOfThePoll);
            ClassicAssert.AreEqual(newDescription, MockPoll.Object.GroupPollDescription);
        }

        [Test]
        public void FirstPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 1";
            TestViewModel.FirstPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, TestViewModel.FirstPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, MockPoll.Object.GroupPollOptions[0]);
        }

        [Test]
        public void SecondPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 2";
            TestViewModel.SecondPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, TestViewModel.SecondPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, MockPoll.Object.GroupPollOptions[1]);
        }

        [Test]
        public void ThirdPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 3";
            TestViewModel.ThirdPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, TestViewModel.ThirdPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, MockPoll.Object.GroupPollOptions[2]);
        }

        [Test]
        public void FourthPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 4";
            TestViewModel.FourthPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, TestViewModel.FourthPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, MockPoll.Object.GroupPollOptions[3]);
        }
    }
}
