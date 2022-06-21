using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace JudicialTest
{
    public class SettingsVM : Singleton<SettingsVM>
    {
        private string _pathFolderResultIP;
        private string _pathFolderResultSP;
        private string _pathFileShablonIP;
        private string _pathFileShablonSP;

        public SerializableDictionary<PathEnum, string> PathList { get; set; } =
            new SerializableDictionary<PathEnum, string>(InitializePaths.Instance.GetSettingsFilePath());

        public string PathFolderResultIP
        {
            get { return _pathFolderResultIP; }
            set
            {
                _pathFolderResultIP = value;
                OnPropertyChanged("PathFolderResultIP");
            }
        }
        public string PathFolderResultSP
        {
            get { return _pathFolderResultSP; }
            set
            {
                _pathFolderResultSP = value;
                OnPropertyChanged("PathFolderResultSP");
            }
        }
        public string PathFileShablonIP
        {
            get { return _pathFileShablonIP; }
            set
            {
                _pathFileShablonIP = value;
                OnPropertyChanged("PathFileShablonIP");
            }
        }
        public string PathFileShablonSP
        {
            get { return _pathFileShablonSP; }
            set
            {
                _pathFileShablonSP = value;
                OnPropertyChanged("PathFileShablonSP");
            }
        }


        public ICommand PickFolderResultIPCommand { get; set; }
        public ICommand PickFolderResultSPCommand { get; set; }
        public ICommand PickShablonIPCommand { get; set; }
        public ICommand PickShablonSPCommand { get; set; }
        public ICommand SavePathsCommand { get; set; }
        public ICommand ReturnPathsCommand { get; set; }

        private SettingsVM()
        {
            PickFolderResultIPCommand = new RelayCommand(PickFolderResultIP);
            PickFolderResultSPCommand = new RelayCommand(PickFolderResultSP);
            PickShablonIPCommand = new RelayCommand(PickShablonIP);
            PickShablonSPCommand = new RelayCommand(PickShablonSP);

            SavePathsCommand = new RelayCommand(SavePaths);
            ReturnPathsCommand = new RelayCommand(ReturnPaths);
        }

        private void PickShablonIP()
        {
            OpenFileDialog _dlgFile = new OpenFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "*.xls*|*.xlsx*"
            };

            if (_dlgFile.ShowDialog() == DialogResult.OK)
                PathFileShablonIP = _dlgFile.FileName.ToString();
        }

        private void PickShablonSP()
        {
            OpenFileDialog _dlgFile = new OpenFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "*.xls*|*.xlsx*"
            };
            
            if (_dlgFile.ShowDialog() == DialogResult.OK)
                PathFileShablonSP = _dlgFile.FileName.ToString();
        }

        private void PickFolderResultIP()
        {
            FolderBrowserDialog _dlgFolder = new FolderBrowserDialog();
            if (_dlgFolder.ShowDialog() == DialogResult.OK)    
                PathFolderResultIP = _dlgFolder.SelectedPath;            
        }

        private void PickFolderResultSP()
        {
            FolderBrowserDialog _dlgFolder = new FolderBrowserDialog();
            if (_dlgFolder.ShowDialog() == DialogResult.OK)
                PathFolderResultSP = _dlgFolder.SelectedPath;
        }
        private void SavePaths()
        {
            bool existFlag = CheckerValidPaths();

            PathList.Clear();
            PathList.Add(PathEnum.NameFileShablonSP, PathFileShablonSP);
            PathList.Add(PathEnum.NameFileShablonIP, PathFileShablonIP);
            PathList.Add(PathEnum.NameFolderResultSP, PathFolderResultSP);
            PathList.Add(PathEnum.NameFolderResultIP, PathFolderResultIP);
            PathList.Writer();

            if(existFlag)
                MessageBox.Show("Сохранено успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Сохранено с ошибкой", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ReturnPaths()
        {
            ReturnChangedPaths();            
        }


        public void InitPathList()
        {
            string shablonsPath = InitializePaths.Instance.GetFolderShablonsPath();

            if (!File.Exists(InitializePaths.Instance.GetSettingsFilePath()))
            {
                PathList.Clear();
                PathList.Add(PathEnum.NameFileShablonIP, Path.Combine(shablonsPath, "Shablon_IP.xlsx"));
                PathList.Add(PathEnum.NameFileShablonSP, Path.Combine(shablonsPath, "Shablon_SP.xlsx"));
                PathList.Add(PathEnum.NameFolderResultIP, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                PathList.Add(PathEnum.NameFolderResultSP, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                PathList.Writer();
            }
            else
            {
                PathList.Reader();
            }
            InitSettingsPaths();
        }



        private void InitSettingsPaths()
        {
            foreach (var path in PathList)
            {
                switch (path.Key)
                {
                    case PathEnum.NameFileShablonSP:
                        if (File.Exists(path.Value))
                            PathFileShablonSP = path.Value;
                        break;
                    case PathEnum.NameFileShablonIP:
                        if (File.Exists(path.Value))
                            PathFileShablonIP = path.Value;
                        break;
                    case PathEnum.NameFolderResultSP:
                        if (Directory.Exists(path.Value))
                            PathFolderResultSP = path.Value;
                        break;
                    case PathEnum.NameFolderResultIP:
                        if (Directory.Exists(path.Value))
                            PathFolderResultIP = path.Value;
                        break;
                }
            }
        }
        private bool CheckerValidPaths()
        {
            bool existFlag = true;
            foreach (var path in PathList)
            {
                switch (path.Key)
                {
                    case PathEnum.NameFileShablonSP:
                        if (!File.Exists(PathFileShablonSP))
                        {
                            PathFileShablonSP = path.Value;
                            existFlag = false;
                        }
                        break;
                    case PathEnum.NameFileShablonIP:
                        if (!File.Exists(PathFileShablonIP))
                        {
                            PathFileShablonIP = path.Value;
                            existFlag = false;
                        }
                        break;
                    case PathEnum.NameFolderResultSP:
                        if (!Directory.Exists(PathFolderResultSP))
                        {
                            PathFolderResultSP = path.Value;
                            existFlag = false;
                        }
                        break;
                    case PathEnum.NameFolderResultIP:
                        if (!Directory.Exists(PathFolderResultIP))
                        {
                            PathFolderResultIP = path.Value;
                            existFlag = false;
                        }
                        break;
                }
            }
            return existFlag;
        }
        private void ReturnChangedPaths()
        {
            foreach (var path in PathList)
            {
                switch (path.Key)
                {
                    case PathEnum.NameFileShablonSP:
                        PathFileShablonSP = path.Value;
                        break;
                    case PathEnum.NameFileShablonIP:
                        PathFileShablonIP = path.Value;
                        break;
                    case PathEnum.NameFolderResultSP:
                        PathFolderResultSP = path.Value;
                        break;
                    case PathEnum.NameFolderResultIP:
                        PathFolderResultIP = path.Value;
                        break;
                }
            }
        }
    }
}
