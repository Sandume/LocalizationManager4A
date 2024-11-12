using System.Collections.ObjectModel;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public class Translation
        {
            public string Id { get; set; }
            public Dictionary<string, string> Languages { get; set; } = new Dictionary<string, string>();
        }
        public ObservableCollection<Translation> Translations { get; set; } = new ObservableCollection<Translation>();

    }
}
