using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resizeToObject : MonoBehaviour
{
    public Transform obj;

    void Start()
    {
        // Place whatever object you want this object to be resized to in obj
        if (obj)
        {
            this.transform.localScale = obj.localScale;
        }

    }

}
