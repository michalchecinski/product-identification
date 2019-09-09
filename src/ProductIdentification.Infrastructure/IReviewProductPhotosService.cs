using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface IReviewProductPhotosService
    {
        Task<ReviewProductPhotosDto> GetPhotosToReview(int productId);
        Task AcceptPhoto(int productId, string photoName);
        Task RejectPhoto(int productId, string photoName);
    }
}