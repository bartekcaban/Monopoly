using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum cameraType
{
    mainView,
    circumnavigation,
    reachPawn,
    followingPawn,
    dice
}

public class CameraMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    float transitionSpeed;
    float diceTransitionSpeed;
    Vector3 initialPosition;
    Vector3 dicePosition;
    Vector3 pawnPosition;
    Vector3 centerPosition;
    cameraType type;
    
    public void SetPawnFollowing(Vector3 pos)
    {
        pawnPosition = pos;
        pawnPosition.x -= 6;
        pawnPosition.y += 6;
        type = cameraType.followingPawn;
    }

    public void SetPawnCamera(Vector3 pos)
    {
        pawnPosition = pos;
        pawnPosition.x -= 6;
        pawnPosition.y += 6;
        type = cameraType.reachPawn;
    }

    public void SetDiceCamera()
    {
        Dice d = (Dice) GameObject.Find("Dice").GetComponent(typeof( Dice ));
        dicePosition = d.transform.position;
        dicePosition.x -= 3;
        dicePosition.y += 5;
        dicePosition.z -= 4;
        type = cameraType.dice;
    }

    public void SetCircumnavigation()
    {
        type = cameraType.circumnavigation;
    }

    public void SetMainView()
    {
        type = cameraType.mainView;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        centerPosition = GameObject.Find("Plane").transform.position;
        initialPosition = transform.position;
        transitionSpeed = 4.0f;
        diceTransitionSpeed = 1.0f;
        type = cameraType.mainView;
    }

    // Update is called once per frame
    void Update()
    {
        switch(type)
        {
            case cameraType.mainView:
                transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * transitionSpeed);
                break;
            case cameraType.dice:
                transform.position = Vector3.Lerp(transform.position, dicePosition, Time.deltaTime * diceTransitionSpeed);
                Quaternion target = Quaternion.Euler(45, 45, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * transitionSpeed);
                break;
            case cameraType.circumnavigation:
                transform.RotateAround(centerPosition, Vector3.up, 45 * Time.deltaTime);
                break;
            case cameraType.followingPawn:
                transform.position = pawnPosition;
                break;
            case cameraType.reachPawn:
                transform.position = Vector3.Lerp(transform.position, pawnPosition, Time.deltaTime * transitionSpeed);
                Quaternion target2 = Quaternion.Euler(30, 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target2, Time.deltaTime * transitionSpeed);
                break;
        }
    }
}
