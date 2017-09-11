using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    string scoreKey = "score";
    int score = 0;
    public int bestScore = 0;

    private static ScoreManager _instance;
    public static ScoreManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreManager();

                _instance.bestScore = PlayerPrefs.GetInt(_instance.scoreKey);
            }

            return _instance;
        }
    }

    public void AddScore(int add)
    {
        score += add;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(scoreKey, bestScore);
        }
    }

    public int GetScore()
    {
        return score;
    }
}
