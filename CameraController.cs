using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float targetZ;
    private float curSize;
    public float maxZoom, minZoom;
    public float camSpeed;
    public GameObject selectedCreature;
    public bool trackingTenna;
    public LogScript log;

    private void Start()
    {
        log = GameObject.FindGameObjectWithTag("pond").transform.Find("UI/Inspector/Log/TennaLog").GetComponent<LogScript>();
        curSize = Camera.main.orthographicSize;
        targetZ = curSize;
    }

    void Update()
    {

        if(Input.mouseScrollDelta.y > 0 && targetZ > minZoom)
        {
            targetZ -= Input.mouseScrollDelta.y;
        } else if(Input.mouseScrollDelta.y < 0 && targetZ < maxZoom)
        {
            targetZ -= Input.mouseScrollDelta.y;
        }

        if(curSize != targetZ)
        {
            Camera.main.orthographicSize += (targetZ - curSize) * Time.deltaTime * camSpeed;
            curSize = Camera.main.orthographicSize;
        }

        if (Input.GetKey("w"))
        {
            trackingTenna = false;
            transform.position += Vector3.up * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("s"))
        {
            trackingTenna = false;
            transform.position -= Vector3.up * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("a"))
        {
            trackingTenna = false;
            transform.position -= Vector3.right * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("d"))
        {
            trackingTenna = false;
            transform.position += Vector3.right * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Time.timeScale = 2;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Time.timeScale = 1;
        }

        if(trackingTenna && selectedCreature != null)
        {
            Vector3 targetPos = this.transform.position - selectedCreature.transform.position;
            this.transform.position -= new Vector3(targetPos.x, targetPos.y, 0) * Time.deltaTime;
        }

    }


    public void SelectCreature(GameObject newCreature)
    {
        if (selectedCreature)
        {
            selectedCreature.transform.Find("Creature_outline").GetComponent<SpriteRenderer>().enabled = false;
        }
        selectedCreature = newCreature;
        trackingTenna = true;
        GameObject.FindGameObjectWithTag("pond").transform.Find("UI/Inspector/Tenna/Inspection").GetComponent<InspectorController>().updateText();
        log.constructLog(newCreature.GetComponent<CreatureController>().tennaLog);

    }

}
