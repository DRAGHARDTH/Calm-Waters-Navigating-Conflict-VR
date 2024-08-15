using System.Collections;
using UnityEngine;
using TMPro;

public class JoeDialogueControl : MonoBehaviour
{
    // Animator component to control animations
    [SerializeField] private Animator joeAnimator;

    // AudioSource component for playing dialogue audio
    [SerializeField] private AudioSource joeAudio;

    // AudioClip for Joe's dialogue
    [SerializeField] private AudioClip joeClip;

    // Delay before starting the dialogue sequence
    [SerializeField] private float delay = 1f;

    // TextMeshPro component to display dialogue text
    [SerializeField] private TextMeshProUGUI text;

    // Array of dialogue lines to display
    [SerializeField] private string[] lines;

    // Speed at which each character of the dialogue text is displayed
    [SerializeField] private float textSpeed = 0.05f;

    private void Start()
    {
        // Ensure Animator and AudioSource components are properly assigned
        joeAnimator = joeAnimator ?? GetComponent<Animator>();
        joeAudio = joeAudio ?? GetComponent<AudioSource>();

        // Clear any previous text in the TextMeshPro component
        text.text = string.Empty;

        // Start the dialogue sequence
        StartCoroutine(PlayDialogue());
    }

    // Coroutine to handle the dialogue sequence
    private IEnumerator PlayDialogue()
    {
        // Delay the start of the dialogue
        yield return new WaitForSeconds(delay);

        // Play the audio clip and display the dialogue text
        StartCoroutine(PlayAudio());
        StartCoroutine(TypeDialogue());

        // Trigger animations at specified times
        StartCoroutine(TriggerAnimations());
    }

    // Coroutine to play the audio clip with a delay
    private IEnumerator PlayAudio()
    {
        joeAudio.clip = joeClip;
        joeAudio.Play();
        yield return null;
    }

    // Coroutine to display each line of dialogue one character at a time
    private IEnumerator TypeDialogue()
    {
        for (int index = 0; index < lines.Length; index++)
        {
            foreach (char c in lines[index].ToCharArray())
            {
                text.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            if (index < lines.Length - 1)
            {
                // Clear text for the next line after a short pause
                yield return new WaitForSeconds(1f);
                text.text = string.Empty;
            }
            else { }

           
        }
    }

    // Coroutine to trigger specific animations at specified times
    private IEnumerator TriggerAnimations()
    {
        yield return new WaitForSeconds(2f);
        joeAnimator.SetTrigger("ArmSpread");

        yield return new WaitForSeconds(5f);
        joeAnimator.SetTrigger("Point");
    }
}
