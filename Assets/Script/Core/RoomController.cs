
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform RoomPoint1;
    public Transform RoomPoint2;

    public GameObject[] RoomList;

    public GameObject YelloRoom;
    public GameObject BlueRoom;

    private void OnEnable()
    {
        InitializeRooms();
    }


    void InitializeRooms()
    {
        int RandomInt = Random.Range(0, RoomList.Length);
        SpawnYello(0);
        SpawnBlue(RandomInt);
    }
    void SpawnYello(int num)
    {
        if(YelloRoom != null)
        {
            Destroy(YelloRoom);
        }
        YelloRoom = Instantiate(RoomList[num], RoomPoint1.position, Quaternion.identity);
    }
    void SpawnBlue(int num)
    {
        if (BlueRoom != null)
        {
            Destroy(BlueRoom);
        }
        BlueRoom = Instantiate(RoomList[num], RoomPoint2.position, Quaternion.identity);
        BlueRoom.transform.localScale = new Vector3(1, 1, -1);
    }
}

