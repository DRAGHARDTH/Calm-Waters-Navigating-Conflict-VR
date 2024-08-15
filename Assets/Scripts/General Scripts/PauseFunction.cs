using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseFunction : MonoBehaviour
{
    // Array of Animator components to control the animations
    [SerializeField] private Animator[] animators;

    // Volume component for post-processing effects
    [SerializeField] private Volume volume;

    // AudioSource to control global audio playback
    [SerializeField] private AudioSource globalAudio;

    // Reference to the ColorAdjustments effect in the post-processing stack
    private ColorAdjustments colorAdjustments;

    // Variables to store the original and paused saturation values
    private float originalSaturation;
    private const float pausedSaturation = -100f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Try to retrieve the ColorAdjustments component from the Volume profile
        if (volume.profile.TryGet(out colorAdjustments))
        {
            // Store the original saturation value
            originalSaturation = colorAdjustments.saturation.value;
        }
        else
        {
            Debug.LogWarning("ColorAdjustments not found in the Volume profile.");
        }
    }

    // Pauses all animations and applies the grayscale effect
    public void PauseAnimation()
    {
        // Pause all animations by setting each Animator's speed to 0
        foreach (var animator in animators)
        {
            animator.speed = 0f;
        }

        // Apply the grayscale effect by setting saturation to -100
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = pausedSaturation;
        }

        // Pause global audio playback
        if (globalAudio != null)
        {
            globalAudio.Pause();
        }
    }

    // Resumes all animations and restores the original color settings
    public void ResumeAnimation()
    {
        // Resume all animations by setting each Animator's speed to 1
        foreach (var animator in animators)
        {
            animator.speed = 1f;
        }

        // Restore the original saturation value to remove the grayscale effect
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = originalSaturation;
        }

        // Resume global audio playback
        if (globalAudio != null)
        {
            globalAudio.UnPause();
        }
    }
}
