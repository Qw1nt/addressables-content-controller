using Qw1nt.Runtime.AddressablesContentController.Common;

namespace Qw1nt.Runtime.Shared.AddressablesContentController.Interfaces
{
    public interface IOperationsTracker
    {
        void Track(ContentOperation operation);

        void Untrack(ContentOperation operation);

        void UntrackAndRelease(ContentOperation operation);
        
        void Purge();
    }
}