using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    string questionsFileName = "questions.json";
    QuestionsData[] loadedData;

    public static DataLoadedEvent DataLoaded = new DataLoadedEvent();

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadQuestions();
    }

    private void LoadQuestions()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, questionsFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            loadedData = JsonHelper.getJsonArray<QuestionsData>(dataAsJson);

            DataLoaded.Invoke(loadedData);
        }
        else
        {
            Debug.LogError("Cannot load questions!");
        }
    }
}

public class DataLoadedEvent : UnityEvent<QuestionsData[]>
{
}

public class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
