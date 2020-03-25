using Microsoft.AspNetCore.Http;

namespace Transactions.API.Interfaces
{
    public interface ICsvService
    {
        void UploadCsv(IFormFile file);
    }
}
