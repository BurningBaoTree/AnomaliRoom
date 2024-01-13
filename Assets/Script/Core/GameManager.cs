using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager inst;
    public PlayerController player;
    public GameManager gm;
    public static GameManager Inst
    {
        get
        {
            return inst;
        }
    }
    private void Awake()
    {
        inst = this;
        initializedGame();
    }
    void initializedGame()
    {
        player = FindObjectOfType<PlayerController>();
    }
}
