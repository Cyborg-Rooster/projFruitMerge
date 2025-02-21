using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

class ServerManager
{
    public static OnlinePlayer OnlinePlayer;
    public static Ranking Ranking;

    public static bool? PostSucessfull;
    public static bool? GetSucessfull;
    public static bool? PutSucessful;
    public static bool FirstTime;

    public static IEnumerator SendPostRequest()
    {
        PostSucessfull = null;

        // Cria um UnityWebRequest com o método POST
        using (UnityWebRequest request = new UnityWebRequest("https://www.gagliardicacambas.com.br/api/index.php/users", "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-API-KEY", Player.ApiKey);
            request.timeout = 3;

            // Aguarda a conclusão da requisição
            yield return request.SendWebRequest();

            // Tratamento de resposta
            if (request.result == UnityWebRequest.Result.Success)
            {
                PostSucessfull = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição POST bem-sucedida: " + jsonResponse);
                OnlinePlayer = JsonConvert.DeserializeObject<OnlinePlayer[]>(jsonResponse)[0];
            }
            else 
            {
                Debug.LogError("Erro na requisição POST: " + request.error);
                PostSucessfull = false; 
            }
        }
    }

    public static IEnumerator SendGetRequest()
    {
        GetSucessfull = null;

        using (UnityWebRequest request = new UnityWebRequest($"https://www.gagliardicacambas.com.br/api/index.php/users?IDUser={Player.IDUser}", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-API-KEY", Player.ApiKey);
            request.timeout = 3;

            // Aguarda a conclusão da requisição
            yield return request.SendWebRequest();

            // Tratamento de resposta
            if (request.result == UnityWebRequest.Result.Success)
            {

                GetSucessfull = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição GET bem-sucedida: " + jsonResponse);
                Ranking = JsonConvert.DeserializeObject<Ranking>(jsonResponse);
            }
            else 
            {
                Debug.LogError("Erro na requisição GET: " + request.error);
                GetSucessfull = false;
            }
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
                PutSucessful = true;

                string jsonResponse = request.downloadHandler.text;

                Debug.Log("Requisição PUT bem-sucedida: " + jsonResponse);
                Ranking = JsonConvert.DeserializeObject<Ranking>(jsonResponse);
            }
            else
            {
                Debug.LogError("Erro na requisição PUT: " + request.error);
                PutSucessful = false;
            }
        }
    }

    public static IEnumerator CheckConnection()
    {
        using (UnityWebRequest request = new UnityWebRequest($"https://www.google.com", "GET"))
        {
            // Aguarda a conclusão da requisição
            yield return request.SendWebRequest();

            // Tratamento de resposta
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Conexão estável");
            }
            else
            {
                Debug.Log("Conexão perdida");
                PostSucessfull = false;
                GetSucessfull = false;
            }
        }
    }
    public static IEnumerator WaitUntilRankingDownloaded()
    {
        yield return new WaitUntil(() => Ranking != null);
    }
}
