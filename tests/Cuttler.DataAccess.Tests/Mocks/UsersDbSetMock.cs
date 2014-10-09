using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;
using Newtonsoft.Json;

namespace Cuttler.DataAccess.Tests.Mocks
{
    public  class UsersDbSetMock : BaseDbSetMock<User>
    {
        private readonly IQueryable<User> data;

        public UsersDbSetMock()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var usersJson = assembly.GetManifestResourceStream("Cuttler.DataAccess.Tests.MockData.Users.json");
            var reader = new StreamReader(usersJson);
            data = JsonConvert.DeserializeObject<List<User>>(reader.ReadToEnd()).AsQueryable();

            Init(Data);
        }

        public IQueryable<User> Data
        {
            get { return data; }
        }
    }
}
