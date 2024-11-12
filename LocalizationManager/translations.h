
#ifndef TRANSLATIONS_H
#define TRANSLATIONS_H

#include <map>
#include <string>

class Translations
{
public:
    static std::map<std::string, std::map<std::string, std::string>> TranslationData;
};

#endif // TRANSLATIONS_H