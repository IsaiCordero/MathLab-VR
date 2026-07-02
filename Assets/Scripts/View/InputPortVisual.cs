using System.Collections;
using UnityEngine;

public class InputPortVisual : MonoBehaviour
{
    [Header("References")]
    public GameObject inputArrowObject;
    public Renderer inputArrowRenderer;
    public Renderer[] inputArrowRenderers;

    [Header("Colors")]
    public Color disconnectedColor = Color.white;
    public Color connectedColor = Color.green;
    public Color invalidColor = Color.red;

    [Header("Feedback")]
    public float invalidFeedbackTime = 0.5f;

    private Coroutine invalidRoutine;

    void Start()
    {
        SetConnected(false);
    }

    public void SetConnected(bool connected)
    {
        if (inputArrowObject != null)
        {
            inputArrowObject.SetActive(true);
        }

        PaintArrow(connected ? connectedColor : disconnectedColor);
    }

    public void SetInvalid()
    {
        if (invalidRoutine != null)
        {
            StopCoroutine(invalidRoutine);
        }

        invalidRoutine = StartCoroutine(InvalidFeedback());
    }

    IEnumerator InvalidFeedback()
    {
        if (inputArrowObject != null)
        {
            inputArrowObject.SetActive(true);
        }

        PaintArrow(invalidColor);

        yield return new WaitForSeconds(invalidFeedbackTime);

        SetConnected(false);
        invalidRoutine = null;
    }

    void PaintArrow(Color color)
    {
        PaintRenderer(inputArrowRenderer, color);

        if (inputArrowRenderers != null)
        {
            foreach (Renderer arrowRenderer in inputArrowRenderers)
            {
                PaintRenderer(arrowRenderer, color);
            }
        }

        if (inputArrowObject != null)
        {
            Renderer[] childRenderers = inputArrowObject.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer arrowRenderer in childRenderers)
            {
                PaintRenderer(arrowRenderer, color);
            }
        }
    }

    void PaintRenderer(Renderer arrowRenderer, Color color)
    {
        if (arrowRenderer == null) return;

        Material[] mats = arrowRenderer.materials;

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