using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static int lastLevelPlayed=-1; 
static string[] levelNames= new string[]{"GameScene","Cat test"};


public void Play()
{
    int level =PlayerPrefs.GetInt("level",0);
    lastLevelPlayed = level;
    SceneManager.LoadScene(levelNames[level]);
}
public void PlayNextLevel()
{
    int level =1+lastLevelPlayed;
    if(level>=levelNames.Length)
    {
       SceneManager.LoadScene("MainMenu");
       return;
    }
    lastLevelPlayed = level;
    PlayerPrefs.SetInt("level",level);
    PlayerPrefs.Save();
    SceneManager.LoadScene(levelNames[level]);
}

public void PlayAgain()
{
    if(lastLevelPlayed==-1)
    {
        lastLevelPlayed=0;
    }
    SceneManager.LoadScene(levelNames[lastLevelPlayed]);
}

    public void PlayVictory()
    {
        SceneManager.LoadScene("Victory");
    }
}
