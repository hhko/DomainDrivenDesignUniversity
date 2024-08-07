﻿using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Features;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Utilities;
using System.Reflection;
using static Shopway.Application.Features.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

//BatchResponseEntryBuilder is only used for BatchResponseBuilder
//So to avoid having generic parameters in this entry builder, it is a subclass in the generic BatchResponseBuilder
partial class BatchResponseBuilder<TBatchRequest, TResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TResponseKey : struct, IUniqueKey
{
    /// <summary>
    /// Builder used to build a single response entry from a single request, with possible errors and result status 
    /// </summary>
    internal sealed class BatchResponseEntryBuilder(TBatchRequest request, TResponseKey responseKey, BatchEntryStatus successStatus)
        : IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>
    {
        private readonly TBatchRequest _request = request;

        /// <summary>
        /// The key, that represents the request uniqueness. Usually the unique composed key, created by few entity properties
        /// </summary>
        private readonly TResponseKey _responseKey = responseKey;

        /// <summary>
        /// This status that will be used, if validation succeeds. If not, the 'Error' status will be used instead.
        /// </summary>
        private readonly BatchEntryStatus _successStatus = successStatus;

        /// <summary>
        /// If there are no errors, then the validation succeeds and the success status is used.
        /// </summary>
        private readonly List<Error> _errors = [];

        /// <summary>
        /// Request used to create the ResponseKey and being validated to inspect errors.
        /// </summary>
        public TBatchRequest Request => _request;
        internal bool IsValid => _errors.IsNullOrEmpty();
        internal bool IsValidAndToInsert => IsValid && _successStatus is Inserted;
        internal bool IsValidAndToUpdate => IsValid && _successStatus is Updated;

        /// <summary>
        /// Validate the provided condition and add an error, if it is true
        /// </summary>
        /// <param name="condition">Condition representing the invalid case</param>
        /// <param name="thenError">Error that will be added to error list if invalid condition is true</param>
        /// <returns>Same instance to be able to chain validation methods</returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> If(bool condition, Error thenError)
        {
            if (condition is true)
            {
                _errors.Add(thenError);
            }

            return this;
        }

        /// <summary>
        /// In order to place validation in parts (in separate methods) use ValidateUsing and pass the respective validation part as an input delegate
        /// </summary>
        /// <param name="requestValidationMethod">Validation action that represent the validation that needs to be performed</param>
        /// <returns>Same instance to be able to chain validation methods</returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> ValidateUsing
        (
            Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod
        )
        {
            requestValidationMethod(this, _request);
            return this;
        }

        /// <summary>
        /// Use the public, static validation method called "Validate", defined in a given ValueObject type 
        /// </summary>
        /// <typeparam name="TValueObject">ValueObject type that we use to validate input parameters</typeparam>
        /// <param name="parameters">Parameters that are required and sufficient to create a ValueObject</param>
        /// <returns>Same instance to be able to chain validation methods</returns>
        /// <exception cref="ArgumentException">Thrown if no parameters were specified</exception>
        /// <exception cref="InvalidOperationException">Thrown if given ValueObject type does not contain the public, static method "Validate"</exception>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> UseValueObjectValidation<TValueObject>(params object[] parameters)
            where TValueObject : IValueObject
        {
            if (parameters.IsNullOrEmpty())
            {
                throw new ArgumentException($"There need to be at least one parameter for the validation method");
            }

            if (AnyNullParameter<TValueObject>(parameters))
            {
                return this;
            }

            var validationMethod = typeof(TValueObject)
                .GetMethod(IValueObject.ValidateMethodName, BindingFlags.Public | BindingFlags.Static);

            if (validationMethod is null)
            {
                throw new InvalidOperationException($"{nameof(IValueObject)}: {typeof(TValueObject).Name} does not contain public, static method \"Validate\"");
            }

            object errors = validationMethod.Invoke(null, parameters)!;

            _errors.AddRange((List<Error>)errors);
            return this;
        }

        /// <summary>
        /// Builds the response entry based on a previous validations. If there is at least one error, status will be set to error.
        /// </summary>
        /// <returns>Response entry: (ResponseKey, ResponseStatus, Errors)</returns>
        internal BatchResponseEntry<TResponseKey> BuildBatchResponseEntry()
        {
            var responseStatus = IsValid
                ? _successStatus
                : BatchEntryStatus.Error;

            return new BatchResponseEntry<TResponseKey>(_responseKey, responseStatus, _errors);
        }

        /// <summary>
        /// Examines if any of input parameters is null
        /// </summary>
        /// <typeparam name="TValueObject">ValueObject</typeparam>
        /// <param name="parameteres">Input parameters</param>
        /// <returns>True if there is null parameter. Otherwise, return false</returns>
        private bool AnyNullParameter<TValueObject>(object[] parameteres)
            where TValueObject : IValueObject
        {
            if (parameteres.Any(parameter => parameter is null))
            {
                _errors.Add(Domain.Common.Errors.Error.New($"Error.{nameof(IValueObject)}", $"At least one of {typeof(TValueObject).Name} components is null"));
                return true;
            }

            return false;
        }
    }
}
