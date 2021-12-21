using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScr : MonoBehaviour
{
    public float movementSpeed = 500f;
    public float colloteTime = 0.5f;
    public float jumpVelocity;
    public float distance;
    public bool onDie = false;
    public LayerMask groundLayer;
    public PhysicsMaterial2D friction;
    public PhysicsMaterial2D anyFriction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        onDie = false;
    }

    float daCollote = 0;
    // Update is called once per frame
    void Update()
    {
        //DEBUG SHIT HERE!!!
        //print((Input.GetAxisRaw("Horizontal") != 0) ? Mathf.Sign(Input.GetAxisRaw("Horizontal")) : 0);

        Movement();
    }

    bool justJump = false;

    void Movement()
    {
        if (onDie) return;
        //Player Movement
        rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed  * 10 * Time.fixedDeltaTime, 0),ForceMode2D.Force);

        bool onGround = Physics2D.Raycast(transform.position, Vector2.down, distance, groundLayer);
        Debug.DrawLine(transform.position, -transform.up * distance + transform.position, Color.red);
        //print(onGround);

        if (!onGround && !justJump)
            rb.AddForce(new Vector2(0, -50 * Time.deltaTime));

        //If is pressing the space button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            if (onGround && !justJump)
            {
                justJump = true;
                print(justJump);
                rb.AddForce(new Vector2(0,jumpVelocity * 1000 * Time.deltaTime),ForceMode2D.Impulse);
            }
            else
            {
                if (daCollote <= colloteTime)
                    if (rb.velocity.y < 0 && !justJump)
                    {
                        justJump = true;
                        print(justJump);
                        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity * 10 * Time.deltaTime);//Jump
                    }
            }
        }

        //print("just jump gey: " + justJump);

        //print("Collote fucking shit: " + daCollote);
        //print("dumbass ground: " + onGround);

        if (onGround)
        {
            justJump = false;
            daCollote = 0;
            rb.sharedMaterial = friction;
        }
        else
        {
            rb.sharedMaterial = anyFriction;
            daCollote += Time.deltaTime;
        }
    }
}