using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondManager : MonoBehaviour
{
    public int critterCount;
    public GameObject critter;
    void Start()
    {
        for(int i = 0; i < critterCount; i++)
        {
            GameObject newCritter = Instantiate(critter, new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), this.transform.rotation);
            newCritter.GetComponent<CreatureController>().offspring = critter;
            newCritter.transform.parent = this.transform.Find("CritterManager");
        }
    }


}
