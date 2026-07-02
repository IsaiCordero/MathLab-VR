using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    private readonly List<GameObject> nodes = new List<GameObject>();

    public int NodeCount => nodes.Count;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterNode(GameObject node)
    {
        if (node == null) return;

        if (!nodes.Contains(node))
        {
            nodes.Add(node);
        }
    }

    public void UnregisterNode(GameObject node)
    {
        if (node == null) return;

        nodes.Remove(node);
    }

    public void DeleteAllNodes()
    {
        GameObject[] nodesToDelete = nodes.ToArray();

        foreach (GameObject node in nodesToDelete)
        {
            if (node != null)
            {
                Destroy(node);
            }
        }

        nodes.Clear();
    }
}