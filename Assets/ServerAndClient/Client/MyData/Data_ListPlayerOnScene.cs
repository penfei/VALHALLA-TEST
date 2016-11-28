using UnityEngine;
using System.Collections.Generic;

//This script contains data all players
public class Data_ListPlayerOnScene : MonoBehaviour
{
    public static int PlayerListCount { get { return playerList.Count; } }
    public static int ItemListCount { get { return ItemList.Count; } }

	public static List<GameObject> PlayersOnScene = new List<GameObject> ();
    public static List<Player_AnimateControl> playerControll = new List<Player_AnimateControl>();
	public static List<int> NULLPlayer = new List<int> ();

    static List<Data_ItemFile> ItemList = new List<Data_ItemFile>();
    static List<Player_Item> ItemObj = new List<Player_Item>();
    static List<int> nullItem = new List<int>();
	
	public static List<Data_PlayerFile> playerList = new List<Data_PlayerFile> ();

    /// <summary>
    /// Before writing player check null player in list
    /// </summary>
    /// <param name="player"></param>
	public static void WritePlayer (Data_PlayerFile player)
	{
		if (NULLPlayer.Count > 0) {
			playerList [NULLPlayer [0]] = player;
            NULLPlayer.RemoveAt(0);
		} else {
			playerList.Add (player);
            PlayersOnScene.Add(null);
            playerControll.Add(null);
        }
        
	}

    /// <summary>
    /// Add new player and ignored null item list
    /// </summary>
    /// <param name="player"></param>
    public static void AddPlayer(Data_PlayerFile player)
    {
            playerList.Add(player);
            PlayersOnScene.Add(null);
            playerControll.Add(null);
    }

    public static void WriteGameObject(GameObject obj, int index)
    {
        if (index > -1 && playerList.Count > index)
        {
            PlayersOnScene[index] = obj;
            playerControll[index] = obj.GetComponent<Player_AnimateControl>();
        }
	}

	public static void DisconnectPlayer (int ID)
	{
		playerList [ID] = null;
		PlayersOnScene [ID] = null;
		NULLPlayer.Add (ID);
		NULLPlayer.Sort ();
	}

	public static void ClearAll ()
	{
		playerList.Clear ();
	}

    public static Data_PlayerFile GetPlayerData(int index)
    {
        Data_PlayerFile d = null;
        if(index >= 0 && playerList.Count > index)
        {
            d = playerList[index];
        }
        return d;
    }

    public static Player_AnimateControl GetPlayerControll(int index)
    {
        Player_AnimateControl d = null;
        if (index >= 0 && playerControll.Count > index)
        {
            d = playerControll[index];
        }
        return d;
    }

    public static int FindPlayerID (int ID)
	{
		for (int i = 0; i < playerList.Count; i++) {
			if (playerList [i] != null) {
				if (ID == playerList [i].charID) {
					ID = i;
					break;
				}
			}
		}
		return ID;
	}

    /// <summary>
    /// Ignore null Item list
    /// </summary>
    /// <param name="data"></param>
    public static void AddNewItem(Data_ItemFile data)
    {
            ItemList.Add(data);
            ItemObj.Add(null);
    }

    /// <summary>
    /// Before writing data find null item in list
    /// </summary>
    /// <param name="data"></param>
    public static void WriteItemData(Data_ItemFile data)
    {
        if(nullItem.Count > 0)
        {
            ItemList[nullItem[0]] = data;
            ItemObj.Add(null);
            nullItem.RemoveAt(0);
        }
        else
        {
            ItemList.Add(data);
            ItemObj.Add(null);
        }
    }

    public static void WriteNewItem(Player_Item item, int index)
    {
        if (index > -1 && ItemObj.Count > index)
        {
            ItemObj[index] = item;
        }
    }

    public static void RemoveItem(int id)
    {
        if(id >= 0 && ItemList.Count > id)
        {
            Player_Item weapon = ItemObj[id];
            if (weapon)
            {
                Destroy(weapon.gameObject);
                ItemList[id] = null;
                ItemObj[id] = null;
                nullItem.Add(id);
                nullItem.Sort();
            }
        }
    }

    public static Data_ItemFile GetItemData(int id)
    {
        Data_ItemFile item = null;

        if (id >= 0 && ItemList.Count > id)
        {
            item = ItemList[id];
        }

        return item;
    }

    public static Player_Item GetItem(int id)
    {
        Player_Item weapon = null;

        if (id >= 0 && ItemList.Count > id)
        {
            weapon = ItemObj[id];
        }

        return weapon;
    }
}
