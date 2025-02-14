using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

class ServerManager
{
    public static OnlinePlayer OnlinePlayer;
    public static Ranking Ranking;
    public static bool? Online;
    public static bool FirstTime;

    public static IEnumerator SendPostRequest()
    {
        // Cria um UnityWebRequest com o método POST
        using (UnityWebRequest request = new UnityWebRequest("https://www.gagliardicacambas.com.br/api/index.php/users", "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-API-KEY", Player.ApiKey);


            // Aguarda a conclusão da requisição
            yield return request.SendWebRequest();

            // Tratamento de resposta
            if (request.result == UnityWebRequest.Result.Success)
            {
                FirstTime = true;
                Online = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição POST bem-sucedida: " + jsonResponse);
                OnlinePlayer = JsonConvert.DeserializeObject<OnlinePlayer[]>(jsonResponse)[0];
            }
            else Online = false;
        }
    }

    public static IEnumerator SendGetRequest()
    {
        using (UnityWebRequest request = new UnityWebRequest("https://www.gagliardicacambas.com.br/api/index.php/users", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-API-KEY", Player.ApiKey);
            request.timeout = 2;

            // Aguarda a conclusão da requisição
            yield return request.SendWebRequest();

            // Tratamento de resposta
            if (request.result == UnityWebRequest.Result.Success)
            {

                Online = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição GET bem-sucedida: " + jsonResponse);
                Ranking = JsonConvert.DeserializeObject<Ranking>(jsonResponse);
            }
            else Online = false;
        }
    }

    public static IEnumerator SendPutRequest()
    {
        using (UnityWebRequest request = new UnityWebRequest("https://www.gagliardicacambas.com.br/api/index.php/users", "PUT"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-API-KEY", Player.ApiKey);
            request.timeout = 2;

            OnlinePlayer = new OnlinePlayer()
            {
                Score = Player.BestScore,
                ApiKey = Player.ApiKey,
                IDUser = Player.IDUser,
            };

            string jsonData = JsonConvert.SerializeObject(OnlinePlayer);

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

            yield return request.SendWebRequest();

            // Verifica o resultado
            if (request.result == UnityWebRequest.Result.Success)
            {
                Online = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição PUT bem-sucedida: " + jsonResponse);
                Ranking = JsonConvert.DeserializeObject<Ranking>(jsonResponse);
            }
            else
            {
                Debug.LogError("Erro na requisição PUT: " + request.error);
                Online = false;
            }
        }
    }

    public static IEnumerator WaitUntilOnlineHasResult()
    {
        yield return new WaitUntil(() => Online != null);
    }

    public static IEnumerator WaitUntilRankingDownloaded()
    {
        yield return new WaitUntil(() => Ranking != null);
    }
}
