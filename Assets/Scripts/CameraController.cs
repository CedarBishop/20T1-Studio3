using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private AvatarSetup avatar;
    private void Start()
    {
        avatar = GetComponentInParent<AvatarSetup>();
    }


    void Update()
    {
        transform.position = new Vector3(avatar.transform.position.x, avatar.transform.position.y, -10);
    }
}
