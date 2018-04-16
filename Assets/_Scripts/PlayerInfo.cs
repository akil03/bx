using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int PlayerNo, Lives, MaxHP, MeshType, TileType, ColorType;
    public Vector3 MoveDirection, realPosition;
    public Quaternion realRotation;
    public Snake Player, Opponent;
    public string PlayerName;
    public float gameTime;
    public bool ready;
    public Text PhotonTime;
    [SerializeField]
    public List<int> TrailList, OwnedList = new List<int>();
    public int CurrentGridPosition;
    public float GameStartTime;
    bool synced;
    public Color color;
    public bool isDead = false;
    public bool isShielded = false;
    public float speed;
    public int currentHP;
    public bool shouldTransmit;
    public bool isCollecting;
    public int isWantRematch;
    // Use this for initialization
    void Awake()
    {
        realPosition = Vector3.zero;
    }


    void Start()
    {
        gameTime = 0.0f;
    }

    public void AssignValues(int _playerNo, string _playerName, int _lives, int _maxhp, float _baseSpeed, int _meshtype, int _tiletype, int _colorType, Snake _player, float _starttime)
    {
        PlayerNo = _playerNo;
        PlayerName = _playerName;
        Lives = _lives;
        MaxHP = _maxhp;
        MeshType = _meshtype;
        TileType = _tiletype;
        ColorType = _colorType;
        Player = _player;
        GameStartTime = _starttime;
        currentHP = _maxhp;
        speed = _baseSpeed;
    }
    public string trailListJson = "";
    public string ownedListJson = "";
    public bool didDieRecently;
    bool isParticleExploded;
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            trailListJson = ConvertListToString(TrailList);
            ownedListJson = ConvertListToString(OwnedList);
            stream.Serialize(ref MoveDirection);
            stream.Serialize(ref realPosition);
            stream.Serialize(ref trailListJson);
            stream.Serialize(ref ownedListJson);
            stream.Serialize(ref isDead);
            stream.Serialize(ref Lives);
            stream.Serialize(ref isShielded);
            stream.Serialize(ref speed);
            stream.Serialize(ref currentHP);
            stream.Serialize(ref shouldTransmit);
            stream.Serialize(ref isCollecting);
            stream.Serialize(ref isWantRematch);

            //stream.Serialize (ref Player);

            if (!synced)
            {
                stream.Serialize(ref PlayerNo);
                stream.Serialize(ref PlayerName);
                stream.Serialize(ref MaxHP);
                stream.Serialize(ref MeshType);
                stream.Serialize(ref TileType);
                stream.Serialize(ref ColorType);
                stream.Serialize(ref GameStartTime);
            }
        }
        else
        {

            stream.Serialize(ref MoveDirection);
            stream.Serialize(ref realPosition);
            stream.Serialize(ref trailListJson);
            stream.Serialize(ref ownedListJson);
            stream.Serialize(ref isDead);
            stream.Serialize(ref Lives);
            stream.Serialize(ref isShielded);
            stream.Serialize(ref speed);
            stream.Serialize(ref currentHP);
            stream.Serialize(ref shouldTransmit);
            stream.Serialize(ref isCollecting);
            stream.Serialize(ref isWantRematch);

            //stream.Serialize (ref Player);

            if (Player && shouldTransmit)
            {
                if (PlayerNo == 2)
                    MoveDirection *= -1;

                if (didDieRecently)
                {
                    trailListJson = "";
                    TrailList.Clear();
                    //	PhotonView.Get (gameObject).RPC("ClearTrail",PhotonTargets.Others);
                    foreach (var g in GroundSpawner.instance.spawnedGroundPieces)
                    {
                        if (g.collectingSnake == Player)
                        {
                            g.RemoveCollectingSnake(Player);
                            g.tailPiece.FadeImmediate();
                        }
                    }
                    Player.transform.position = Player.originalPos;
                    Player.transform.localScale = Vector3.one;
                    Player.snakeMeshContainer.transform.localScale = Vector3.one;
                    didDieRecently = false;
                    isParticleExploded = false;
                }

                Player.currentMoveDirection = MoveDirection;
                Player.Lives = Lives;
                Player.isCollectingNewGroundPieces = isCollecting;

                TrailList = ConvertStringToIntList(trailListJson);
                OwnedList = ConvertStringToIntList(ownedListJson);

                Player.snakeMeshProprietes.Shield.SetActive(isShielded);
                Player.isShielded = isShielded;

                Player.Lives = Lives;
                Player.speed = speed;
                Player.currentHP = currentHP;
                Player.snakeMeshContainer.transform.localScale = Vector3.one;

                if (isDead)
                    StartCoroutine(Player.Die());
                //Player.InputDirection = MoveDirection;
                //Player.transform.position = realPosition;
            }

            if (!shouldTransmit)
            {
                if (!didDieRecently)
                {
                    didDieRecently = true;
                    PhotonNetwork.Instantiate("Particles/CharDie", Player.transform.position, Player.transform.rotation, new byte());
                    Player.snakeMeshContainer.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.Unset);
                    isParticleExploded = true;
                    Player.transform.localScale = Vector3.zero;
                    //					PhotonView.Get (gameObject).RPC("ClearTrail",PhotonTargets.All);
                }
            }

            //	Player.MoveToDirection (MoveDirection,position);
            //	stream.Serialize (ref gameTime);
            //	MoveToDirectionRPC(MoveDirection,position);

            if (!synced && !PhotonView.Get(gameObject).isMine)
            {
                stream.Serialize(ref PlayerNo);
                stream.Serialize(ref PlayerName);
                stream.Serialize(ref MaxHP);
                stream.Serialize(ref MeshType);
                stream.Serialize(ref TileType);
                stream.Serialize(ref ColorType);
                stream.Serialize(ref GameStartTime);
                //stream.Serialize (ref color);

                CreateNetworkSnake();
                synced = true;
            }
        }
    }


    public string ConvertListToString(List<int> list)
    {
        if (list.Count == 0)
            return "";


        string str = "";
        foreach (int i in list)
            str += i + ",";
        str = str.Remove(str.Length - 1, 1);
        return str;
    }


    public string ConvertListToString(List<string> list)
    {
        if (list.Count == 0)
            return "";


        string str = "";
        foreach (string i in list)
            str += i + ",";
        str = str.Remove(str.Length - 1, 1);
        return str;
    }


    public List<int> ConvertStringToIntList(string str)
    {
        List<int> result = new List<int>();
        if (str == "")
            return result;


        string[] strs = str.Split(',');
        foreach (var strr in strs)
            result.Add(int.Parse(strr));
        return result;
    }

    public List<string> ConvertStringToStringList(string str)
    {
        List<string> result = new List<string>();
        if (str == "")
            return result;


        string[] strs = str.Split(',');
        foreach (var strr in strs)
            result.Add(strr);
        return result;
    }

    //[PunRPC]
    public void MoveToDirectionRPC(Vector3 direction, Vector3 position)
    {
        if (Player)
        {
            Player.transform.position = position;
            Player.MoveToDirection(direction);
        }
    }


    [PunRPC]
    public void GameOver()
    {
        print("player info gameover rps nothing here!");
    }

    [PunRPC]
    public void KillPlayer()
    {

        StartCoroutine(Player.Die());

    }

    [PunRPC]
    public void MakeTrail(int index)
    {

        GroundSpawner.instance.spawnedGroundPieces[index].SetCollectingSnake(Player);

    }

    [PunRPC]
    public void MakeFill()
    {

        //GroundSpawner.instance.spawnedGroundPieces[index].SetSnakeOwner(Player);



        List<GroundPiece> newOwnedGroundPieces = new List<GroundPiece>();

        foreach (GroundPiece groundPiece in Player.tailGroundPieces)
        {
            newOwnedGroundPieces.Add(groundPiece);
            groundPiece.SetSnakeOwner(Player);
            Player.isFill = true;
        }

        GroundPiece[] groundPiecesToCheck = Poly.GetGroundPiecesToCheck(Player);

        foreach (GroundPiece piece in groundPiecesToCheck)
        {
            piece.tempHasToBeChecked = true;

            if (piece.snakeOwener != Player)
            {
                piece.ownerIDForCheck = 1;
            }
            else
            {
                piece.ownerIDForCheck = 0;
            }
        }

        Poly.FloodFill(groundPiecesToCheck[0], 1, 2);

        foreach (GroundPiece piece in groundPiecesToCheck)
        {

            if (piece.ownerIDForCheck == 1)
            {
                newOwnedGroundPieces.Add(piece);
                piece.SetSnakeOwner(Player);
                Player.isFill = true;
            }

            piece.tempHasToBeChecked = false;
        }

        foreach (GroundPiece piece in newOwnedGroundPieces)
        {
            piece.pieceWhenCollected.sr.color = Player.spriteColor;
            piece.ShowCollectedPiece(Player.collectedPieceSprite);
        }


        Player.isCollectingNewGroundPieces = false;
        Player.tailGroundPieces = new List<GroundPiece>();

    }



    [PunRPC]
    public void CheckKill()
    {
        if (Player.isCollectingNewGroundPieces)
            PhotonView.Get(gameObject).RPC("KillPlayer", PhotonTargets.AllViaServer);

    }





    //	[PunRPC]
    public void CreateNetworkSnake()
    {
        SnakesSpawner.instance.CreateSnakeFromInfo(PlayerNo, PlayerName, Lives, MaxHP, speed, MeshType, TileType, ColorType, false, this);
        ready = true;

        if (GameStartTime != 0)
        {
            StartCoroutine(SnakesSpawner.instance.StartMultiplayerGame(GameStartTime));
        }
    }

    [PunRPC]
    public void ClearTrail()
    {
        Player.InstantHideTrail();
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        print("Disconnected player id is " + otherPlayer.ID);
        print("Disconnected player is local or not?  " + otherPlayer.IsLocal);
        PhotonView.Get(Server.instance.gameObject).RPC("OnOpponentLeave", PhotonTargets.All);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        //		if (!hasFocus&&!Application.isEditor) {
        //			PhotonNetwork.LeaveRoom ();
        //			Application.LoadLevel (0);
        //		}
    }


    void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }

}
