# Domain Layer :anchor:

In this layer we define: 

- Entities (and entities ids)
- Aggregates
- Value Objects
- Enumerations
- Domain Events
- Errors
- Enums
- Results
- Constants
- Common components
- IRepositories
- Interfaces and utilities of generic purpose

## Validation

Validation in the Domain Layer is mainly connected to ValueObjects. 

Errors, that occur when in-domain validation fails, need to be explicitly handled. This is done by "Validator" that is defined in .Infrastructure project.

We can examine handling the in-domain validation in for instance "CreateProductCommandHandler.Handle"

## Ulid instead of Guid

Ulid is a Universally Unique Lexicographically Sortable Identifier. 

The biggest advantage of Ulid is that is it Lexicographically sortable. This is very important feature, because it allows 
to use cursor pagination and therefore significantly increase the performance of some queries.

Other advantages of ulid:
- Canonically encoded as a 26 character string
- Uses Crockford's base32 for better efficiency and readability (5 bits per character)
- Case insensitive
- No special characters (URL safe)
- Monotonic sort order (correctly detects and handles the same millisecond)

Moreover, we use [Ulid](https://github.com/Cysharp/Ulid) NuGet Package that provides more efficient operation on Uilds than similar ones on Guids.

## Source Generated EntityIds

In order to create a EntityId in the desired format for the Shopway application, we utilize our custom **Shopway.SourceGenerator**, 
which is also included in this repository. See **ReadMe.SourceGenerator.md** for details. 

To use it, simply decorate an entity with the **[GenerateEntityId]** attribute.". The generated entity id (and its JsonConverter) is in the following form:

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the dr-marek-jaskula source generator
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Products;

public readonly record struct ProductId : IEntityId<ProductId>
{
    public const string Name = "ProductId";
    public const string Namespace = "Shopway.Domain.Products";

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

public sealed class ProductIdJsonConverter : JsonConverter<ProductId>
{
    public override ProductId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var entityIdAsString = reader.GetString();

        if (Ulid.TryParse(entityIdAsString, out var ulid))
        {
            return ProductId.Create(ulid);
        }

        throw new InvalidOperationException($"'{entityIdAsString}' cannot be parsed to Ulid");
    }

    public override void Write(Utf8JsonWriter writer, ProductId entityId, JsonSerializerOptions options)
    {
        writer.WriteStringValue(entityId.Value.ToString());
    }
}
```

NOTE: the **using Shopway.Domain.Common.BaseTypes.Abstractions;** is hardcoded in the source generator, because from the Shopway application perspective it is
more than sufficient. To make it more customizable, see remarks made in **ReadMe.SourceGenerator.md**. If You want to see how to deal with it, see SourceGenerator
for id converters or comparers, where we use a property on a attribute to store the namespace - the desired approach would be to use generic parameter, however the 
Roslyn source generator seems not to support that option.

NOTE: for tutorial purposes, the UserId remains defined as before in **UserId.cs**, This approach is not preferred because if the definition of EntityId changes, 
we would need to manually update all such ids manually. However, it has been left in place for the clarity of individuals analyzing this repository.

## EntityIds and EntityIdConverter

EntityId is a strongly typed id that is a readonly record struct (due to the fact that ulid is a struct). 
EntityIds AND its JsonConverters are generated using my own custom source generator **Shopway.SourceGenerator**.

NOTE: It is important for serialization purposes that **Value** should have an `init` setter. Otherwise,
**System.Text.Json** serializer would incorrectly serialize its value. This problem holds for all **reaonly record struct** types.

To create a new id we use a static method "New". For instance, "ProductId.New()".

To create a strongly typed id based on a given ulid, we use "Create" method. For instance, "Product.Create(ulid)".

Due to the fact that entity id is a record struct, we can use the '==' operator to compare ids.

Ulid base EntityId allows the comparing two entities. Therefore, the IComparable interface is implemented for IEntityId. Nevertheless,
overriding the '<', '>', '>=', '<=' operator should be done both in the interface and for each concrete EntityId (for instance ProductId).

## Domain Event

"A Domain Event captures the memory of something interesting which affects the domain" � Martin Fowler

Domain event should not leave the bounded context. Therefore, we can use the domain specific language - for instance we can use strongly typed ids.

## Referencing Aggregates

Entities in the aggregates (also aggregate roots) should not contain the direct references to entities in other aggregates, but they can contain ids of them.
Therefore, one aggregate will not be queried as a part of other aggregate. For instance, **OrderHeader** contains property **UserId** but not 
a reference **User**.

## Data Processing (Filter, SortBy, Mapping)

Filtering, sorting and mapping are done by objects that implements ```IFilter<TEntity>```, ```ISortBy<TEntity>```, ``` IMapping<TEntity, TOutput>``` interfaces. The apply method 
invoked in UseSpecification method that is located in the BaseRepository. The dynamic filtering, sorting and mapping can be obtained by objects 
that implements ```IDynamicFilter<TEntity>```, ```IDynamicSortBy<TEntity>```, ```IDynamicMapping<TEntity>```. To set the filtering, sorting and mapping in the specification we use 

```csharp
specification
    .AddFilter(filter);

specification
    .AddSortBy(sortBy);

specification
    .AddMapping(mapping);
```

We can also determine filtering, sorting and mapping in the specification without these objects, using 
```
specification
    .AddFilters(product => product.Id == productId);

specification
    .OrderBy(x => x.ProductName, SortDirection.Ascending)
    .ThenBy(x => x.Price, SortDirection.Descending);

specification
    .AddMapping(x => x.ProductName.Value)
```

## Dynamic or Static Filter

Dynamic filter creates expression trees. Method that handles creating and applying them is the extension method in **QueryableUtilities**
in the Domain called **Where**.

Principles:
1. Each FilterByEntry will generate one or more expressions (see point 3.)
   
   Example: ```Price < 15```

2. Final expression is a combination of all FilterByEntry expressions separated by AND operator 

    Example: ```Price < 15 && Name == "Marek"```

3. Each FilterByEntry generates multiple expressions that are separated by OR operator

    Example: ```Price < 15 || Description = "Fine"```

4. Each expression from multiple FilterByEntry expression is generated based on a single Predicate
5. Each FilterByEntry contains a list of Predicates 

Final expression will look like this:

```(Price < 15 || Description = "Fine") && Name == "Marek"```

To sum up, each expression created from Predicate will be separated by ||, and each predicate created from FilterByEntry
will be separated by &&.

Therefore, we dynamically create any expression we need. 

**Moreover**, we can extend each dynamic filter in the **Apply** method by any static filters, so we have full control on 
filtering.

Static option for filtering the result applies the predicates we have written on our own:

```csharp
public sealed record ProductStaticFilter : IFilter<Product>
{
    public string? ProductName { get; init; }
    public string? Revision { get; init; }
    public decimal? Price { get; init; }
    public string? UomCode { get; init; }

    private bool ByProductName => ProductName.NotNullOrEmptyOrWhiteSpace();
    private bool ByRevision => Revision.NotNullOrEmptyOrWhiteSpace();
    private bool ByPrice => Price.HasValue;
    private bool ByUomCode => UomCode.NotNullOrEmptyOrWhiteSpace();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Filter(ByProductName, product => ((string)(object)product.ProductName).Contains(ProductName!))
            .Filter(ByRevision, product => ((string)(object)product.Revision).Contains(Revision!))
            .Filter(ByPrice, product => ((decimal)(object)product.Price) == Price)
            .Filter(ByUomCode, product => ((string)(object)product.UomCode).Contains(UomCode!));
    }
}
```

## Dynamic or Static SortBy

Dynamic sort use Linq.Dynamic.Core library.

Static option for sorting the result:

```csharp
public sealed record ProductStaticSortBy : ISortBy<Product>
{
    public SortDirection? ProductName { get; init; }
    public SortDirection? Revision { get; init; }
    public SortDirection? Price { get; init; }
    public SortDirection? UomCode { get; init; }

    public SortDirection? ThenProductName { get; init; }
    public SortDirection? ThenRevision { get; init; }
    public SortDirection? ThenPrice { get; init; }
    public SortDirection? ThenUomCode { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        queryable = queryable
            .SortBy(ProductName, product => product.ProductName)
            .SortBy(Revision, product => product.Revision)
            .SortBy(Price, product => product.Price)
            .SortBy(UomCode, product => product.UomCode);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenProductName, product => product.ProductName)
            .ThenSortBy(ThenRevision, product => product.Revision)
            .ThenSortBy(ThenPrice, product => product.Price)
            .ThenSortBy(ThenUomCode, product => product.UomCode);
    }
}
```

## Dynamic or Static Mapping

For static mapping or by using just an expression or by using predefined mapping (see .Application/Mappings/ProductMapping) we can use any 
return any predefined type or use ```DataTransferObject``` (see below). However, for dynamic mapping we ```must``` use the ```DataTransferObject``` that is a dictionary with additional
features. The most important capability of ```DataTransferObject``` is the possibility to build an expression dynamically and in such a way
that only the required properties are included, so no additional data will be queried from the database (in this case I am referring Entity Framework Core).

Static option for customizable mapping with DataTransferObject:

```csharp
public sealed record ProductStaticMapping : IMapping<Product, DataTransferObject>
{
    public static readonly ProductStaticMapping Instance = new()
    {
        Id = true,
        ProductName = true,
        Revision = true,
        Price = true,
        UomCode = true,
    };

    public bool? Id { get; init; }
    public bool? ProductName { get; init; }
    public bool? Revision { get; init; }
    public bool? Price { get; init; }
    public bool? UomCode { get; init; }
    public ReviewStaticMapping Reviews { get; init; } = new();

    private bool IncludeId => Id is true;
    private bool IncludeProductName => ProductName is true;
    private bool IncludeRevision => Revision is true;
    private bool IncludePrice => Price is true;
    private bool IncludeUomCode => UomCode is true;
    private bool IncludeReviews => Reviews.AnySelected();

    public IQueryable<DataTransferObject> Apply(IQueryable<Product> queryable)
    {
        //WARNING! this static query will result in querying ALL fields from the database and then map to ones we require.
        //This is caused by the behavior of EF Core - if the property is present in the select statement it will be queried.
        //To solve this problem we could also create collection of MappingEntries from all given values and then use queryable.Map(mappingEnties)
        //Then, only the required field would be queried. I left this solution for demonstration purposes.
        //If someone decides that performance reasons are nos crucial for him and the following syntax looks good, then You can stick with this
        //My personal option would be to use queryable.Map(mappingEntries) approach, for !performance! and !maintenance! reasons.
        return queryable
            .Select(product => new DataTransferObject()
                .AddIf(IncludeId, nameof(product.Id), $"{product.Id}")
                .AddIf(IncludeUomCode, nameof(product.UomCode), $"{product.UomCode}")
                .AddIf(IncludePrice, nameof(product.Price), $"{product.Price}")
                .AddIf(IncludeRevision, nameof(product.Revision), $"{product.Revision}")
                .AddIf(IncludeProductName, nameof(product.ProductName), $"{product.ProductName}")
                .AddIf(IncludeReviews, nameof(product.Reviews), product.Reviews.Select(review => new DataTransferObject()
                    .AddIf(Reviews!.IncludeUsername, nameof(review.Username), $"{review.Username}")
                    .AddIf(Reviews!.IncludeStars, nameof(review.Stars), $"{review.Stars}")
                    .AddIf(Reviews!.IncludeTitle, nameof(review.Title), $"{review.Title}")
                    .AddIf(Reviews!.IncludeDescription, nameof(review.Description), $"{review.Description}"))));
    }

    public sealed class ReviewStaticMapping
    {
        public bool? Username { get; init; }
        public bool? Stars { get; init; }
        public bool? Title { get; init; }
        public bool? Description { get; init; }

        public bool IncludeUsername => Username is true;
        public bool IncludeStars => Stars is true;
        public bool IncludeTitle => Title is true;
        public bool IncludeDescription => Description is true;

        public bool AnySelected()
        {
            return IncludeUsername
                || IncludeStars
                || IncludeTitle
                || IncludeDescription;
        }
    }
}
```

## Customize Dynamic or Static Filtering/Sorting/Mapping

We can make any customization for filters/sort/mapping in "Apply" method.

We can also combine dynamic and static filtering/sorting/mapping.

Moreover, we can combine dynamic/static filtering/sorting/mapping with manual approach.

Example of manual approach:

```
specification
    .AddFilters(product => product.Id == productId);
```

## Generic Dynamic Filtering/Sorting/Mapping

The go to approach should be using the developer created filtering/sorting/mapping when needed. In many cases we can use the generic dynamic ones.

Filter example:

```csharp
public sealed class DynamicFilter<TEntity, TEntityId> : IDynamicFilter<TEntity>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public static IReadOnlyCollection<string> AllowedFilterProperties => [];

    public static IReadOnlyCollection<string> AllowedFilterOperations => [];

    public IList<FilterByEntry> FilterProperties { get; init; } = [];

    public IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, ILikeProvider<TEntity>? likeProvider = null)
    {
        if (FilterProperties.IsNullOrEmpty())
        {
            return queryable;
        }

        return queryable
            .Where(FilterProperties.CreateFilterExpression(likeProvider));
    }

    public static DynamicFilter<TEntity, TEntityId>? From(DynamicFilter? dynamicFilter)
    {
        if (dynamicFilter is null)
        {
            return null;
        }

        return new DynamicFilter<TEntity, TEntityId>()
        {
            FilterProperties = dynamicFilter.FilterProperties
        };
    }
}
```

To see how to use ultimate dynamic and generic data processing, see Proxy Generic Features in .Application layer.