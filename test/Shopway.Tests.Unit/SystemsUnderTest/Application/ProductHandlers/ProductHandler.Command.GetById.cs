using Shopway.Domain.Abstractions.Repositories;
using Shopway.Tests.Unit.Abstractions;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;
using static System.Threading.CancellationToken;

namespace Shopway.Tests.Unit.SystemsUnderTest.Application.ProductHandlers;

public class CreateProductCommandHandlerTests : TestBase
{
    private readonly GetProductByIdQueryHandler _sut;
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    public CreateProductCommandHandlerTests()
    {
        _sut = new(_productRepository);
    }

	[Fact]
	public async Task Handle_ShouldSucceed_WhenCreateValidProduct()
	{
        //Arrange
        var productId = ProductId.New();

        var expected = Product.Create
        (
            productId, 
            ProductName.Create("Name").Value, 
            Price.Create(2).Value, 
            UomCode.Create("kg").Value, 
            Revision.Create("2").Value
        );

        _productRepository
            .GetByIdAsync(productId)
            .Returns(expected);

        var query = new GetProductByIdQuery(productId);

        //Act
        var result = await _sut
            .Handle(query, None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var actual = result.Value;

        actual.Id.Should().Be(expected.Id.Value);
        actual.ProductName.Should().Be(expected.ProductName.Value);
        actual.Revision.Should().Be(expected.Revision.Value);
        actual.Price.Should().Be(expected.Price.Value);
        actual.UomCode.Should().Be(expected.UomCode.Value);
    }
}