using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // Serialized fields allow you to set values in the Unity editor
    [SerializeField] private string[] dialogueLines; // Array of dialogue lines for this character
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource component for playing dialogue audio
    [SerializeField] private AudioClip[] dialogueClips; // Array of audio clips corresponding to each dialogue line
    [SerializeField] private TextMeshProUGUI dialogueText; // Reference to the TextMeshPro component for displaying dialogue text
    [SerializeField] private Animator animator; // Reference to the Animator component for character animations
    [SerializeField] private float textSpeed = 0f; // Speed at which the text is displayed, character by character

    private int currentLineIndex = 0; // Tracks the current dialogue line being displayed

    // Coroutine to control the character's dialogue and animations
    public IEnumerator Speak()
    {
        // Wait for 1 second before starting the dialogue (optional delay)
        yield return new WaitForSeconds(1f);

        // Clear the dialogue text to start displaying the new line
        dialogueText.text = string.Empty;

        // Play the corresponding audio clip for the current dialogue line
        audioSource.clip = dialogueClips[currentLineIndex];
        audioSource.Play();

        // Trigger different animations based on the current dialogue line
        if (currentLineIndex == 0)
        {
            animator.SetTrigger("Yell");
        }
        else if (currentLineIndex % 2 == 0)
        {
            animator.SetTrigger("Point");
        }
        else
        {
            animator.SetTrigger("ArmSpread");
        }

        // Display the dialogue text one character at a time
        foreach (char c in dialogueLines[currentLineIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Increment the line index and wrap around if necessary to prevent out-of-bounds errors
        currentLineIndex = (currentLineIndex + 1) % dialogueLines.Length;

        // Wait for 1 second after the dialogue line is displayed before continuing
        yield return new WaitForSeconds(1f);
    }
}





