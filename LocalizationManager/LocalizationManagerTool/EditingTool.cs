using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public class Translation
        {
            public string Id { get; set; }
            public string En { get; set; }
            public string Fr { get; set; }
            public string Es { get; set; }
            public string Ja { get; set; }
        }

        public List<string> Columns = new List<string> { "Id", "En", "Fr", "Es", "Ja" };
        public ObservableCollection<Translation> Translations { get; set; }

    }
}
