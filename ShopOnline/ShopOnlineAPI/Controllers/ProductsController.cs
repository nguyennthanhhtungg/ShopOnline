using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Services;
using ShopOnlineAPI.Ultilities;
using ShopOnlineAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAll();

            List<ProductViewModel> productViewModelListMapped = mapper.Map<List<ProductViewModel>>(products);

            return Ok(productViewModelListMapped);
        }

        [HttpGet]
        [Route("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string keyWords)
        {
            var productList = await productService.GetProductListByKeyWords(keyWords);

            List<ProductViewModel> productViewModelListMapped = mapper.Map<List<ProductViewModel>>(productList);

            return Ok(productViewModelListMapped);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetById(id);

            ProductViewModel productViewModelMapped = mapper.Map<ProductViewModel>(product);

            return Ok(productViewModelMapped);
        }

        [HttpGet]
        [Route("GetProductImageById")]
        public async Task<IActionResult> GetProductImageById([FromQuery]int id)
        {
            var product = await productService.GetById(id);

            ProductViewModel productViewModelMapped = mapper.Map<ProductViewModel>(product);

            return File(productViewModelMapped.Image.OpenReadStream(), productViewModelMapped.Image.ContentType);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Product Info is invalid!"
                });
            }

            try
            {
                Product productMapped = mapper.Map<Product>(productViewModel);

                var product = await productService.Add(productMapped);

                ProductViewModel productViewModelMapped = mapper.Map<ProductViewModel>(product);

                return Ok(productViewModelMapped);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return BadRequest(new
                {
                    ErrorMessage = "Product Info is invalid!"
                });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    ErrorMessage = "Product Info is invalid!"
                });
            }

            try
            {
                Product productMapped = mapper.Map<Product>(productViewModel);

                var product = await productService.Update(productMapped);

                ProductViewModel productViewModelMapped = mapper.Map<ProductViewModel>(product);

                return Ok(productViewModelMapped);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return BadRequest(new
                {
                    ErrorMessage = "Product Info is invalid!"
                });
            }
        }
    }
}
