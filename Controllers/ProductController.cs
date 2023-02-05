using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCachingTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemoryCache _memoryCache;
        private const string ProductCacheKey = "PRODUCTS_CACHE";

        public ProductController(IProductService productService, IMemoryCache memoryCache)
        {
            _productService = productService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IList<Product>> GetProducts()
        {
            IList<Product> products = null;

            var productCacheFound = _memoryCache.TryGetValue(ProductCacheKey, out products);

            if (productCacheFound)
            {
                Console.WriteLine($"{DateTime.Now} - Products Found in Cache! Returning Cache Data.");
                return products;
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} - Products Not Found in Cache! Returning Data From IProductService.");

                // Key is not available in the cache, so get data from IProduct Service.
                products = await _productService.GetProducts();

                // Set cache options. We will cache the data for 1 day.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                // Save data in cache.
                _memoryCache.Set(ProductCacheKey, products, cacheEntryOptions);

                return products;
            }
        }

    }
}
