using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardGUI : MonoBehaviour
{
    public Text playerName;
    public Text mmr;
    public Image sprite;
    public string id;
    public string region;
    public Image OnlineImage;
    public bool isOnline, isInGame;
    public GSLeaderboardData leaderboardData;
    Transform parentt;
    public StringObject userId;
    public StringObject opponentId;
    public StringObject opponentName;
    public StringObject server;
    public EventObject challengeLoadingON;
    ParameterlessDelegate detailsSet;
    public PlayerData data;

    public Color startColor;


    private void OnEnable()
    {
        StartCoroutine(KeepChecking());
    }


    private void OnDisable()
    {
        StopCoroutine(KeepChecking());
    }

    public void Set(string id, Transform scrollParent, ParameterlessDelegate callback)
    {
        detailsSet = callback;
        this.id = id;
        parentt = scrollParent;
        GameSparkRequests getPlayerDetail = new GameSparkRequests();
        getPlayerDetail.Request("GetPlayerDataWithID", "ID", id, Callback);
        transform.SetParent(scrollParent);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        CheckOnline();
    }

    void Callback(string str)
    {
        leaderboardData = JsonUtility.FromJson<GSLeaderboardData>(str);
        data = JsonUtility.FromJson<PlayerData>(leaderboardData.scriptData.AllData.scriptData.data);
        playerName.text = leaderboardData.scriptData.AllData.displayName;
        if (!string.IsNullOrEmpty(leaderboardData.scriptData.AllData.scriptData.FBID))
        {
            StartCoroutine(LoadImage(leaderboardData.scriptData.AllData.scriptData.FBID));
        }
        if (data != null)
        {
            mmr.text = data.MMR.ToString();
            OnlineImage.gameObject.SetActive(IsOnline());
            OnlineImage.color = IsFree() ? Color.green : Color.red;
        }
        detailsSet();
    }

    public bool IsOnline()
    {
        return leaderboardData.scriptData.AllData.online;
    }

    public bool IsFree()
    {
        return leaderboardData.scriptData.AllData.scriptData.IsInGame == 0;
    }

    public void SetOpponentID()
    {
        print(id == userId.value + "" + !IsOnline() + "" + !IsFree() + "" + !PhotonNetwork.connected);
        ObliusGameManager.isFriendlyBattle = true;
        if (id == userId.value || !IsOnline() || !IsFree() || !PhotonNetwork.connected)
            return;
        opponentId.value = id;
        opponentName.value = playerName.text;
        var regions = data.PING.ToRegions();
        List<PhotonRegion> myRegions = PhotonPingManager.ping.ToRegions();
        List<PhotonRegion> result = new List<PhotonRegion>();
        for (int i = 0; i < regions.Count; i++)
        {
            PhotonRegion r = new PhotonRegion();
            r.region = regions[i].region;
            r.ping = regions[i].ping + myRegions[i].ping;
            result.Add(r);
        }
        result = result.OrderBy(a => a.ping).ToList();
        server.value = result[0].region;
        challengeLoadingON.Fire();
    }


    public void OnClick()
    {
        if (id == userId.value)
            return;

        ProfileSelectManager.instance.selectedProfile = this;
        ProfileSelectManager.instance.AssignValues();
        ProfileSelectManager.instance.gameObject.SetActive(true);
    }

    IEnumerator LoadImage(string id)
    {
        WWW www = new WWW("http://graph.facebook.com/" + id + "/picture?width=100&height=100");
        yield return www;
        if (www.texture != null)
            sprite.sprite = Sprite.Create(www.texture, new Rect(0, 0, 100, 100), Vector2.zero);
    }

    public void CheckOnline()
    {
        GameSparkRequests getPlayerDetail = new GameSparkRequests();
        getPlayerDetail.Request("GetPlayerDataWithID", "ID", id, Callback);
    }

    IEnumerator KeepChecking()
    {
        CheckOnline();
        yield return new WaitForSeconds(5);
        StartCoroutine(KeepChecking());
    }
}