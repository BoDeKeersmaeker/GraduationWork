using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator2D : MonoBehaviour
{
    private List<GameObject> Rooms = new List<GameObject>();
   [SerializeField]
    private List<GameObject> CubePrefabs = null;
    [SerializeField]
    private Vector3 SpawnAreaPosition = Vector3.zero;
    [SerializeField]
    private float SpawnAreaWidth = 100f;
    [SerializeField]
    private float SpawnAreaHeight = 100f;
    [SerializeField]
    private float SpawnAreaDepth = 100f;
    [SerializeField]
    private int AmountOfRooms = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRooms()
    {
        for (int i = 0; i < AmountOfRooms; i++)
            SpawnRoom();
    }

    void SpawnRoom()
    {
        Vector3 tempPosition;
        tempPosition.x = SpawnAreaPosition.x + Random.Range(0f, SpawnAreaWidth + 1f) - SpawnAreaWidth / 2f;
        tempPosition.y = /*SpawnAreaPosition.y + Random.Range(0f, SpawnAreaHeight + 1f) - SpawnAreaHeight / 2f*/0f;
        tempPosition.z = SpawnAreaPosition.z + Random.Range(0f, SpawnAreaDepth + 1f) - SpawnAreaDepth / 2f;

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

        Rooms.Add(Instantiate(CubePrefabs[tempCubeVersion], tempPosition, tempRotation));
    }
}