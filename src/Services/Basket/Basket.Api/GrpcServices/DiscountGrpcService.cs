using Discount.Grpc;
using System.Threading.Tasks;

namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoService.GetDiscountAsync(request);
        }
    }
}
