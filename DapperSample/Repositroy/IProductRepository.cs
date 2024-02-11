using Dapper;
using DapperSample.Models;
using DapperSample.MyCommand;
using DapperSample.ResultDto;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Configuration;

namespace DapperSample.Repositroy
{
    public interface IProductRepository
    {
        ApiResult CreateProduct(InsertProductInputDto req);
        ApiResult UpdateProduct(UodateProductInputDto req);
        ApiResult DeleteProduct(long id);
        ApiResult<IEnumerable<ProductOutPutDto>> GetProducts(); 
        ApiResult<ProductOutPutDto> GetProductById(long productId);
        ApiResult<IEnumerable<ProductOutPutDto>> GetProductByName(string productName);
        
    }
    public class ProductRepository : IProductRepository
    {
        private readonly ICommandText _commandText;
        private readonly string _ConnectionString;
        public ProductRepository(ICommandText commandText, IConfiguration configuration)
        {
            _commandText = commandText;
            _ConnectionString = configuration["ConnectionStrings:connectionString"];
        }
        public ApiResult CreateProduct(InsertProductInputDto req)
        {
            try
            {
                 Product product=new Product()
                 {
                     IsDeleted= false,
                     Size=req.Size,
                     Status=req.Status,
                     Nooeh=req.Nooeh,
                     Price=req.Price,
                     ProductName=req.ProductName,
                     Insert_ByUserID=req.Insert_ByUserID,
                 };
                var productsTask = ExecuteCommand(_ConnectionString, conn => conn.Query<Product>(_commandText.AddProduct,
                    new { ProductName= product.ProductName, Price =product.Price, Nooeh =product.Nooeh, Status =product.Status, Size = product .Size, IsDeleted =product.IsDeleted, Insert_ByUserID =product.Insert_ByUserID}));
                var result = productsTask;
                return new ApiResult
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.Created,
                    Message = "محصول با موفقیت ساخته شد"
                };
            }
            catch (Exception)
            {

                return new ApiResult { IsSuccess = false,StatusCode=(int)HttpStatusCode.InternalServerError };
            }
        }

        public ApiResult DeleteProduct(long id)
        {
            var productsTask = ExecuteCommand(_ConnectionString, conn =>
            {
                var query = conn.Query<Product>(_commandText.RemoveProduct, new { Id = id });
                return query.FirstOrDefault(); // return the first product or null if none
            });

            if (productsTask==null)
            {
                return new ApiResult
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,

                };
            }
            return new ApiResult
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.NoContent,
            };
        }

        public ApiResult<ProductOutPutDto> GetProductById(long productId)
        {
            try
            {
                var productsTask = ExecuteCommand<Product>(_ConnectionString, conn => conn.Query<Product>(_commandText.GetProductById, new { @Id = productId }).SingleOrDefault());
                if (productsTask==null)
                {
                    return new ApiResult<ProductOutPutDto>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = ""
                    };
                }
                ProductOutPutDto productOutPutDto = new ProductOutPutDto()
                {
                    Size = productsTask.Size,
                    Status = productsTask.Status,
                    Id = productId,
                    Nooeh = productsTask.Nooeh,
                    Price = productsTask.Price,
                    ProductName = productsTask.ProductName,
                };
                return new ApiResult<ProductOutPutDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = productOutPutDto
                };
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        public ApiResult<IEnumerable<ProductOutPutDto>> GetProductByName(string productName)
        {
            var productsTask = ExecuteCommand(_ConnectionString, conn => conn.Query<Product>(_commandText.GetProducts,new { @ProductName=productName }));
            var productsEnumerable = productsTask;
            var result = productsEnumerable.Select(s => new ProductOutPutDto
            {
                Size = s.Size,
                Status = s.Status,
                Id = s.Id,
                Nooeh = s.Nooeh,
                Price = s.Price,
                ProductName = s.ProductName,
            }).ToList();


            return new ApiResult<IEnumerable<ProductOutPutDto>>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = result,
                Message = "موفق"
            };
        }
        public ApiResult<IEnumerable<ProductOutPutDto>> GetProducts()
        {
            try
            {
                var productsTask = ExecuteCommand(_ConnectionString, conn => conn.Query<Product>(_commandText.GetProducts));
                var productsEnumerable = productsTask;
                var result = productsEnumerable.Select(s=> new ProductOutPutDto
                {
                    Size = s.Size,
                    Status = s.Status,
                    Id = s.Id,
                    Nooeh = s.Nooeh,
                    Price = s.Price,
                    ProductName = s.ProductName,
                }).ToList();


                return new ApiResult<IEnumerable<ProductOutPutDto>>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = result,
                    Message = "موفق"
                };

            }
            catch (Exception)
            {

                return new ApiResult<IEnumerable<ProductOutPutDto>>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "خطا در سرور"
                };
            }
        }

        public ApiResult UpdateProduct(UodateProductInputDto req)
        {
            var productsTask = ExecuteCommand(_ConnectionString, conn => conn.Query<Product>(_commandText.AddProduct,
                new { ProductName = req.ProductName, Price = req.Price, Nooeh = req.Nooeh, Status = req.Status, Size = req.Size, Update_ByUserID = req.Update_ByUserID, Update_DateTime=req.Update_DateTime, Id =req.Id}));
            var result = productsTask;
            return new ApiResult
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.NoContent,
            };
        }
        #region utilities
        private void ExecuteCommand(string Connection,Action<SqlConnection> task)
        {
            using (var conn=new SqlConnection(Connection))
            {
                conn.Open();
                task(conn);
            }
        }
        private T ExecuteCommand<T>(string Connection, Func<SqlConnection, T> task)
        {
            using (var conn = new SqlConnection(Connection))
            {
                conn.Open();
               return task(conn);
            }
        }
        #endregion
    }

    public class InsertProductInputDto
    {
        public string ProductName { get; set; }
        public int Size { get; set; }
        public string Nooeh { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public long Insert_ByUserID { get; set; }
    }
    public class UodateProductInputDto
    {
            
        public long Id { get; set; }
        public string ProductName { get; set; }
        public int Size { get; set; }
        public string Nooeh { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public DateTime Update_DateTime { get; set; }
        public long Update_ByUserID { get; set; }
    }
    public class ProductOutPutDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public int Size { get; set; }
        public string Nooeh { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
    }
}
