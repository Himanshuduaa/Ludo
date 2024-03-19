using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil 
{
    public static GameObject findGameObject(Transform parent, string name)
    {
        if (parent == null)
            return null;

        foreach (Transform t in parent)
        {
            Debug.Log("Traves="+t.name);
            if (t.name == name)
                return t.gameObject;
        }

        return null;
    }
}
