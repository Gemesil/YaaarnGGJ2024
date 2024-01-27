using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public string displayText;
    public bool isComplete;

    public Quest(string text)
    {
        displayText = text;
        isComplete = false;
    }
    
    // hard coded way to check if the quest was completed. not ideal, but its a game jam so no time for a dynamic solution
    public bool IsComplete()
    {
        // Implement the specific completion logic for each quest here
        if (displayText == "Press WASD to walk")
        {
            return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
        }
        else if (displayText == "Press Mouse1 to shoot")
        {
            return Input.GetMouseButtonDown(0);
        }
        // Add more cases for additional quests

        // If no specific logic, return false
        return false;
    }

    public void MarkAsComplete()
    {
        isComplete = true;
    }
}

public class TutorialManager : MonoBehaviour
{
    public Quest[] quests;
    private int _currentQuestIndex = 0;
    private Text _questText;

    void Start()
    {
        DisplayCurrentQuest();
    }

    void Update()
    {
        // Check for quest completion based on the current quest's logic
        if (IsQuestComplete())
        {
            MoveToNextQuest();
        }
    }

    void DisplayCurrentQuest()
    {
        if (_currentQuestIndex < quests.Length)
        {
            _questText.text = quests[_currentQuestIndex].displayText;
        }
        else
        {
            // All quests completed
            _questText.text = "Tutorial Completed!";
        }
    }

    bool IsQuestComplete()
    {
        // Check if the current quest is complete
        return _currentQuestIndex < quests.Length && quests[_currentQuestIndex].isComplete;
    }

    void MoveToNextQuest()
    {
        _currentQuestIndex++;

        // Check if all quests are completed
        if (_currentQuestIndex < quests.Length)
        {
            DisplayCurrentQuest();
        }
        else
        {
            // All quests completed, perform any additional actions
            // e.g., transition to the main game scene
            Debug.Log("Tutorial Completed!");
        }
    }
}