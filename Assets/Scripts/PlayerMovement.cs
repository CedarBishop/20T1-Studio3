using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private FixedJoystick joystick;


    PhotonView photonView;
    Rigidbody2D rigidbody;

    Vector2 movementDirection;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
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
            movementDirection.y = joystick.Vertical;

#elif UNITY_EDITOR || UNITY_STANDALONE

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
#endif


        Vector2 movementVelocity = movementDirection.normalized * Time.deltaTime * movementSpeed;
        rigidbody.velocity = movementVelocity;
    }

}
