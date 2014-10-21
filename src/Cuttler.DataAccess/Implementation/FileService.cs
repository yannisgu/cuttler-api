using System;
using System.IO;
using System.Threading.Tasks;

namespace Cuttler.DataAccess.Implementation
{
    public class FileService : IFileService
    {
        public Task<Stream> GetStreamAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}