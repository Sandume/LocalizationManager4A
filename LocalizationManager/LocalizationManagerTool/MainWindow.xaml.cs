using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq; 


namespace LocalizationManagerTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public List<string> Columns = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Columns.Add("id");
            Columns.Add("en");
            Columns.Add("fr");
            Columns.Add("es");
            Columns.Add("ja");

            foreach (string column in Columns)
            {
                //Pour ajouter une colonne à notre datagrid
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = column;
                textColumn.Binding = new Binding(column);
                dataGrid.Columns.Add(textColumn);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|XML files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                if (filePath.EndsWith(".csv"))
                {
                    ImportCsv(filePath);
                }
                else if (filePath.EndsWith(".json"))
                {
                    ImportJson(filePath);
                }
                else if (filePath.EndsWith(".xml"))
                {
                    ImportXml(filePath);
                }
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void ImportCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var values = line.Split(',');
                // Crée un objet avec ces valeurs et ajoute-le à la source de données
            }
        }

        private void ImportJson(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var translations = JsonConvert.DeserializeObject<List<Translation>>(jsonData);
            // Remplis ta DataGrid avec ces traductions
        }

        public class Translation
        {
            public string Id { get; set; }
            public string En { get; set; }
            public string Fr { get; set; }
            public string Es { get; set; }
            public string Ja { get; set; }
        }

        private void ImportXml(string filePath)
        {
            XDocument xDoc = XDocument.Load(filePath);
            foreach (var element in xDoc.Descendants("translation"))
            {
                var translation = new Translation
                {
                    Id = element.Element("id")?.Value,
                    En = element.Element("en")?.Value,
                    Fr = element.Element("fr")?.Value,
                    Es = element.Element("es")?.Value,
                    Ja = element.Element("ja")?.Value
                };
                // Ajoute ce modèle à la source de données
            }
        }
    }
}