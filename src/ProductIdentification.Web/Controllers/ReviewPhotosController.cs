using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Web.Controllers
{
    public class ReviewPhotosController : Controller
    {
        private readonly IReviewProductPhotosService _reviewProductPhotosService;

        public ReviewPhotosController(IReviewProductPhotosService reviewProductPhotosService)
        {
            _reviewProductPhotosService = reviewProductPhotosService;
        }

        public async Task<IActionResult> Product(int id)
        {
            var model = await _reviewProductPhotosService.GetPhotosToReview(id);
            return View(model);
        }
        
        public async Task<IActionResult> Accept(string photoName, int productId)
        {
            await _reviewProductPhotosService.AcceptPhoto(productId, photoName);
            return RedirectToAction(nameof(Product), productId);
        }
        
        public async Task<IActionResult> Reject(string photoName, int productId)
        {
            await _reviewProductPhotosService.RejectPhoto(productId, photoName);
            return RedirectToAction(nameof(Product), productId);
        }
    }
}