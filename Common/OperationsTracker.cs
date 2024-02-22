using System.Collections.Generic;
using Runtime.Shared.AddressablesContentController.Extensions;
using Runtime.Shared.AddressablesContentController.Interfaces;

namespace Runtime.Shared.AddressablesContentController.Common
{
    public class OperationsTracker : IOperationsTracker
    {
        private readonly HashSet<ContentOperation> _operations = new(256);

        public void Track(ContentOperation operation)
        {
            _operations.Add(operation);
        }

        public void Untrack(ContentOperation operation)
        {
            if (_operations.Contains(operation) == false)
                return;

            _operations.Remove(operation);
        }

        public void UntrackAndRelease(ContentOperation operation)
        {
            Untrack(operation);
            operation.Release();
        }

        public void Purge()
        {
            foreach (var operation in _operations)
                operation.Release();
        }
    }
}