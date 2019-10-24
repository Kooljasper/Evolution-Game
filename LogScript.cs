using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScript : MonoBehaviour
{
    public GameObject logHolder;
    public GameObject EntryTemplate;
    public List<Transform> logEntry;
    public int entryCount;
    public float preScroll;
    Slider slider;

    void Start()
    {
        slider = this.transform.Find("Slider").GetComponent<Slider>();
        preScroll = slider.value;
        logHolder = this.transform.Find("LogHolder").gameObject;
    }


    public void Scroll()
    {

        logHolder.transform.position += new Vector3(0, slider.value - preScroll, 0) * 40;
        preScroll = slider.value;
    }

    public void constructLog(List<string> logs)
    {

        clearLog();
        foreach(string s in logs)
        {
            addToLog(s);
        }
        slider.value = 0;
        
    }

    void clearLog()
    {
        foreach (Transform e in logHolder.transform)
        {
            Destroy(e.gameObject);
        }

        entryCount = 0;
        slider.maxValue = 1;
    }

    public void addToLog(string s)
    {
        entryCount++;
        GameObject entry = Instantiate(EntryTemplate);
        entry.transform.SetParent(logHolder.transform);
        entry.transform.localScale = Vector3.one;
        entry.name = "Entry" + entryCount;
        if (entryCount % 2 == 0)
        {
            entry.GetComponent<RawImage>().color += new Color(0.05f, 0.05f, 0.05f);
        }
        entry.transform.GetChild(0).GetComponent<Text>().text = s;
        slider.maxValue++;

    }

    private void OnMouseOver()
    {
        slider.value += Input.mouseScrollDelta.y;
        Scroll();
    }

}
