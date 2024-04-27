using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using NUnit.Framework;
using UBB_SE_2024_Popsicles.Models;
using UBB_SE_2024_Popsicles.ViewModels;
using Moq;
using NUnit.Framework.Legacy;
using System.ComponentModel;

namespace Test_UBB_SE_2024_Popsicles.GroupPostViewModelTests
{
    public class GroupPostViewModelTests
    {
        private GroupPostViewModel testViewModel;

        [SetUp]
        public void Setup()
        {
            testViewModel = new GroupPostViewModel();
        }

        [Test]
        public void GroupSpecificUserNameTest()
        {
            string userName = "TestUser";
            testViewModel.GroupSpecificUserName = userName;

            ClassicAssert.AreEqual(userName, testViewModel.GroupSpecificUserName);
        }

        [Test]
        public void DateAndTimeOfPostingTest()
        {
            string dateTime = "2024-04-27 14:30:00";
            testViewModel.DateAndTimeOfPosting = dateTime;

            ClassicAssert.AreEqual(dateTime, testViewModel.DateAndTimeOfPosting);
        }

        [Test]
        public void PostDescriptionTest()
        {
            string description = "Test post description";
            testViewModel.PostDescription = description;

            ClassicAssert.AreEqual(description, testViewModel.PostDescription);
        }

        [Test]
        public void LikeCounterInStringFormatTest()
        {
            string likeCount = "10";
            testViewModel.LikeCounterInStringFormat = likeCount;

            ClassicAssert.AreEqual(likeCount, testViewModel.LikeCounterInStringFormat);
        }

        [Test]
        public void CommentsLeftOnThePostTest()
        {
            string comments = "Test comment";
            testViewModel.CommentsLeftOnThePost = comments;

            ClassicAssert.AreEqual(comments, testViewModel.CommentsLeftOnThePost);
        }
    }
}