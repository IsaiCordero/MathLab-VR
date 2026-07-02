using UnityEngine;

public class DeleteButton : FixedGrabbableButton
{
    [Header("References")]
    public CableMeta plugConnection;

    protected override void OnButtonPressed()
    {
        DeleteSecurity();
    }

    void DeleteSecurity()
    {
        if (plugConnection == null) return;

        Transform rootBlock = plugConnection.blockOriginal;

        CableMeta[] allCables = FindObjectsOfType<CableMeta>();

        foreach (CableMeta c in allCables)
        {
            bool cableBelongsToDeletedBlock = c.transform.IsChildOf(rootBlock);
            bool cableConnectedToDeletedBlock = c.DestinyPort != null && c.DestinyPort.IsChildOf(rootBlock);

            if (cableBelongsToDeletedBlock || cableConnectedToDeletedBlock)
            {
                c.ResetPosition();
            }
        }

        plugConnection.ResetPosition();

        if (NodeManager.Instance != null)
        {
            NodeManager.Instance.UnregisterNode(rootBlock.gameObject);
        }

        Destroy(rootBlock.gameObject);
    }
}