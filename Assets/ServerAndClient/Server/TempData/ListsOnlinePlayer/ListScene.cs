using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ListScene : MonoBehaviour
{
    public static Data_PlayerFile_Sr playerGet;
    public static int playerNumber = 0;
    [Header("-----Zombie setting-----")]
    public int zombieNumber = 50;
    [Header("SceneData")]
    public static List<GameObject>
        objPlayers = new List<GameObject>();
    public static List<Player_MovePlayer> playerMoveScr = new List<Player_MovePlayer>();
    public static List<Data_PlayerFile_Sr> playerList = new List<Data_PlayerFile_Sr>();
    public static List<NetworkConnection> listConnection = new List<NetworkConnection>();
    public static List<int> playerListNULLVALLUE = new List<int>();
    public static List<int> ConnectionIDList = new List<int>();
    public static int playerListCount { get { return playerList.Count; } }
    //Items
    static List<Player_Item_Sr> ItemsList = new List<Player_Item_Sr>();
    static List<int> nullItem = new List<int>();
    public static int itemListCount { get { return ItemsList.Count; } }

    public static int lastWritePlayerID;

    #region Zombie Load On Scene
    void Start()
    {
        GameObject obj = (GameObject)Resources.Load("Zombie");
        Vector3 vect = Vector3.zero;

        float Hight = Random.Range(0.1f, 0.15f);
        float Midle = Random.Range(0.15f, 0.25f);
        float Low = 1 - (Hight + Midle);

        int zombieLow = Mathf.FloorToInt(zombieNumber * Low);
        int zombieMidle = Mathf.FloorToInt(zombieNumber * Midle);
        int zombieHight = Mathf.FloorToInt(zombieNumber * Hight);

        for (int i = 0; i < zombieLow; i++)
        {
            do
            {
                vect = new Vector3(Random.Range(0, 200), 120f, Random.Range(0, 200));
            }
            while (vect.x < 20f || vect.z < 20f);
            GameObject Zombie = (GameObject)Instantiate(obj, vect, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            NetworkConnection conn = new NetworkConnection();
            Data_PlayerFile_Sr data = ScriptableObject.CreateInstance<Data_PlayerFile_Sr>();
            data.HPMax = Random.Range(60, 110);
            data.SetHP(data.HPMax);
            switch (Random.Range(1, 3))
            {
                case 1:
                    data.nick = "zombie";
                    break;
                case 2:
                    data.nick = "sickzombie";
                    break;
                case 3:
                    data.nick = "policezombie";
                    break;
            }
            data.PlayerReady = true;
            data.attackPower = Random.Range(10, 17);
            Zombie.GetComponent<Player_MovePlayer>().zombie = true;
            WritePlayer(conn, data, Zombie);
        }

        for (int i = 0; i < zombieMidle; i++)
        {
            do
            {
                vect = new Vector3(Random.Range(0, 200), 120f, Random.Range(0, 200));
            }
            while (vect.x < 20f || vect.z < 20f);
            GameObject Zombie = (GameObject)Instantiate(obj, vect, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            NetworkConnection conn = new NetworkConnection();
            Data_PlayerFile_Sr data = ScriptableObject.CreateInstance<Data_PlayerFile_Sr>();
            data.HPMax = Random.Range(180, 250);
            data.SetHP(data.HPMax);
            data.nick = "mutantzombie";
            data.PlayerReady = true;
            data.attackPower = Random.Range(20, 30);
            Zombie.GetComponent<Player_MovePlayer>().zombie = true;
            WritePlayer(conn, data, Zombie);
        }

        for (int i = 0; i < zombieHight; i++)
        {
            do
            {
                vect = new Vector3(Random.Range(0, 200), 120f, Random.Range(0, 200));
            }
            while (vect.x < 20f || vect.z < 20f);
            GameObject Zombie = (GameObject)Instantiate(obj, vect, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            NetworkConnection conn = new NetworkConnection();
            Data_PlayerFile_Sr data = ScriptableObject.CreateInstance<Data_PlayerFile_Sr>();
            data.HPMax = Random.Range(300, 400);
            data.SetHP(data.HPMax);
            data.nick = "strongzombie";
            data.PlayerReady = true;
            data.attackPower = Random.Range(36, 48);
            Zombie.GetComponent<Player_MovePlayer>().zombie = true;
            WritePlayer(conn, data, Zombie);
        }

    }
    #endregion


    #region Player Metods
    public static void WritePlayer(NetworkConnection tempdata, Data_PlayerFile_Sr player, GameObject obj)
    {
        if (!listConnection.Contains(tempdata))
        {
            if (playerListNULLVALLUE.Count > 0)
            {
                player.charID = playerListNULLVALLUE[0];
                listConnection[playerListNULLVALLUE[0]] = tempdata;
                ConnectionIDList[playerListNULLVALLUE[0]] = tempdata.connectionId;
                playerList[playerListNULLVALLUE[0]] = player;
                objPlayers[playerListNULLVALLUE[0]] = obj;
                playerMoveScr[playerListNULLVALLUE[0]] = obj.GetComponent<Player_MovePlayer>();
                obj.GetComponent<Player_MovePlayer>().index = playerListNULLVALLUE[0];
                playerMoveScr[playerListNULLVALLUE[0]].index = playerListNULLVALLUE[0];
                lastWritePlayerID = playerListNULLVALLUE[0];
                playerListNULLVALLUE.RemoveAt(0);
                playerNumber++;
            }
            else
            {
                player.charID = playerList.Count;
                obj.GetComponent<Player_MovePlayer>().index = playerList.Count;
                lastWritePlayerID = playerList.Count;
                listConnection.Add(tempdata);
                ConnectionIDList.Add(tempdata.connectionId);
                playerList.Add(player);
                objPlayers.Add(obj);
                playerMoveScr.Add(obj.GetComponent<Player_MovePlayer>());
                playerNumber++;
            }
        }
    }

    public static Data_PlayerFile_Sr GetPlayerData(int index)
    {
        Data_PlayerFile_Sr d = null;
        if (index >= 0 && playerList.Count > index)
        {
            d = playerList[index];
        }
        return d;
    }

    public static Player_MovePlayer GetPlayerControll(int index)
    {
        Player_MovePlayer d = null;
        if (index >= 0 && playerList.Count > index)
        {
            d = playerMoveScr[index];
        }
        return d;
    }

    public static NetworkConnection GetPlayerConnection(int index)
    {
        NetworkConnection d = null;
        if (index >= 0 && playerList.Count > index)
        {
            d = listConnection[index];
        }
        return d;
    }

    public static int GetConnectionID(int index)
    {
        int id = -1;
        if (index > -1 && ConnectionIDList.Count > index)
        {
            id = ConnectionIDList[index];
        }
        return id;
    }

    public static bool DisconnectPlayer(int conn)
    {
        bool yes = false;
        if (ConnectionIDList.Contains(conn))
        {
            yes = true;
            int pl_id = ConnectionIDList.IndexOf(conn);
            //Remove player weapon
            Player_MovePlayer controll = playerMoveScr[pl_id];
            if (controll.weaponOnMe)
            {
                Networking_Server.SendInt(Networking_msgType_Sr.RemoveItenOnScene, controll.weaponOnMe.index);
                RemoveItem(controll.weaponOnMe.index);
            }
            //Remove player data
            ConnectionIDList[pl_id] = -1;
            Destroy(GameObject.Find(playerList[pl_id].nick));
            listConnection[pl_id] = null;
            playerList[pl_id] = null;
            objPlayers[pl_id] = null;
            playerMoveScr[pl_id] = null;
            playerListNULLVALLUE.Add(pl_id);
            playerListNULLVALLUE.Sort();
            playerNumber--;
            Message_Sr.DisconnectPlayer disc = new Message_Sr.DisconnectPlayer();
            disc.ID = pl_id;
            Networking_Server.SendMSG(Networking_msgType_Sr.DisconnectPlayer, disc);
        }
        return yes;
    }

    public static bool CheckPlayer(int index, string login, string password)
    {
        if (playerList[index].login == login && playerList[index].password == password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Items Metods
    public static void AddNewItemOnScene(Player_Item_Sr item)
    {
        if(nullItem.Count > 0)
        {
            ItemsList[nullItem[0]] = item;
            item.index = nullItem[0];
            nullItem.RemoveAt(0);
        }
        else
        {
            item.index = ItemsList.Count;
            ItemsList.Add(item);
        }
    }

    public static void RemoveItem(int id)
    {
        if(id > -1 && ItemsList.Count > id)
        {
            ItemsList[id] = null;
            nullItem.Add(id);
        }
    }

    public static Player_Item_Sr GetItem(int id)
    {
        Player_Item_Sr item = null;

        if (id > -1 && ItemsList.Count > id)
        {
           item = ItemsList[id];
        }

        return item;
    }
    #endregion
}
