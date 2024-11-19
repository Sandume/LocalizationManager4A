
public class ImportTrad
{

    struct Translation
    {
        public string Id { get; set; }
        public Dictionary<string, string> Languages { get; set; } = new Dictionary<string, string>();
    }

    public List<Translation> Translations { get; set; } = new List<Translation>();

    public void InitializeCSV(string filePath)
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
    }

    public void InitializeJSON(string filePath)
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
        }
    }

    public void InitializeXML(string filePath)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath);

        Translations.Clear();

        foreach (XmlNode translationNode in xmlDocument.GetElementsByTagName("Translation"))
        {
            string id = string.Empty;
            XmlNode idNode = translationNode.SelectSingleNode("Id");
            if (idNode != null)
            {
                id = idNode.InnerText;
            }

            var languages = new Dictionary<string, string>();
            XmlNode languagesNode = translationNode.SelectSingleNode("Languages");
            if (languagesNode != null)
            {
                foreach (XmlNode languageNode in languagesNode.ChildNodes)
                {
                    string key = languageNode.Name;
                    string value = languageNode.InnerText;
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
}