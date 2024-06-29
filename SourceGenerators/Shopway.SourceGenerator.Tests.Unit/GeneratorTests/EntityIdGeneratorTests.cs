﻿using Shopway.SourceGenerator.Generators;
using Shopway.SourceGenerator.Tests.Unit.Utilities;
using static Shopway.SourceGenerator.Tests.Unit.Utilities.Constants;

namespace Shopway.SourceGenerator.Tests.Unit.GeneratorTests;

[Trait(nameof(UnitTest), UnitTest.SourceGenerator)]
public sealed class EntityIdGeneratorTests
{
    private readonly EntityIdGenerator _entityIdGenerator;

    public EntityIdGeneratorTests()
    {
        _entityIdGenerator = new EntityIdGenerator();
    }

    [Fact]
    public void EntityIdGenerator_ShouldGenerateEntityIdAttribute()
    {
        //Act
        var actualResult = _entityIdGenerator.Generate(string.Empty);

        //Assert
        actualResult.Should().Be(@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the dr-marek-jaskula source generator
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace System;

/// <summary>
/// Add to entities to indicate that entity id structure should be generated
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = ""Generated by the dr-marek-jaskula source generator."")]
public class GenerateEntityIdAttribute : global::System.Attribute;");
    }

    [Fact]
    public void EntityIdGenerator_ShouldGenerateEntityId()
    {
        //Arrange
        (string input, string output) = GetEntityToGenerateId();

        //Act
        var actualResult = _entityIdGenerator.Generate(input);

        //Assert
        actualResult.Should().Contain(output);
    }

    private static (string input, string output) GetEntityToGenerateId()
    {
        var input = """
        using System;

        namespace MyNamespace;

        [GenerateEntityId]
        public sealed class Product;
        """;

        var output = """
        //------------------------------------------------------------------------------
        // <auto-generated>
        //     This code was generated by the dr-marek-jaskula source generator
        //
        //     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
        // </auto-generated>
        //------------------------------------------------------------------------------

        #nullable enable

        using Shopway.Domain.Common.BaseTypes.Abstractions;
        using System.Diagnostics;

        namespace MyNamespace;

        [DebuggerDisplay("{Value}")]
        public readonly record struct ProductId : IEntityId<ProductId>
        {
            public const string Name = "ProductId";
            public const string Namespace = "MyNamespace";

            private ProductId(Ulid id)
            {
                Value = id;
            }

            public Ulid Value { get; }

            public static ProductId New()
            {
                return new ProductId(Ulid.NewUlid());
            }

            public static ProductId Create(Ulid id)
            {
                return new ProductId(id);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public override string ToString()
            {
                return Value.ToString();
            }

            public int CompareTo(IEntityId? other)
            {
                if (other is null)
                {
                    return 1;
                }

                if (other is not ProductId otherId)
                {
                    throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
                }

                return Value.CompareTo(otherId.Value);
            }

            public static bool operator >(ProductId a, ProductId b) => a.CompareTo(b) is 1;
            public static bool operator <(ProductId a, ProductId b) => a.CompareTo(b) is -1;
            public static bool operator >=(ProductId a, ProductId b) => a.CompareTo(b) >= 0;
            public static bool operator <=(ProductId a, ProductId b) => a.CompareTo(b) <= 0;
        }
        """;

        return (input, output);
    }
}
