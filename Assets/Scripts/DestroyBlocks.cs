using UnityEngine;

public class DestroyBlocks : MonoBehaviour
{
    public string tagDelBloque = "Block"; 

    private void OnTriggerEnter(Collider other)
    {
        CableMeta cable = other.GetComponentInParent<CableMeta>();
        if(cable == null) cable = other.GetComponentInParent<CableMeta>();

        if (other.CompareTag(tagDelBloque) || cable != null)
        {
            GameObject deleteObject;

            if(cable != null)
            {
                deleteObject = cable.bloqueOrigenPadre.gameObject;
                cable.ResetearPosicion();
            }
            else
            {
                deleteObject = Creator(other.gameObject);
            }

            CableMeta[] allCables = FindObjectsOfType<CableMeta>();
            foreach(CableMeta c in allCables)
            {
                if (c.transform.IsChildOf(deleteObject.transform))
                {
                    c.ResetearPosicion();
                }
            }

            Destroy(deleteObject);
        }
    }

    private GameObject Creator(GameObject obj)
    {
        if (obj.transform.parent == null) return obj;
        
        if (obj.GetComponent<CableMeta>() != null) return obj;

        return Creator(obj.transform.parent.gameObject);
    }
}