using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.DiscountApi.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace BookMarketPlace.Services.DiscountApi.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<ICustomResponse<bool>> DeleteById(int id)
        {
            var response = await _dbConnection.ExecuteAsync("DELETE FROM discount where id=@Id", new { Id = id });

            if (response > 0)
            {
                return ResponseNoContent<bool>.Success(204);
            }
            return ResponseNoContent<bool>.Error(new List<string> { "Silme Sırasında Hata" }, 500);
        }

        public async Task<ICustomResponse<List<Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Discount>("Select * from discount");

            return Response<List<Discount>>.Success(discounts.ToList(), 200); 
        }

        public async Task<ICustomResponse<Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = await _dbConnection.QueryFirstOrDefaultAsync<Discount>("Select * from discount where userid=@User_Id and code=@Code", new { User_Id = userId, Code = code });

            if (discount == null)
            {
                return ResponseNoContent<Discount>.Error(new List<string> { "Kayıt Bulunamadı" }, 404);
            }
            return Response<Discount>.Success(discount, 200);
        }

        public async Task<ICustomResponse<Discount>> GetById(int id)
        {

            var discount = await _dbConnection.QueryFirstOrDefaultAsync<Discount>("Select * from discount where id=@Id",new {Id=id});

            if (discount == null) {
                return ResponseNoContent<Discount>.Error(new List<string> { "Kayıt Bulunamadı" }, 404);
            }
            return Response<Discount>.Success(discount, 200); 
        }

        public async Task<ICustomResponse<Discount>> Save(Discount discount)
        {
            var response = await _dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES (@UserId,@Rate,@Code)", discount);
            
            if (response>0)
            {
                var responseDiscount = await _dbConnection.QueryFirstOrDefaultAsync<Discount>("Select * from discount where id=@Id", new { Id = response });
                return Response<Discount>.Success(responseDiscount, 200);
            }
            return Response<Discount>.Error(new List<string> { "Kayıt Sırasında Hata"},500); 
        }

        public async Task<ICustomResponse<bool>> Update(Discount discount)
        {
            var response = await _dbConnection.ExecuteAsync("UPDATE discount set userid=@User_Id,code=@Code,rate=@Rate", new
            {
                User_Id=discount.UserId,
                Code=discount.Code,
                Rate=discount.Rate
            });

            if (response>0)
            {
                return ResponseNoContent<bool>.Success(204);
            }
            return ResponseNoContent<bool>.Error(new List<string> { "Güncelleme Sırasında Hata" }, 500);
        }
    }
}
