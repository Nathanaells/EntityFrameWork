using AutoMapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductCreateDTO>();

        CreateMap<ProductResponseDTO, Product>();

        CreateMap<Product, UpdateProductDTO>();


    }
}