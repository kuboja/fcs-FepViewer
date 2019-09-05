using System.ComponentModel;
using System.IO;
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

        public MainWindowsViewModel()
        {
            TreeData = new XmlChildItem[] { new XmlChildItem { Expression = "not loaded", IsSelected = true, } };
        }

        public void LoadXml()
        {
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

            XmlSerializer ser = new XmlSerializer(typeof(XmlRootItem));
            FileStream file = new FileStream(FilePath, FileMode.Open);
            XmlTextReader reader = new XmlTextReader(file);

            var treeData = (XmlRootItem)ser.Deserialize(reader);
            treeData.Item.IsExpanded = true;
            treeData.Item.IsSelected = true;

            TreeData = new XmlChildItem[] { treeData.Item };
        }

        public void ExpandAllFirst()
        {
            if (TreeData.Length > 0)
            {
                TreeData[0].ExpandAllFirst();
            }
        }
    }
}
