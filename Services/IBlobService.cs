using NewMark.API.Models;

namespace NewMark.API.Service
{
    public interface IBlobService
    {
        /// <summary>
        /// Interface method to fetch and deserialize JSON data from Azure Blob Storage.
        /// </summary>
        /// <returns>Deserialized C# object of type BlobData.</returns>
        Task<List<Property>> GetBlobDataAsync();
    }
}
