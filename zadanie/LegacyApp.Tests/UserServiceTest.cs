using System;
using LegacyApp;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LegacyApp.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void AddUser_ShouldReturnFalse_WhenUserIsUnder21()
        {
            // Arrange
            UserService userService = new UserService();
            
            // Act
            var result = userService.AddUser("John", "Doe", "johndoe@example.com", new DateTime(2005, 1, 1), 1);
            
            // Assert
            Assert.That(result == false);
        }

        [Test]
        public void AddUser_ShouldReturnFalse_WhenEmailIsInvalid()
        {
            // Arrange
            UserService userService = new UserService();
            
            // Act
            var result = userService.AddUser("John", "Doe", "notAnEmail", new DateTime(1980, 1, 1), 1);
            
            // Assert
            Assert.That(result == false);
        }

        [Test]
        public void AddUser_ShouldReturnFalse_For_Client1()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[1];
            
            // Act
            bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            
            // Assert
            Assert.That(result == false);
        }

        [Test]
        public void AddUser_ShouldReturnTrue_For_Client2()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[2];
            
            // Act
            bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            
            // Assert
            Assert.That(result);
        }
        
        [Test]
        public void AddUser_ShouldReturnTrue_For_Client3()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[3];
            
            // Act
            bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            
            // Assert
            Assert.That(result);
        }
        
        [Test]
        public void AddUser_ShouldReturnTrue_For_Client4()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[4];
            
            // Act
            bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            
            // Assert
            Assert.That(result);
        }
        
        [Test]
        public void AddUser_ShouldReturnTrue_For_Client5()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[5];
            
            // Act
            bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            
            // Assert
            Assert.That(result);
        }
        
        [Test]
        public void AddUser_ShouldReturnTrue_For_Client6()
        {
            // Arrange
            UserService userService = new UserService();
            Client client = ClientRepository.Database[6];
            
            // Act
            try
            {
                bool result = userService.AddUser("test", client.Name, client.Email, new DateTime(1975, 1, 1), client.ClientId);
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }            
            // Assert
            Assert.Fail();
        }
    }
}