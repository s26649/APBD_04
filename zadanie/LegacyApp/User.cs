using System;

namespace LegacyApp
{
    public class User
    {
        public object Client { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public bool HasCreditLimit { get; internal set; }
        public int CreditLimit { get; internal set; }
        
        // oddzielenie obliczania wieku do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        public int CalculateAge()
        {
            var now = DateTime.Now;
            int age = now.Year - DateOfBirth.Year;
            if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
                age--;
            return age;
        }
        
        // oddzielenie logiki walidacji do osobnej metody, poprawka czytelności (Single Responsibility Principle)
        public bool ValidateUserData()
        {
            return !string.IsNullOrEmpty(FirstName) 
                   && !string.IsNullOrEmpty(LastName) 
                   && EmailAddress.Contains("@") 
                   && CalculateAge() >= 21;
        }
    }
}