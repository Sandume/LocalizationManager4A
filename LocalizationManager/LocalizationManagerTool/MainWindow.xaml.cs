using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;


namespace LocalizationManagerTool
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Translations = new ObservableCollection<Translation>();
            dataGrid.ItemsSource = Translations;
        }

        #region buttons

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is Translation selectedTranslation)
                {
                    Translations.Remove(selectedTranslation);
                }
            }
        }

        private void UpdateDataGridColumns()
        {
            dataGrid.Columns.Clear();

            DataGridTextColumn idColumn = new DataGridTextColumn
            {
                Header = "Id",
                Binding = new Binding("Id")
            };
            dataGrid.Columns.Add(idColumn);

            if (Translations.Count > 0)
            {
                foreach (string language in Translations.First().Languages.Keys)
                {
                    DataGridTextColumn languageColumn = new DataGridTextColumn
                    {
                        Header = language,
                        Binding = new Binding($"Languages[{language}]")
                    };
                    dataGrid.Columns.Add(languageColumn);
                }
            }
        }

        private void AddLanguage(string languageCode)
        {
            if (Translations.Any() && Translations.First().Languages.ContainsKey(languageCode))
            {
                MessageBox.Show($"La langue '{languageCode}' existe déjà.");
                return;
            }

            foreach (var translation in Translations)
            {
                translation.Languages[languageCode] = string.Empty; 
            }

            if (Translations.Count == 0)
            {
                var tempTranslation = new Translation { Id = "Temp" };  
                tempTranslation.Languages[languageCode] = string.Empty;
                Translations.Add(tempTranslation);
            }

            UpdateDataGridColumns();

            dataGrid.Items.Refresh();
        }

        private void AddLanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string newLanguageCode = Microsoft.VisualBasic.Interaction.InputBox(
                "Entrez le code de la nouvelle langue (par exemple : 'It' pour Italien) :",
                "Ajouter une langue",
                "");

            if (!string.IsNullOrWhiteSpace(newLanguageCode))
            {
                AddLanguage(newLanguageCode);
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
                dataGrid.Items.Refresh();
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
                MessageBox.Show("Fichier CSV exporté avec succès !");
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
                MessageBox.Show("Fichier JSON exporté avec succès !");
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
                MessageBox.Show("Fichier XML exporté avec succès !");
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
                MessageBox.Show("Fichier CS exporté avec succès !");
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
                MessageBox.Show("Fichier H exporté avec succès !");
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
                MessageBox.Show("Fichier CPP exporté avec succès !");
            }
        }

        #endregion

        #region CSV
        private void ImportCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var headers = lines[0].Split(',');

            Translations.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var translation = new Translation
                {
                    Id = values[0]
                };

                for (int j = 1; j < headers.Length; j++)
                {
                    translation.Languages[headers[j]] = values[j];
                }

                Translations.Add(translation);
            }

            UpdateDataGridColumns();
        }

        private void ExportCsv(string filePath)
        {
            var lines = new List<string>();

            var headers = new List<string> { "Id" };
            if (Translations.Any())
            {
                headers.AddRange(Translations.First().Languages.Keys);
            }
            lines.Add(string.Join(",", headers));

            foreach (var translation in Translations)
            {
                var values = new List<string> { translation.Id };

                values.AddRange(headers.Skip(1).Select(lang =>
                    translation.Languages.ContainsKey(lang) ? translation.Languages[lang] : string.Empty));

                lines.Add(string.Join(",", values));
            }

            File.WriteAllLines(filePath, lines);
        }
        #endregion

        #region json
        private void ImportJson(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);

            var importedTranslations = JsonConvert.DeserializeObject<List<Translation>>(jsonContent);

            if (importedTranslations != null)
            {
                Translations.Clear();
                foreach (var translation in importedTranslations)
                {
                    Translations.Add(translation);
                }

                dataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'importation du fichier JSON.");
            }
        }

        private void ExportJson(string filePath)
        {
            var json = JsonConvert.SerializeObject(Translations, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        #endregion

        #region Xmls
        private void ImportXml(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);

            Translations.Clear();

            foreach (var translationElement in xmlDocument.Descendants("Translation"))
            {
                string id = translationElement.Element("Id")?.Value ?? string.Empty;

                var languages = new Dictionary<string, string>();
                var languagesElement = translationElement.Element("Languages");
                if (languagesElement != null)
                {
                    foreach (var languageElement in languagesElement.Elements())
                    {
                        string key = languageElement.Name.LocalName;
                        string value = languageElement.Value;
                        languages[key] = value;
                    }
                }

                Translations.Add(new Translation
                {
                    Id = id,
                    Languages = languages
                });
            }
        }

        private void ExportXml(string filePath)
        {
            XElement root = new XElement("Translations");

            foreach (var translation in Translations)
            {
                XElement translationElement = new XElement("Translation");
                translationElement.Add(new XElement("Id", translation.Id));
                XElement languagesElement = new XElement("Languages");
                foreach (var language in translation.Languages)
                {
                    languagesElement.Add(new XElement(language.Key, language.Value));
                }

                translationElement.Add(languagesElement);
                root.Add(translationElement);
            }
            XDocument xmlDocument = new XDocument(root);
            xmlDocument.Save(filePath);
        }

        #endregion

        #region ExportClass
        private void ExportClass(string filePath)
        {
            var classContent = @"
using System.Collections.Generic;

public class Translations
{
    public static Dictionary<string, Dictionary<string, string>> TranslationData = new Dictionary<string, Dictionary<string, string>>
    {
";

            foreach (var translation in Translations)
            {
                classContent += $"        {{ \"{translation.Id}\", new Dictionary<string, string> {{ ";

                foreach (var language in translation.Languages)
                {
                    classContent += $"{{ \"{language.Key}\", \"{language.Value}\" }}, ";
                }

                classContent = classContent.TrimEnd(',', ' ') + " }},\n";
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
    static std::map<std::string, std::map<std::string, std::string>> TranslationData;
};

#endif // TRANSLATIONS_H";
            File.WriteAllText(Path.ChangeExtension(filePath, ".h"), headerContent);
        }

        private void ExportCppSource(string filePath)
        {
            var sourceContent = @"
#include ""Translations.h""

std::map<std::string, std::map<std::string, std::string>> Translations::TranslationData = {
";

            foreach (var translation in Translations)
            {
                sourceContent += $"    {{ \"{translation.Id}\", {{";  

                foreach (var language in translation.Languages)
                {
                    sourceContent += $"{{ \"{language.Key}\", \"{language.Value}\" }}, ";
                }

                sourceContent = sourceContent.TrimEnd(',', ' ') + " }},\n";
            }

            sourceContent += @"};
";
            File.WriteAllText(Path.ChangeExtension(filePath, ".cpp"), sourceContent);
        }
    }
    #endregion
}