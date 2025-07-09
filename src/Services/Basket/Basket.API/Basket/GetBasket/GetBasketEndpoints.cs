namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserId);

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("basket/{userName}", async (string userName, ISender sender) =>
        {
            var request = new GetBasketQuery(userName);
            var result = await sender.Send(request);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        }).WithName("GetBasket")
        .Produces<GetBasketResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get a user's shopping cart")
        .WithDescription("Get Basket");
    }
}
