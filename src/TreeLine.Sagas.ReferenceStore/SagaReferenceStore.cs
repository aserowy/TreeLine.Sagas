﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TreeLine.Sagas.ReferenceStore.Accessing;

namespace TreeLine.Sagas.ReferenceStore
{
    // TODO retries in seconds and time to live configurable with builder 
    internal sealed class SagaReferenceStore : IReferenceStore
    {
        private static readonly IList<int> _retriesInSeconds = new List<int> { 1, 1, 2, 3, 5, 8, 13 };

        private readonly IReferenceRepository _repository;

        public SagaReferenceStore(IReferenceRepository repository)
        {
            _repository = repository;
        }

        public Task AddReferences(params ISagaReference[] eventReference)
        {
            return Retry(_retriesInSeconds, () => _repository.CreateOrUpdateAsync(eventReference));
        }

        public async Task<IList<ISagaReference>?> GetReferencesAsync(Guid referenceId)
        {
            var result = await Retry(_retriesInSeconds, () => _repository.GetAsync(referenceId)).ConfigureAwait(false);

            return result.ToList();
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types",
            Justification = "Depending on the concrete implementation of IReferenceRepository, Retry produces unknown Exceptions.")]
        private static async Task<T> Retry<T>(IList<int> retriesInSeconds, Func<Task<T>> doFunc)
        {
            var exceptions = new List<Exception>();
            foreach (var span in retriesInSeconds)
            {
                try
                {
                    return await doFunc
                        .Invoke()
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }

                await Task
                    .Delay((int)TimeSpan.FromSeconds(span).TotalMilliseconds)
                    .ConfigureAwait(false);
            }

            if (exceptions.Count.Equals(1))
            {
                throw exceptions.Single();
            }
            else
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}