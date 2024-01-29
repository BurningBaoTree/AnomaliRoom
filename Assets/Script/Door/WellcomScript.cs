using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellcomScript : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerController player;

    public DoorInteractive door;

    public Action EnterBlueZone;
    private void Start()
    {
        gameManager = GameManager.Inst;
        player = GameManager.Inst.player;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.transform.position.z > this.transform.position.z)
            {
                Debug.Log("현재 옐로 포인트 안에 있습니다.");
            }
            else
            {
                Debug.Log("현재 블루 포인트 안에 있습니다.");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.transform.position.z > this.transform.position.z)
            {
                Debug.Log("옐로 포인트로 진입했습니다...");
            }
            else
            {
                Debug.Log("블루 포인트로 진입했습니다...");
            }
            door.InteractiveUse?.Invoke(false);
        }
    }
}
