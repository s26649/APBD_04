using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUserData(firstName, lastName, email, dateOfBirth)) return false;
            
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            if (!SetCreditLimitAndCheck(user, client)) return false;

            UserDataAccess.AddUser(user);
            return true;
        }
        
        // oddzielenie tworzenia usera do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }
        
        // oddzielenie logiki walidacji do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        private bool ValidateUserData(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return !string.IsNullOrEmpty(firstName) 
                   && !string.IsNullOrEmpty(lastName) 
                   && email.Contains("@") 
                   && CalculateAge(dateOfBirth) >= 21;
        }
        
        // oddzielenie obliczania wieku do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) 
                age--;
            return age;
        }
        
        // oddzielenie logiki ustawiania limitu kredytowego i sprawdzania do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        private bool SetCreditLimitAndCheck(User user, Client client)
        {
            using (var userCreditService = new UserCreditService())
            {
                switch (client.Type)
                {
                    case "VeryImportantClient":
                        user.HasCreditLimit = false;
                        break;
                    case "ImportantClient":
                        var importantClientCreditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
                        user.CreditLimit = importantClientCreditLimit;
                        user.HasCreditLimit = true;
                        break;
                    default:
                        var normalClientCreditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        user.CreditLimit = normalClientCreditLimit;
                        user.HasCreditLimit = true;
                        break;
                }
            }

            return !(user.HasCreditLimit && user.CreditLimit < 500);
        }
    }
}
