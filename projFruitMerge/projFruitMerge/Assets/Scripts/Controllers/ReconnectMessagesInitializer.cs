using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ReconnectMessagesInitializer : MonoBehaviour
{
    [SerializeField] TextAsset Json;
    public static Dictionary<string, ReconnectMessages> Messages;

    public void Initialize()
    {
        string jsonContent = Json.text;

        Messages = JsonConvert.DeserializeObject<Dictionary<string, ReconnectMessages>>(jsonContent);
    }

    public static string GetMessage(int index)
    {
        var rm = Messages[index.ToString()];

        return RandomManager.GetRandomObject<string>(rm.Message.ToList());
    }

}
