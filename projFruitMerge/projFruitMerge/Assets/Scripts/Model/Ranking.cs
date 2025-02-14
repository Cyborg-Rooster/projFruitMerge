using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
class Ranking
{
    public OnlinePlayer[] ranking;
    public int user_position;

    public void ResortRanking(int score)
    {
        var r = ranking.ToList();

        var item = r.FirstOrDefault(i => i.ApiKey == Player.ApiKey);
        if (item != null)
        {
            // Atualizar o score
            item.Score = score;

            // Reordenar a lista para manter a ordem decrescente
            r.Sort((a, b) => b.Score.CompareTo(a.Score));

            ranking = r.ToArray();
        }
    }
}
