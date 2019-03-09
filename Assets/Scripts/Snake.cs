using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Snake : MonoBehaviour
{

    public float speed = 1f;
    public float normalSpeed;
    public Vector3 originalPos;

    [HideInInspector]
    public Vector3 nextMoveDirection;
    public Vector3 currentMoveDirection;
    public Vector3 previousMoveDirection;

    public GroundPiece groundPieceToReach;
    public GroundPiece lastReachedGroundPiece;

    BoxCollider2D boxCollider;

    public List<GroundPiece> ownedGroundPieces;


    public bool isCollectingNewGroundPieces;
    public List<GroundPiece> tailGroundPieces;

    public bool isBot;
    public bool isOncePlayer;
    public SnakeAI AI;

    public bool haveToDie;

    public SnakeMeshContainer snakeMeshContainer;

    public SnakeMeshProprietes snakeMeshProprietes;
    public Sprite tailPieceSprite;
    public Sprite collectedPieceSprite;
    public Color spriteColor;
    public SnakeNameTextMesh snakeNameTextMesh;
    public string name;

    public List<string> loadedPowers;
    public GameObject ShieldParticles, SpeedParticle, BlastersParticle, MissileParticle, FreezeParticle, RegenParticle, Explosion;
    public AudioClip FillSound;

    public Vector3 InputDirection;

    public int maxHP = 350, currentHP;
    public int movementDirection = 1;
    public int playerID, Lives;
    public PlayerInfo _networkSnake;
    public bool isLocal;
    public string ReasonDeath;

    public Camera AvatarCam;
    public RenderTexture Avatar1, Avatar2;
    public CameraShake.Properties DeathShakeProperties;

    public float energy;
    void Awake()
    {
        AI = GetComponent<SnakeAI>();
        boxCollider = GetComponent<BoxCollider2D>();
        snakeMeshContainer = GetComponentInChildren<SnakeMeshContainer>();
        ownedGroundPieces = new List<GroundPiece>();
        currentHP = maxHP;
    }

    void OnEnable()
    {
        SwipeHandler.OnDoubleTap += UsePower;
    }



    void OnDisable()
    {
        SwipeHandler.OnDoubleTap -= UsePower;
    }

    public void SetSpeed()
    {
        speed = normalSpeed;

        if (_networkSnake)
            _networkSnake.speed = speed;
    }

    public IEnumerator StartMove()
    {
        if (!isBot)
        {
            if (playerID == 1)
            {
                nextMoveDirection = transform.up;
            }
            else
            {
                nextMoveDirection = -transform.up;
            }
        }
        else
            nextMoveDirection = -transform.up;

        currentMoveDirection = nextMoveDirection;
        previousMoveDirection = currentMoveDirection;
        yield return snakeMeshContainer.DOSpawnAnimation();
        StartCoroutine(MoveToTurningPoint());
    }
    bool isMultiplayerMine;
    public void Initialize()
    {
        if (playerID == 1)
            AvatarCam.targetTexture = Avatar1;
        else
            AvatarCam.targetTexture = Avatar2;


        InstantReplayManager.instance.StartRecording(playerID - 1);
        snakeMeshProprietes = GetComponentInChildren<SnakeMeshProprietes>();
        snakeMeshProprietes.RandomizePattern();

        //collectedPieceSprite = snakeMeshProprietes.collectedPiece;
        tailPieceSprite = snakeMeshProprietes.tailPiece;
        snakeMeshProprietes.collectedPiece = collectedPieceSprite;
        //spriteColor = snakeMeshProprietes.snakeColor;
        snakeMeshProprietes.snakeColor = spriteColor;

        if (snakeMeshProprietes.Mesh.GetComponent<AvatarController>())
        {
            snakeMeshContainer.AnimController = snakeMeshProprietes.Mesh.GetComponent<AvatarController>();
            snakeMeshContainer.AnimController.Run();
        }

        SetName();
        ReasonDeath = "";
        currentHP = maxHP;
        if (PhotonNetwork.inRoom)
        {
            if (isLocal)
            {
                CameraHandler.instance.objectToFollow = this.gameObject;

                if (playerID == 1)
                {
                    CameraHandler.instance.SetDirection(0);
                    movementDirection = 1;
                }
                else
                {
                    CameraHandler.instance.SetDirection(180);
                    movementDirection = -1;
                }
            }
            else
            {
                if (playerID == 1)
                {
                    movementDirection = 1;
                }
                else
                {
                    movementDirection = -1;
                }
            }

        }
        else if (!isBot)
        {
            CameraHandler.instance.SetDirection(0);
            movementDirection = 1;
        }


        AvatarCam.transform.SetParent(snakeMeshContainer.transform);
    }


    float lerpTime = 0.2f;
    // Update is called once per frame
    void LateUpdate()
    {
        energy = Mathf.Clamp(energy, 0, 10);
        if (!isBot)
        {
            if (PhotonNetwork.inRoom)
            {
                if (isLocal)
                {
                    //NetworkMovement ();
                    Movement();
                    MoveToDirection(InputDirection);
                }
                else
                {
                    if (_networkSnake)
                    {
                        //Movement();
                      //  MoveToDirection(_networkSnake.MoveDirection);
                        transform.position = Vector3.Lerp(transform.position, _networkSnake.realPosition, 0.25f);
                      //  SyncTrail();
                     //   SyncFill();
                    }
                }
            }
            else
                Movement();
        }
    }
    void GetKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwipeHandler.instance.lastSwipeDirection = SwipeHandler.SwipeDirection.down;
            if (snakeMeshContainer.AnimController)
                snakeMeshContainer.AnimController.GoRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwipeHandler.instance.lastSwipeDirection = SwipeHandler.SwipeDirection.up;
            if (snakeMeshContainer.AnimController)
                snakeMeshContainer.AnimController.GoRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwipeHandler.instance.lastSwipeDirection = SwipeHandler.SwipeDirection.left;
            if (snakeMeshContainer.AnimController)
                snakeMeshContainer.AnimController.GoRight();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwipeHandler.instance.lastSwipeDirection = SwipeHandler.SwipeDirection.right;
            if (snakeMeshContainer.AnimController)
                snakeMeshContainer.AnimController.GoRight();
        }
    }
    bool isInputRecieved;
    void GetTouchInput()
    {
        isInputRecieved = true;
        if (SwipeHandler.instance.lastSwipeDirection == SwipeHandler.SwipeDirection.up)
        {
            //if (nextMoveDirection != -transform.up)
            //MoveToDirection (transform.up);
            if (InputDirection == transform.up)
            {
                isInputRecieved = false;
                return;
            }
            InputDirection = transform.up;
        }

        if (SwipeHandler.instance.lastSwipeDirection == SwipeHandler.SwipeDirection.down)
        {
            if (InputDirection == -transform.up)
            {
                isInputRecieved = false;
                return;
            }
            InputDirection = -transform.up;
        }

        if (SwipeHandler.instance.lastSwipeDirection == SwipeHandler.SwipeDirection.left)
        {
            if (InputDirection == -transform.right)
            {
                isInputRecieved = false;
                return;
            }
            InputDirection = -transform.right;
        }

        if (SwipeHandler.instance.lastSwipeDirection == SwipeHandler.SwipeDirection.right)
        {
            if (InputDirection == transform.right)
            {
                isInputRecieved = false;
                return;
            }
            InputDirection = transform.right;
        }
    }

    void Movement()
    {
        GetKeyboardInput();
        GetTouchInput();
        if (isInputRecieved)
        {
           // print("input");
            MoveToDirection(InputDirection);
        }
    }



    void NetworkMovement()
    {
        GetKeyboardInput();
        GetTouchInput();
        if (_networkSnake.isMine())
        {
            _networkSnake.realPosition = transform.position;
            _networkSnake.TrailList = ConvertPiecesToInt(tailGroundPieces);
            _networkSnake.OwnedList = ConvertPiecesToInt(ownedGroundPieces);

            if(lastReachedGroundPiece)
                _networkSnake.CurrentGridPosition = lastReachedGroundPiece.indexInGrid;
            if(groundPieceToReach)
                _networkSnake.TargetGridPosition = groundPieceToReach.indexInGrid;
        }

        if (isInputRecieved)
        {
            PhotonView.Get(_networkSnake.gameObject).RPC("SetMovementDirection", PhotonTargets.All, InputDirection);
            //if (!_networkSnake.isMine())
            //{
            //    transform.position = _networkSnake.realPosition;
            //    lastReachedGroundPiece = GroundSpawner.instance.spawnedGroundPieces[_networkSnake.CurrentGridPosition];
            //    groundPieceToReach = GroundSpawner.instance.spawnedGroundPieces[_networkSnake.TargetGridPosition];
           //     SyncTrail();
            //    SyncFill();
            //}
            //else
            //{
            //    nextMoveDirection = InputDirection * movementDirection;
            //}   
                                       
        }
    }

    

    GroundPiece lastSyncedPiece;
    public void MoveToDirection(Vector3 vector)
    {
        if (PhotonNetwork.inRoom)
        {
            if (isLocal)
            {
                _networkSnake.MoveDirection = vector;
                _networkSnake.realPosition = transform.position;
                //CheckBuggyFreeze ();
                //_networkSnake.CurrentGridPosition = groundPieceToReach.indexInGrid;
               // _networkSnake.UpdateGrid(ConvertPiecesToInt(tailGroundPieces), ConvertPiecesToInt(ownedGroundPieces));
                _networkSnake.TrailList = ConvertPiecesToInt(tailGroundPieces);
                _networkSnake.OwnedList = ConvertPiecesToInt(ownedGroundPieces);
            }
            else
            {
                currentMoveDirection = vector;
                //				if (GroundSpawner.instance.spawnedGroundPieces [_networkSnake.CurrentGridPosition] != lastSyncedPiece) {
                //					lastSyncedPiece = GroundSpawner.instance.spawnedGroundPieces [_networkSnake.CurrentGridPosition];
                //					CheckReachedGroundPiece (lastSyncedPiece);
                //
                //				}

                //SyncTrail ();
                //SyncFill ();
                //_networkSnake.MoveDirection = vector;
                //_networkSnake.realPosition = transform.position;
                //return;
            }
            //	_networkSnake.realRotation = snakeMeshContainer.transform.localRotation;
        }

        nextMoveDirection = vector * movementDirection;

    }

    Vector3 prevPosition;
    float freezeCounter;
    public bool isFrozen;
    public bool isGameStarted;
    void CheckBuggyFreeze()
    {
        if (transform.position == originalPos && isDead && !isGameStarted)
            return;

        if (transform.position == prevPosition)
        {
            freezeCounter++;
        }
        else
        {
            freezeCounter = 0;
            isFrozen = false;
            prevPosition = transform.position;
        }

        if (freezeCounter > 100)
        {
            freezeCounter = 0;
            isFrozen = true;
            SnakesSpawner.instance.RespawnSnake(this);
        }
    }


    public List<int> ConvertPiecesToInt(List<GroundPiece> GroundList)
    {
        List<int> GroundIntList = new List<int>();

        foreach (GroundPiece GP in GroundList)
        {
            GroundIntList.Add(GP.indexInGrid);
        }

        return GroundIntList;
    }

    public List<GroundPiece> ConvertIntToPieces(List<int> GroundIntList)
    {
        List<GroundPiece> GroundList = new List<GroundPiece>();
        foreach (int i in GroundIntList)
        {
            GroundList.Add(GroundSpawner.instance.spawnedGroundPieces[i]);
        }


        return GroundList;
    }
    int trailCount = 0;
    public void SyncTrail()
    {
        if (trailCount == _networkSnake.TrailList.Count)
            return;

        foreach (GroundPiece GP in tailGroundPieces)
        {
            GP.RemoveCollectingSnake(this);
        }

        tailGroundPieces = ConvertIntToPieces(_networkSnake.TrailList);

        foreach (GroundPiece GP in tailGroundPieces.ToList())
        {
            GP.SetCollectingNetworkSnake(this);
        }
        trailCount = _networkSnake.TrailList.Count;
    }
    int fillCount = 0;
    public void SyncFill()
    {
        if (fillCount == _networkSnake.OwnedList.Count)
            return;
        ownedGroundPieces = ConvertIntToPieces(_networkSnake.OwnedList);
        foreach (GroundPiece GP in ownedGroundPieces.ToList())
        {
            GP.SetNetworkSnakeOwner(this);

        }
        fillCount = _networkSnake.OwnedList.Count;
    }


    public void MoveToDirection(Vector3 vector, Vector3 pos)
    {
        nextMoveDirection = vector * movementDirection;
        transform.position = pos;


    }



    public IEnumerator MoveToTurningPoint()
    {
        while (true)
        {
            //			if (!lastReachedGroundPiece)
            //				SnakesSpawner.instance.FixSpawnBug (this);

            if (lastReachedGroundPiece.IsBoundPiece())
            {
                //currentHP = 0;
                KillSnake(this);
                ReasonDeath = name + " decided to hit the wall !!";
                if (!PlayerPrefs.HasKey("TutorialComplete") && !isBot)
                    GUIManager.instance.ShowTutorialLog(1, "Do not hit the wall !!", 3);
                else
                    GUIManager.instance.ShowLog(name + " decided to hit the wall !!", 3);
            }

            if (currentHP == 0)
            {
                KillSnake(this);
                ReasonDeath = name + " got hit pretty badly !!";
                GUIManager.instance.ShowLog(name + " got hit pretty badly !!", 3);
            }
            if (haveToDie&&!PhotonNetwork.inRoom)
            {
                print("kill by local");
                StartCoroutine(Die());
                break;
            }

            if (currentMoveDirection == -nextMoveDirection)
            {
                nextMoveDirection = currentMoveDirection;
                SwipeHandler.instance.isSwiped = false;
            }

            groundPieceToReach = GetNewGroundPieceToReach();
            previousMoveDirection = currentMoveDirection;
            currentMoveDirection = nextMoveDirection;
            Vector3 targetPos = groundPieceToReach.transform.position;
            targetPos.z = transform.position.z;
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            CheckReachedGroundPiece(groundPieceToReach);
            if (isBot)
            {
                AI.Notify(groundPieceToReach, isCollectingNewGroundPieces);
            }
            lastReachedGroundPiece = groundPieceToReach;
            yield return new WaitForEndOfFrame();
        }
    }

    public bool isFill;
    public void CheckReachedGroundPiece(GroundPiece pieceToCheck)
    {
        //		if (pieceToCheck == null)
        //			return;
        if (pieceToCheck.collectingSnake != null)
        {
            //KillSnake (pieceToCheck.collectingSnake);
            if (PhotonNetwork.inRoom && pieceToCheck.collectingSnake._networkSnake.isCollecting)
                KillSnake(pieceToCheck.collectingSnake);
            else
                KillSnake(pieceToCheck.collectingSnake);
        }
        if (pieceToCheck.snakeOwener != this)
        {
            if (isCollectingNewGroundPieces == false)
            {
                isCollectingNewGroundPieces = true;
                AI.Reset();
            }
            //if (PhotonNetwork.inRoom && isLocal)
            //{
            //    PhotonView.Get(_networkSnake.gameObject).RPC("MakeTrail", PhotonTargets.All, pieceToCheck.indexInGrid);
            //}
            //else
                pieceToCheck.SetCollectingSnake(this);
        }
        else
        {
            if (isCollectingNewGroundPieces)
            {
                //if (PhotonNetwork.inRoom && isLocal)
                //{
                //    PhotonView.Get(_networkSnake.gameObject).RPC("MakeFill", PhotonTargets.All);
                //    return;
                //}
                List<GroundPiece> newOwnedGroundPieces = new List<GroundPiece>();
                foreach (GroundPiece groundPiece in tailGroundPieces)
                {
                    newOwnedGroundPieces.Add(groundPiece);
                    groundPiece.SetSnakeOwner(this);
                    isFill = true;
                }
                GroundPiece[] groundPiecesToCheck = Poly.GetGroundPiecesToCheck(this);
                foreach (GroundPiece piece in groundPiecesToCheck)
                {
                    piece.tempHasToBeChecked = true;

                    if (piece.snakeOwener != this)
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
                        piece.SetSnakeOwner(this);
                        isFill = true;
                    }
                    piece.tempHasToBeChecked = false;
                }

                foreach (GroundPiece piece in newOwnedGroundPieces)
                {
                    piece.pieceWhenCollected.sr.color = spriteColor;
                    piece.ShowCollectedPiece(collectedPieceSprite);
                    energy += 0.025f;
                }

                if (!isBot)
                {
                    ScoreHandler.instance.SetScore(ownedGroundPieces.Count);
                }

                isCollectingNewGroundPieces = false;
                tailGroundPieces = new List<GroundPiece>();
            }
        }

        if (PhotonNetwork.inRoom)
            _networkSnake.isCollecting = isCollectingNewGroundPieces;


        if (isFill && !isBot)
        {
            isFill = false;
            SoundsManager.instance.Play(FillSound);
        }
    }


    public GroundPiece GetNewGroundPieceToReach()
    {
        GroundPiece piece = groundPieceToReach;
        //		try {
        if (nextMoveDirection == transform.up)
        {
            piece = lastReachedGroundPiece.column.groundPieces[lastReachedGroundPiece.indexInColumn - 1];
        }

        if (nextMoveDirection == -transform.up)
        {
            piece = lastReachedGroundPiece.column.groundPieces[lastReachedGroundPiece.indexInColumn + 1];
        }

        if (nextMoveDirection == transform.right)
        {
            piece = lastReachedGroundPiece.row.groundPieces[lastReachedGroundPiece.indexInRow + 1];
        }

        if (nextMoveDirection == -transform.right)
        {
            piece = lastReachedGroundPiece.row.groundPieces[lastReachedGroundPiece.indexInRow - 1];
        }

        return piece;

        //		} catch {
        //			return groundPieceToReach;
        //		}		
    }





    public void SetFirstOwnedGroundPieces(GroundPiece spawnPoint)
    {

        List<GroundPiece> newOwnedGroundPieces = new List<GroundPiece>();


        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList].groundPieces[spawnPoint.indexInColumn]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList].groundPieces[spawnPoint.indexInColumn - 1]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList].groundPieces[spawnPoint.indexInColumn + 1]);

        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList - 1].groundPieces[spawnPoint.indexInColumn]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList - 1].groundPieces[spawnPoint.indexInColumn - 1]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList - 1].groundPieces[spawnPoint.indexInColumn + 1]);

        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList + 1].groundPieces[spawnPoint.indexInColumn]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList + 1].groundPieces[spawnPoint.indexInColumn - 1]);
        newOwnedGroundPieces.Add(GroundSpawner.instance.columns[spawnPoint.column.indexInColumnsList + 1].groundPieces[spawnPoint.indexInColumn + 1]);


        foreach (GroundPiece piece in newOwnedGroundPieces)
        {
            piece.SetSnakeOwner(this);
        }

        foreach (GroundPiece piece in newOwnedGroundPieces)
        {
            piece.pieceWhenCollected.sr.color = spriteColor;
            piece.ShowCollectedPiece(collectedPieceSprite);
        }
    }

    bool isNetworkKill;
    bool isLog;
    public void KillSnake(Snake targetSnake)
    {
        if (targetSnake == this)
        {
            ReasonDeath = name + " got confused and hit his own trail !!";

            if (!PlayerPrefs.HasKey("TutorialComplete") && !isBot)
                GUIManager.instance.ShowTutorialLog(1, "Do not hit your own trail !!", 3);
            else
                GUIManager.instance.ShowLog(name + " got confused and hit his own trail !!", 3);


        }
        else
        {
            if (playerID == 1)
                AccountDetails.instance.Save(totalKills: 1);

            targetSnake.ReasonDeath = name + " ended " + targetSnake.name + "'s trail game !!";
            if (snakeMeshContainer.AnimController)
                snakeMeshContainer.AnimController.Kill();


            if (!PlayerPrefs.HasKey("TutorialComplete") && !isLog)
            {
                isLog = true;

                if (!isBot)
                    GUIManager.instance.ShowTutorialLog("Nicely done !!", 2);
                else
                    GUIManager.instance.ShowTutorialLog("Try completing shorter trails to play safe!!", 2);

                Invoke("EnableLog", 2);
            }
            else
            {
                if (!targetSnake.isShielded)
                    GUIManager.instance.ShowLog(name + " ended " + targetSnake.name + "'s trail game !!", 3);

            }
        }

        if (!isBot)
            isOncePlayer = true;

        if (targetSnake.isShielded)
        {

        }
        else
        {
            targetSnake.haveToDie = true;

            if (PhotonNetwork.inRoom && !isNetworkKill && isLocal)
            {
                PhotonView.Get(targetSnake._networkSnake.gameObject).RPC("KillPlayer", PhotonTargets.AllViaServer);
                PhotonNetwork.SendOutgoingCommands();
                targetSnake._networkSnake.shouldTransmit = false;
                isNetworkKill = true;
                Invoke("EnableNetworkKill", 2);
            }
        }

    }


    void EnableNetworkKill()
    {
        isNetworkKill = false;
    }

    void EnableLog()
    {
        isLog = false;
    }

    public void ForceDie()
    {
        isOncePlayer = !isBot;
        tailGroundPieces.Reverse();
        GroundPiece[] piecesToFade = tailGroundPieces.ToArray();

        foreach (GroundPiece piece in piecesToFade)
        {
            if (piece.collectingSnake == this)
            {
                piece.tailPiece.FadeImmediate();
            }
        }

        GroundPiece[] piecesToFade2 = ownedGroundPieces.ToArray();

        foreach (GroundPiece piece in piecesToFade2)
        {
            if (piece.snakeOwener == this)
            {
                piece.pieceWhenCollected.FadeImmediate();
            }
        }

        if (isOncePlayer)
            isBot = false;

        SnakesSpawner.instance.GetNotifiedSnakeDeath(this);
        DestroyImmediate(gameObject);
    }


    public void InstantHideTrail()
    {
        foreach (GroundPiece GP in tailGroundPieces)
        {
            if (GP.collectingSnake == this)
            {
                GP.tailPiece.FadeImmediate();
            }
            GP.RemoveCollectingSnake(this);
        }
        tailGroundPieces.Clear();

    }

    public bool isDead;

    public void EndRecording()
    {
        InstantReplayManager.instance.StopRecording(playerID - 1);
    }


    public IEnumerator Die()
    {

        //if (_networkSnake)
            //yield return new WaitForSeconds(0.5f);

        if (isDead)
            yield break;

        isDead = true;


        energy += 2;
        currentHP = 0;



        if(playerID==1)
            AccountDetails.instance.Save(totalDeaths: 1);

        isDead = true;

        Invoke("EndRecording", 0.7f);
        snakeMeshContainer.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.Unset);
        Instantiate(Explosion, transform.position, transform.rotation);
        FindObjectOfType<CameraShake>().StartShake(DeathShakeProperties);

        if (PhotonNetwork.inRoom)
        {
            //GameObject go = PhotonNetwork.Instantiate("Particles/" + Explosion.name, snakeMeshProprietes.Mesh.transform.position, transform.rotation, 0);
           // go.transform.SetParent(snakeMeshProprietes.Mesh.transform);

            if (isLocal)
            {
                Lives--;
                _networkSnake.Lives = Lives;
                _networkSnake.isDead = true;

            }
            if (Lives < 1)
            {


                NetworkGameOver();


                yield return StartCoroutine(FadeOutTailPieces());
                yield return StartCoroutine(FadeOutCollectedGroundPieces());

                foreach (GroundPiece piece in ownedGroundPieces)
                {
                    piece.RemoveSnakeOwner(this);
                }

                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }

                if (!ObliusGameManager.isFriendlyBattle)
                {
                    Destroy(_networkSnake.gameObject);
                    SnakesSpawner.instance.GetNotifiedNetworkDeath(this);
                    DestroyImmediate(gameObject);
                }
                yield break;
            }
            else
            {
                if (isLocal)
                {
                    _networkSnake.shouldTransmit = false;
                    _networkSnake.TrailList.Clear();
                }
                yield return StartCoroutine(FadeOutTailPieces());
                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }
                //tailGroundPieces.Clear ();
                PhotonView.Get(_networkSnake).RPC("ClearTrail", PhotonTargets.Others);
                //snakeMeshContainer.transform.DOScale (Vector3.one, 0.4f).SetEase (Ease.Unset);
                if (isLocal)
                {
                    _networkSnake.shouldTransmit = false;

                    SnakesSpawner.instance.RespawnNetworkSnake(this);

                }
                yield break;
            }








            //			yield return  StartCoroutine (FadeOutTailPieces ());	
            //			yield return StartCoroutine (FadeOutCollectedGroundPieces ());
            //
            //			foreach (GroundPiece piece in ownedGroundPieces) {
            //				piece.RemoveSnakeOwner (this);
            //			}
            //
            //			foreach (GroundPiece piece in tailGroundPieces) {
            //				piece.RemoveCollectingSnake (this);
            //			}
            //
            //
            //			if(SnakesSpawner.instance.playerSnake==this)
            //				GUIManager.instance.gameOverGUI.OnLose();
            //			else
            //				GUIManager.instance.gameOverGUI.OnWin ();
            //			PhotonView.Get (Server.instance.gameObject).RPC ("GameOver",PhotonTargets.AllViaServer);
            //			Destroy (_networkSnake.gameObject);
            //			PhotonNetwork.LeaveRoom ();
            //			SnakesSpawner.instance.GetNotifiedNetworkDeath (this);
            //			DestroyImmediate (gameObject);
            //
            //			yield break;
        }


        //Instantiate(Explosion, transform.position, transform.rotation);

        GUIManager.instance.gameOverGUI.Reason.text = ReasonDeath;

        if (!isBot)
        {
            isOncePlayer = true;
            Lives--;
            if (Lives < 1)
            {
                GUIManager.instance.gameOverGUI.OnLose();
                ObliusGameManager.instance.GameOver(0);
                yield return StartCoroutine(FadeOutTailPieces());
                yield return StartCoroutine(FadeOutCollectedGroundPieces());

                foreach (GroundPiece piece in ownedGroundPieces)
                {
                    piece.RemoveSnakeOwner(this);
                }

                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }

                SnakesSpawner.instance.GetNotifiedSnakeDeath(this);
                DestroyImmediate(gameObject);
                yield break;
            }
            else
            {
                yield return StartCoroutine(FadeOutTailPieces());
                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }
                //snakeMeshContainer.transform.DOScale (Vector3.one, 0.4f).SetEase (Ease.Unset);
                SnakesSpawner.instance.RespawnSnake(this);

                yield break;
            }

        }
        else
        {
            Lives--;
            if (Lives < 1)
            {
                GUIManager.instance.gameOverGUI.OnWin();
                ObliusGameManager.instance.GameOver(0);
                yield return StartCoroutine(FadeOutTailPieces());
                yield return StartCoroutine(FadeOutCollectedGroundPieces());

                foreach (GroundPiece piece in ownedGroundPieces)
                {
                    piece.RemoveSnakeOwner(this);
                }

                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }

                SnakesSpawner.instance.GetNotifiedSnakeDeath(this);
                DestroyImmediate(gameObject);
                yield break;
            }
            else
            {
                yield return StartCoroutine(FadeOutTailPieces());
                foreach (GroundPiece piece in tailGroundPieces)
                {
                    piece.RemoveCollectingSnake(this);
                }
                //snakeMeshContainer.transform.DOScale (Vector3.one, 0.4f).SetEase (Ease.Unset);
                SnakesSpawner.instance.RespawnSnake(this);
                yield break;
            }
        }

    }

    void NetworkGameOver()
    {
        print("gameover called");
        PhotonView.Get(Server.instance.gameObject).RPC("GameOver", PhotonTargets.AllViaServer, PhotonView.Get(_networkSnake.gameObject).viewID, ReasonDeath);
        PhotonNetwork.SendOutgoingCommands();
    }



    IEnumerator FadeOutTailPieces()
    {

        tailGroundPieces.Reverse();
        GroundPiece[] piecesToFade = tailGroundPieces.ToArray();

        foreach (GroundPiece piece in piecesToFade)
        {
            if (piece.collectingSnake == this)
            {
                yield return StartCoroutine(piece.tailPiece.FadeOut());
            }
        }

    }

    IEnumerator FadeOutCollectedGroundPieces()
    {
        GroundPiece[] piecesToFade = ownedGroundPieces.ToArray();

        foreach (GroundPiece piece in piecesToFade)
        {
            if (piece.snakeOwener == this)
            {
                StartCoroutine(piece.pieceWhenCollected.FadeOut());
            }
        }
        yield return new WaitForSeconds(0.3f);
    }
    public bool isHeadOn;
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (PhotonNetwork.inRoom)
        {
            if (!isLocal)
                return;
            else
                InGameGUI.instance.userSnake = this;
        }

        if (coll.tag == "Player")
        {
            isHeadOn = true;
            print("head2head");
        }

        if (coll.tag == "Diamond")
        {
            if (!isBot)
            {
                GUIManager.instance.inGameGUI.InstantiateTakeGUIDiamond(Camera.main.WorldToScreenPoint(coll.gameObject.transform.position));
                ScoreHandler.instance.increaseSpecialPoints(1);
            }
            Destroy(coll.gameObject);

        }

        if (loadedPowers.Count == 3)
            return;

        if (coll.tag == "Shield")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                ActivateShield();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

        if (coll.tag == "Health")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                UseHealth();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }



        if (coll.tag == "Speed")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                ActivateSpeed();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

        if (coll.tag == "Heatseeker")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                ActivateMissile();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

        if (coll.tag == "3Shots")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                ActivateBlasters();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

        if (coll.tag == "Freeze")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                FireFreeze();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

        if (coll.tag == "Mine")
        {
            if (!isBot)
            {
                loadedPowers.Add(coll.tag);
                InGameGUI.instance.EquipPowerup();
            }
            else
                DropMine();
            SoundsManager.instance.Play(FillSound);
            if (!PhotonNetwork.inRoom)
                PowerUpManager.instance.RemovePowerUp(coll.gameObject);
            else
                PowerUpManager.instance.RemovePowerUpNetwork(coll.gameObject);
        }

    }

    public void UsePower()
    {


        if (loadedPowers.Count < 1)
            return;

        switch (loadedPowers[0])
        {

            case "Speed":
                ActivateSpeed();
                break;

            case "Shield":
                ActivateShield();
                break;

            case "Health":
                UseHealth();
                break;

            case "Heatseeker":
                ActivateMissile();
                break;

            case "3Shots":
                ActivateBlasters();
                break;

            case "Freeze":
                FireFreeze();
                break;

            case "Mine":
                DropMine();
                break;


        }

        loadedPowers.RemoveAt(0);
        InGameGUI.instance.EquipPowerup();

    }

    public void SpawnDiamonds()
    {

        int numberOfDiamonds = Random.Range(2, 4);
        GameObject diamondPrefab = (GameObject)Resources.Load("Diamond");


        for (int i = 0; i <= numberOfDiamonds; i++)
        {

            GameObject diamond = (GameObject)Instantiate(diamondPrefab);
            diamond.transform.position = transform.position;
            diamond.transform.position += (Vector3)Random.insideUnitCircle;

        }
    }

    public void SetName()
    {
        name = GetName();
        snakeNameTextMesh = GetComponentInChildren<SnakeNameTextMesh>();
        snakeNameTextMesh.SetColor(snakeMeshProprietes.snakeColor);
        snakeNameTextMesh.SetText(name);
    }


    public string GetName()
    {


        if (isBot)
        {
            if (ObliusGameManager.BotType == 0)
                return NamesManager.instance.GetRandomName();
            else
                return NamesManager.instance.GetFakeName();
        }
        else
        {

            if (GUIManager.instance.mainMenuGUI.playerNameField.text != "")
            {
                return GUIManager.instance.mainMenuGUI.playerNameField.text;
            }
            else
            {
                return "guest" + Random.Range(0, 999999);
            }
        }

    }
    public bool isShielded, isSpeed;
    public void ActivateShield()
    {
        snakeMeshProprietes.Shield.SetActive(true);
        isShielded = true;
        Invoke("DeactivateShield", 6);
        if (PhotonNetwork.inRoom)
            StartCoroutine(ActivateNetworkShield());
    }

    public IEnumerator ActivateNetworkShield()
    {
        _networkSnake.isShielded = true;
        yield return new WaitForSeconds(6);
        _networkSnake.isShielded = false;
    }

    public void DeactivateShield()
    {
        snakeMeshProprietes.Shield.SetActive(false);
        isShielded = false;
    }

    public void ActivateSpeed()
    {
        GameObject go;
        if (PhotonNetwork.inRoom)
        {
            int val = 0;
            val = 100 + (AccountDetails.instance.playerData.speed * 50);
            go = PhotonNetwork.Instantiate("Particles/" + SpeedParticle.name, snakeMeshProprietes.Mesh.transform.position, transform.rotation, 0, new object[] { val });
            go.transform.SetParent(snakeMeshProprietes.Mesh.transform);
            speed = speed * 3.5f;
            Invoke("DeactivateSpeed", 3);
            _networkSnake.speed = speed;
        }
        else
        {
            go = Instantiate(SpeedParticle, snakeMeshProprietes.Mesh.transform);

            go.transform.localPosition = Vector3.zero;
            speed = speed * 3.5f;
            isSpeed = true;
            Invoke("DeactivateSpeed", 3);
        }
    }

    public void DeactivateSpeed()
    {
        speed = speed / 3.5f;
        isSpeed = false;

        if (PhotonNetwork.inRoom)
            _networkSnake.speed = speed;
    }

    public void ActivateMissile()
    {
        GameObject go;
        if (PhotonNetwork.inRoom)
        {
            int val = 0;
            val = (AccountDetails.instance.playerData.rocket);
            go = PhotonNetwork.Instantiate("Particles/" + MissileParticle.name, transform.position, transform.rotation, 0, new object[] { val });
        }
        else
        {
            go = Instantiate(MissileParticle);
            go.transform.position = transform.position;
            ProjectileScript projectile = go.GetComponent<ProjectileScript>();
            projectile.level = AccountDetails.instance.playerData.rocket;

            if (isBot)
                projectile.Launch(InGameGUI.instance.userSnake);
            else
                projectile.Launch(InGameGUI.instance.opponentSnake);
        }
    }

    public void ActivateBlasters()
    {
        FireBlaster();

        Invoke("FireBlaster", 1);
        Invoke("FireBlaster", 2);
    }

    public void FireBlaster()
    {
        GameObject go;
        if (PhotonNetwork.inRoom)
        {
            int val = 0;
            val = (AccountDetails.instance.playerData.minishots);
            go = PhotonNetwork.Instantiate("Particles/" + BlastersParticle.name, transform.position, transform.rotation, 0, new object[] { val });
        }
        else
        {
            go = Instantiate(BlastersParticle);

            go.transform.position = transform.position;
            ProjectileScript projectile = go.GetComponent<ProjectileScript>();
            projectile.level = AccountDetails.instance.playerData.minishots;

            if (isBot)
                projectile.Launch(InGameGUI.instance.userSnake);
            else
                projectile.Launch(InGameGUI.instance.opponentSnake);
        }
    }

    public void FireFreeze()
    {
        GameObject go;
        if (PhotonNetwork.inRoom)
        {
            int val = 0;
            val = (AccountDetails.instance.playerData.freeze);
            go = PhotonNetwork.Instantiate("Particles/" + FreezeParticle.name, transform.position, transform.rotation, 0, new object[] { val });
        }
        else
        {
            go = Instantiate(FreezeParticle);
            go.transform.position = transform.position;
            ProjectileScript projectile = go.GetComponent<ProjectileScript>();
            projectile.level = AccountDetails.instance.playerData.freeze;
            if (isBot)
                projectile.Launch(InGameGUI.instance.userSnake);
            else
                projectile.Launch(InGameGUI.instance.opponentSnake);

        }
    }


    public void TakeDamage(float damage)
    {
        if (isShielded)
            return;


        currentHP = Mathf.FloorToInt((float)currentHP - damage);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (PhotonNetwork.inRoom)
            _networkSnake.currentHP = currentHP;
    }

    public void UseHealth()
    {
        GameObject go;
        if (PhotonNetwork.inRoom)
        {
            int val = 0;
            val = 100 + (AccountDetails.instance.playerData.heal * 50);
            go = PhotonNetwork.Instantiate("Particles/" + RegenParticle.name, snakeMeshProprietes.Mesh.transform.position, transform.rotation, 0, new object[] { val });
            //		if(isLocal)
            go.transform.SetParent(snakeMeshProprietes.Mesh.transform);
            //		else
            //			go.transform
            currentHP = maxHP;
            _networkSnake.currentHP = currentHP;
            if (PhotonNetwork.inRoom)
                _networkSnake.currentHP = currentHP;
        }
        else
        {
            go = Instantiate(RegenParticle, snakeMeshProprietes.Mesh.transform);
            go.transform.localPosition = Vector3.zero;

            currentHP = maxHP;
        }
    }

    public void EnableFreezeHit()
    {
        if (isShielded)
            return;

        speed = speed / 3;
        Invoke("DisableFreezeHit", 3);
        if (PhotonNetwork.inRoom)
            _networkSnake.speed = speed;
    }

    public void DisableFreezeHit()
    {
        speed = speed * 3;

        if (PhotonNetwork.inRoom)
            _networkSnake.speed = speed;
    }

    public void DropMine()
    {

    }

}
