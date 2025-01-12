using Cysharp.Threading.Tasks;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Core;

namespace Qw1nt.Runtime.AddressablesContentController.Extensions
{
    public static class GroupExtensions
    {
        public static async UniTask<T> InGroup<T>(this UniTask<ContentOperation<T>> operation, ContentGroup group)
            where T : class
        {
            var loaded = await operation;
            group.Add(loaded);

            return loaded.GetResult();
        }
    }
}