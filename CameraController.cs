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
    private void Start()
    {
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
            this.transform.parent = null;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.position += Vector3.up * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("s"))
        {
            this.transform.parent = null;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.position -= Vector3.up * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("a"))
        {
            this.transform.parent = null;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

            transform.position -= Vector3.right * Time.deltaTime * camSpeed * 10;
        }
        if (Input.GetKey("d"))
        {
            this.transform.parent = null;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(Time.timeScale != 8)
            {
                Time.timeScale = 8;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }


    public void SelectCreature(GameObject newCreature)
    {
        if (selectedCreature)
        {
            selectedCreature.transform.Find("Creature_outline").GetComponent<SpriteRenderer>().enabled = false;
        }
        selectedCreature = newCreature;
    }

}
