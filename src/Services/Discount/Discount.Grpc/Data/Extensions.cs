using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class Extensions 
{
    public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
        context.Database.MigrateAsync();

        return app;
    }
}
