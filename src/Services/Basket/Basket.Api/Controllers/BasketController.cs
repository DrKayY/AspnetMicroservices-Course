using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepo, DiscountGrpcService discountGrpcService)
        {
            _basketRepo = basketRepo;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepo.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepo.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepo.DeleteBasket(userName);

            return Ok();
        }
    }
}
