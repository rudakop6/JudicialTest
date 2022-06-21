using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JudicialTest
{
    public partial class CreateLinkWindow : Window
    {
        private static CreateLinkWindow _instance;
        public static CreateLinkWindow Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CreateLinkWindow();

                return _instance;
            }
        }

        public CreateLinkWindow()
        {
            _instance = this;
            InitializeComponent();
            this.Closing += Window_Closing;

            CreateLink_ApartDebtorForm.DataContext = CreateLink_ApartDebtorVM.Instance;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void CB_AllHouses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateLink_ApartDebtorVM.Instance.CB_HouseSelectionChanged();
        }

        private void CB_AllApartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
