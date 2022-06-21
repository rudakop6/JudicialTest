using System.Windows.Input;

namespace JudicialTest
{
    public interface IPickableEntityVM
    {
        ICommand PickCommand { get; set; }

        void PickEntity();
    }
}
