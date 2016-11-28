using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Networking_Server : MonoBehaviour
{

    public Text fps;
    public Toggle useWebSockets;
    public Text error;
    private bool isAtStartup = false;
    public int Port;
    public int MaxConnection;

    static List<GameObject> Weapons = new List<GameObject>();

    delegate void Ex(string e);
    static event Ex Error;

    void Awake()
    {
        Error += ErrorMsg;
    }

    void ErrorMsg(string e)
    {
        error.text = e;
    }

    public static void ErrorMessage(string e)
    {
        Error(e);
    }

    void Start()
    {
        LoadWeapon();
    }

    void Update()
    {
        float f = 1 / Time.deltaTime;
        fps.text = string.Format("FPS: " + f.ToString());
    }

    public void ServerStart()
    {
        if (SQL_sqlConnect.SqlUp)
        {
            if (!isAtStartup)
            {
                
                ConnectionConfig config = new ConnectionConfig();
                config.AddChannel(QosType.Reliable);
                config.AddChannel(QosType.Unreliable);
                NetworkServer.useWebSockets = useWebSockets.isOn;
                //NetworkServer.Configure(config, MaxConnection);
                HostTopology host = new HostTopology(config, MaxConnection);
                NetworkServer.Configure(host);
                if (NetworkServer.Listen(Port))
                {
                    
                    NetworkServer.RegisterHandler(MsgType.Connect, OnConn);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.LoginSend, Networking_OnConnect.LoginGet);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.PlayerMove, Networking_OnPlayerAction.OnPlMove);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.PlayerSelect, Networking_OnPlayerSelect.PlayerSelect);
                    NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.OnPlayerReady, OnReady);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.Registration, SQL_AccountRegistration.CreatNewAccount);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.CharCreate, SQL_AccountRegistration.CreateChar);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.Chat, Networking_Chat_Sr.ChatHandler);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.PasswordRecovery, SQL_PasswordRecovery.PasswordRecovery);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.MouseButton, Networking_OnPlayerAction.OnMouseButton);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.Respawn, Networking_OnPlayerAction.HandleRespawn);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.Reload, Networking_OnPlayerAction.HandleReload);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.TopList, SQL_sqlConnect.SendTopList);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.DropWeapon, Networking_OnPlayerAction.HandleDropWeapon);
                    NetworkServer.RegisterHandler(Networking_msgType_Sr.PickUp, Networking_OnPlayerAction.HandlePickUpItem);
                    isAtStartup = true;
                    Debug.Log("Server start");
                    ErrorMsg("Server start");
                }
            }
        }
        else
        {
            ErrorMsg("Please first connect to MySQL!");
        }
    }

    void OnReady(NetworkMessage netmsg)
    {
        Message_Sr.PlayerSetReady ready = netmsg.ReadMessage<Message_Sr.PlayerSetReady>();
        if (ListScene.CheckPlayer(ready.id, ready.log, ready.pass))
        {
            ListScene.playerList[ready.id].PlayerReady = true;

            Data_PlayerFile_Sr data = ListScene.GetPlayerData(ready.id);
            if (data)
            {
                data.SetHP(data.HPMax);
            }
        }
    }

    void OnDisconnect(NetworkMessage netmsg)
    {
        ListScene.DisconnectPlayer(netmsg.conn.connectionId);
        int accID = -1;
        accID = Networking_OnConnect.onlineConn.IndexOf(netmsg.conn);
        if (accID != -1)
        {
            Networking_OnConnect.onlineConn.RemoveAt(accID);
            Networking_OnConnect.onlineAcc.RemoveAt(accID);
        }
        netmsg.conn.Disconnect();
    }

    void OnConn(NetworkMessage netmsg)
    {
        NetworkServer.SetClientReady(netmsg.conn);
        Debug.Log(netmsg.conn);
    }

    /// <summary>
    /// Send unreliable message all ready player
    /// </summary>
    /// <param name="type">msg type</param>
    /// <param name="msg">message</param>
    public static void SendMSG(short type, MessageBase msg)
    {
        for (int l = 0; l < ListScene.ConnectionIDList.Count; l++)
        {
            if (ListScene.GetConnectionID(l) != -1)
            {
                Data_PlayerFile_Sr data = ListScene.GetPlayerData(l);
                if (data != null && data.PlayerReady)
                {
                    NetworkConnection conn = ListScene.GetPlayerConnection(l);
                    if (conn != null)
                    {
                        conn.SendUnreliable(type, msg);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Send unreliable message all ready player
    /// </summary>
    /// <param name="msg">message</param>
    public static void SendMSG(NetworkWriter msg)
    {
        for (int l = 0; l < ListScene.ConnectionIDList.Count; l++)
        {
            if (ListScene.GetConnectionID(l) != -1)
            {
                Data_PlayerFile_Sr data = ListScene.GetPlayerData(l);
                if (data != null && data.PlayerReady)
                {
                    NetworkConnection conn = ListScene.GetPlayerConnection(l);
                    if (conn != null)
                    {
                        conn.SendWriter(msg, 1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Send unreliable messsage all other player
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="index">This player</param>
    public static void SendAllOtherPlayer(NetworkWriter msg, int index)
    {
        for (int l = 0; l < ListScene.ConnectionIDList.Count; l++)
        {
            if (index != l)
            {
                if (ListScene.GetConnectionID(l) != -1)
                {
                    Data_PlayerFile_Sr data = ListScene.GetPlayerData(l);
                    if (data != null && data.PlayerReady)
                    {
                        NetworkConnection conn = ListScene.GetPlayerConnection(l);
                        if (conn != null)
                        {
                            conn.SendWriter(msg, 1);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Send unreliable messsage all other player
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="msgType">msg type</param>
    /// <param name="index">This player</param>
    public static void SendAllOtherPlayer(short msgType, MessageBase msg, int index)
    {
        for (int l = 0; l < ListScene.ConnectionIDList.Count; l++)
        {
            if (index != l)
            {
                if (ListScene.GetConnectionID(l) != -1)
                {
                    Data_PlayerFile_Sr data = ListScene.GetPlayerData(l);
                    if (data != null && data.PlayerReady)
                    {
                        NetworkConnection conn = ListScene.GetPlayerConnection(l);
                        if (conn != null)
                        {
                            conn.SendUnreliable(msgType, msg);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Send unreliable message this player
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="index">This player</param>
    public static void SendThisPlayer(NetworkWriter msg, int index)
    {
        if (ListScene.GetConnectionID(index) != -1)
        {
            Data_PlayerFile_Sr data = ListScene.GetPlayerData(index);
            if (data != null && data.PlayerReady)
            {
                NetworkConnection conn = ListScene.GetPlayerConnection(index);
                if (conn != null)
                {
                    conn.SendWriter(msg, 1);
                }
            }
        }
    }

    /// <summary>
    /// Send unreliable message this player
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="msgType">msg type</param>
    /// <param name="index">This player</param>
    public static void SendThisPlayer(short msgType, MessageBase msg,  int index)
    {
        if (ListScene.GetConnectionID(index) != -1)
        {
            Data_PlayerFile_Sr data = ListScene.GetPlayerData(index);
            if (data != null && data.PlayerReady)
            {
                NetworkConnection conn = ListScene.GetPlayerConnection(index);
                if (conn != null)
                {
                    conn.SendUnreliable(msgType, msg);
                }
            }
        }
    }

    public static void SendInt(NetworkConnection conn, short msgType, int value)
    {
        if (conn != null)
        {
            NetworkWriter wr = new NetworkWriter();
            wr.StartMessage(msgType);
            wr.Write(value);
            wr.FinishMessage();
            conn.SendWriter(wr, 0);
        }
    }

    public static void SendInt(short msgType, int value)
    {
        NetworkWriter wr = new NetworkWriter();
        wr.StartMessage(msgType);
        wr.Write(value);
        wr.FinishMessage();

        for (int l = 0; l < ListScene.ConnectionIDList.Count; l++)
        {
            if (ListScene.GetConnectionID(l) != -1)
            {
                Data_PlayerFile_Sr data = ListScene.GetPlayerData(l);
                if (data != null && data.PlayerReady)
                {
                    NetworkConnection conn = ListScene.GetPlayerConnection(l);
                    if (conn != null)
                    {
                        conn.SendWriter(wr, 0);
                    }
                }
            }
        }
    }

    void LoadWeapon()
    {
        GameObject go = Resources.Load<GameObject>("Weapons/Machine");
        if (go)
        {
            Weapons.Add(go);
        }
        else
        {
            new NotImplementedException("Not find weapon model!");
        }
        go = Resources.Load<GameObject>("Weapons/ShotGun");
        if (go)
        {
            Weapons.Add(go);
        }
        else
        {
            new NotImplementedException("Not find weapon model!");
        }
        go = Resources.Load<GameObject>("Weapons/Rifle");
        if (go)
        {
            Weapons.Add(go);
        }
        else
        {
            new NotImplementedException("Not find weapon model!");
        }
        go = Resources.Load<GameObject>("Weapons/GaussGun");
        if (go)
        {
            Weapons.Add(go);
        }
        else
        {
            new NotImplementedException("Not find weapon model!");
        }
    }

    public static Player_Weapon_Sr InstantiateWeapon(weaponType_Sr type)
    {
        Player_Weapon_Sr weap = null;
        GameObject go = null;

        switch (type)
        {
            case weaponType_Sr.Machine:
                go = Instantiate(Weapons[0]);
                break;
            case weaponType_Sr.ShotGun:
                go = Instantiate(Weapons[1]);
                break;
            case weaponType_Sr.Rifle:
                go = Instantiate(Weapons[2]);
                break;
            case weaponType_Sr.GaussGun:
                go = Instantiate(Weapons[3]);
                break;
        }

        if (go)
        {
            weap = go.GetComponent<Player_Weapon_Sr>();
            ListScene.AddNewItemOnScene(weap);
            SendInstantiateWeapon(weap);
        }

        return weap;
    }

    public static Player_Weapon_Sr InstantiateWeapon(weaponType_Sr type, Vector3 position)
    {
        Player_Weapon_Sr weap = null;
        GameObject go = null;

        switch (type)
        {
            case weaponType_Sr.Machine:
                go = (GameObject)Instantiate(Weapons[0], position, Quaternion.identity);
                break;
            case weaponType_Sr.ShotGun:
                go = (GameObject)Instantiate(Weapons[1], position, Quaternion.identity);
                break;
            case weaponType_Sr.Rifle:
                go = (GameObject)Instantiate(Weapons[2], position, Quaternion.identity);
                break;
            case weaponType_Sr.GaussGun:
                go = (GameObject)Instantiate(Weapons[3], position, Quaternion.identity);
                break;
        }

        if (go)
        {
            weap = go.GetComponent<Player_Weapon_Sr>();
            ListScene.AddNewItemOnScene(weap);
            SendInstantiateWeapon(weap);
        }

        return weap;
    }

    public static void SendInstantiateWeapon(Player_Weapon_Sr weapon)
    {
        Message_Sr.LoadItem_Sr item = new Message_Sr.LoadItem_Sr();
        item.item = weapon;
        SendMSG(Networking_msgType_Sr.NewItemOnScene, item);
    }
}

