using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple3: MonoBehaviour
{

    public static Grapple3 instance;
   
    LineRenderer lr;
    DistanceJoint2D dj;
    Rigidbody2D rb_m;
   
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float grappleSpeed;
    [SerializeField] float grappleShotSpeed = 20f;
    

    public bool isGrappling = false;
    public bool secondHalf = false;
    [HideInInspector] public bool retracting = false;

    private float horizontal;

    Vector2 target;
    Vector2 startPoint;
    Vector2 halfWay;
  

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();
        rb_m = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        instance = this;
    }





    private void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0) && !isGrappling)
        {
            startGrapple();
        }

        if (retracting)
        {
            while (Vector2.Distance(transform.position, target) > (Vector2.Distance(startPoint, target) / 2))
            {
                Vector2 grapplePos = Vector2.Lerp(transform.position, target, grappleSpeed * Time.deltaTime);


                transform.position = grapplePos;
                lr.SetPosition(0, transform.position);
            }

                secondHalf = true;
                retracting = false;
                dj.enabled = true;
            
            



        }

        if (secondHalf)
            {
             

                
                lr.SetPosition(0, transform.position);
                rb_m.gravityScale = 2f;

                if (dj.enabled == true)
                {
                   lr.SetPosition(1, target);
                  if (Input.GetKey(KeyCode.Space))
                  {
                    dj.enabled = false;
                    lr.enabled = false;
                    isGrappling = false;
                    secondHalf = false;
                    rb_m.gravityScale = 9.8f;
                    Vector2 swingLaunch = (transform.position).normalized;
                    Debug.Log(swingLaunch);
                    swingLaunch *= 10f;
                    Debug.Log(swingLaunch);
                    swingLaunch.x *= horizontal;
                    rb_m.AddForce(swingLaunch, ForceMode2D.Impulse);
                  }
                    

                }


            }
        


    }

    private void startGrapple()
    {
        
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grappleLayer);
       
        if (hit.collider != null)
        {
            isGrappling = true;
            target = hit.point;
            lr.enabled = true;
            lr.positionCount = 2;
            startPoint = transform.position;
            dj.connectedAnchor = hit.point;
            

            StartCoroutine(grapple());
        }

        IEnumerator grapple()
        {
            float t = 0;
            float time = 10f;

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position);

            Vector2 newPos;

            for (; t < time; t += grappleShotSpeed * Time.deltaTime)
            {
                newPos = Vector2.Lerp(transform.position, target, t / time);
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, newPos);
                yield return null;
            }

            lr.SetPosition(1, target);
            retracting = true;

        }
    
    }

   
       



}
