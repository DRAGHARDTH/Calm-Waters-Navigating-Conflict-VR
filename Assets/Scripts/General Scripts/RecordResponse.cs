using System.Collections;
using UnityEngine;

public class RecordResponse : MonoBehaviour
{
    // Variables to keep track of the number of each type of response
    [SerializeField] private int calmCount = 0;
    [SerializeField] private int neutralCount = 0;
    [SerializeField] private int escalateCount = 0;

    // Reference to the final screen GameObject
    [SerializeField] private GameObject finalScreen;

    // Methods to increment the respective response counts
    public void Calm()
    {
        calmCount++;
    }

    public void Neutral()
    {
        neutralCount++;
    }

    public void Escalate()
    {
        escalateCount++;
    }

    // Coroutine to determine the final outcome based on the player's choices
    public IEnumerator DetermineFinalOutcome()
    {
        // Activate the final screen UI
        finalScreen.SetActive(true);

        int finalOutcome;

        // Determine the final outcome based on the highest count
        // If escalateCount is the highest, or it's equal to neutral or calm, escalate is chosen
        if (escalateCount >= neutralCount && escalateCount >= calmCount)
        {
            finalOutcome = 2; // Escalate outcome
        }
        // If neutralCount is the highest, and it's greater than calm, neutral is chosen
        else if (neutralCount >= calmCount && neutralCount > escalateCount)
        {
            finalOutcome = 1; // Neutral outcome
        }
        // If calmCount is the highest, or it's equal to neutral but higher than escalate, calm is chosen
        else
        {
            finalOutcome = 0; // Calm outcome
        }

        // Trigger the final narration or actions based on the outcome
        yield return StartCoroutine(finalScreen.GetComponent<NarratorController>().Speak(finalOutcome));

        // Wait for 3 seconds before closing the application
        yield return new WaitForSeconds(3f);

        // Log a message and quit the application
        Debug.Log("Application ends");
        Application.Quit();
    }
}
