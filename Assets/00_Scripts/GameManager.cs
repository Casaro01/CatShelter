using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // inventory and cat list
    public List<Item> playerItemInventory = new List<Item>();
    public List<Cat> playerCatList = new List<Cat>();

    // player money
    public int playerMoney;

    void Start()
    {
        // get info from savefile: INVENTORY, PLAYER MONEY, CAT LIST AND GAMEOBJECT POSITION

        // place gameojbects in specified position, load player money and inventory

        // start instance of input manager and UI manager

        PlayerPrefs.SetInt("Cat00_transform_x", 120);
        PlayerPrefs.GetInt("Cat00_transform_x");
    }

    void Update()
    {
        // player inventory, active cat list and playermoney are managed step by step by every script that can modify them
    }
}
