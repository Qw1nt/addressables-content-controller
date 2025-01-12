using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Qw1nt.Runtime.AddressablesContentController.Common;

namespace Qw1nt.Runtime.AddressablesContentController.Core
{
    public struct ContentGroup
    {
        private readonly ContentGroupId _id;

        private readonly HashSet<ContentOperation> _operations;
        private ContentController _source;

        public ContentGroup(ContentGroupId id, HashSet<ContentOperation> operations, ContentController source)
        {
            _id = id;
            _operations = operations;
            _source = source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Add(ContentOperation operation)
        {
            _operations.Add(operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unload()
        {
            foreach (var operation in _operations)
                _source.Release(operation.Key);

            _operations.Clear();
        }
    }
}