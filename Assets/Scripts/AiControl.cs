using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AiControl : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private float walkSpeed = 15f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    public Weapon inHands;

    public BasicKnife knife;
    private GameObject weaponSlot;
    private Vector2 weaponSlotLocation;
    private bool isAttacking = false;

    private GameObject player;
    private Transform playerPos;

    public bool botShouldAttack = true;
    public GameObject[] patrolPoints;
    private int currentWaypoint = 0;
    private int currentPatrolPoint;

    // a* specific code
    Seeker seeker;
    Path path;
    bool reachedEndOfPath;
    public float nextWaypointDistance = 3;

    CircleCollider2D ccSight;
    float fieldOfViewAngle = 90f;

    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathComplete;
        player = GameObject.FindGameObjectWithTag("Player");
        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
        
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        ccSight = gameObject.transform.Find("Vision").gameObject.GetComponent<CircleCollider2D>();

        knife = new BasicKnife("Advanced Dagger", 100, .6f, "dagger 2");
        inHands = knife;
        weaponSlot = transform.GetChild(0).gameObject;
        weaponSlotLocation = weaponSlot.transform.localPosition;
        weaponSlot.GetComponent<SpriteRenderer>().sprite = inHands.icon;
        weaponSlot.GetComponent<SpriteRenderer> ().sortingLayerName = "Weapons";
        Patrol();
    }

    void FixedUpdate()
    {
        Attack();
        if(path!=null){
            reachedEndOfPath = false;
            float distanceToWaypoint;
            float distanceToPatrolPoint;
            while(true){
                //  If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position);
                if (distanceToWaypoint < nextWaypointDistance) {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count) {
                        currentWaypoint++;
                    } else {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        break;
                    }
                }else {
                    break;
                }
            }
            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            velocity = dir * walkSpeed * speedFactor;
            
            // attempting rotation
            //Subtracting the position of the player from the mouse position
            Vector3 difference = path.vectorPath[currentWaypoint] - transform.position;
            difference.Normalize (); //Normalizing the vector. Meaning that all the sum of the vector will be equal to 1

            float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg; //Find the angle in degrees
            gameObject.transform.rotation = Quaternion.Euler (0f, 0f, rotZ);

            // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
            transform.position += velocity * Time.deltaTime;
        }
        if(reachedEndOfPath){
            currentPatrolPoint++;
            if(currentPatrolPoint == patrolPoints.Length){
                currentPatrolPoint = 0;
            }
            seeker.StartPath(transform.position, patrolPoints[currentPatrolPoint].transform.position);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(velocity, direction);
            // direction.Normalize();
            // float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if(angle < fieldOfViewAngle * .5f){
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, ccSight.radius, layerMask);
                if(hit.collider.gameObject.tag == "Player"){
                    if(Manager.weaponSystem.inHands.nameOf != "Fists"){
                        Debug.Log("Player is a threat");
                    }
                }
            }
        }
     }

    void Patrol(){
        // cycle between available patrol points
        currentWaypoint = 0;
        seeker.StartPath(transform.position, patrolPoints[currentPatrolPoint].transform.position);
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }else{
            Debug.LogError("Path not setup properly");
        }
    }

    void Attack()
    {

        //should the bot choose to attack
        if (botShouldAttack)
        {
            //this will disable bot consecutive attacks
            StartCoroutine(botWaitToAttack());

            // Debug.Log("Bot Attack");
            isAttacking = true;

            if (inHands.type == 0)
            {
                // Debug.Log("Bot Attacking with a stab weapon: " + inHands.nameOf);
                StartCoroutine(Stab());

            }
            else if (inHands.type == 1)
            {
                // Debug.Log("Bot Attacking with a shoot weapon: " + inHands.nameOf);
            }
        }
    }

    private IEnumerator botWaitToAttack()
    {
        botShouldAttack = false;
        yield return new WaitForSeconds(3.0f);
        botShouldAttack = true;
    }


    private IEnumerator Stab()
    {
        Vector2 stabLocation = Vector2.up * 200.0f;
        Vector2 startStabLocation = weaponSlotLocation;
        weaponSlot.transform.localPosition = Vector3.Slerp(startStabLocation, stabLocation, Time.deltaTime);
        yield return new WaitForSeconds(0.2f);
        weaponSlot.transform.localPosition = weaponSlotLocation;
        isAttacking = false;
    }

}
