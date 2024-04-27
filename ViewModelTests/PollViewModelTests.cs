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
        public Mock<Poll> mockPoll;
        public List<string> mockList;
        public PollViewModel testViewModel;

        [SetUp]
        public void Setup()
        {
            mockList = new List<string>() {  "Option 1", "Option 2", "Option 3", "Option 4" };
            mockPoll = new Mock<Poll>(It.IsAny<Guid>(), It.IsAny<Guid>(), "Test description", It.IsAny<Guid>());

            mockPoll.Object.Options = mockList;

            testViewModel = new PollViewModel(mockPoll.Object);
            
        }

        //----------------------------------------Get/Set Functions--------------------------------

        [Test]
        public void PollThatIsEncapsulatedByThisInstanceOnViewModelTest()
        {
            Poll actualPoll = testViewModel.PollThatIsEncapsulatedByThisInstanceOnViewModel;
            ClassicAssert.AreSame(mockPoll.Object, actualPoll);
        }

        [Test]
        public void DescriptionOfThePollTest()
        {
            string newDescription = "New Test description";
            testViewModel.DescriptionOfThePoll = newDescription;

            ClassicAssert.AreEqual(newDescription, testViewModel.DescriptionOfThePoll);
            ClassicAssert.AreEqual(newDescription, mockPoll.Object.Description);
        }


        [Test]
        public void FirstPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 1";
            testViewModel.FirstPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, testViewModel.FirstPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, mockPoll.Object.Options[0]);
        }

        [Test]
        public void SecondPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 2";
            testViewModel.SecondPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, testViewModel.SecondPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, mockPoll.Object.Options[1]);
        }

        [Test]
        public void ThirdPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 3";
            testViewModel.ThirdPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, testViewModel.ThirdPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, mockPoll.Object.Options[2]);
        }

        [Test]
        public void FourthPossibleOptionOfThePollTest()
        {
            string newOption = "New Option 4";
            testViewModel.FourthPossibleOptionOfThePoll = newOption;

            ClassicAssert.AreEqual(newOption, testViewModel.FourthPossibleOptionOfThePoll);
            ClassicAssert.AreEqual(newOption, mockPoll.Object.Options[3]);
        }
    }
}
