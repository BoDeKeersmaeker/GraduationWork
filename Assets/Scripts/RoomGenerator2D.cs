using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator2D : BaseRoomGenerator
{
    [SerializeField]
    private List<GameObject> CubePrefabs = null;
    [SerializeField]
    private Vector3 SpawnAreaPosition = Vector3.zero;
    [SerializeField]
    private float GridCellWidth = 1f;
    [SerializeField]
    private float GridCellHeight = 1f;
    [SerializeField]
    private float GridCellDepth = 1f;
    [SerializeField]
    private int AmountOfRooms = 100;

    private const int AmountOfGridCellsWidth = 100;
    private const int AmountOfGridCellsHeight = 1;
    private const int AmountOfGridCellsDepth = 100;
    GameObject[,,] Rooms = new GameObject[AmountOfGridCellsWidth, AmountOfGridCellsHeight, AmountOfGridCellsDepth]; 

    // Start is called before the first frame update
    void Start()
    {
        
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

        int tempCubeVersion = Random.Range(0, CubePrefabs.Count);

        if (CubePrefabs[tempCubeVersion] != null && CubePrefabs[tempCubeVersion])
            Rooms[tempX, tempY, tempZ] = Instantiate(CubePrefabs[tempCubeVersion], tempPosition, tempRotation);
        else
            Debug.LogError("Tried to spawn invalid cube prefab. Instantiate skipped.");
    }

    public override GameObject[,,] GetRooms()
    {
        return Rooms;
    }
}