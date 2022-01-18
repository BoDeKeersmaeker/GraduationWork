using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator2D : BaseRoomGenerator
{
    private List<GameObject> tempRooms = new List<GameObject>();
    [SerializeField]
    private List<GameObject> CubePrefabs = null;
    [SerializeField]
    private Vector3 SpawnAreaPosition = Vector3.zero;
    [SerializeField]
    private float GridCellWidth = 10f;
    [SerializeField]
    private float GridCellHeight = 10f;
    [SerializeField]
    private float GridCellDepth = 10f;
    [SerializeField]
    private int AmountOfGridCellsWidth = 100;
    [SerializeField]
    private int AmountOfGridCellsHeight = 100;
    [SerializeField]
    private int AmountOfGridCellsDepth = 100;
    [SerializeField]
    private int AmountOfRooms = 100;

    GameObject[,,] Rooms; 

    // Start is called before the first frame update
    void Start()
    {
        Rooms = new GameObject[AmountOfGridCellsWidth, AmountOfGridCellsHeight, AmountOfGridCellsDepth];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GenerateRooms()
    {
        for (int i = 0; i < AmountOfRooms; i++)
            SpawnRoom();
    }

    void SpawnRoom()
    {
        int tempX = Random.Range(0, AmountOfGridCellsWidth);
        int tempY = Random.Range(0, AmountOfGridCellsHeight);
        int tempZ = Random.Range(0, AmountOfGridCellsDepth);

        Vector3 tempPosition;
        tempPosition.x = SpawnAreaPosition.x + GridCellWidth * tempX;
        tempPosition.y = SpawnAreaPosition.y + GridCellHeight * tempY;
        tempPosition.z = SpawnAreaPosition.z + GridCellDepth * tempZ;

        Quaternion tempRotation = transform.rotation;
        int tempRotationOffsetIndex = Random.Range(0, 4);
        switch (tempRotationOffsetIndex)
        {
            case 1:
                tempRotation.y += 90f;
                break;
            case 2:
                tempRotation.y += 180f;
                break;
            case 3:
                tempRotation.y -= 90f;
                break;
        }

        //int tempCubeVersion = Random.Range(0, CubePrefabs.Count);
        int tempCubeVersion = 0;

        if (CubePrefabs[tempCubeVersion] != null && CubePrefabs[tempCubeVersion])
        {
            GameObject temp = Instantiate(CubePrefabs[tempCubeVersion], tempPosition, tempRotation);
            tempRooms.Add(temp);
            //Rooms[tempX, tempY, tempZ] = temp;
        }
        else
            Debug.LogError("Tried to spawn invalid cube prefab. Instantiate skipped.");
    }

    public override GameObject[,,] GetRooms()
    {
        return Rooms;
    }
}