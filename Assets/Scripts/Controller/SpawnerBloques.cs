using UnityEngine;

public class SpawnerBloques : FixedGrabbableButton
{
    [Header("Spawn")]
    public GameObject functionBlockPrefab;
    public float spawnDistance = -0.25f;
    public int defaultFunctionIndex = 0;
    public Vector3 spawnRotationOffset = Vector3.zero;

    protected override void OnButtonPressed()
    {
        SpawnFunctionBlock();
    }

    private void SpawnFunctionBlock()
    {
        if (functionBlockPrefab == null) return;

        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

        Transform cameraTransform = Camera.main.transform;
        Vector3 directionToPlayer = cameraTransform.position - spawnPosition;
        directionToPlayer.y = 0f;

        Quaternion spawnRotation = Quaternion.LookRotation(directionToPlayer);
        spawnRotation *= Quaternion.Euler(spawnRotationOffset);

        GameObject newBlock = Instantiate(functionBlockPrefab, spawnPosition, spawnRotation);

        if (NodeManager.Instance != null)
        {
            NodeManager.Instance.RegisterNode(newBlock);
        }

        IFunctionBlock functionBlock = newBlock.GetComponent<IFunctionBlock>();

        if (functionBlock != null)
        {
            functionBlock.SetFunctionByIndex(defaultFunctionIndex);
        }
    }
}