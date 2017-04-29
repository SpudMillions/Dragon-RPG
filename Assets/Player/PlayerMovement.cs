using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    private bool isInDirectMode = false; //TODO consider making static?

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.G)) //G for gamepad
        {
            //toggle boolean on and off with G key
            isInDirectMode = !isInDirectMode;
            currentClickTarget = transform.position; // clear the click target
        }

        if(isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    //determine if layer is moveable, and move player
    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;
                case Layer.Enemy:
                    print("Not moving to enemy");
                    break;
                case Layer.RaycastEndStop:
                    break;
                default:
                    print("shouldnt be here playermovement.cs");
                    return;
            }
        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    //control player movement for gamepad
    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(m_Move, false, false);

    }
}

