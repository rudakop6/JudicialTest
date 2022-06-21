using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JudicialTest
{
    public class EstateAddressesVM : Singleton<EstateAddressesVM>, IEntityVM
    {

        private EstateAddress _selectedEstateAddress;
        private ObservableCollection<EstateAddress> _allEstateAddresses;

        public EstateAddress SelectedEstateAddress
        {
            get { return _selectedEstateAddress; }
            set
            {
                _selectedEstateAddress = value;
                VisibilityButtons();
                OnPropertyChanged("SelectedEstateAddress");
            }
        }

        public ObservableCollection<EstateAddress> AllEstateAddresses
        {
            get
            {
                if (_allEstateAddresses == null)
                {
                    _allEstateAddresses = new ObservableCollection<EstateAddress>();
                    _allEstateAddresses.CollectionChanged += new NotifyCollectionChangedEventHandler(AllExtracts_CollectionChanged);
                }
                return _allEstateAddresses;
            }
            set
            {
                _allEstateAddresses = value;
                _allEstateAddresses.CollectionChanged += new NotifyCollectionChangedEventHandler(AllExtracts_CollectionChanged);
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        private EstateAddressesVM()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            AddCommand = new RelayCommand(AddEntity);
            SaveCommand = new RelayCommand(SaveEntity);
            RemoveCommand = new RelayCommand(RemoveEntity);
        }

        public void AddEntity()
        {           

        }

        public void RemoveEntity()
        {
            
        }

        public void SaveEntity()
        {
            MainWindow.Instance.DB.SaveChanges();
        }

        public void VisibilityButtons()
        {
            
        }

        private void AllExtracts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

    }
}
