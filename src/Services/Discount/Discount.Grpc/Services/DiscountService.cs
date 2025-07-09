using Discount.Grpc.Data;
using Discount.Grpc.Modes;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(
    DiscountDbContext discountDbContext,
    ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountDbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };

        logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async  Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request data."));

        discountDbContext.Coupons.Add(coupon);
        await discountDbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created for ProductName : {ProductName}, Amount {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UdateDicount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request data."));

        discountDbContext.Coupons.Update(coupon);
        await discountDbContext.SaveChangesAsync();

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = discountDbContext.Coupons.FirstOrDefault(x => x.ProductName == request.ProductName);

        if(coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Discount not found."));

        discountDbContext.Coupons.Remove(coupon!);

        await discountDbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully deleted. Product Name: {ProductName}", request.ProductName);

        return new DeleteDiscountResponse { Success = true };
    }
}
