using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensions
{
    //Breadth-first search
    public static Transform FindChildren(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindChildren(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    public static void SetLayer(this Transform root, int layer)
    {
        Stack<Transform> children = new Stack<Transform>();
        children.Push(root);
        while (children.Count > 0)
        {
            Transform currentTransform = children.Pop();
            currentTransform.gameObject.layer = layer;
            foreach (Transform child in currentTransform)
                children.Push(child);
        }
    }

    public static void DestroyChildren(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildrenImmediate(this Transform parent)
    {
        while (parent.childCount > 0)
        {
            Object.DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }
}
