using System;
using System.IO;

namespace JudicialTest
{
    public class InitializePaths
    {
        private static InitializePaths _instance;
        public static InitializePaths Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InitializePaths();

                return _instance;
            }
        }

        private readonly string _pathDbFile;
        private readonly string _pathMyDocumentsFolder;
        private readonly string _pathSettingsFile;
        private readonly string _pathShablonsFolder;
        private readonly string _connectionString;

        private InitializePaths()
        {
            _pathMyDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Inferite");

            _pathDbFile = Path.Combine(_pathMyDocumentsFolder, "LocalDB");
            _pathSettingsFile = Path.Combine(_pathMyDocumentsFolder, "Settings.xml");
            _pathShablonsFolder = Path.Combine(_pathMyDocumentsFolder, "Shablons");

            AppDomain.CurrentDomain.SetData("DataDirectory", _pathDbFile);
            _connectionString = @"data source=(LocalDb)\MSSQLLocalDB; 
                                    Initial Catalog=JudicialDB; 
                                    AttachDbFilename='|DataDirectory|JudicialDB.mdf'; 
                                    integrated security=True; MultipleActiveResultSets=True; App=EntityFramework";
        }
        public string GetDbFilePath()
        {
            return _pathDbFile;
        }
        public string GetSettingsFilePath()
        {
            return _pathSettingsFile;
        }
        public string GetFolderShablonsPath()
        {
            return _pathShablonsFolder;
        }
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
