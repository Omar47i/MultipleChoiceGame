using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    List<QuestionsData> questions = new List<QuestionsData>();         // All the questions.

    List<QuestionsData> currentQuestions = new List<QuestionsData>();  // Current questions being used in the game as we 
                                                                       // will remove the used question to avoid redundancy.
    float difficultyLevel = 1;
    int _round = 1;
    public int round
    {
        get
        {
            return _round;
        }
    }

    string bestRoundKey = "BestRound";
    [HideInInspector]
    public int bestRound;

    string _answer;
    public string answer
    {
        get
        {
            return _answer;
        }
    }

    List<string> _options = new List<string>();
    public List<string> options
    {
        get
        {
            return _options;
        }
    }

    string _question;
    public string question
    {
        get
        {
            return _question;
        }
    }

    public int timeLimit
    {
        get
        {
            return difficultyData.timeLimit;
        }
    }

    public int optionsCount
    {
        get
        {
            return difficultyData.optionsCount;
        }
    }

    DifficultyData difficultyData = new DifficultyData();

    void Awake()
    {
        // register for the DataLoaded event 
        DataController.DataLoaded.AddListener(GetQuestions);

        bestRound = PlayerPrefs.GetInt(bestRoundKey, 1);
    }

    void GetQuestions(QuestionsData[] jsonData)
    {
        questions = new List<QuestionsData>(jsonData);
        currentQuestions = new List<QuestionsData>(jsonData);
    }

    public void GetNextQuestion()
    {
        if (currentQuestions.Count == 0)
        {
            // .. Populate the questions list if it gets empty
            for (int i = 0; i < questions.Count; i++)
            {
                currentQuestions.Add(questions[i]);
            }
        }

        int random = Random.Range(0, currentQuestions.Count);
        _question = currentQuestions[random].Question;
        _answer = currentQuestions[random].Options[0];
        _options = currentQuestions[random].Options;

        // .. Remove the question from the current questions list
        currentQuestions.RemoveAt(random);
    }

    public void NextRound()
    {
        _round++;
        difficultyLevel += .5f;

        difficultyData.optionsCount = Mathf.Clamp(difficultyData.optionsCount + (int)difficultyLevel, 0, 4);
        difficultyData.timeLimit = Mathf.Clamp(difficultyData.timeLimit - (int)(difficultyLevel * 2f), 6, 10);

        if (_round > bestRound)
        {
            bestRound = _round;
            PlayerPrefs.SetInt(bestRoundKey, bestRound);
        }
    }

    void OnDestroy()
    {
        DataController.DataLoaded.RemoveListener(GetQuestions);
    }

    private class DifficultyData
    {
        public int timeLimit = 10;      // start game with time limit of 10 seconds
        public int optionsCount = 2;    // start game with two options
    }
}
