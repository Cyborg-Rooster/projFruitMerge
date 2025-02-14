using UnityEngine;

public class RankingItemController : MonoBehaviour
{
    [SerializeField] GameObject Index;
    [SerializeField] GameObject ID;
    [SerializeField] GameObject ScoreText;
    [SerializeField] GameObject Score;   
    
    public void SetText(int index, string id, string scoreText, int score)
    {
        UIManager.SetText(Index, index);
        UIManager.SetText(ID, id);
        UIManager.SetText(ScoreText, scoreText);
        UIManager.SetText(Score, score);
    }
}
