﻿using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public const int MaxFlatNumber = 1000;
    public const int MaxBuildingNumber = 1000;
    public const int MinFlatNumber = 1;
    public const int MinBuildingNumber = 1;
    public static readonly string[] AvailableCountries = new string[4] { "Poland", "Germany", "England", "Russia" };
    private static readonly Regex _zipCodeRegex = new(@"^\d{5}?$", RegexOptions.Compiled);

    public string City { get; }
    public string Country { get; }
    public string ZipCode { get; }
    public string Street { get; }
    public int Building { get; }
    public int? Flat { get; }

    private Address(string city, string country, string zipCode, string street, int building, int? flat)
    {
        City = city;
        Country = country;
        ZipCode = zipCode;
        Street = street;
        Building = building;
        Flat = flat;
    }

    //Empty constructor in this case is required by EF Core, because has a complex type as a parameter in the default constructor.
    private Address() 
    { 
    }

    public static ValidationResult<Address> Create(string city, string country, string zipCode, string street, int building, int? flat)
    {
        var errors = Validate(city, country, zipCode, street, building, flat);
        return errors.CreateValidationResult(() => new Address(city, country, zipCode, street, building, flat));
    }

    public static List<Error> Validate(string city, string country, string zipCode, string street, int building, int? flat)
    {
        var errors = Empty<Error>();

        if (ValidateCountry(country) is { IsValid: false } countryValidation)
        {
            errors.Add(countryValidation.Error);
        }

        if (ValidateCity(city) is { IsValid: false } cityValidation)
        {
            errors.Add(cityValidation.Error);
        }

        if (ValidateZipCode(zipCode) is { IsValid: false } zipCodeValidation)
        {
            errors.Add(zipCodeValidation.Error);
        }

        if (ValidateStreet(street) is { IsValid: false } streetValidation)
        {
            errors.Add(streetValidation.Error);
        }

        if (ValidateBuilding(building) is { IsValid: false } buildingValidation)
        {
            errors.Add(buildingValidation.Error);
        }

        if (ValidateFlat(flat) is { IsValid: false } flatValidation)
        {
            errors.Add(flatValidation.Error);
        }

        return errors;
    }

    /// <returns>Street, City, Country, ZipCode, Building and Flat if not null</returns>
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return ZipCode;
        yield return Building;

        if (Flat is not null)
        {
            yield return Flat;
        }
    }

    #region Validation Methods

    private static (bool IsValid, Error Error) ValidateCountry(string country)
    {
        return country switch
        {
            string when country.IsNullOrEmptyOrWhiteSpace() => (false, AddressError.EmptyCountry),
            string when AvailableCountries.Contains(country) => (false, AddressError.UnsupportedCountry),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateCity(string city)
    {
        return city switch
        {
            string when city.IsNullOrEmptyOrWhiteSpace() => (false, AddressError.EmptyCity),
            string when city.ContainsIllegalCharacter() || city.ContainsDigit() => (false, AddressError.ContainsIllegalCharacterOrDigit),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateZipCode(string zipCode)
    {
        var result = _zipCodeRegex.Match(zipCode);

        return result.Success switch
        {
            false => (false, AddressError.ZipCodeDoesNotMatch),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateStreet(string street)
    {
        return street switch
        {
            string when street.IsNullOrEmptyOrWhiteSpace() => (false, AddressError.EmptyCity),
            string when street.ContainsIllegalCharacter() => (false, AddressError.ContainsIllegalCharacter),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateBuilding(int building)
    {
        return building switch
        {
            < MinBuildingNumber or > MaxBuildingNumber => (false, AddressError.WrongBuildingNumber),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateFlat(int? flat)
    {
        return flat switch
        {
            null => (true, Error.None),
            < MinFlatNumber or > MaxFlatNumber => (false, AddressError.WrongFlatNumber),
            _ => (true, Error.None)
        };
    }

    #endregion Validation Methods
}

