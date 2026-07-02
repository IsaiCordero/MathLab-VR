using Oculus.Interaction;
using UnityEngine;

public class BlockInputModeController : MonoBehaviour
{
    [Header("Editable Objects")]
    public GameObject editButton;
    public Grabbable grabbableTarget;

    public void SetEditable(bool isEditable)
    {
        if (editButton != null)
        {
            editButton.SetActive(isEditable);
        }

        if (grabbableTarget != null)
        {
            grabbableTarget.enabled = isEditable;
        }
    }
}