using UnityEngine;

public class DestroyBlocks : MonoBehaviour
{
    public string tagDelBloque = "Block"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagDelBloque) || other.GetComponentInParent<CableMeta>() != null)
        {
            GameObject deleteObject = Creator(other.gameObject);
            
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