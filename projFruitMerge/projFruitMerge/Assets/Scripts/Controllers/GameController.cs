using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject PointText;

    public int Points;

    public void AddPoints(int points)
    {
        Points += points;
        TextManager.SetText(PointText, Points.ToString());
    }
}
