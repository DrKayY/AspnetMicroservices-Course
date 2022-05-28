using Dapper;
using Discount.Api.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _config;
        private readonly string _conString;

        public DiscountRepository(IConfiguration config)
        {
            _config = config;
            _conString = _config["DatabaseSettings:ConnectionString"];
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var cmd = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)";

            using (var connection = new NpgsqlConnection(_conString))
            {
                var affected = await connection.ExecuteAsync(cmd, new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

                if (affected == 0)
                    return false;

                return true;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var cmd = "DELETE FROM Coupon WHERE ProductName = @ProductName";

            using (var connection = new NpgsqlConnection(_conString))
            {
                var affected = await connection.ExecuteAsync(cmd, new { ProductName = productName });

                if (affected == 0)
                    return false;

                return true;
            }
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var cmd = "SELECT * FROM Coupon WHERE ProductName = @ProductName";

            using (var connection = new NpgsqlConnection(_conString))
            {
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(cmd, new { ProductName = productName });

                if (coupon == null)
                    return new Coupon()
                    {
                        ProductName = "No Discount",
                        Description = "No Discount Described",
                        Amount = 0
                    };

                return coupon;
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var cmd = "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id";

            using (var connection = new NpgsqlConnection(_conString))
            {
                var affected = await connection.ExecuteAsync(cmd, new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

                if (affected == 0)
                    return false;

                return true;
            }
        }
    }
}
