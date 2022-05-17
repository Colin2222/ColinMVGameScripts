using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public GameDataSaved saved;
    public GameDataUnsaved current;

    [System.NonSerialized]
    public int numData;

    public float currentTime;
    public int currentDay;
    public float lengthOfDay;
    private Text clockText;
    private Text dayText;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        findClockGUI();
    }

    // Update is called once per frame
    void Update()
    {
        // update the current time and day, display in UI
        currentTime += Time.deltaTime;
        if(currentTime > lengthOfDay){
            currentTime = 0;
            currentDay += 1;
        }
        displayTime();
    }

    void SaveGame()
    {
        foreach(KeyValuePair<string,bool> data in current.gameData)
        {
            saved.gameData[data.Key] = data.Value;
        }
    }

    void LoadGame()
    {
        foreach(KeyValuePair<string,bool> data in saved.gameData)
        {
            current.gameData[data.Key] = data.Value;
        }
    }

    public bool GetData(string key)
    {
        return current.gameData[key];
    }

    // update the UI elements to show the current time
    void displayTime(){
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        dayText.text = string.Format("Day {0}", currentDay);
    }

    // finds the UI elements that the time values will be displaye on
    public void findClockGUI(){
        clockText = GameObject.FindWithTag("UIClockText").GetComponent<Text>();
        dayText = GameObject.FindWithTag("UIDayText").GetComponent<Text>();
    }
}
