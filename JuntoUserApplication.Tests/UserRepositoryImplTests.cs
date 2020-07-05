using System;
using System.Threading.Tasks;
using Junto;
using Junto.Users.Domain;
using Junto.Users.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace JuntoUserApplication.Tests
{
    public class UserServiceImplTest
    {
        [Fact]
        public async Task UserServiceSignUp_ValidatesInputAndDoesSignUp()
        {
            var username = "TestUsername";
            var password = "TestPassword";

            Moq.Mock<IUserRepository> userRepositoryMock = new Moq.Mock<IUserRepository>();
            Moq.Mock<IPasswordService> passwordServiceMock = new Moq.Mock<IPasswordService>();
            IOptions<JwtSettings> jwtSettingsMock = Options.Create<JwtSettings>(new JwtSettings { JwtSecret = "anySecret" });

            var userService = new UserServiceImpl(userRepositoryMock.Object, passwordServiceMock.Object, jwtSettingsMock);

            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Signup("", ""));
            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Signup(null, null));
            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Signup("lt6", "password"));
            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Signup("username", "lt6"));

            await userService.Signup(username, password);

            passwordServiceMock
                .Verify(mock =>
                    mock.HashPassword(It.IsAny<string>()), Times.Once());

            userRepositoryMock
                .Verify(mock =>
                    mock.Create(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task UserServiceLogin_ValidatesAndLogsIn()
        {
            var username = "TestUsername";
            var password = "TestPassword";
            var wrongPassword = "WrongPassword";
            var hashedPassword = "HashedPassword";

            Moq.Mock<IUserRepository> userRepositoryMock = new Moq.Mock<IUserRepository>();
            Moq.Mock<IPasswordService> passwordServiceMock = new Moq.Mock<IPasswordService>();
                IOptions<JwtSettings> jwtSettingsMock = Options.Create<JwtSettings>(new JwtSettings { JwtSecret = "fedaf7d8863b48e197b9287d492b708e" });

            userRepositoryMock.Setup(mock => mock.FindByUsername(username)).Returns(Task.Run(() => new User(Guid.NewGuid(), username, hashedPassword)));
            passwordServiceMock.Setup(mock => mock.VerifyPassword(wrongPassword, hashedPassword)).Returns(false);
            passwordServiceMock.Setup(mock => mock.VerifyPassword(password, hashedPassword)).Returns(true);

            var userService = new UserServiceImpl(userRepositoryMock.Object, passwordServiceMock.Object, jwtSettingsMock);

            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Login(null, null));
            await Assert.ThrowsAnyAsync<Exception>(async () => await userService.Login(username, wrongPassword));

            var authenticatedUser = await userService.Login(username, password);

            userRepositoryMock
                .Verify(mock => mock.FindByUsername(username), Times.Exactly(2));
        }
    }
}