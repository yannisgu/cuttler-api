using System;
using Cuttler.Entities;

namespace Cuttler.Api.ViewModels
{
    public class BaseUserViewModel
    {
        public BaseUserViewModel(User user)
        {
            UserName = user.UserName;
            Street = user.Street;
            Zip = user.Zip;
            Location = user.Location;
            Country = user.Country;

        }

        public BaseUserViewModel()
        {
        }

        public virtual string UserName { get; set; }

        public virtual string Street { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Location { get; set; }

        public virtual string Country { get; set; }

        public virtual User AsUser()
        {
            return UpdateUser(new User());
        }

        public virtual User UpdateUser(User user)
        {

            user.Street = Street;
            user.Zip = Zip;
            user.Location = Location;
            user.Country = Country;
            user.UserName = UserName;
            return user;
        }
    }

    public class UserViewModel : BaseUserViewModel
    {
        public UserViewModel()
        {
        }

        public UserViewModel(User user) : base(user)
        {

        }

        public Guid Id { get; set; }

    }

    public class NewUserViewModel : BaseUserViewModel
    {

        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
    }
}