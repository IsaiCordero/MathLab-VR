using UnityEngine;
using Oculus.Interaction;

public class SpawnerBloques : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject functionBlockPrefab;
    public float spawnDistance = -0.25f;
    public int defaultFunctionIndex = 0;

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
        GameObject newBlock = Instantiate(functionBlockPrefab, spawnPosition, Quaternion.identity);

        SelectFunction selectFunction = newBlock.GetComponent<SelectFunction>();
        if (selectFunction != null)
        {
            selectFunction.SetFunctionByIndex(defaultFunctionIndex);
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
