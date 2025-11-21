namespace Entities;

public class User
{
    public string UserName { get; private set; }
    public string Password {get; private set;}
    public string Email {get; private set;}
    public string LastName {get; private set;}
    public string FirstName {get; private set;}
    
    public int Id { get; set; } = 0;

    // Builder inner class
    public class Builder
    {
        private string _username = "Default Name";
        private int _id = 0;
        private string _email = "Default email";
        private string _lastName = "Default lastname";
        private string _firstName = "Default firstname";
        private string _password = "Default password";
        

        public Builder SetUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Name cannot be null or empty");
            _username = username;
            return this;
        }

        public Builder SetId(int id)
        {
            if (id < 0) 
                throw new ArgumentException("Id cannot be negative");
            _id = id;
            return this;
        }

        public Builder SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty");
            _email = email;
            return this;
        }

        public Builder SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Last name cannot be null or empty");
            _lastName = lastName;
            return this;
        }

        public Builder SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("First name cannot be null or empty");
            _firstName = firstName;
            return this;
        }

        public Builder SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty");
            _password = password;
            return this;
        }

        public User Build()
        {
            return new User
            {
                UserName = _username,
                Password = _password,
                Email = _email,
                FirstName = _firstName,
                LastName = _lastName
            };
        }
    }
}