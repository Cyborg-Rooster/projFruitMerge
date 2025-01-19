using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LanguageInitializer : MonoBehaviour
{
    [SerializeField] TextAsset Json;
    public static Dictionary<string, GameText> bruteValues;

    public void Initialize()
    {
        string jsonContent = Json.text;

        bruteValues = JsonConvert.DeserializeObject<Dictionary<string, GameText>>(jsonContent);
    }

    public int GetLocalizationIndex()
    {
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;

        if (cultureInfo.Name.Equals("pt-BR", System.StringComparison.OrdinalIgnoreCase)) return 0;
        else return 1;
    }

    public static int GetLanguageLenght()
    {
        return bruteValues.Count - 1;
    }

}
