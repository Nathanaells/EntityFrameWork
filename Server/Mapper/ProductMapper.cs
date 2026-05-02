using AutoMapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductCreateDTO, Product>();

        CreateMap<Product, ProductResponseDTO>();

        CreateMap<UpdateProductDTO, Product>();


    }
}