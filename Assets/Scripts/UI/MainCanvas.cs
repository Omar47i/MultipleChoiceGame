using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    GameObject blockingImage;
    public Text roundText;
    public Text timeLimitText;
    public Text questionText;
    public Text scoreText;
    public Text winsText;
    public Text losesText;
    public Text bestScore;
    public Text bestRound;

    public List<Text> options;
    public List<GameObject> optionsButtons;

    bool countDown = false;
    bool waitForCelebration = false;
    float timeLimit;
    float celebrationTimer = 1.4f;
    int wins = 0;
    int loses = 0;
    int answerID;

    QuestionsManager questionManager;

    void Start()
    {
        blockingImage = transform.Find("BlockingImage").gameObject;
        questionManager = GetComponent<QuestionsManager>();

        // Update next question
        UpdateQuestion();

        // .. Set best score and round
        bestScore.text = ScoreManager.instance.bestScore.ToString();
        bestRound.text = questionManager.bestRound.ToString();
    }

    void UpdateQuestion()
    {
        // .. Populate the game UI with a question
        questionManager.GetNextQuestion();

        roundText.text = questionManager.round.ToString();
        timeLimitText.text = questionManager.timeLimit.ToString();
        questionText.text = questionManager.question;

        int i = 0;
        List<int> ranIndices = new List<int>(questionManager.optionsCount);

        KnuthShuffleIndices(ref ranIndices);

        for ( ; i < questionManager.optionsCount; i++)
        {
            options[i].text = questionManager.options[ranIndices[i]];
            optionsButtons[i].SetActive(true);
        }

        while (i < 4)
        {
            options[i].text = "";
            optionsButtons[i].SetActive(false);
            i++;
        }

        timeLimit = questionManager.timeLimit;
        countDown = true;

        winsText.text = wins.ToString();
        losesText.text = loses.ToString();
    }

    void KnuthShuffleIndices(ref List<int> indices)
    {
        // .. Initialize the indices list with index id
        for (int j = 0; j < indices.Capacity; j++)
        {
            indices.Add(j);
        }

        for (int i = 0; i < indices.Capacity - 1; i++)
        {
            int ran = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[ran];
            indices[ran] = temp;
        }

        for (int i = 0; i < indices.Count; i++)
        {
            answerID = (indices[i] == 0) ? i : answerID;
        }
    }

    void Update()
    {
        if (countDown)
        {
            timeLimit -= Time.deltaTime;
            timeLimitText.text = ((int)timeLimit).ToString();

            if (timeLimit < 0)
            {
                // timeout
                countDown = false;

                UpdateQuestion();
            }
        }

        if (waitForCelebration)
        {
            celebrationTimer -= Time.deltaTime;

            if (celebrationTimer <= 0)
            {
                waitForCelebration = false;
                celebrationTimer = 1.4f;

                ResetButtonsColors();
                UpdateQuestion();

                blockingImage.SetActive(false);
            }
        }
    }

    public void OnOptionClick(int id)   // 0: A, 1: B, 2: C, 3: D
    {
        countDown = false;

        if (id == answerID)
        {
            // play win effect
            waitForCelebration = true;
            blockingImage.SetActive(true);
            optionsButtons[id].GetComponent<Image>().color = Color.green;

            wins++;
            loses = 0;
            losesText.text = "0";
            CheckWinCondition(true);

            ScoreManager.instance.AddScore(10 * questionManager.round);
            scoreText.text = ScoreManager.instance.GetScore().ToString();
        }
        else
        {
            loses++;
            wins = 0;
            winsText.text = "0";

            CheckWinCondition(false);

            waitForCelebration = true;
            blockingImage.SetActive(true);

            optionsButtons[id].GetComponent<Image>().color = Color.red;
        }
    }

    void CheckWinCondition(bool checkWin)
    {
        if (checkWin)
        {
            if (wins >= 5)
            {
                wins = 0;
                loses = 0;
                questionManager.NextRound();
            }
        }
        else
        {
            if (loses >= 3)
            {
                // Restart round
                wins = 0;
                loses = 0;
            }
        }
    }

    void ResetButtonsColors()
    {
        for (int i = 0; i < optionsButtons.Count; i++)
        {
            optionsButtons[i].GetComponent<Image>().color = Color.white;
        }
    }
}
