namespace MemoryCachingTutorial
{
    public interface IProductService
    {
        Task<IList<Product>> GetProducts();
    }
}
