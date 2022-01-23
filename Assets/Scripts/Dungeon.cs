using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{

    [SerializeField]
    BaseRoomGenerator RoomGenerator = null;
    [SerializeField]
    BaseAlgorithm Algorithm = null;
    // Start is called before the first frame update
    void Start()
    {
        //RoomGenerator2D script = gameObject.AddComponent<RoomGenerator2D>() as RoomGenerator2D;
        //RoomGenerator2D roomGenerator2D = GameObject.FindObjectOfType<RoomGenerator2D>();
        //if (roomGenerator2D)
        //    roomGenerator2D.GenerateRooms();

        if(RoomGenerator)
            RoomGenerator.GenerateRooms();

        if (RoomGenerator && Algorithm)
            Algorithm.Generate(RoomGenerator.GetRooms());

        Algorithm.TempDebug();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
