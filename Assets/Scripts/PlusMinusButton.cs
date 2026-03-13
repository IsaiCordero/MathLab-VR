using UnityEngine;
using Oculus.Interaction;

public enum ButtonType { Plus, Minus }
public class PlusMinusButton : MonoBehaviour
{
    public NumberBlock numberBlock;
    public ButtonType type;

    private Grabbable grabbable;
    private Vector3 fixedPost;
    private Quaternion fixedLocalRot;
    void Start()
    {
        fixedLocalRot = transform.localRotation;
        fixedPost = transform.localPosition;
        grabbable = GetComponent<Grabbable>();
        if(grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandleEvent;
        }
    }

    void LateUpdate()
    {
        transform.localPosition = fixedPost;
        transform.localRotation = fixedLocalRot;
    } 

    private void HandleEvent(PointerEvent evt)
    {
        if(evt.Type == PointerEventType.Select)
        {
            if(type == ButtonType.Plus)
            {
                numberBlock.Add();
            }
            else
            {
                numberBlock.Subtract();
            }
        }
    }


}
