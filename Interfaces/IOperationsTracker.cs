using Runtime.Shared.AddressablesContentController.Common;

namespace Runtime.Shared.AddressablesContentController.Interfaces
{
    public interface IOperationsTracker
    {
        void Track(ContentOperation operation);

        void Untrack(ContentOperation operation);

        void UntrackAndRelease(ContentOperation operation);
        
        void Purge();
    }
}