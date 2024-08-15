using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationController : MonoBehaviour
{
    // References to the dialogue controllers for each character and narrator
    [SerializeField] private DialogueController remy;
    [SerializeField] private DialogueController kate;
    [SerializeField] private NarratorController narrator;

    // UI elements for displaying player choices
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private Button calmButton;
    [SerializeField] private Button neutralButton;
    [SerializeField] private Button escalationButton;
    [SerializeField] private TextMeshProUGUI calmButtonText;
    [SerializeField] private TextMeshProUGUI neutralButtonText;
    [SerializeField] private TextMeshProUGUI escalateButtonText;

    // Arrays holding the text options for each type of response
    [SerializeField] private string[] calmOptions;
    [SerializeField] private string[] neutralOptions;
    [SerializeField] private string[] escalationOptions;

    // Reference to other functional scripts
    [SerializeField] private PauseFunction pauseFunction;
    [SerializeField] private RecordResponse record;

    // Tracks the current round of conversation and the maximum number of rounds
    private int conversationRound = 0;
    private int maxRound = 5;

    // Tracks the player's selected response
    private int response = 0;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ConversationSequence());
    }

    // Manages the sequence of dialogue and player choices in the conversation
    private IEnumerator ConversationSequence()
    {
        // Initial round of dialogue: Remy, then Kate, then the narrator based on the previous response
        yield return StartCoroutine(remy.Speak());
        yield return StartCoroutine(kate.Speak());
        yield return StartCoroutine(narrator.Speak(response));

        // Prompt the player for a choice, then proceed to the next round
        yield return StartCoroutine(PromptPlayerChoice(conversationRound));
        conversationRound++;

        // Loop through the remaining rounds of conversation
        while (conversationRound < maxRound)
        {
            yield return StartCoroutine(narrator.Speak(response));
            yield return StartCoroutine(remy.Speak());
            yield return StartCoroutine(kate.Speak());
            yield return StartCoroutine(PromptPlayerChoice(conversationRound));

            conversationRound++;
        }

        // Final round of dialogue with the narrator based on the last response
        yield return StartCoroutine(narrator.Speak(response));

        // Determine the final outcome based on the player's choices
        StartCoroutine(record.DetermineFinalOutcome());
    }

    // Displays the player's choices and waits for them to make a selection
    private IEnumerator PromptPlayerChoice(int index)
    {
        // Pause animations or actions in the scene while the player makes a choice
        pauseFunction.PauseAnimation();
        promptPanel.SetActive(true);

        // Display the appropriate choice options for this round
        calmButtonText.text = calmOptions[index];
        neutralButtonText.text = neutralOptions[index];
        escalateButtonText.text = escalationOptions[index];

        // Add listeners to the buttons to handle player input
        bool choiceMade = false;
        calmButton.onClick.AddListener(() => OnButtonClick(0, ref choiceMade));
        neutralButton.onClick.AddListener(() => OnButtonClick(1, ref choiceMade));
        escalationButton.onClick.AddListener(() => OnButtonClick(2, ref choiceMade));

        // Wait until the player makes a choice
        yield return new WaitUntil(() => choiceMade);

        // Hide the prompt panel and remove listeners after a choice is made
        promptPanel.SetActive(false);
        calmButton.onClick.RemoveAllListeners();
        neutralButton.onClick.RemoveAllListeners();
        escalationButton.onClick.RemoveAllListeners();
    }

    // Handles the player's button click and records the response
    private void OnButtonClick(int selectedResponse, ref bool choiceMade)
    {
        response = selectedResponse; // Record the player's response
        choiceMade = true; // Mark that a choice has been made
    }
}
