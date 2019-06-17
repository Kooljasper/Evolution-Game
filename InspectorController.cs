using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InspectorController : MonoBehaviour
{
    public CreatureController iC;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(iC != null)
        {
            this.GetComponent<Text>().text = "Creature: \nName: " + iC.name + "\nEnergy: " + iC.energy + "\nGeneration: " + iC.generation + "\nChildren: " + iC.childCount;
        }
    }
}
