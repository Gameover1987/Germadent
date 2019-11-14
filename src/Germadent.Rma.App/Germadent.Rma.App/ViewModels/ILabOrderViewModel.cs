namespace Germadent.Rma.App.ViewModels
{
    public interface ILabOrderViewModel
    {
        void Initialize(bool isReadOnly);

        bool IsReadOnly { get; }
    }
}