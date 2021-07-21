using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopOnlineAPI.Extensions;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Services;
using ShopOnlineAPI.Ultilities;
using ShopOnlineAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public ProductsController(IProductService productService, IMapper mapper, IConfiguration configuration)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAll();

            List<ProductViewModel> productViewModelListMapped = mapper.Map<List<ProductViewModel>>(products);

            return Ok(productViewModelListMapped);
        }

        [HttpGet]
        [Route("ProductBySearching")]
        public async Task<IActionResult> ProductBySearching(string keyWords)
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
        [Route("ProductImageByIdAndDimension")]
        public async Task<IActionResult> ProductImageByIdAndDimension([FromQuery]int id, [FromQuery] int width, [FromQuery] int height)
        {
            var product = await productService.GetById(id);

            ProductViewModel productViewModelMapped = mapper.Map<ProductViewModel>(product);

            using (var image = Image.FromStream(productViewModelMapped.Image.OpenReadStream(), true, true))
            {
                var newImage = image.Resize(width, height);

                return File(newImage.ToByteArray(ImageFormat.Bmp), productViewModelMapped.Image.ContentType);
            }
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

            //Check whether image size is too large or not (must be less than 100000 bytes)
            if(productViewModel.Image.Length > long.Parse(configuration["MaxImageSize"]))
            {
                return BadRequest(new
                {
                    ErrorMessage = "Image Size is too large (must be less than 100000 bytes)!"
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
