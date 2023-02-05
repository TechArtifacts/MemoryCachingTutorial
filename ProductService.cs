using Newtonsoft.Json;

namespace MemoryCachingTutorial
{
    public class ProductResponse
    {
        public List<Product> Products { get; set; }
    }
    public class ProductService : IProductService
    {
        public string ProductApiURL = "https://dummyjson.com/products";

        public async Task<IList<Product>> GetProducts()
        {
            using (var client = new HttpClient())
            {
                // Send a GET request to the API
                HttpResponseMessage response = await client.GetAsync(ProductApiURL);

                // Check the status code of the response
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var data = JsonConvert.DeserializeObject<ProductResponse>(responseString);
                        return data.Products;
                    }
                }
            }
            return null;
        }
    }
}
