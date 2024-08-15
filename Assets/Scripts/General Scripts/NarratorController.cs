using System.Collections;
using TMPro;
using UnityEngine;

public class NarratorController : MonoBehaviour
{
    // Serialized fields allow values to be set in the Unity editor
    [SerializeField] private string[] calmDialogueLines; // Array of dialogue lines for the calm scenario
    [SerializeField] private string[] neutralDialogueLines; // Array of dialogue lines for the neutral scenario
    [SerializeField] private string[] escalationDialogueLines; // Array of dialogue lines for the escalation scenario
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component for playing dialogue audio
    [SerializeField] private AudioClip[] calmDialogueClips; // Array of audio clips for the calm dialogue
    [SerializeField] private AudioClip[] neutralDialogueClips; // Array of audio clips for the neutral dialogue
    [SerializeField] private AudioClip[] escalationDialogueClips; // Array of audio clips for the escalation dialogue
    [SerializeField] private GameObject textBox; // Reference to the UI element for displaying dialogue text
    [SerializeField] private TextMeshProUGUI dialogueText; // Reference to the TextMeshPro component for displaying the dialogue
    [SerializeField] private float textSpeed = 0.05f; // Speed at which the text is displayed, character by character
    [SerializeField] private PauseFunction pauseFunction; // Reference to a script controlling the pausing of animations or actions

    private int currentLineIndex = 0; // Tracks the current dialogue line being displayed

    // Coroutine to control the narrator's speech, based on the index provided
    public IEnumerator Speak(int index)
    {
        // Pause animations or other actions in the scene when starting the dialogue
        if (currentLineIndex == 0)
        {
            pauseFunction.PauseAnimation();
        }

        // Clear the text box and display the dialogue box
        dialogueText.text = string.Empty;
        textBox.SetActive(true);

        // Select the appropriate dialogue lines and audio clips based on the provided index
        string[] selectedDialogueLines = null;
        AudioClip[] selectedDialogueClips = null;

        switch (index)
        {
            case 0: // Calm dialogue
                selectedDialogueLines = calmDialogueLines;
                selectedDialogueClips = calmDialogueClips;
                break;
            case 1: // Neutral dialogue
                selectedDialogueLines = neutralDialogueLines;
                selectedDialogueClips = neutralDialogueClips;
                break;
            case 2: // Escalation dialogue
                selectedDialogueLines = escalationDialogueLines;
                selectedDialogueClips = escalationDialogueClips;
                break;
            default:
                Debug.LogWarning("Invalid index passed to Speak function.");
                yield break;
        }


        // Play the corresponding audio clip
        audioSource.clip = selectedDialogueClips[currentLineIndex];
        audioSource.Play();

        // Display the dialogue text one character at a time
        foreach (char c in selectedDialogueLines[currentLineIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Increment the line index and wrap around if necessary to prevent out-of-bounds errors
        currentLineIndex = (currentLineIndex + 1) % selectedDialogueLines.Length;

        // Wait for a moment before closing the text box and resuming animations
        yield return new WaitForSeconds(2f);

        // Hide the text box and resume any paused animations or actions
        textBox.SetActive(false);
        pauseFunction.ResumeAnimation();
    }
}
