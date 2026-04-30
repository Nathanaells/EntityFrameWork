// using FluentValidation;
// using FluentValidation.Results;
// using Implemented_MVC.DTOs;

// public class ProductService
// {
//     private readonly IValidator<ProductDTO> _productValidator;

//     public ProductService(IValidator<ProductDTO> productValidator)
//     {
//         _productValidator = productValidator;
//     }

//     public async Task<ApiResponseDto<ProductDTO>> CreateProduct(ProductDTO productDto)
//     {
//         ValidationResult validationResult = _productValidator.Validate(productDto);

//         if (!validationResult.IsValid)
//         {
//             return ApiResponseDto<ProductDTO>.ErrorResult(
//                 "Invalid product data.",
//                 validationResult.Errors.Select(e => e.ErrorMessage).ToList()
//             );
//         }

//         // Here you would typically save the product to the database
//         // For this example, we'll just return the validated product

//         return ApiResponseDto<ProductDTO>.SuccessResult(productDto);
//     }

//     public async Task<ApiResponseDto<ProductDTO>> GetProductById(int id)
//     {
//         // Here you would typically retrieve the product from the database
//         // For this example, we'll just return a dummy product

//         ProductDTO product = new ProductDTO
//         {
//             Id = id,
//             Name = "Sample Product",
//             Price = 9.99m,
//             StoreId = 1,
//         };

//         return ApiResponseDto<ProductDTO>.SuccessResult(product);
//     }

//     public async Task<ApiResponseDto<List<ProductDTO>>> GetProductsByStoreId(int storeId)
//     {
//         // Here you would typically retrieve the products from the database
//         // For this example, we'll just return a list of dummy products

//         List<ProductDTO> products = new List<ProductDTO>
//         {
//             new ProductDTO
//             {
//                 Id = 1,
//                 Name = "Product 1",
//                 Price = 9.99m,
//                 StoreId = storeId,
//             },
//             new ProductDTO
//             {
//                 Id = 2,
//                 Name = "Product 2",
//                 Price = 19.99m,
//                 StoreId = storeId,
//             },
//             new ProductDTO
//             {
//                 Id = 3,
//                 Name = "Product 3",
//                 Price = 29.99m,
//                 StoreId = storeId,
//             },
//         };

//         return ApiResponseDto<List<ProductDTO>>.SuccessResult(products);
//     }

//     public async Task<ApiResponseDto<ProductDTO>> UpdateProduct(int id, ProductDTO productDto)
//     {
//         ValidationResult validationResult = _productValidator.Validate(productDto);

//         if (!validationResult.IsValid)
//         {
//             return ApiResponseDto<ProductDTO>.ErrorResult(
//                 "Invalid product data.",
//                 validationResult.Errors.Select(e => e.ErrorMessage).ToList()
//             );
//         }

//         // Here you would typically update the product in the database
//         // For this example, we'll just return the updated product

//         productDto.Id = id; // Ensure the ID is set to the provided ID

//         return ApiResponseDto<ProductDTO>.SuccessResult(productDto);
//     }

//     public async Task<ApiResponseDto<Product>> DeleteProduct(int id)
//     {
//         // Here you would typically delete the product from the database
//         // For this example, we'll just return true to indicate success

//         return ApiResponseDto<bool>.SuccessResult(true);
//     }
// }
