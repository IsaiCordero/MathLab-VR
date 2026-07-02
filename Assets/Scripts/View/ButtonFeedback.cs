using System.Collections;
using UnityEngine;
using Oculus.Interaction;

public class ButtonFeedback : MonoBehaviour
{
    [Header("Interaction")]
    public Grabbable grabbableButton;

    [Header("Visual")]
    public Renderer[] renderersToFlash;
    public Color normalColor = new Color(0.17f, 0.18f, 0.21f);
    public Color pressedColor = Color.white;
    public float flashTime = 0.15f;

    [Header("Pulse")]
    public Transform objectToPulse;
    public float pulseScale = 1.08f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Vector3 originalScale;
    private Coroutine feedbackRoutine;

    void Start()
    {
        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised += OnPointerEvent;
        }

        if (objectToPulse == null)
        {
            objectToPulse = transform;
        }

        originalScale = objectToPulse.localScale;
        SetColor(normalColor);
    }

    void OnDestroy()
    {
        if (grabbableButton != null)
        {
            grabbableButton.WhenPointerEventRaised -= OnPointerEvent;
        }
    }

    void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            PlayFeedback();
        }
    }

    public void PlayFeedback()
    {
        if (feedbackRoutine != null)
        {
            StopCoroutine(feedbackRoutine);
        }

        feedbackRoutine = StartCoroutine(FeedbackRoutine());
    }

    IEnumerator FeedbackRoutine()
    {
        SetColor(pressedColor);

        if (objectToPulse != null)
        {
            objectToPulse.localScale = originalScale * pulseScale;
        }

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSeconds(flashTime);

        SetColor(normalColor);

        if (objectToPulse != null)
        {
            objectToPulse.localScale = originalScale;
        }

        feedbackRoutine = null;
    }

    void SetColor(Color color)
    {
        if (renderersToFlash == null) return;

        foreach (Renderer rend in renderersToFlash)
        {
            if (rend == null) continue;

            Material[] mats = rend.materials;

            foreach (Material mat in mats)
            {
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", color);
                }

                if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", color);
                }

                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.SetColor("_EmissionColor", color * 0.6f);
                }
            }
        }
    }
}