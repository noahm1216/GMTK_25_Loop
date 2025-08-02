using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text narrativeText;
    public GameObject choicePanel;
    public Button[] choiceButtons;

    [Header("Current Scenario")]
    public string currentNarrative;
    public Choice[] currentChoices;

    [Header("Timeline Control")]
    public PlayableDirector playableDirector;

    void Start()
    {
        // Hide all choice buttons initially
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Start the first scenario
        StartScenario(currentNarrative, currentChoices);
    }

    public void StartScenario(string narrative, Choice[] choices)
    {
        currentNarrative = narrative;
        currentChoices = choices;

        // Update UI
        narrativeText.text = currentNarrative;

        // Set up choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i].text;

                // Clear previous listeners and add new one
                choiceButtons[i].onClick.RemoveAllListeners();
                int choiceIndex = i; // Important for closure
                choiceButtons[i].onClick.AddListener(() => SelectChoice(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= currentChoices.Length) return;

        // Play the corresponding timeline
        if (currentChoices[choiceIndex].timeline != null)
        {
            playableDirector.playableAsset = currentChoices[choiceIndex].timeline;
            playableDirector.Play();
        }

        // Invoke any additional events
        currentChoices[choiceIndex].onSelected.Invoke();

        // Hide choices while timeline plays
        choicePanel.SetActive(false);
    }

    // Call this from Timeline when the cinematic is over
    public void OnTimelineFinished()
    {
        // Show choices again (or load next scenario)
        choicePanel.SetActive(true);

        // For prototype: just reset to same choices
        StartScenario(currentNarrative, currentChoices);
    }
}