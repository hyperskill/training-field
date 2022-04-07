using System;
using UnityEngine;
using System.Collections.Generic;

public static class PMHelper
{
    public static GameObject Exist(String s)
    {
        return GameObject.Find(s);
    }
    public static T Exist<T>(GameObject g)
    {
        return g.GetComponent<T>();
    }
    public static bool Child(GameObject g, GameObject g2)
    {
        return g.transform.IsChildOf(g2.transform);
    }

    public static RaycastHit findHit(Vector3 origin, Vector3 direction, string layer)
    {
        RaycastHit[] hit = Physics.RaycastAll(origin, direction);
        return Array.Find(hit, h => LayerMask.LayerToName(h.collider.gameObject.layer).Equals(layer)); 
    }

    public static bool CheckRectTransform(RectTransform rect)
    {
        if (rect.anchorMax.x > 1 || rect.anchorMax.x < 0 ||
            rect.anchorMax.y > 1 || rect.anchorMax.y < 0 ||
            rect.anchorMin.x > 1 || rect.anchorMin.x < 0 ||
            rect.anchorMin.y > 1 || rect.anchorMin.y < 0)
        {
            return false;//Incorrect anchors
        }

        if (rect.anchorMin.x >= rect.anchorMax.x ||
            rect.anchorMin.y >= rect.anchorMax.y)
        {
            return false;//Incorrect anchors
        }

        if (rect.offsetMin != Vector2.zero || rect.offsetMax != Vector2.zero)
        {
            return false;//Might be troubles with changing resolution
        }
        
        return true;
    }

    public static GameObject[] FindObjectsWithLayer(string layer)
    {
        int layerInt = LayerMask.NameToLayer(layer);
        List<GameObject> objects = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == layerInt)
            {
                objects.Add(obj);
            }
        }

        return objects.ToArray();
    }
    public static GameObject FindObjectWithLayer(string layer)
    {
        int layerInt = LayerMask.NameToLayer(layer);
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == layerInt)
            {
                return obj;
            }
        }
        return null;
    }
}