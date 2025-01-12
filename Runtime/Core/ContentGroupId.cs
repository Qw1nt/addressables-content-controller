using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qw1nt.Runtime.AddressablesContentController.Core
{
    public readonly struct ContentGroupId : IEqualityComparer<ContentGroupId>
    {
        private readonly int _source;

        public ContentGroupId(int id)
        {
            _source = id;
        }

        public static ContentGroupId Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(0);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return _source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ContentGroupId x, ContentGroupId y)
        {
            return x._source == y._source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(ContentGroupId obj)
        {
            return obj._source;
        }
    }
}