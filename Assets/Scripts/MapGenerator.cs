using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private Transform[] roofFloorPrefab;

    [Header("MAP Options")]
    [Range(2, 32)]
    [SerializeField] private int roomNumber = 2;
    [Range(0, 1)]
    [SerializeField] private float roomSize5x5 = 0.25f;
    [Range(0, 1)]
    [SerializeField] private float roomSize10x10 = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float roomSize20x20 = 0.25f;
    [Range(0, 1)]
    [SerializeField] private float roomToHallway = 0.5f;


    [Header("Single Room Options")]
    [SerializeField] RoomSize roomSizeMenu;
    [SerializeField] private RoomType roomTypeMenu;
    [SerializeField] private ConnectionType connectionTypeMenu;

    private List<Room> mapRooms = new List<Room>();
    private Vector3[] roomsSize =
    {
        new Vector3 (5, 5, 5),
        new Vector3 (10, 5, 10),
        new Vector3 (20, 5, 20)
    };


    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void GenerateMap()
    {
        ClearMapGenerator();

        int roomCount = roomNumber;

        //generate first room (5/5 one enternance) with random orientation
        mapRooms.Add(GenerateRoom(roomsSize[0], 1, 1));
        mapRooms[0].roomHolder.rotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
        roomCount--;

        CheckAndGenerateRoom(roomCount, mapRooms[0].enternances[0]);

    }

    private int CheckAndGenerateRoom(int roomCount, Transform enternance)
    {
        //check how big can be adjectent room
        Physics.SyncTransforms();
        int maxRoomSizeIndex = -1;
        for (int i = 0; i < 3; i++)
        {
            Vector3 roomPos = enternance.position + (enternance.position - enternance.parent.position).normalized * (roomsSize[i].x / 2 + 0.1f);

            Collider[] col = Physics.OverlapBox(roomPos, roomsSize[i] / 2 - Vector3.one * 0.1f, Quaternion.identity);

            if (col.Length == 0)
            {
                maxRoomSizeIndex = i;
                //Debug.Log("Room: " + i);
            }
        }

        //if no room can be created - close enternance and return;
        if (maxRoomSizeIndex == -1)
        {
            //close enternance
            Transform ent = Instantiate(wallPrefab, enternance.position, enternance.rotation);
            ent.parent = enternance.parent;
            ent.localScale = enternance.localScale;
            DestroyImmediate(enternance.gameObject);
        }
        else
        {
            //create room
            //get size
            float sizeRandom = Random.Range(0f, roomSize5x5 + roomSize10x10 + roomSize20x20);
            int sizeIndex = 0;
            if ((sizeRandom > roomSize5x5 + roomSize10x10) && maxRoomSizeIndex > 1)
            {
                sizeIndex = 2;
            }
            else if ((sizeRandom > roomSize5x5) && maxRoomSizeIndex > 0)
            {
                sizeIndex = 1;
            }
            //get type of room
            float typeRandom = Random.Range(0f, 1f);
            int typeIndex = 1;
            if(typeRandom < roomToHallway)
            {
                typeIndex = 2;
            }

            //check how much enternances can have room
            //int minEnt = roomCount == 1 ? 1 : 2;
            //int maxEnt = Mathf.Clamp(roomCount, 1, 4);
            //int connectionRandom = Random.Range(minEnt, maxEnt);
        
            int connectionRandom = Mathf.Clamp(roomCount, 1, 4);
            if (connectionRandom == 2)
            {
                connectionRandom = Random.Range(0, 1) == 0 ? 2 : 5;
            }

            Room roomTemp = GenerateRoom(roomsSize[sizeIndex], typeIndex, connectionRandom);
            roomCount--;
            Vector3 dirVector = (enternance.position - enternance.parent.position).normalized;
            roomTemp.roomHolder.position = enternance.position + dirVector * (roomsSize[sizeIndex].x / 2);
            //orient room
            int k = 1;
             while (dirVector + (roomTemp.enternances[0].position - roomTemp.roomHolder.position).normalized != Vector3.zero)
            {
                roomTemp.roomHolder.eulerAngles += Vector3.up * 90 * k;
                k++;
            }
            mapRooms.Add(roomTemp);

            //run this function for all its enternances with modified room count
            for (int i = 1; i < roomTemp.enternances.Count; i ++)
            {
                int roomCountToPass = (int)roomCount / (roomTemp.enternances.Count + 1 - i);
                roomCount += CheckAndGenerateRoom( roomCountToPass, roomTemp.enternances[i]);
            }


        }

        return roomCount;
    }


    public Room GenerateRoom(Vector3 _roomSize, int _roomType, int _connectionType)
    {
        //vars
        Room _room = new Room();
        _room.enternances = new List<Transform>();

        float wallThicknes = 0.2f;
        Vector3 enternanceSize = new Vector3(4, 5, wallThicknes);
        Vector3 doorSize = new Vector3(3, 3, wallThicknes);

        ////create room holder object
        string roomName = connectionTypeMenu.ToString() + " " + roomTypeMenu.ToString() + " " + _roomSize.x + "x" + _roomSize.z;
        Transform roomHolder = new GameObject(roomName).transform;
        //roomHolder.localScale = _roomSize;
        roomHolder.parent = transform;
        _room.roomHolder = roomHolder;

        //generate central floor and roof tile
        for (int i = 0; i < 2; i ++)
        {
            int mod = i % 2 == 0 ? 1 : -1;
            CreateFloorTile(roomHolder, roomHolder.position - Vector3.up * mod * _roomSize.y / 2, new Vector3(enternanceSize.x, enternanceSize.x, roofFloorPrefab[i].localScale.z), mod, roofFloorPrefab[i]);
        }

        ////posible enternances positions table
        Vector3[] enternancePos = new Vector3[4];
        int modX = 1;
        int modXmod = 1;
        int modZ = 0;
        int modZmod = -1;

        for (int i = 0; i < enternancePos.Length; i++)
        {
            enternancePos[i] = new Vector3((_roomSize.x / 2 - wallThicknes / 2) * modX, 0, (_roomSize.z / 2 - wallThicknes / 2) * modZ);
            modXmod *= modX == -1 || modX == 1 ? -1 : 1;
            modZmod *= modZ == -1 || modZ == 1 ? -1 : 1;
            modX += modXmod;
            modZ += modZmod;
        }

        //generating enternanaces
        bool hallway = _roomType == 1 ? false : true;
        Transform ent;
        Vector3 pos;
        Vector3 size;
        int _index;
        bool strightRoom = _connectionType == 5 ? true : false;
        bool generateEnternance = false;

        for (int i = 1; i <= 4; i++)
        {
            _index = i - 1;
            if (i <= _connectionType)
            {
                if (!strightRoom)
                {
                    generateEnternance = true;
                }
                else if (i == 1 || i == 3)
                {
                    generateEnternance = true;
                }
                else
                {
                    generateEnternance = false;
                }
            }
            else
            {
                generateEnternance = false;
            }

            if (generateEnternance)
            {
                ent = CreateEnternance(wallThicknes, roomHolder, enternancePos[_index], enternanceSize, doorSize);
                _room.enternances.Add(ent);

                if (hallway)
                {
                    //generate hallway walls (on enternanace tiles)
                    for (int j = -1; j < 3; j += 2)
                    {
                        size = new Vector3((_roomSize.x - enternanceSize.x) / 2, _roomSize.y, wallThicknes);
                        pos = new Vector3((enternancePos[_index].x - ((_roomSize.x - enternanceSize.x) / 4) * enternancePos[_index].normalized.x) * Mathf.Abs(enternancePos[_index].normalized.x) + ((enternanceSize.x - wallThicknes) / 2 * j) * enternancePos[_index].normalized.z, 0, (enternancePos[_index].z - ((_roomSize.z - enternanceSize.x) / 4) * enternancePos[_index].normalized.z) * Mathf.Abs(enternancePos[_index].normalized.z) + ((enternanceSize.x - wallThicknes) / 2 * j) * enternancePos[_index].normalized.x);
                        ent = CreateWall(pos, size, roomHolder);
                        ent.rotation = Quaternion.Euler(0, 90 * enternancePos[_index].normalized.z, 0);
                    }
                }
            }

            if (generateEnternance || !hallway)
            {
                // floor and roof on enternance position
                for (int j = 0; j < 2; j ++)
                {
                    int mod = j % 2 == 0 ? 1 : -1;
                    pos = new Vector3(enternancePos[_index].normalized.x * (_roomSize.x + enternanceSize.x) / 4, -mod * _roomSize.y / 2, enternancePos[_index].normalized.z * (_roomSize.x + enternanceSize.x) / 4);
                    size = new Vector3(enternanceSize.x, (_roomSize.x - enternanceSize.x) / 2, 1);
                    ent = CreateFloorTile(roomHolder, pos, size, mod, roofFloorPrefab[j]);
                    ent.localRotation = Quaternion.Euler(ent.localEulerAngles.x, ent.localEulerAngles.y, 90 * enternancePos[_index].normalized.x);
                }
                if (!generateEnternance)
                {
                    //generate wall instead if room
                    ent = CreateWall(enternancePos[_index], enternanceSize, roomHolder);
                    ent.rotation = Quaternion.Euler(0, 90 * enternancePos[_index].normalized.x, 0);
                }

                if (!hallway)
                {
                    //floor on corridor
                    for (int j = 0; j < 2; j ++)
                    {
                        int mod = j % 2 == 0 ? 1 : -1;
                        size = new Vector3((_roomSize.x - enternanceSize.x) / 2, (_roomSize.x - enternanceSize.x) / 2, 1);
                        pos = new Vector3(enternancePos[_index].normalized.x * (_roomSize.x + enternanceSize.x) / 4 + enternancePos[_index].normalized.z * (enternanceSize.x + size.x) / 2, -mod * _roomSize.y / 2, enternancePos[_index].normalized.z * (_roomSize.x + enternanceSize.x) / 4 - enternancePos[_index].normalized.x * (enternanceSize.x + size.y) / 2);
                        ent = CreateFloorTile(roomHolder, pos, size, mod, roofFloorPrefab[j]);
                    }

                    //generate wall on corridor
                    size = new Vector3((_roomSize.x - enternanceSize.x) / 2, _roomSize.y, wallThicknes);
                    for (int j = -1; j < 3; j += 2)
                    {
                        pos = new Vector3((enternancePos[_index].x) * Mathf.Abs(enternancePos[_index].normalized.x) + ((_roomSize.z + enternanceSize.x) / 4 * j) * Mathf.Abs(enternancePos[_index].normalized.z), 0, (enternancePos[_index].z) * Mathf.Abs(enternancePos[_index].normalized.z) + ((_roomSize.x + enternanceSize.x) / 4 * j) * Mathf.Abs(enternancePos[_index].normalized.x));
                        ent = CreateWall(pos, size, roomHolder);
                        ent.rotation = Quaternion.Euler(0, 90 * enternancePos[_index].normalized.x, 0);
                    }
                }

            }
            else
            {
                //generate wall on central floor piece
                pos = new Vector3((enternanceSize.x - wallThicknes) / 2 * enternancePos[_index].normalized.x, 0, (enternanceSize.x - wallThicknes) / 2 * enternancePos[_index].normalized.z);;
                ent = CreateWall(pos, enternanceSize, roomHolder);
                ent.rotation = Quaternion.Euler(0, 90 * enternancePos[_index].normalized.x, 0);
            }
        }

        return _room;
    }

    private void OnValidate()
    {
        //UpdateMapOptions();
    }

    private Transform CreateFloorTile(Transform _parent, Vector3 _pos, Vector3 _size, int isFloor, Transform _prefab)
    {
        Transform tempFloor;

        tempFloor = Instantiate(_prefab, _pos, Quaternion.identity);

        tempFloor.rotation = Quaternion.Euler(90 * isFloor,0,0);
        tempFloor.localScale = _size;
        tempFloor.parent = _parent;

        return tempFloor;
    }

    private Transform CreateWall(Vector3 _pos, Vector3 _size, Transform _parent)
    {
        Transform tempWall;
        tempWall = Instantiate(wallPrefab, _pos, Quaternion.identity) ;
        tempWall.localScale = _size;
        tempWall.parent = _parent;
        tempWall.parent = _parent;

        return tempWall;
    }

    private Transform CreateEnternance(float _wallThicknes, Transform _parent, Vector3 _pos, Vector3 _enternanceSize, Vector3 _doorSize)
    {
        //create enternances
        //generate enternance prefab
        Transform entpref = new GameObject("door").transform;
        entpref.localScale = _enternanceSize;
        //enternance walls
        Transform entel;
        for (int i = -1; i <= 1; i += 2)
        {
            entel = Instantiate(wallPrefab, entpref.position, Quaternion.identity);
            entel.localScale = new Vector3((_enternanceSize.x - _doorSize.x) / 2, _enternanceSize.y, _enternanceSize.z);
            entel.parent = entpref;
            entel.position = Vector3.right * (_enternanceSize.x / 2 - (_enternanceSize.x - _doorSize.x) / 4) * i;
        }

        //top at doors
        entel = Instantiate(wallPrefab, entpref.position, Quaternion.identity);
        entel.localScale = new Vector3(_doorSize.x, _enternanceSize.y - _doorSize.y, _enternanceSize.z);
        entel.parent = entpref;
        entel.position = Vector3.up * ((_enternanceSize.y / 2) - (_enternanceSize.y - _doorSize.y) / 2);

        //set enternance pos
        entpref.position = _pos;
        entpref.LookAt(_parent);
        entpref.parent = _parent;

        return entpref;
    }

    public void GenerateSingleRoom()
    {
        ClearMapGenerator();

        Vector3 roomSize;
        //getting room size (height is always 5)
        switch (roomSizeMenu)
        {
            case RoomSize.size_5x5:
                roomSize = roomsSize[0];
                break;

            case RoomSize.size_10x10:
                roomSize = roomsSize[1];
                break;

            case RoomSize.size_20x20:
                roomSize = roomsSize[2];
                break;
            default:
                roomSize = roomsSize[0];
                break;
        }

        //getting room type and options for that type
        int roomType;
        switch (roomTypeMenu)
        {
            case RoomType.Room:
                roomType = 1;
                break;
            case RoomType.Hallway:
                roomType = 2;
                break;
            default:
                roomType = 1;
                break;
        }
        //getting options for connection type
        int connectionType;
        switch (connectionTypeMenu)
        {
            case ConnectionType.oneEnternance:
                connectionType = 1;
                break;
            case ConnectionType.Stright:
                connectionType = 5;
                break;
            case ConnectionType.Corner:
                connectionType = 2;
                break;
            case ConnectionType.Tcross:
                connectionType = 3;
                break;
            case ConnectionType.Xcross:
                connectionType = 4;
                break;
            default:
                connectionType = 1;
                break;
        }
        mapRooms.Add(GenerateRoom(roomSize, roomType, connectionType));
    }

    public void ClearMapGenerator()
    {
        foreach (Room _room in mapRooms)
        {
            DestroyImmediate(_room.roomHolder.gameObject);
        }
        mapRooms.Clear();
        foreach (Transform _obj in transform)
        {
            DestroyImmediate(_obj.gameObject);
        }
    }

    public void ResetMapGeneratorOptions()
    {
        
    }

    [System.Serializable]
    public class Room
    {
        public Transform roomHolder;
        public List<Transform> enternances;
    }

    enum RoomSize
    {
        size_5x5, size_10x10, size_20x20
    }

    public enum RoomType
    {
        Room, Hallway
    }

    public enum ConnectionType
    {
        Stright, Corner, Tcross, Xcross, oneEnternance
    }

}
