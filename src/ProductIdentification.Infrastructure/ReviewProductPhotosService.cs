using System.Threading.Tasks;
using ProductIdentification.Common;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class ReviewProductPhotosService : IReviewProductPhotosService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IQueueService _queueService;

        public ReviewProductPhotosService(IProductRepository productRepository,
                                          IFileRepository fileRepository,
                                          IQueueService queueService)
        {
            _productRepository = productRepository;
            _fileRepository = fileRepository;
            _queueService = queueService;
        }

        public async Task<ReviewProductPhotosDto> GetPhotosToReview(int productId)
        {
            var product = await _productRepository.Get(productId);

            return new ReviewProductPhotosDto
            {
                Photos = await _fileRepository.FilesList(product.StoragePathToVerify()),
                Product = product
            };
        }

        public async Task AcceptPhoto(int productId, string photoName)
        {
            var product = await _productRepository.Get(productId);

            await _fileRepository.CopyFile(product.StoragePathToVerify(), product.StoragePathVerified(), photoName);

            await _queueService.SendMessageAsync(QueueNames.UpdateProduct, new UpdateProductMessage(product.Id));
        }

        public async Task RejectPhoto(int productId, string photoName)
        {
            var product = await _productRepository.Get(productId);

            await _fileRepository.CopyFile(product.StoragePathToVerify(), "rejected", photoName);
        }
    }
}