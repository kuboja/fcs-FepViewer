using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace FepViewer
{
    public class MainWindowsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public XmlChildItem[] TreeData { get; set; }

        public string FilePath { get; set; }

        public bool Autoload { get; set; }

        public bool LoadButtonEnable { get; set; }

        public MainWindowsViewModel()
        {
            ResetData();
            Autoload = SettingHelper.Settings.Autoload;
            LoadButtonEnable = true;
        }

        public void ResetData()
        {
            TreeData = new XmlChildItem[] { new XmlChildItem { Expression = "not loaded yet", IsSelected = true, } };
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public async Task LoadXml()
        {
            LoadButtonEnable = false;

            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show("Je potřeba zadat cestu k XML souboru!", "Zadejte XML soubor", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
                return;
            }

            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Soubor neexistuje!", "Zadejte XML soubor", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
                return;
            }

            try
            {
                var treeData = await DeserializeObjectAsync<XmlRootItem>(FilePath);
                treeData.Item.IsExpanded = true;
                treeData.Item.IsSelected = true;

                TreeData = new XmlChildItem[] { treeData.Item };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala chyba při zpracování souboru:\n\n " + ex.Message);
            }

            LoadButtonEnable = true;
        }

        public static async Task<T> DeserializeObjectAsync<T>(string xmlFilePath)
        {
            return await Task.Run(() =>
            {
                using (FileStream file = new FileStream(xmlFilePath, FileMode.Open))
                {
                    using (XmlTextReader reader = new XmlTextReader(file))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(XmlRootItem));
                        var theObject = (T)ser.Deserialize(reader);
                        return theObject;
                    }
                }
            });
        }

        internal async Task OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "FEP xml files (*.fepxml)|*.fepxml"
            };
            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(FilePath);
            }
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                if (Autoload)
                {
                    await LoadXml();
                }
            }
        }

        public void ExpandAllFirst()
        {
            if (TreeData.Length > 0)
            {
                TreeData[0].ExpandAllFirst();
            }
        }

        public void SaveSettings()
        {
            SettingHelper.Settings.LastFilepath = FilePath;
            SettingHelper.Settings.Autoload = Autoload;
            SettingHelper.SaveAll();
        }
    }
}
