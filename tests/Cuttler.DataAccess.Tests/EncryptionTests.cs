using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Cuttler.DataAccess.Tests
{
    [TestFixture]
    public class EncryptionTests
    {
        [Test]
        public void PasswordMatch_should_return_true_when_password_match()
        {
            PasswordService service = new PasswordService();
            string password = "?^¨)aAd@b";
            Assert.IsTrue(service.ValidatePassword(password, service.CreateHash(password)));
        }


        [Test]
        public void PasswordMatch_should_return_false_when_password_do_not_match()
        {
            PasswordService service = new PasswordService();
            string password = "?^¨)aAd@b";
            Assert.IsFalse(service.ValidatePassword(password + "b", service.CreateHash(password)));
        }
    }
}
