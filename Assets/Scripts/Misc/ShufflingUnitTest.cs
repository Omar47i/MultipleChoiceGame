// Unit test the Knuth shuffling algorithms used for shuffling the questions to avoid redunadant experience
// Also tak into consideration that the 0 index is always the answer so test that we can extract the right answer
// after the shuffle completes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShufflingUnitTest : MonoBehaviour
{
    [Header("Test Values")]
    private List<int> twoQuestions;
    private List<int> threeQuestions;
    private List<int> fourQuestions;
    private List<List<int>> QuestionIDs;

    int answerID;
    public float delay;

    void Start()
    {
        // .. Initialize the test values
        twoQuestions = new List<int>(2) { 0, 1 };
        threeQuestions = new List<int>(3) { 0, 1, 2 };
        fourQuestions = new List<int>(4) { 0, 1, 2, 3 };

        QuestionIDs = new List<List<int>>();
        QuestionIDs.Add(twoQuestions);
        QuestionIDs.Add(threeQuestions);
        QuestionIDs.Add(fourQuestions);

        StartCoroutine(StartDelayed());
    }

    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < QuestionIDs.Count; i++)
        {
            // test shuffling for each questions list
            List<int> questionsList = QuestionIDs[i];

            KnuthShuffleIndices(ref questionsList);

            for (int j = 0; j < questionsList.Count; j++)
            {
                // Test that we got the asnwer index right after shuffling
                Debug.Assert(
                    questionsList[answerID] == 0,
                    "Answer ID must be zero as we always assume that the first option in the multiple selection is the right one");
            }
        }

        Debug.Log("Unit Test Completed!");
    }

    void KnuthShuffleIndices(ref List<int> indices)
    {
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
}
