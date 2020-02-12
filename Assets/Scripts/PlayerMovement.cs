using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private FixedJoystick joystick;
    private PlayerCombat playerCombat;
    private Platform platform;


    PhotonView photonView;
    Rigidbody2D rigidbody;

    Vector2 movementDirection;

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        platform = playerCombat.platform;
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
        switch (platform)
        {
            case Platform.PC:
                movementDirection.x = Input.GetAxis("Horizontal");
                movementDirection.y = Input.GetAxisRaw("Vertical");
                break;
            case Platform.Mobile:
                movementDirection.x = joystick.Horizontal;
                movementDirection.y = joystick.Vertical;
                break;
            default:
                break;
        }
       
        Vector2 movementVelocity = movementDirection.normalized * Time.deltaTime * movementSpeed;
        rigidbody.velocity = movementVelocity;
    }

}
