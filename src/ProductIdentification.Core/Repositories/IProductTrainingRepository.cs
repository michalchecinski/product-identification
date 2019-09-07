using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Core.Repositories
{
    public interface IProductTrainingRepository
    {
        ProductTrainingModel Get(int productId);
        Task<IEnumerable<ProductTrainingModel>> GetAllAsync();
        IEnumerable<ProductTrainingModel> GetAllToTrain();
        IEnumerable<ProductTrainingModel> GetAllTraining();
        bool Any();
        void Update(ProductTrainingModel productTrainingModel);
        void Add(ProductTrainingModel productTrainingModel);
        void Delete(ProductTrainingModel productTrainingModel);
    }
}