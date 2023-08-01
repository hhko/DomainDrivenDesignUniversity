﻿using RestSharp;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Tests.Integration.Utilities;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Tests.Integration.Container.Utilities;
using Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var expected = await fixture.DataGenerator.AddProductAsync();

        var request = GetRequest(expected.Id.Value.ToString());

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var actual = response.DeserializeResponseResult<ProductResponse>();
        actual.ShouldMatch(expected);
    }

    [Fact]
    public async Task GetById_ShouldReturnErrorResponse_WhenProductNotExists()
    {
        //Arrange
        var invalidProductId = ProductId.New();

        var request = GetRequest(invalidProductId.Value.ToString());

        //Act
        var response = await _restClient!.ExecuteGetAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ValidationProblemDetails>();
        problemDetails!.ShouldConsistOf(InvalidReference(invalidProductId.Value, nameof(Product)));
    }
}