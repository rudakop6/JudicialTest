using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JudicialTest
{
    public interface IEntityVM
    {
        ICommand AddCommand { get; set; }
        ICommand SaveCommand { get; set; }
        ICommand RemoveCommand { get; set; }

        void InitializeCommand();
        void AddEntity();
        void SaveEntity();
        void RemoveEntity();
        void VisibilityButtons();
    }
}
