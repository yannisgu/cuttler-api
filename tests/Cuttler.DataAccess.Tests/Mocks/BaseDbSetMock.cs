using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuttler.Entities;
using Moq;

namespace Cuttler.DataAccess.Tests.Mocks
{
    public class BaseDbSetMock<T> : Mock<DbSet<T>> where T : class
    {
        public void Init(IQueryable<T> data)
        {
            As<IDbAsyncEnumerable<T>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            As<IQueryable<T>>()
              .Setup(m => m.Provider)
              .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

            As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator()); 
        }
    }
}
