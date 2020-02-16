using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class PlayerCombat : MonoBehaviour
{

    public int health;
    PhotonView photonView;
    public Projectile bulletPrefab;
    static long idCount;
    [SerializeField]
    private float bulletSpawnOffset;
    public int roomNumber;


    private FixedJoystick fixedJoystick;
    public Projectile projectilePrefab;
    public float fireRate = 0.1f;
    Vector2 joysticDirection;
    bool canShoot;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        idCount = 0;
        if (int.TryParse(PhotonNetwork.NickName, out roomNumber))
        {
            print("Room number parsed " + roomNumber);
        }
        fixedJoystick = GameObject.Find("Right Joystick").GetComponent<FixedJoystick>();
        joysticDirection = Vector2.up;
        transform.right = joysticDirection;
        canShoot = true;

    }


    void Update()
    {

#if UNITY_ANDROID || UNITY_IPHONE

        joysticDirection = new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical);
                if (Mathf.Abs(joysticDirection.x) > 0.25f || Mathf.Abs(joysticDirection.y) > 0.25f)
                {
                    transform.right = joysticDirection;
                    if (canShoot)
                    {
                        canShoot = false;
                        Shoot();
                    }
                }

#elif UNITY_EDITOR || UNITY_STANDALONE


        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
        transform.right = directionToTarget;
        if (Input.GetButtonDown("Fire1"))
        {
            if (canShoot)
            {
                canShoot = false;
                Shoot();
            }
        }

#endif
    }


    void Shoot()
    {
        if (photonView.IsMine)
        {

            photonView.RPC("RPC_SpawnAndInitProjectile", RpcTarget.Others, new Vector2(transform.position.x + (transform.right.x * bulletSpawnOffset), transform.position.y + (transform.right.y * bulletSpawnOffset)), transform.rotation, idCount);
            Projectile bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + (transform.right.x * bulletSpawnOffset), transform.position.y + (transform.right.y * bulletSpawnOffset)), transform.rotation);
            bullet.light.color = Color.cyan;
            bullet.isMyProjectile = true;
            bullet.id = idCount;

            Destroy(bullet, 3);
            idCount++;

        }
        StartCoroutine("DelayShoot");
    }


    IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    [PunRPC]
    void RPC_SpawnAndInitProjectile(Vector2 origin, Quaternion quaternion, long id)
    {
        Projectile bullet = Instantiate(bulletPrefab, origin, quaternion);
        bullet.isMyProjectile = false;
        bullet.id = id;
    }

    public void TakeDamage(int damage)
    {
        if (UIManager.instance != null)
        {
            health -= damage;
            photonView.RPC("RPC_UpdateHealth", RpcTarget.All, health, roomNumber);
            print(roomNumber.ToString() + " is on " + health + " health");
            if (health <= 0)
            {
                GetComponent<AvatarSetup>()?.Die();

            }
        }        
    }

    [PunRPC]
    void RPC_UpdateHealth(int health, int playerNumber)
    {
        UIManager.instance.HealthUpdate(health, playerNumber);
    }

}
