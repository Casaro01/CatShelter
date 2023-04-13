using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // cat list

    // object inventory

    // player money

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
        // keep tabs on inventory, player money, cat list
        

    }
}
