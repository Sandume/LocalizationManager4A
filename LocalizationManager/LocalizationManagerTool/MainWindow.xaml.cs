using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;


namespace LocalizationManagerTool
{

    public partial class MainWindow : Window
    {

        public class Translation
        {
            public string Id { get; set; }
            public string En { get; set; }
            public string Fr { get; set; }
            public string Es { get; set; }
            public string Ja { get; set; }
        }

        public List<string> Columns = new List<string>();
        public ObservableCollection<Translation> Translations { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Translations = new ObservableCollection<Translation>();
            dataGrid.ItemsSource = Translations;

            foreach (string column in Columns)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = column;
                textColumn.Binding = new Binding(column);
                dataGrid.Columns.Add(textColumn);
            }
        }

        #region buttons
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
            if (dataGrid.SelectedItem is Translation selectedTranslation)
            {
                // Créer une fenêtre d'édition ou utiliser des champs de texte pour modifier les valeurs
                // Exemple simple d'édition avec MessageBox
                MessageBox.Show($"Éditez les valeurs pour : {selectedTranslation.Id}");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une traduction à éditer.");
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "translations.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportCsv(saveFileDialog.FileName);
            }
        }

        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "translations.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportJson(saveFileDialog.FileName);
            }
        }

        private void ExportXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.xml)|*.xml",
                FileName = "translations.xml"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportXml(saveFileDialog.FileName);
            }
        }

        private void ExportClassButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CS files (*.cs)|*.cs",
                FileName = "translations.cs"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportClass(saveFileDialog.FileName);
            }
        }

        private void ExportCppHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Header files (*.h)|*.h",
                FileName = "translations.h"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportCppHeader(saveFileDialog.FileName);
            }
        }

        private void ExportCppSourceButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.cpp)|*.cpp",
                FileName = "translations.cpp"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportCppSource(saveFileDialog.FileName);
            }
        }

        #endregion

        #region CSV
        private void ImportCsv(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                Translations.Clear();
                foreach (var line in lines.Skip(1))
                {
                    var values = line.Split(',');
                    if (values.Length == 5)
                    {
                        Translations.Add(new Translation
                        {
                            Id = values[0],
                            En = values[1],
                            Fr = values[2],
                            Es = values[3],
                            Ja = values[4]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'importation : {ex.Message}");
            }
        }

        private void ExportCsv(string filePath)
        {
            var lines = new List<string> { "id,en,fr,es,ja" };
            lines.AddRange(Translations.Select(t => $"{t.Id},{t.En},{t.Fr},{t.Es},{t.Ja}"));
            File.WriteAllLines(filePath, lines);
        }
        #endregion

        #region json
        private void ImportJson(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var translations = JsonConvert.DeserializeObject<List<Translation>>(jsonData);
            Translations.Clear();
            foreach (var translation in translations)
            {
                Translations.Add(translation);
            }
        }

        private void ExportJson(string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(Translations, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }
        #endregion

        #region Xmls
        private void ImportXml(string filePath)
        {
            XDocument xDoc = XDocument.Load(filePath);
            Translations.Clear();
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
                Translations.Add(translation);
            }
        }

        private void ExportXml(string filePath)
        {
            var xDoc = new XDocument(new XElement("translations",
                Translations.Select(t => new XElement("translation",
                    new XElement("id", t.Id),
                    new XElement("en", t.En),
                    new XElement("fr", t.Fr),
                    new XElement("es", t.Es),
                    new XElement("ja", t.Ja)
                ))
            ));
            xDoc.Save(filePath);
        }

        #endregion

        #region ExportClass
        private void ExportClass(string filePath)
        {
            var classContent = @"
using System.Collections.Generic;

public class Translations
{
    public static Dictionary<string, string> TranslationData = new Dictionary<string, string>
    {
";
            foreach (var translation in Translations)
            {
                classContent += $"        {{ \"{translation.Id}\", \"{translation.En}\" }},\n";
            }
            classContent += @"
    };
}";
            File.WriteAllText(filePath, classContent);
        }
        #endregion

        #region exportCPP
        private void ExportCppHeader(string filePath)
        {
            var headerContent = @"
#ifndef TRANSLATIONS_H
#define TRANSLATIONS_H

#include <map>
#include <string>

class Translations
{
public:
    static std::map<std::string, std::string> TranslationData;
};

#endif // TRANSLATIONS_H";
            File.WriteAllText(Path.ChangeExtension(filePath, ".h"), headerContent);
        }

        private void ExportCppSource(string filePath)
        {
            var sourceContent = @"
#include ""Translations.h""

std::map<std::string, std::string> Translations::TranslationData = {
";
            foreach (var translation in Translations)
            {
                sourceContent += $"    {{ \"{translation.Id}\", \"{translation.En}\" }},\n";
            }
            sourceContent += @"};
";
            File.WriteAllText(Path.ChangeExtension(filePath, ".cpp"), sourceContent);
        }
    }
    #endregion
}