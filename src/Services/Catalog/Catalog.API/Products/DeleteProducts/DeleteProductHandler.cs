
namespace Catalog.API.Products.DeleteProducts;

public record DeleteProductCommand(Guid Id) :
    ICommand<DeleteProductdResult>;
public record DeleteProductdResult(bool IsSuccess);

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required.");
    }
}

internal class DeleteProductCommandHandler(
    IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductdResult>
{
    public async Task<DeleteProductdResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync();

        return new DeleteProductdResult(true);
    }
}
