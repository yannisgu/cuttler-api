using System.IO;
using System.Threading.Tasks;

namespace Cuttler.DataAccess
{
    public interface IFileService
    {
        Task<Stream> GetStreamAsync(string filePath);

    }
}
