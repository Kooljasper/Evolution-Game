using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public string sexiestCritter;
    public int crittersDied = 0;
    public int crittersBorn = 0;
    Transform cm;
    List<int> genCount = new List<int>();

    private Text text;

    private void Start()
    {
        cm = GameObject.Find("CritterManager").transform;
        text = this.GetComponent<Text>();
        updateText();

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


    void updateText()
    {
        text.text = "General Stats: \nCritters: " + countCritters() + "\nSexiest Critter (most children): " + getSexyCritter() + "\nGenerations: " + getGenCount() + "\nCritters Died: " + crittersDied + "\nCritters Born: " + crittersBorn;
    }


    public int countCritters()
    {
        int critterCount = cm.childCount;
        return critterCount;
    }

    public string getSexyCritter()
    {
        string sexiestCritter = "No children yet.";
        int maxChild = 0;
        foreach (Transform f in cm)
        {
            if (f.GetComponent<CreatureController>())
            {
                int critterChildCount = f.GetComponent<CreatureController>().childCount;
                if (critterChildCount > maxChild)
                {
                    maxChild = critterChildCount;
                    sexiestCritter = f.name + ", with " + critterChildCount + " children";
                }
            }
        }

        return sexiestCritter;
    }

    public int getGenCount()
    {
        foreach (Transform c in cm)
        {
            if (c.GetComponent<CreatureController>())
            {
                int g = c.GetComponent<CreatureController>().generation;
                if (!genCount.Contains(g))
                {
                    genCount.Add(g);
                }
            }
        }

        int gens = genCount.Count;

        return gens;
    }
}
