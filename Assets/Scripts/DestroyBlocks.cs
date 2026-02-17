using UnityEngine;

public class DestroyBlocks : MonoBehaviour
{
    public string tagBlock = "Block"; 

    private void OnTriggerEnter(Collider other)
    {
        GameObject deleteBlock = OriginalBlock(other.gameObject);

        if (deleteBlock != null)
        {
            CableMeta[] allCables = Object.FindObjectsOfType<CableMeta>();
            
            foreach (CableMeta cable in allCables)
            {
                if (cable.bloqueOrigenPadre == deleteBlock.transform)
                {
                    cable.ResetearPosicion();
                }

                if (cable.transform.IsChildOf(deleteBlock.transform))
                {
                    cable.ResetearPosicion();
                }
            }

            Destroy(deleteBlock);
        }
    }

    private GameObject OriginalBlock(GameObject obj)
    {
        if (obj.CompareTag(tagBlock)) return obj;

        Transform actual = obj.transform;
        while (actual.parent != null)
        {
            if (actual.parent.CompareTag(tagBlock)) return actual.parent.gameObject;
            actual = actual.parent;
        }
        
        CableMeta script = obj.GetComponentInParent<CableMeta>();
        if (script != null) return script.bloqueOrigenPadre.gameObject;

        return null;
    }
}