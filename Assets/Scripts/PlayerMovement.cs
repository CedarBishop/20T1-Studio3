using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private FixedJoystick joystick;


    PhotonView photonView;
    Rigidbody rigidbody;

    Vector3 movementDirection;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        joystick = GameObject.Find("Left Joystick").GetComponent<FixedJoystick>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            BasicMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameSetup.instance.DisconnectPlayer();
        }
    }

    void BasicMovement()
    {

#if UNITY_IPHONE || UNITY_ANDROID

            movementDirection.x = joystick.Horizontal;
            movementDirection.z = joystick.Vertical;

#elif UNITY_EDITOR || UNITY_STANDALONE

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.z = Input.GetAxisRaw("Vertical");
#endif


        Vector3 movementVelocity = movementDirection.normalized * Time.deltaTime * movementSpeed;
        rigidbody.velocity = movementVelocity;
    }

}
