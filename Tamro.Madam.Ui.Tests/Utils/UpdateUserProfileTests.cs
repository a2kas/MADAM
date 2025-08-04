using Shouldly;
using Fluxor;
using Moq;
using NUnit.Framework;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Tests.Utils
{
    public class TestUserProfileState : UserProfileState
    {
        public TestUserProfileState(UserProfileStateModel userProfile)
        {
            UserProfile = userProfile;
        }

        public new UserProfileStateModel UserProfile { get; set; }
    }

    [TestFixture]
    public class UpdateUserProfileTests
    {
        private Mock<IState<UserProfileState>> _userProfileStateMock;
        private UserSettingsUtils _userProfileSettingsUtils;
        private UserProfileStateModel _userProfileStateModel;

        [SetUp]
        public void SetUp()
        {
            _userProfileStateMock = new Mock<IState<UserProfileState>>();

            _userProfileStateModel = new UserProfileStateModel();

            var testUserProfileState = new TestUserProfileState(_userProfileStateModel);

            _userProfileStateMock.Setup(x => x.Value).Returns(testUserProfileState);

            _userProfileSettingsUtils = new UserSettingsUtils(_userProfileStateMock.Object);
        }

        [Test]
        public void GetDefaultPageSize_WithNullSettings_ReturnsDefaultPageSize()
        {
            var defaultPageSize = _userProfileSettingsUtils.GetRowsPerPageSetting();
            defaultPageSize.ShouldBe(15);
        }
    }
}
