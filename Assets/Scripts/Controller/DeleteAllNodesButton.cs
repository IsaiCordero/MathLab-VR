using UnityEngine;

public class DeleteAllNodesButton : FixedGrabbableButton
{
    protected override void OnButtonPressed()
    {
        if (NodeManager.Instance != null)
        {
            NodeManager.Instance.DeleteAllNodes();
        }
    }
}