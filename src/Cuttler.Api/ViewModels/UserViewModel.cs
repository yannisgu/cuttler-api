using Cuttler.Entities;

namespace Cuttler.Api.ViewModels
{
    public class UserViewModel 
    {
        public virtual string UserName { get; set; }

        public virtual string Street { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Location { get; set; }

        public virtual string Country { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public User AsUser()
        {
            return new User()
            {
                Street = Street,
                Zip = Zip,
                Location = Location,
                Country = Country,
                UserName = UserName
            };
        }
    }
}