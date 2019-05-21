using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    straight,
    left,
    right,
    backwards
};

public enum Track
{
    a,
    b,
    c,
    d
}

public class PawnMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Track track;
    int destinationFieldId;
    bool canMove;
    bool destinationReached;
    bool moveToCenter;
    bool rotate;
    Vector3 fieldPosition;
    float targetRotation;
    float velocity = 300f;
    Direction direction = Direction.straight;
    float trackBonus;
    bool directionChanged;
    Vector3 moveRightPos;
    Vector3 moveLeftPos;
    Vector3 moveStraightPos;
    Vector3 moveBackwardsPos;

    public void AllowMovement(int id)
    {
        destinationFieldId = id;
        canMove = true;
        destinationReached = false;
        moveToCenter = false;
    }

    public bool IsDestinationReached()
    {
        return destinationReached;
    }

    public void SetDestinationReached(bool b)
    {
        destinationReached = b;
    }

    // Start is called before the first frame update
    void Start()
    {
        destinationReached = false;
        canMove = false;
        moveToCenter = false;
        rotate = false;
        targetRotation = 0f;
        trackBonus = 0f;
        directionChanged = false;

        //moveRightPos = GetComponent<Property>().id;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotate)
        {
            switch (track)
            {
                case Track.a:
                    trackBonus = -1.2f;
                    break;
                case Track.b:
                    trackBonus = -0.4f;
                    break;
                case Track.c:
                    trackBonus = 0.4f;
                    break;
                case Track.d:
                    trackBonus = 1.2f;
                    break;
            }

            Vector3 dest;
            if (!directionChanged)
            {
                switch (direction)
                {
                    case Direction.straight:
                        dest = transform.position;
                        dest.z = fieldPosition.z + trackBonus;
                        if (transform.position.z >= dest.z)
                        {
                            direction = Direction.right;
                            directionChanged = true;
                        }
                        break;
                    case Direction.backwards:
                        dest = transform.position;
                        dest.z = fieldPosition.z - trackBonus;
                        if (transform.position.z <= dest.z)
                        {
                            direction = Direction.left;
                            directionChanged = true;
                        }
                        break;
                    case Direction.right:
                        dest = transform.position;
                        dest.x = fieldPosition.x + trackBonus;
                        if (transform.position.x >= dest.x)
                        {
                            direction = Direction.backwards;
                            directionChanged = true;
                        }
                        break;
                    case Direction.left:
                        dest = transform.position;
                        dest.x = fieldPosition.x - trackBonus;
                        if (transform.position.x <= dest.x)
                        {
                            direction = Direction.straight;
                            directionChanged = true;
                        }
                        break;
                }
            }
        }
        if (moveToCenter)
        {
            Vector3 destination;
            switch (direction)
            {
                case Direction.straight:
                    destination = transform.position;
                    destination.z = fieldPosition.z + trackBonus;
                    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2);
                    break;
                case Direction.backwards:
                    destination = transform.position;
                    destination.z = fieldPosition.z - trackBonus;
                    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2);
                    break;
                case Direction.right:
                    destination = transform.position;
                    destination.x = fieldPosition.x + trackBonus;
                    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2);
                    break;
                case Direction.left:
                    destination = transform.position;
                    destination.x = fieldPosition.x - trackBonus;
                    transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2);
                    break;
            }

            trackBonus = 0f;
        }

        if (Move())
        {

            Quaternion target = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);

            Vector3 pos = transform.position;
            
                if (direction == Direction.right)
                {
                    Vector3 v = new Vector3();
                    v.Set(velocity * Time.deltaTime, 0, 0);
                    rb.velocity = v;
            }
                else if (direction == Direction.straight)
                {
                    Vector3 v = new Vector3();
                    v.Set(0, 0, velocity * Time.deltaTime);
                    rb.velocity = v;
                //v = transform.position;
                //v.z = fieldPosition.z + trackBonus + 10;
                //transform.position = Vector3.Lerp(transform.position, v, Time.deltaTime * 0.25f);
            }
                else if (direction == Direction.left)
                {
                    Vector3 v = new Vector3();
                    v.Set(-velocity * Time.deltaTime, 0, 0);
                    rb.velocity = v;
                }
                else if (direction == Direction.backwards)
                {
                    Vector3 v = new Vector3();
                    v.Set(0, 0, -velocity * Time.deltaTime);
                    rb.velocity = v;
                }

        }
        else
        {
            Vector3 v = new Vector3();
            v.Set(0, 0, 0);
            rb.velocity = v;
        }
    }

    bool Move()
    {
        if(destinationReached)
        {
            canMove = false;
            return false;
        }
        else if (canMove)
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (canMove && (col.GetComponentInParent<Property>().id == 1 || col.GetComponentInParent<Property>().id == 11 || col.GetComponentInParent<Property>().id == 21 || col.GetComponentInParent<Property>().id == 31))
        {
            rotate = false;
            directionChanged = false;
        }

        if (canMove && (col.GetComponentInParent<Property>().id == 0 || col.GetComponentInParent<Property>().id == 10 || col.GetComponentInParent<Property>().id == 20 || col.GetComponentInParent<Property>().id == 30))
        {
            rotate = true;
            targetRotation += 90f;
            fieldPosition = col.GetComponentInParent<Property>().transform.position;
        }

        if ( canMove && col.GetComponentInParent<Property>().id == destinationFieldId )
        {
            Debug.Log("xd");
            fieldPosition = col.GetComponentInParent<Property>().transform.position;
            destinationReached = true;
            moveToCenter = true;
        }
    }
}
