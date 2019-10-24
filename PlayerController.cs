using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0,100)]
    public float jumpForce;
    public float jumpTime = 1;

    public float energy = 100;

    [Range(0,100)]
    public float turnSpeed;

    public float matingEnergy = 10;

    [Range(0,100)]
    public float lifeSpan, ageRate;

    
    public Vector3 skin;

    private Rigidbody2D rb;
    private GameObject sprite;
    private bool canJump;
    public bool readyToMate;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        sprite = this.transform.Find("Creature").gameObject;

        canJump = true;
        jumpForce *= 0.3f;
        rb.mass = sprite.transform.localScale.x * 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canJump)
        {
            canJump = false;
            Leap();
            StartCoroutine(jumpCoolDown(jumpTime));
        }
        rotate();
    }

    IEnumerator jumpCoolDown(float time)
    {
        yield return new WaitForSeconds(time);

        canJump = true;
    }

    void Leap()
    {
        this.GetComponent<Animator>().Play("Leap Animation");
        rb = this.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 100 * jumpForce);
        energy -= 1;

    }

    void rotate()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        float angleDelta = angle - this.transform.rotation.eulerAngles.z + 360;
        if (angleDelta >= 180)
        {
            angleDelta -= 360;
        }
        if (angleDelta > 1 || angleDelta < -1)
        {
            Vector3 targetRotation = new Vector3(0, 0, angleDelta);
            transform.Rotate(targetRotation, turnSpeed * Time.deltaTime);
            energy -= 0.5f * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            energy += lifeSpan * 2;
            lifeSpan -= ageRate;
            Destroy(collision.gameObject);
        }

    }


}
