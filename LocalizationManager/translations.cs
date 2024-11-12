
using System.Collections.Generic;

public class Translations
{
    public static Dictionary<string, Dictionary<string, string>> TranslationData = new Dictionary<string, Dictionary<string, string>>
    {
        { "0", new Dictionary<string, string> { { "it", "holla" }, { "en", "hello" }, { "fr", "salut" } }},
        { "1", new Dictionary<string, string> { { "fr", "patte" }, { "en", "spagetti" }, { "it", "pasta" } }},

    };
}