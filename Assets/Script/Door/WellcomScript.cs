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
                Debug.Log("���� ���� ����Ʈ �ȿ� �ֽ��ϴ�.");
            }
            else
            {
                Debug.Log("���� ��� ����Ʈ �ȿ� �ֽ��ϴ�.");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.transform.position.z > this.transform.position.z)
            {
                Debug.Log("���� ����Ʈ�� �����߽��ϴ�...");
            }
            else
            {
                Debug.Log("��� ����Ʈ�� �����߽��ϴ�...");
            }
            door.InteractiveUse?.Invoke(false);
        }
    }
}
