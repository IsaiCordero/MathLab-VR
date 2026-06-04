using UnityEngine;
using Oculus.Interaction;

public class SpawnerBloques : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject functionBlockPrefab;
    public float spawnDistance = -0.25f;
    public int defaultFunctionIndex = 0;
    public Vector3 spawnRotationOffset = Vector3.zero;

    [Header("Interaction")]
    public Grabbable grabbableBoton;

    private Vector3 fixedLocalPosition;
    private Quaternion fixedLocalRotation;

    void Start()
    {
        fixedLocalPosition = transform.localPosition;
        fixedLocalRotation = transform.localRotation;

        if (grabbableBoton != null)
        {
            grabbableBoton.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    void LateUpdate()
    {
        transform.localPosition = fixedLocalPosition;
        transform.localRotation = fixedLocalRotation;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            SpawnFunctionBlock();
        }
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

        TwoInputFunction functionTwoInput = newBlock.GetComponent<TwoInputFunction>();
        if (functionTwoInput != null)
        {
            functionTwoInput.SetFunctionByIndex(defaultFunctionIndex);
            return;
        }

        FunctionOneInput oneInputFunction = newBlock.GetComponent<FunctionOneInput>();
        if(oneInputFunction != null)
        {
            oneInputFunction.SetFunctionByIndex(defaultFunctionIndex);
        }

        OneInputNumberFunction oneInputNumberFunction = newBlock.GetComponent<OneInputNumberFunction>();
        if (oneInputNumberFunction != null)
        {
            oneInputNumberFunction.SetFunctionByIndex(defaultFunctionIndex);
        }
    }

    private void OnDestroy()
    {
        if (grabbableBoton != null)
        {
            grabbableBoton.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }
}
