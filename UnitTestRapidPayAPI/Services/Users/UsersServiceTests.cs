using AutoMapper;
using Moq;
using RapidPayAPI.Repositories.Users;
using RapidPayAPI.Services.Users;
using RapidPayAPI.Services.Users.Models;

namespace RapidPayAPI.Tests.Services.Users
{
    [TestFixture]
    public class UsersServiceTests
    {
        private Mock<IUsersRepository> _mockUsersRepository;
        private Mock<IMapper> _mockMapper;
        private UsersService _usersService;

        [SetUp]
        public void Setup()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockMapper = new Mock<IMapper>();
            _usersService = new UsersService(_mockUsersRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetUserAsync_UserExists_ReturnsMappedUserResult()
        {
            // Arrange
            var userName = "testUser";
            var user = new User { UserName = userName, Id = 1 };
            var userResult = new UserResult { UserName = userName, Id = 1 };

            _mockUsersRepository.Setup(x => x.GetUserAsync(userName)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserResult>(user)).Returns(userResult);

            // Act
            var result = await _usersService.GetUserAsync(userName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userName, result.UserName);
            _mockUsersRepository.Verify(x => x.GetUserAsync(userName), Times.Once);
            _mockMapper.Verify(m => m.Map<UserResult>(user), Times.Once);
        }
    }
}
