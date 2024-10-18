using AutoMapper;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        //[Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            //return View(await _productRepository.GetAllAsync(p => p.IsActive));
            return View(_mapper.Map<List<ProductListViewDto>>(await _productService.GetAllAsync(p => p.IsActive)));
        }

        public async Task<IActionResult> Search(string q)
        {
            // return View(await _productService.GetAllAsync(p => p.Name.Contains(q)));
            return View(_mapper.Map<List<ProductListViewDto>>(await _productService.GetAllAsync(p => p.Name.Contains(q))));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = new ProductDetailViewModel();
            var product = await _productService.GetProductByCategoryAndBrandAsync(id);
            model.Product = product;
            model.RelatedProducts = await _productService.GetAllAsync(p => p.CategoryId == product.CategoryId && p.Id != id);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
