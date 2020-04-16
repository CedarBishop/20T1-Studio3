
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;


public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom instance;

    PhotonView photonView;

    public bool isGameLoaded;
    private int currentScene;
    private int lobbyScene = 2;
    private int gameScene;

    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    int maxPlayersInRoom = 2;
    public int playersInGame;
    public string roomNumberString;
    public string nickName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        InitNickname();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene != 0  && currentScene != 1 && currentScene != 5 && currentScene != 6)
        {
            CreatePlayer();
            SoundManager.instance.PlayMusic(false);
        }
        else
        {
            SoundManager.instance.PlayMusic(true);
        }

    }

    [PunRPC]
    void RPC_TellMasterToStartGame()
    {
        gameScene = Random.Range(3, 5);
        PhotonNetwork.LoadLevel(gameScene);
    }


    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(("PhotonPrefabs/PhotonNetworkPlayer") ,transform.position,Quaternion.identity, 0);
       // PhotonNetwork.Instantiate(("PhotonPrefabs/Skill Selection Holder"), transform.position, Quaternion.identity, 0);

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("Joined Room");
        if (PhotonNetwork.PlayerList.Length >= maxPlayersInRoom)
        {
            CreateRoom();
        }
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();



        StartGame();
    }


    void StartGame()
    {
        if (playersInRoom == maxPlayersInRoom)
        {
            photonView.RPC("RPC_TellMasterToStartGame", RpcTarget.MasterClient);
            return;
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }


        PhotonNetwork.LoadLevel(lobbyScene);


    }

    public void CreateRoom()
    {
        print("Trying to create a new room");
        string randomRoomName;
        if (string.IsNullOrEmpty(roomNumberString))
        {
            int randomNum = UnityEngine.Random.Range(0, 10000);
            randomRoomName = randomNum.ToString();
        }
        else
        {
            randomRoomName = roomNumberString;
        }

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2};
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        GameManager.instance.PlayerForfeited(int.Parse(PhotonNetwork.NickName));
        print(otherPlayer.NickName + " has left the game");
        playersInRoom--;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine("DisconnectAndLoad");
        }
    }

    public void EndMatch ()
    {
        photonView.RPC("RPC_EndMatch",RpcTarget.All);
    }

    [PunRPC]
    void RPC_EndMatch()
    {
        StartCoroutine("DisconnectAndLoad");
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }

    void InitNickname()
    {
        string str = PlayerPrefs.GetString("PlayerNickname","");
        if (!string.IsNullOrEmpty(str))
        {
            nickName = str;
        }
        else
        {
            nickName = "???";
        }
    }

}
