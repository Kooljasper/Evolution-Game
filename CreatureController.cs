using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureController : MonoBehaviour
{

    string[] parts = new string[] {"chad", "lew", "is", "si", "mon", "char", "lie", "ke", "vin", "ah", "doh", "gen", "ran", "ju", "faj", "eg", "rai", "lin", "durg", "hingen", "yote", "vahn", "cha", "bre", "den", "ty", "ler", "au", "ham", "chen", "lion", "le", "der", "de", "eh", "oh", "b", "ko", "toe", "ten", "ting", "cow" };

    [Header("Life")]
    public string surname = "";
    public float energy = 10;
    public bool lookingForMate = false;
    public int childCount = 0;
    public List<string> children;
    public List<string> parents;
    public int generation = 1;
    public bool colossal;

    [Header("Creature Stats")]
    public float jumpForce;
    public float jumpTime;
    public float turnSpeed;
    public float matingEnergy = 10;
    public float accuracy = 4;
    public float lifeSpan = 10;
    public float ageRate = 0.05f;
    public Vector3 skin;
    private float jumpEnergy;



    private bool randomGenes = true;

    public GameObject offspring;
    private float lifeSpanGene;
    private Color imgColor;

    Transform target;
    Vector3 dir;

    private float time;
    public GameObject sprite;

    private void Start()
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        sprite = this.transform.Find("Creature").gameObject;

        if (randomGenes)
        {
            surname = randomName(2);
            this.gameObject.name = surname;
            jumpForce = Random.Range(5f, 30f);
            jumpEnergy = 1 + jumpForce / 20;
            jumpTime = Random.Range(0.2f, 3f);
            turnSpeed = Random.Range(50f, 250f);
            energy = Random.Range(40, 50);
            accuracy = Random.Range(0.5f, 5f);
            sprite.transform.localScale *= Random.Range(1f, 5f);

            lifeSpan = Random.Range(25f, 40f);
            lifeSpanGene = lifeSpan;
            ageRate = Random.Range(0.3f, 2.4f);
            matingEnergy = Random.Range(10f, 30f);
            skin = new Vector3(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            sprite.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            if(sprite.GetComponent<SpriteRenderer>().color.r < 0.3f && sprite.GetComponent<SpriteRenderer>().color.g < 0.3f && sprite.GetComponent<SpriteRenderer>().color.b < 0.3f)
            {
                sprite.GetComponent<SpriteRenderer>().color += new Color(0.3f, 0.3f, 0.3f);
            }

            if (Random.Range(0, 50) > 48)
            {
                colossal = true;
                sprite.transform.localScale *= 5;
                jumpForce *= 6;
                rb.mass *= 4;
                rb.drag *= 2;
                energy *= 10;
                matingEnergy *= 10;
                jumpTime *= 5f;
                turnSpeed *= 0.6f;
                lifeSpan *= 3;
            }
        }
        jumpEnergy = 1 + jumpForce / 10;

        rb.drag = Random.Range(1f, 5f);
        rb.mass = sprite.transform.localScale.x * 0.7f;
    }

   


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= jumpTime)
        {
            Leap();
            time = 0;
        }


        if(energy > matingEnergy * 10)
        {
            lookingForMate = true;
            tag = "mating";
        }
        if (energy < matingEnergy * 9)
        {
            lookingForMate = false;
            tag = "creature";
        }

        if (FindTarget())
        {
           target = FindTarget().transform;
        }

        if (lookingForMate && FindMate())
        {
            target = FindMate().transform;
        }

        

        if (target != null)
        {
            rotate();
        }
    }

    void rotate()
    {
        dir = target.position - this.transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        float angleDelta = angle - this.transform.rotation.eulerAngles.z + 360;
        if (angleDelta >= 180)
        {
            angleDelta -= 360;
        }
        if (angleDelta > accuracy || angleDelta < -accuracy)
        {
            Vector3 targetRotation = new Vector3(0, 0, angleDelta);
            transform.Rotate(targetRotation, turnSpeed * Time.deltaTime);
            energy -= 0.5f * Time.deltaTime;
        }
    }

    void Leap()
    {
        if (energy > 0)
        {
            this.GetComponent<Animator>().Play("Leap Animation");
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * 100 * jumpForce);
            if (!colossal)
                energy -= jumpEnergy;
            else
                energy -= jumpEnergy / 4;
        } else
        {
            Die();
        }
    }



    GameObject FindTarget()
    {
        GameObject[] potFood = GameObject.FindGameObjectsWithTag("food");
        GameObject food = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject f in potFood)
        {
            float dist = Vector3.Distance(f.transform.position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
                food = f;
            }
        }
        return food;
    }

    GameObject FindMate()
    {
        GameObject[] potMate = GameObject.FindGameObjectsWithTag("mating");
        GameObject mate = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject f in potMate)
        {
            float dist = Vector3.Distance(f.transform.localPosition, currentPos);
            if (colossal == f.GetComponent<CreatureController>().colossal && dist < minDist && f != this.gameObject)
            {
                minDist = dist;
                mate = f;
            }
        }
        return mate;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            energy += lifeSpan * 3;
            lifeSpan -= ageRate;
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "mating" && lookingForMate)
        {
            if (colossal && collision.gameObject.GetComponent<CreatureController>().colossal)
            {
                lookingForMate = false;
                Mate(collision.gameObject);
            }
            if(!colossal && !collision.gameObject.GetComponent<CreatureController>().colossal)
            {
                lookingForMate = false;
                Mate(collision.gameObject);
            }
        }

    }

    void Mate(GameObject mate)
    {
        
        CreatureController mateStats = mate.GetComponent<CreatureController>();

        if (this.gameObject.name.GetHashCode() >= mate.name.GetHashCode())
        {
            mateStats.energy -= mateStats.matingEnergy * 5;
            energy -= matingEnergy * 5;
            GameObject child = Instantiate(offspring, this.transform.localPosition, this.transform.rotation);
            CreatureController childStats = child.GetComponent<CreatureController>();

            child.transform.parent = this.transform.parent;
            child.transform.Find("Creature").localScale = (sprite.transform.localScale + mateStats.sprite.transform.localScale) / 2;
            child.transform.Find("Creature").GetComponent<SpriteRenderer>().color = (sprite.GetComponent<SpriteRenderer>().color + sprite.GetComponent<SpriteRenderer>().color) / 2;

            if (Random.Range(1, 100) < 99)
            {
                childStats.randomGenes = false;
                childStats.jumpForce = (jumpForce + mateStats.jumpForce) / 2;
                childStats.jumpTime = (jumpTime + mateStats.jumpTime) / 2;
                childStats.turnSpeed = (turnSpeed + mateStats.turnSpeed) / 2;
                childStats.energy = matingEnergy + mateStats.matingEnergy;
                childStats.accuracy = (accuracy + mateStats.accuracy) / 2;
                childStats.lifeSpan = (lifeSpanGene + mateStats.lifeSpanGene) / 2;
                childStats.lifeSpanGene = childStats.lifeSpan;
                childStats.ageRate = (ageRate + mateStats.ageRate) / 2;
                childStats.generation = generation + 1;
                childStats.lookingForMate = false;
                childStats.childCount = 0;
                childStats.offspring = offspring;
                childStats.skin = (skin + mateStats.skin) / 2;
                childStats.matingEnergy = (matingEnergy + mateStats.matingEnergy) / 2;


                if (Random.Range(0, 100) > 98)
                {
                    childStats.colossal = true;
                }
                childStats.surname = surname;
                child.name = randomName(Random.Range(2, 4)) + " " + surname;

                
            }
            else
            {
                print("Mutation occured.");
                child.transform.localScale = Vector3.one;
            }
            children.Add(child.name);
            childStats.parents.Add(this.gameObject.name);
            childStats.parents.Add(mate.name);
            childCount++;
            mateStats.childCount++;
            //Debug.Log(this.gameObject.name + " has bred with " + mate.name);
            GameObject.Find("TextManager").GetComponent<TextManager>().crittersBorn++;
        }

    }

    private void OnMouseEnter()
    {
        Debug.Log("Highlighting");
        this.transform.Find("Creature_outline").GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseExit()
    {
        if(Camera.main.GetComponent<CameraController>().selectedCreature != this.gameObject)
            this.transform.Find("Creature_outline").GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnMouseDown()
    {
        Camera.main.GetComponent<CameraController>().SelectCreature(this.gameObject);

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = this.transform.position;
            Camera.main.transform.position = new Vector3(pos.x, pos.y, -10);
            Camera.main.transform.rotation = new Quaternion(0, 0, 0, 0);
            Camera.main.transform.parent = this.transform;
        }

        InspectorController inspect = transform.parent.parent.Find("Canvas/Inspection").GetComponent<InspectorController>();
        inspect.iC = this.transform.GetComponent<CreatureController>();

    }

    string randomName(int syllables)
    {
        string name = "";
        for(int i = 0; i < syllables; i++)
        {
            name += parts[Random.Range(0, parts.Length)];
        }

        return name;
    }
    void Die()
    {
        if(this.transform.childCount != 0)
        {
            Camera.main.transform.parent = null;
        }
        //print(gameObject.name + " Has lost the energy to keep trucking. They have died.");
        GameObject.Find("TextManager").GetComponent<TextManager>().crittersDied++;
        Destroy(this.gameObject);
    }
}
