using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

/// helper classes /// <summary>
/// https://github.com/tpai/unitygame-photon-demo/blob/master/Assets/Photon%20Unity%20Networking/UtilityScripts/InRoomRoundTimer.cs
/// </summary>
/// 
///         //photonView.isMine

public class MultiplayerManager : MonoBehaviour
{
    public string multiplayerVersion = "1";
    public GameModel GameModelScript;

    public Transform redCube;
    public Transform greenCube;
    public Transform blueCube;
    public Transform bowlingSpotCube;

    private PhotonView photonView;
    private string roomName = "Room02031";
    private string testString = "";
    private int modifyTossText = 0;
    private string bowlingSpeedText = "1";
    private string bowlingSwingSpinText = "0.0";

    private string ballAngleText = "";
    private string horizontalSpeedText = "";
    private string ballProjectileAngleText = "";
    private string ballProjectileHeightText = "";
    private string ballTimingFirstBounceDistanceText = "";
    private string ballProjectileAnglePerSecondText = "";
    private string slipShotText = "";
    private string ballToFineLegText = "";
    private string edgeCatchText = "";

    private string PhotonStatusString = "Please wait....";
    private bool connectedToGameRoom = false;

    private float aliveUpdatedTime = 0.0f;
    private float aliveUpdateInterval = 10.0f; // alive signal we be passed once in 10 seconds... // it can be changed based on requirement..


    void Awake()
    {
        photonView = PhotonView.Get(this);
        EnablePhotonConnection();
    }

    void Update()
    {
        UpdateAliveStatus();
    }

    private void UpdateAliveStatus()
    {
        //connectedToGameRoom = true;
        if ((aliveUpdatedTime + aliveUpdateInterval) < Time.time && connectedToGameRoom == true)
        {
            SendAlive();
            aliveUpdatedTime = Time.time;
        }
    }

    //Multiplayer-Shankar-V2
    public void UpdateScreenIndex(int _index)
    {
        //0 - NONE
        //1 - Batman Selection
        //2 - Bowler Selection
        //3 - Waiting for Bowling Input
        //4 - Target Screen
        if (PhotonNetwork.isMasterClient)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(RoomProperty.masterScreenIndex, _index);
            PhotonNetwork.room.SetCustomProperties(hash);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(RoomProperty.clientScreenIndex, _index);
            PhotonNetwork.room.SetCustomProperties(hash);
        }
    }

    public int GetMasterScreenIndex()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.masterScreenIndex];
    }

    public int GetClientScreenIndex()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.clientScreenIndex];
    }

    public void SetUmpireDecision(bool _bool, bool lbwDecision)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.umpireDecision, _bool);
        hash.Add(RoomProperty.isLBW, lbwDecision);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool GetUmpireDecision()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.umpireDecision];
    }
    //10Aug
    public bool GetLBW()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isLBW];
    }
    //10Aug
    public void SetBattingTeamDRSCount(int _battingTeamCount)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.battingTeamDRSCount, _battingTeamCount);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public int GetBattingTeamDRSCount()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.battingTeamDRSCount];
    }

    public void SetBowlingTeamDRSCount(int _bowlingTeamCount)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.bowlingTeamDRSCount, _bowlingTeamCount);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public int GetBowlingTeamDRSCount()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.bowlingTeamDRSCount];
    }

    public void SetReviewAskedStatus(int _status)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.reviewAskedStatus, _status);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public int GetReviewAskedStatus()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.reviewAskedStatus];
    }

    public void SetRunout(bool _isRunout)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.isRunOut, _isRunout);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool GetRunout()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isRunOut];
    }

    public void SetTightRunOut(bool tightCall, bool veryTightRunoutCall)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.isTightCall, tightCall);
        hash.Add(RoomProperty.isVeryTightCall, veryTightRunoutCall);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool GetTightCall()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isTightCall];
    }

    public bool GetVeryTightCall()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isVeryTightCall];
    }

    public void SetStumpOut(bool stumpOut)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.isStumped, stumpOut);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool GetStumpOut()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isStumped];
    }
    //Multiplayer-Shankar-V2

    private void CreateOrJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;

        // Time To Live...
        //roomOptions.PlayerTtl = CONTROLLER.TIME_TO_LIVE;
        //roomOptions.EmptyRoomTtl = CONTROLLER.TIME_TO_LIVE;
        // Time To Live...

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add(RoomProperty.battingTeamIndex, 0);
        roomOptions.CustomRoomProperties.Add(RoomProperty.bowlingTeamIndex, 1);
        roomOptions.CustomRoomProperties.Add(RoomProperty.tossWonBy, 1);
        roomOptions.CustomRoomProperties.Add(RoomProperty.masterScreenIndex, 0);//27July-Multiplayer
        roomOptions.CustomRoomProperties.Add(RoomProperty.clientScreenIndex, 0);//27July-Multiplayer

        // can be used for match making... // needs to be implemented later...
        roomOptions.CustomRoomPropertiesForLobby = new string[1] {
            RoomProperty.skillLevel
        };
        Debug.Log("Match making parameter count :: " + roomOptions.CustomRoomPropertiesForLobby.Length);
        // can be used for match making...

        PhotonStatusString = "Create / Join room ....";

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }
    /*
        private void ChangeRoomProperty (string armString)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
            hash.Add (RoomProperty.tossWinnerFirstBatting, armString);
            PhotonNetwork.room.SetCustomProperties (hash);
            Debug.Log ("Room Property changed TOSS *** ");
        }        
    */
    string modifyTossText1 = "";
    void OnGUI()
    {
        if (connectedToGameRoom == true)
        {
            GUI.TextField(new Rect(10, Screen.height - 100, 200, 25), "GetPing () : " + PhotonNetwork.GetPing());
            GUI.TextField(new Rect(10, Screen.height - 65, 200, 25), "RoundTripTime : " + PhotonNetwork.networkingPeer.RoundTripTime);
        }

        GUI.TextField(new Rect(10, Screen.height - 35, 300, 25), PhotonStatusString);
        //PhotonNetwork.connectionStateDetailed.ToString ();

        if (PhotonNetwork.connected == true && connectedToGameRoom == true)
        {
            // Player name - display...
            GUI.TextField(new Rect(Screen.width / 2.0f, 0, 100, 30), PhotonNetwork.playerName);

            // to modify the toss from Master device...
            modifyTossText1 = GUI.TextField(new Rect(10, 10, 100, 30), "" + modifyTossText);
            if (GUI.Button(new Rect(120, 10, 200, 30), "TossWinnerFirstBatting(1/0)"))
            {
                SetMatchTossWinnerFirstBattingStatus(modifyTossText);
            }

            bowlingSpeedText = GUI.TextField(new Rect(10, 60, 100, 30), bowlingSpeedText);
            if (GUI.Button(new Rect(120, 60, 200, 30), "BowlingSpeed(1 to 10)"))
            {
                SetBowlingSpeed(int.Parse(bowlingSpeedText), 0.61f);
            }

            bowlingSwingSpinText = GUI.TextField(new Rect(10, 110, 100, 30), bowlingSwingSpinText);
            if (GUI.Button(new Rect(120, 110, 200, 30), "BowlingSwingSpin(0/4) (-5/5)"))
            {
                SetBowlingSwingSpin(float.Parse(bowlingSwingSpinText));
            }

            if (GUI.Button(new Rect(120, 160, 200, 30), "UpdateBowlingSpot"))
            {
                SetBowlingSpotPosition(bowlingSpotCube.position);
            }


            ballAngleText = GUI.TextField(new Rect(10, 200, 100, 20), ballAngleText);
            horizontalSpeedText = GUI.TextField(new Rect(10, 225, 100, 20), horizontalSpeedText);
            ballProjectileAngleText = GUI.TextField(new Rect(10, 250, 100, 20), ballProjectileAngleText);
            ballProjectileHeightText = GUI.TextField(new Rect(10, 275, 100, 20), ballProjectileHeightText);

            ballTimingFirstBounceDistanceText = GUI.TextField(new Rect(10, 300, 100, 20), ballTimingFirstBounceDistanceText);
            ballProjectileAnglePerSecondText = GUI.TextField(new Rect(10, 325, 100, 20), ballProjectileAnglePerSecondText);
            slipShotText = GUI.TextField(new Rect(10, 350, 100, 20), slipShotText);
            ballToFineLegText = GUI.TextField(new Rect(10, 375, 100, 20), ballToFineLegText);
            edgeCatchText = GUI.TextField(new Rect(10, 400, 100, 20), edgeCatchText);


            /*
            if (GUI.Button(new Rect (0, 250, 150, 50), "Create/Join Room"))
            {
                CreateOrJoinRoom ();
            }
            
            if (GUI.Button(new Rect (0, 300, 100, 40), "Join Room"))
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            */
        }
    }


    public void UpdateBallParametersInGUI()
    {
        ballAngleText = "" + GetBallAngle();
        horizontalSpeedText = "" + GetHorizontalSpeed();
        ballProjectileAngleText = "" + GetBallProjectileAngle();
        ballProjectileHeightText = "" + GetBallProjectileHeight();

        ballTimingFirstBounceDistanceText = "" + GetBallTimingFirstBounceDistance();
        ballProjectileAnglePerSecondText = "" + GetBallProjectileAnglePerSecond();
        slipShotText = "" + GetSlipShot();
        ballToFineLegText = "" + GetBallToFineLeg();
        edgeCatchText = "" + GetEdgeCatch();
    }

    void EnablePhotonConnection()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        //PhotonNetwork.automaticallySyncScene = true;

        PhotonStatusString = "Connecting Master-server....";

        // the following line checks if this client was just created (and not yet online). if so, we connect
        Debug.Log("Enable connection status :: " + PhotonNetwork.connectionStateDetailed);
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            PhotonNetwork.gameVersion = PhotonNetwork.versionPUN + "_" + multiplayerVersion;
            Debug.Log("PhotonNetwork.gameVersion : " + PhotonNetwork.gameVersion);

            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.autoJoinLobby = false; // we join randomly always or by match making. No need to join a lobby to get the list of rooms.

            try
            {
                // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
                //PhotonNetwork.ConnectUsingSettings ("0.9");
                PhotonNetwork.ConnectUsingSettings(PhotonNetwork.gameVersion);
            }
            catch
            {
                Debug.Log("Couldn't connect to the server");
            }
        }

        PhotonNetwork.playerName = "Player" + Random.Range(1, 9999);
    }

    /*
    [PunRPC]
    void SpectatorLocationUpdate (Vector3 v1, PhotonMessageInfo info) // testing... method... 
    {
        Debug.Log ("SpectatorLocationUpdate : " + v1);
        blueCube.position = (Vector3) v1;
    }    
    */

    public void SendAlive()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        if (IsMasterClient())
        {
            hash.Add(RoomProperty.masterAlive, 1);
            Debug.Log("SendAlive :: Master");
        }
        else
        {
            hash.Add(RoomProperty.clientAlive, 1);
            Debug.Log("SendAlive :: Alive");
        }
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public void SendBowlerSide(string side)
    {
        //photonView.RPC("SetBowlerSide", PhotonTargets.All, side);
        photonView.RPC("SetBowlerSide", PhotonTargets.Others, side);
    }

    [PunRPC]
    void SetBowlerSide(string bowlerSide)
    {
        Debug.Log("SetBowlerSide : RPC :: " + bowlerSide);
        GameModelScript.GroundControllerScript.ISetBowlerSide(bowlerSide);
    }
    //Multiplayer-Shankar-V4
    public void SelectBatsman(int _index)
    {
        photonView.RPC("SelectBatsmanIndex", PhotonTargets.Others, _index);
    }

    [PunRPC]
    void SelectBatsmanIndex(int _batsmanIndex)
    {
        Debug.Log("SelectBatsmanIndex : RPC :: " + _batsmanIndex);
        GameModelScript.BatsmanSelectionScript.SelectBatsmanIndexToOthers(_batsmanIndex);
    }

    public void ContinueFromBatsmanSelectionScreen()
    {
        photonView.RPC("SkipThisScreen", PhotonTargets.Others);
    }

    [PunRPC]
    void SkipThisScreen()
    {
        GameModelScript.BatsmanSelectionScript.ContinueFromBatsmanSelectionScreen();
    }

    public void SelectBowler(int _index)
    {
        photonView.RPC("SelectBowlerIndex", PhotonTargets.Others, _index);
    }

    [PunRPC]
    void SelectBowlerIndex(int _bowlerIndex)
    {
        Debug.Log("SelectBowlerIndex : RPC :: " + _bowlerIndex);
        GameModelScript.BowlerSelectionScript.SetBowlerIndexToOthers(_bowlerIndex);
    }

    public void ContinueFromBowlerSelectionScreen()
    {
        photonView.RPC("SkipBowlerSelection", PhotonTargets.Others);
    }

    [PunRPC]
    void SkipBowlerSelection()
    {
        GameModelScript.BowlerSelectionScript.ContinueFromBowlerSelectionScreen();
    }
    //Multiplayer-Shankar-V4
    //Multiplayer-Shankar-V5
    public void ShowManualField()
    {
        photonView.RPC("ShowManualFieldToOpponent", PhotonTargets.Others);
    }

    [PunRPC]
    void ShowManualFieldToOpponent()
    {
        GameModelScript.PreviewScreenScript.ManualFieldPlacement();
    }

    public void HideManualField()
    {
        photonView.RPC("HideManualFieldForOpponent", PhotonTargets.Others);
    }

    [PunRPC]
    void HideManualFieldForOpponent()
    {
        if (ManualField.instance != null)
        {
            ManualField.instance.saveAndCloseManualField("save");
        }
    }

    public void ShowTossResultToAll()
    {
        photonView.RPC("ShowTossResult", PhotonTargets.All);
    }

    [PunRPC]
    void ShowTossResult()
    {
        GameModelScript.ShowTossResultToEveryOne();
    }

    //Multiplayer-Shankar-V5
    public void TossCoinStatus()
    {
        int random = Random.Range(0, 100);
        string coinStatus = string.Empty;

        if (random > 50)
        {
            coinStatus = "tail";
        }
        else
        {
            coinStatus = "heads";
        }
        Debug.Log("coinStatus :: " + coinStatus);
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.coinStatus, coinStatus);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: RoomProperty.coinStatus " + coinStatus);
    }

    public string GetTossCoinStatus()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.coinStatus];
    }

    public void SetUserTossCoinStatus(string userOption)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.tossAskedStatus, userOption);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetUserTossCoinStatus " + userOption);
    }

    public string GetTossCoinUserOptionStatus()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.tossAskedStatus];
    }

    public void SetMatchTossWonBy(string wonBy)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.matchTossWonBy, wonBy);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetTossWonBy " + wonBy);
    }

    public string GetMatchTossWonBy()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.matchTossWonBy];
    }

    public void SetMatchTossWinnerFirstBattingStatus(int batting)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.tossWinnerFirstBatting, batting);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetMatchTossWinnerFirstBatting " + batting);
    }

    public int GetMatchTossWinnerFirstBattingStatus()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.tossWinnerFirstBatting];
    }

    public void SetBowlingSpeed(int bowlingSpeed, float fillMeterValue)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.bowlingSpeed, bowlingSpeed);
        hash.Add(RoomProperty.fillMeterValue, fillMeterValue);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetBowlingSpeed " + bowlingSpeed);
    }

    public int GetBowlingSpeed()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.bowlingSpeed];
    }

    public float GetFillMeterValue()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.fillMeterValue];
    }

    public void SetBowlingSwingSpin(float bowlingSwingSpin)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.bowlingSwingSpin, bowlingSwingSpin);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetBowlingSwingSpin " + bowlingSwingSpin);
    }

    public float GetBowlingSwingSpin()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.bowlingSwingSpin];
    }

    public void SetBowlingSpotPosition(Vector3 bowlingSpotPosition)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.bowlingSpotPosition, bowlingSpotPosition);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetBowlingSpotPosition " + bowlingSpotPosition);
    }

    public Vector3 GetBowlingSpotPosition()
    {
        return (Vector3)PhotonNetwork.room.CustomProperties[RoomProperty.bowlingSpotPosition];
    }

    public void SetThrowParameters(int throwIndex)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.fielderThrowIndex, throwIndex);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetThrowParameters ");
    }

    public int GetThrowParameters()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.fielderThrowIndex];
    }

    // will be call from GroundController's BowlNextBall method....
    public void SetResetParameters(string ballStatus, int throwIndex, int isUnsuccessful, int misfield)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.ballStatus, ballStatus);
        hash.Add(RoomProperty.fielderThrowIndex, throwIndex);
        hash.Add(RoomProperty.isUnsuccessful, isUnsuccessful);
        hash.Add(RoomProperty.misfield, misfield);

        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetResetParameters");
    }

    public void SetMisfield(int misfield)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.misfield, misfield);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetMisfield");
    }


    // will be call from GroundController's BowlNextBall method....
    public void SetResetParameters(string ballStatus, int throwIndex)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.ballStatus, ballStatus);
        hash.Add(RoomProperty.fielderThrowIndex, throwIndex);
        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetResetParameters");
    }

    //Multiplayer-Shankar
    public void SetBatsmanPosition(Vector3 batsmanPosition)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.batsmanPosition, batsmanPosition);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public Vector3 GetBatsmanPosition()
    {
        return (Vector3)PhotonNetwork.room.CustomProperties[RoomProperty.batsmanPosition];
    }

    public void SetBatsmanAnimation(byte animIndex)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.batsmanAnimation, animIndex);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public byte GetBatsmanAnimation()
    {
        return (byte)PhotonNetwork.room.CustomProperties[RoomProperty.batsmanAnimation];
    }
    //Multiplayer-Shankar
    //Multiplayer-Shankar-V5
    public void SetMyFieldIndexToOpponent(int fielderChangeIndex)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.fieldIndex, fielderChangeIndex);
        PhotonNetwork.room.SetCustomProperties(hash);
    }
    public int GetOpponentFieldIndexToMe()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.fieldIndex];
    }

    public void SetBattingParameters(string ballStatus, Vector3 ballHitPosition, float ballAngle, float horizontalSpeed, float ballProjectileAngle, float ballProjectileHeight, float ballTimingFirstBounceDistance, float ballProjectileAnglePerSecond, bool slipShot, bool ballToFineLeg, bool edgeCatch)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

        Debug.Log("SetBattingParameters -> ballHitPosition : " + ballHitPosition);
        hash.Add(RoomProperty.ballStatus, ballStatus);
        hash.Add(RoomProperty.ballHitPosition, ballHitPosition);
        hash.Add(RoomProperty.battingBallAngle, ballAngle);
        hash.Add(RoomProperty.horizontalSpeed, horizontalSpeed);
        hash.Add(RoomProperty.ballProjectileAngle, ballProjectileAngle);
        hash.Add(RoomProperty.ballProjectileHeight, ballProjectileHeight);
        hash.Add(RoomProperty.ballTimingFirstBounceDistance, ballTimingFirstBounceDistance);
        hash.Add(RoomProperty.ballProjectileAnglePerSecond, ballProjectileAnglePerSecond);
        hash.Add(RoomProperty.slipShot, slipShot);
        hash.Add(RoomProperty.ballToFineLeg, ballToFineLeg);
        hash.Add(RoomProperty.edgeCatch, edgeCatch);

        PhotonNetwork.room.SetCustomProperties(hash);
        Debug.Log("Room Property changed :: SetBattingParameters -- DONE ");
    }

    public void SetBallStatus(string ballStatus)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.ballStatus, ballStatus);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public void SetFieldingAttemptParameters(int fielderIndex, float triggerDistance, int animationId)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.ballCollectingFielderIndex, fielderIndex);
        hash.Add(RoomProperty.ballCollectingTriggerDistance, triggerDistance);
        hash.Add(RoomProperty.ballCollectingFielderAnimationId, animationId);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public int GetBallCollectingFielderIndex()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.ballCollectingFielderIndex];
    }

    public float GetBallCollectingTriggerDistance()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballCollectingTriggerDistance];
    }

    public int GetBallCollectingFielderAnimationId()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.ballCollectingFielderAnimationId];
    }

    public void SetOnPadsParameters(string ballStatus, Vector3 ballHitPosition, int bodyColliderIndex)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

        Debug.Log("SetOnPadsParameters -> ballHitPosition : " + ballHitPosition);
        hash.Add(RoomProperty.ballStatus, ballStatus);
        hash.Add(RoomProperty.ballHitPosition, ballHitPosition);
        hash.Add(RoomProperty.bodyColliderIndex, bodyColliderIndex);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public void SetRunnerStatus(string status)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.runnerStatus, status);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public void SetStrikerStatus(string status)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.strikerStatus, status);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public string GetRunnerStatus()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.runnerStatus];
    }

    public string GetStrikerStatus()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.strikerStatus];
    }

    public void EnableRunForOpponent(float ghostBallDistance)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.runStartedTime, ghostBallDistance);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public float GetRunStartedGhostBallDistance()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.runStartedTime];
    }
    //09Aug
    public void SetDiveInitiated(float ghostBallDistance)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.diveStartedTime, ghostBallDistance);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public float GetDiveStartedGhostBallDistance()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.diveStartedTime];
    }
    //09Aug
    public void CancelRunForOpponent(float ghostBallDistance)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.runCancelledTime, ghostBallDistance);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public float GetRunCancelledGhostBallDistance()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.runCancelledTime];
    }
    //09Aug
    public void SetWideBall(bool _bool)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.isWideBall, _bool);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool GetWideBall()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.isWideBall];
    }
    //09Aug
    public string GetBallStatus()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.ballStatus];
    }

    public Vector3 GetBallHitPosition()
    {
        return (Vector3)PhotonNetwork.room.CustomProperties[RoomProperty.ballHitPosition];
    }

    public int GetBodyColliderIndex()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.bodyColliderIndex];
    }

    public float GetBallAngle()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.battingBallAngle];
    }

    public float GetHorizontalSpeed()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.horizontalSpeed];
    }

    public float GetBallProjectileAngle()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballProjectileAngle];
    }

    public float GetBallProjectileHeight()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballProjectileHeight];
    }

    public float GetBallTimingFirstBounceDistance()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballTimingFirstBounceDistance];
    }

    public float GetBallProjectileAnglePerSecond()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballProjectileAnglePerSecond];
    }

    public bool GetSlipShot()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.slipShot];
    }

    public bool GetBallToFineLeg()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.ballToFineLeg];
    }

    public bool GetEdgeCatch()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.edgeCatch];
    }
    //5
    public void SetBattingInitiatedParameters(float ballZPositionWhenUserInput, bool powerShot, string shotPlayed)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.ballZPositionWhenUserInput, ballZPositionWhenUserInput);
        hash.Add(RoomProperty.powerShot, powerShot);
        hash.Add(RoomProperty.shotPlayed, shotPlayed);
        PhotonNetwork.room.SetCustomProperties(hash);

        Debug.Log("Room Property changed :: SetBattingInitiatedParameters");
        Debug.Log("ballZPositionWhenUserInput : " + ballZPositionWhenUserInput);
        Debug.Log("powerShot : " + powerShot);
        Debug.Log("shotPlayed : " + shotPlayed);
    }


    public float GetBallZPositionWhenUserInput()
    {
        return (float)PhotonNetwork.room.CustomProperties[RoomProperty.ballZPositionWhenUserInput];
    }

    public bool GetPowerShot()
    {
        return (bool)PhotonNetwork.room.CustomProperties[RoomProperty.powerShot];
    }

    public string GetShotPlayed()
    {
        return (string)PhotonNetwork.room.CustomProperties[RoomProperty.shotPlayed];
    }

    public int GetMSS()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.masterSyncStatus];
    }

    public int GetCSS()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.clientSyncStatus];
    }

    public int GetWhoAskedToss()
    {
        return (int)PhotonNetwork.room.CustomProperties[RoomProperty.tossBy];
    }

    private void SetTeamIndex()
    {
        if (IsMasterClient() == true)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(RoomProperty.masterTeamIndex, 17); // should be dynamic for user's choice...
            PhotonNetwork.room.SetCustomProperties(hash);
            Debug.Log("Room Property changed :: masterTeamIndex " + "17");
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hash2 = new ExitGames.Client.Photon.Hashtable();
            hash2.Add(RoomProperty.clientTeamIndex, 0); // should be dynamic for user's choice...
            PhotonNetwork.room.SetCustomProperties(hash2);
            Debug.Log("Room Property changed :: clientTeamIndex " + "1");
        }
    }

    public void SetWhoToAskToss()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        if (Random.Range(0, 10) > 4)
        {
            hash.Add(RoomProperty.tossBy, 0);
        }
        else
        {
            hash.Add(RoomProperty.tossBy, 1);//Should be 1
        }
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public void UpdateMyStatus()
    {
        if (IsMasterClient() == true)
        {
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(RoomProperty.masterSyncStatus, 1);
            PhotonNetwork.room.SetCustomProperties(hash);
            Debug.Log("Master UpdateMyStatus");
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hash2 = new ExitGames.Client.Photon.Hashtable();
            hash2.Add(RoomProperty.clientSyncStatus, 1);
            PhotonNetwork.room.SetCustomProperties(hash2);
            Debug.Log("Client UpdateMyStatus");
        }
    }

    public void ResetMyStatus()
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(RoomProperty.masterSyncStatus, 0);
        hash.Add(RoomProperty.clientSyncStatus, 0);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    public bool IsMasterClient()
    {
        return PhotonNetwork.isMasterClient;
    }

    // Various Event-Listeners are...

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        connectedToGameRoom = true;
        aliveUpdatedTime = Time.time;
        Debug.Log("Room name :: " + PhotonNetwork.room.Name);
        PhotonStatusString = "Joined room.";
        SetTeamIndex();
        UpdateScreenIndex(0);
        SetMyFieldIndexToOpponent(CONTROLLER.fielderChangeIndex);
        if (IsMasterClient())
        {
            SetWhoToAskToss();
            TossCoinStatus();
            ResetMyStatus();//Multiplayer-Shankar-V6
        }
        SetBattingTeamDRSCount(2);
        SetBowlingTeamDRSCount(2);
    }

    public void OnPhotonCreateRoomFailed()
    {
        //ErrorDialog = "Error: Can't create room (room name maybe already used).";
        Debug.Log("OnPhotonCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
    }

    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        //ErrorDialog = "Error: Can't join room (full or unknown room name). " + cause[1];
        Debug.Log("OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
    }

    public void OnPhotonRandomJoinFailed()
    {
        //ErrorDialog = "Error: Can't join random room (none found).";
        Debug.Log("OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms.");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        //PhotonNetwork.LoadLevel(SceneNameGame);
    }

    public void OnDisconnectedFromPhoton()
    {
        connectedToGameRoom = false;
        Debug.Log("Disconnected from Photon.");
        //PhotonNetwork.ReconnectAndRejoin ();
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        //this.connectFailed = true;
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("As OnConnectedToMaster() got called, the PhotonServerSetting.AutoJoinLobby must be off. Joining lobby by calling PhotonNetwork.JoinLobby().");
        //PhotonNetwork.JoinLobby(); // not needed... can join/create the room based on match-making...
        CreateOrJoinRoom();
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        //Debug.Log ("Call -> OnPhotonCustomRoomPropertiesChanged");

        PhotonStatusString = "Photon custom room property changed.";

        if (propertiesThatChanged.ContainsKey(RoomProperty.tossWonBy) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : Toss : " + propertiesThatChanged[RoomProperty.tossWonBy]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.coinStatus) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : coinStatus : " + propertiesThatChanged[RoomProperty.coinStatus]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.tossAskedStatus) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : tossAskedStatus : " + propertiesThatChanged[RoomProperty.tossAskedStatus]);
            GameModelScript.PreTossScript.UpdateAfterToss();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.tossWinnerFirstBatting) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : tossWinnerFirstBatting : " + propertiesThatChanged[RoomProperty.tossWinnerFirstBatting]);
            /*if (IsMasterClient () == true)
            {
                GameModelScript.ShowTossResultToMaster ();
            }*/
            GameModelScript.PostTossScript.WaitForTossResult();
        }


        if (propertiesThatChanged.ContainsKey(RoomProperty.ballZPositionWhenUserInput) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballZPositionWhenUserInput : " + propertiesThatChanged[RoomProperty.ballZPositionWhenUserInput]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.powerShot) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : powerShot : " + propertiesThatChanged[RoomProperty.powerShot]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.shotPlayed) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : shotPlayed : " + propertiesThatChanged[RoomProperty.shotPlayed]);

            GameModelScript.GroundControllerScript.UpdateUserShotInput();
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.bowlingSpeed) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : bowlingSpeed : " + propertiesThatChanged[RoomProperty.bowlingSpeed]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.fillMeterValue) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : fillMeterValue : " + propertiesThatChanged[RoomProperty.fillMeterValue]);
            GameModelScript.BowlingControlsScript.UpdateLineNoBallZPos();
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.bowlingSwingSpin) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : bowlingSwingSpin : " + propertiesThatChanged[RoomProperty.bowlingSwingSpin]);
            GameModelScript.GroundControllerScript.UpdateBallSpeedSwingSpin();
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.bowlingSpotPosition) == true)
        {
            GameModelScript.GroundControllerScript.UpdateBowlingSpotPosition((Vector3)propertiesThatChanged[RoomProperty.bowlingSpotPosition], true); // second param is for interpolation
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.ballHitPosition) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballHitPosition : " + propertiesThatChanged[RoomProperty.ballHitPosition]);
            GameModelScript.GroundControllerScript.UpdateBallHitPosition();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.battingBallAngle) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : battingBallAngle : " + propertiesThatChanged[RoomProperty.battingBallAngle]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.horizontalSpeed) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : horizontalSpeed : " + propertiesThatChanged[RoomProperty.horizontalSpeed]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.ballProjectileAngle) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballProjectileAngle : " + propertiesThatChanged[RoomProperty.ballProjectileAngle]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.ballProjectileHeight) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballProjectileHeight : " + propertiesThatChanged[RoomProperty.ballProjectileHeight]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.ballTimingFirstBounceDistance) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballTimingFirstBounceDistance : " + propertiesThatChanged[RoomProperty.ballTimingFirstBounceDistance]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.ballProjectileAnglePerSecond) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballProjectileAnglePerSecond : " + propertiesThatChanged[RoomProperty.ballProjectileAnglePerSecond]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.slipShot) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : slipShot : " + propertiesThatChanged[RoomProperty.slipShot]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.ballToFineLeg) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballToFineLeg : " + propertiesThatChanged[RoomProperty.ballToFineLeg]);
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.edgeCatch) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : edgeCatch : " + propertiesThatChanged[RoomProperty.edgeCatch]);

            // 99999 // removed
            //GameModelScript.GroundControllerScript.UpdateUserShotBallParameters (); 
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.masterTeamIndex) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : masterTeamIndex : " + propertiesThatChanged[RoomProperty.masterTeamIndex]);
            Debug.Log("*************");
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.clientTeamIndex) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : clientTeamIndex : " + propertiesThatChanged[RoomProperty.clientTeamIndex]);
            Debug.Log("*************");
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.batsmanPosition) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : UpdatingBatsmanPosition");
            GameModelScript.GroundControllerScript.UpdateBatsmanPosition();
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.batsmanAnimation) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : UpdatingBatsmanAnimation");
            GameModelScript.GroundControllerScript.UpdateBatsmanAnimation();
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.ballCollectingFielderIndex) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballCollectingFielderIndex");
            GameModelScript.GroundControllerScript.UpdateBallCollectingFielderIndex((int)propertiesThatChanged[RoomProperty.ballCollectingFielderIndex]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.ballCollectingTriggerDistance) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballCollectingTriggerDistance");
            GameModelScript.GroundControllerScript.UpdateBallCollectingTriggerDistance((float)propertiesThatChanged[RoomProperty.ballCollectingTriggerDistance]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.ballCollectingFielderAnimationId) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : ballCollectingFielderAnimationId");
            GameModelScript.GroundControllerScript.UpdateBallCollectingFielderAnimationId((int)propertiesThatChanged[RoomProperty.ballCollectingFielderAnimationId]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.fielderThrowIndex) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : fielderThrowIndex : " + propertiesThatChanged[RoomProperty.fielderThrowIndex]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.isUnsuccessful) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : isUnsuccessful : " + propertiesThatChanged[RoomProperty.isUnsuccessful]);
            GameModelScript.GroundControllerScript.UpdateIsUnsuccessfulBoundaryDive((int)propertiesThatChanged[RoomProperty.isUnsuccessful]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.misfield) == true)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged : misfield : " + propertiesThatChanged[RoomProperty.misfield]);
            GameModelScript.GroundControllerScript.UpdateMisfield((int)propertiesThatChanged[RoomProperty.misfield]);
        }

        if (propertiesThatChanged.ContainsKey(RoomProperty.fieldIndex) == true)
        {
            Debug.Log("fieldIndex : " + propertiesThatChanged[RoomProperty.fieldIndex]);
            GameModelScript.GroundControllerScript.UpdateResetFielders();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.masterSyncStatus) == true)
        {
            Debug.Log("masterSyncStatus :: " + propertiesThatChanged[RoomProperty.masterSyncStatus]);
            GameModelScript.MatchIntroScript.skipMatchIntro();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.clientSyncStatus) == true)
        {
            Debug.Log("clientSyncStatus :: " + propertiesThatChanged[RoomProperty.clientSyncStatus]);
            GameModelScript.MatchIntroScript.skipMatchIntro();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.runStartedTime) == true)
        {
            Debug.Log("runStartedTime :: " + propertiesThatChanged[RoomProperty.runStartedTime]);
            GameModelScript.GroundControllerScript.SetRunStartedGhostBallDistance();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.diveStartedTime) == true)
        {
            Debug.Log("diveStartedTime :: " + propertiesThatChanged[RoomProperty.diveStartedTime]);//90Aug
            GameModelScript.GroundControllerScript.SetDiveForBowlingTeam();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.runCancelledTime) == true)
        {
            Debug.Log("runCancelledTime :: " + propertiesThatChanged[RoomProperty.runCancelledTime]);
            GameModelScript.GroundControllerScript.SetRunCancelledGhostBallDistance();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.reviewAskedStatus) == true)
        {
            Debug.Log("reviewAskedStatus :: " + propertiesThatChanged[RoomProperty.reviewAskedStatus]);
            GameModelScript.GroundControllerScript.UpdateAfterReview();
        }
        if (propertiesThatChanged.ContainsKey(RoomProperty.battingTeamDRSCount) == true)
        {
            Debug.Log("battingTeamDRSCount remianing :: " + propertiesThatChanged[RoomProperty.battingTeamDRSCount]);
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
        }
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        connectedToGameRoom = false;
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        Debug.Log("QuitApplication ***");
        Application.Quit();
    }
}