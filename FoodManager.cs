using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public int foodCount;
    public GameObject foodItem;
    void Start()
    {
        GameObject[] food = new GameObject[foodCount];
        for(int i = 0; i < foodCount; i++)
        {

            food[i] = Instantiate(foodItem, new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), this.transform.rotation);
            food[i].transform.parent = this.transform;
        }
    }

    private void Update()
    {
        if(this.transform.childCount < foodCount)
        {
            GameObject newFood = Instantiate(foodItem, new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), this.transform.rotation);
            newFood.transform.parent = this.transform;
        }
    }
}
