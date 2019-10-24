using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InspectorController : MonoBehaviour
{
    public CreatureController iC;

    private void Start()
    {
        StartCoroutine(updateLoop());
    }

    IEnumerator updateLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            updateText();
        }
    }

    private void Awake()
    {
        updateText();
        StartCoroutine(updateLoop());
        
    }


    public void updateText()
    {
        if (iC != null)
        {
            this.transform.Find("Portrait/Tenna").GetComponent<RawImage>().color = iC.sprite.GetComponent<SpriteRenderer>().color;
            this.GetComponent<Text>().text = "Name: " + iC.name + "\nEnergy: " + (int)iC.energy + "\nGeneration: " + iC.generation + "\nChildren: " + iC.childCount;
        }
    }

}
