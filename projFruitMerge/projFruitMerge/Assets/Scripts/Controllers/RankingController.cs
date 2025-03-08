using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RankingController : MonoBehaviour
{
    [SerializeField] RankingItemController RankingItemController;
    [SerializeField] GameObject ScrollRect;
    [SerializeField] GameObject Content;

    List<GameObject> RankingItems = new List<GameObject>();

    private void CreateRanking(string pontuation)
    {
        for(int i = 0; i < ServerManager.Ranking.ranking.Length; i++)
        {
            var r = ServerManager.Ranking.ranking[i];
            var go = Instantiate(RankingItemController, transform);

            RankingItems.Add(go.gameObject);
            go.SetText(i + 1, r.IDUser, pontuation, r.Score);
        }

    }

    public void Refresh(string pontuation)
    {
        foreach (var item in RankingItems) Destroy(item);
        RankingItems.Clear();

        CreateRanking(pontuation);
    }
}
