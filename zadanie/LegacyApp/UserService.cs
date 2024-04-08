using System;

namespace LegacyApp
{
    public class UserService
    {
        public ClientRepository ClientRepository { get; set; }
        public UserCreditService UserCreditService { get; set; }
        
        private const int MinCreditLimit = 500;

        public UserService()
        {
            ClientRepository = new ClientRepository();
            UserCreditService = new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var tempUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth
            };
            
            if (!tempUser.ValidateUserData()) return false;
            
            var client = ClientRepository.GetById(clientId);
            tempUser.Client = client;

            if (!SetCreditLimitAndCheck(tempUser, client)) return false;

            UserDataAccess.AddUser(tempUser);
            return true;
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

            return !(user.HasCreditLimit && user.CreditLimit < MinCreditLimit);
        }
    }
}
