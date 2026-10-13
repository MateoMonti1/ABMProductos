using ABMProductos.Entities;
using ABMProductos.Models.DTOs.Requests;
using ABMProductos.Models.DTOs.Responses;
using ABMProductos.Repositories.Interfaces;
using ABMProductos.Services.Interfaces;

namespace ABMProductos.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductForReadDto> GetAllProducts()
    {
        List<ProductForReadDto> result = new List<ProductForReadDto>();

        foreach (Product product in _repository.GetAllProducts())
        {
            result.Add(MapToDto(product));
        }

        return result;
    }

    public ProductForReadDto? GetProductById(int id)
    {
        Product? product = _repository.GetProductById(id);

        if (product == null)
        {
            return null;
        }

        return MapToDto(product);
    }

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        List<ProductForReadDto> result = new List<ProductForReadDto>();

        foreach (Product product in _repository.SearchProductsByName(name))
        {
            result.Add(MapToDto(product));
        }

        return result;
    }

    public ProductStatsDto GetStats()
    {
        List<Product> products = _repository.GetAllProducts();

        if (products.Count == 0)
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0,
                MostExpensiveName = "No hay productos cargados"
            };
        }

        Product mostExpensive = products.OrderByDescending(p => p.Price).First();

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = Math.Round(products.Average(p => p.Price), 2),
            MostExpensiveName = mostExpensive.Name
        };
    }

    public bool NameIsTaken(string name)
    {
        return _repository.GetAllProducts()
            .Any(p => p.Name.ToLower() == name.ToLower());
    }

    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        Product product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        _repository.AddProduct(product);

        return MapToDto(product);
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        Product? product = _repository.GetProductById(id);

        if (product == null)
        {
            return;
        }

        product.Name = dto.Name;
        product.Price = dto.Price;

        _repository.UpdateProduct(product);
    }

    public void DeleteProduct(int id)
    {
        Product? product = _repository.GetProductById(id);

        if (product == null)
        {
            return;
        }

        _repository.DeleteProduct(product);
    }

    private ProductForReadDto MapToDto(Product product)
    {
        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
