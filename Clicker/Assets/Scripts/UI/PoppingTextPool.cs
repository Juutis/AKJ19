using System.Collections.Generic;
using UnityEngine;

public class PoppingTextPool : MonoBehaviour
{
    [SerializeField]
    private UIPoppingText objectPrefab;
    [SerializeField]
    private Transform uiPoppingTextContainer;
    public int poolSize = 10;

    private List<UIPoppingText> objects;

    void Start() {
        InitializePool();
    }

    private void InitializePool()
    {
        objects = new List<UIPoppingText>();

        for (int i = 0; i < poolSize; i++)
        {
            UIPoppingText obj = Instantiate(objectPrefab, uiPoppingTextContainer);
            obj.Init(this);
            obj.gameObject.SetActive(false);
            objects.Add(obj);
        }
    }

    public UIPoppingText GetObject()
    {
        foreach (UIPoppingText obj in objects)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        UIPoppingText newObj = Instantiate(objectPrefab);
        objects.Add(newObj);
        return newObj;
    }

    public void ReturnObject(UIPoppingText obj)
    {
        obj.gameObject.SetActive(false);
    }
}
