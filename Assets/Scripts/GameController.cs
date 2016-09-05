﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public int SegmentsOfLevel = 1;
    public Transform CorridorPrefab;    // Префаб коридора
    public Transform DoorPrefab;        // Префаб двери
    public GameObject Player;           // Префаб игрока

    //private GameObject _mainCamera;

    private void Awake()
    {
        //_mainCamera = GameObject.FindWithTag("MainCamera");
        _GenerateLevel();
        Instantiate(Player);
    }

    /// <summary>
    /// Построение уровня
    /// </summary>
    private void _GenerateLevel()
    {
        GameObject levelContainer = new GameObject("Level");
        levelContainer.AddComponent<LevelData>();
        Transform prevSegment = null;

        for (int i = 0; i < SegmentsOfLevel; i++)
        {
            Transform currentSegment = Instantiate(CorridorPrefab);
            currentSegment.parent = levelContainer.transform;

            if (prevSegment != null)
            {
                float offsetX = prevSegment.position.x - prevSegment.GetComponent<CorridorScript>().LeftPoint.position.x;
                Vector3 offset = new Vector3(offsetX, 0);
                currentSegment.transform.position = prevSegment.GetComponent<CorridorScript>().RightPoint.position + offset;

            }
            else
            {
                //Устанавливаем левый край сцены
                LevelData.LeftLimiter = currentSegment.GetComponent<CorridorScript>().LeftPoint;
            }


            if (i == 0)
                _InstDoor("StartDoor", levelContainer, currentSegment.GetComponent<CorridorScript>().LeftPoint);
            if (i == SegmentsOfLevel -1)
                _InstDoor("EndDoor", levelContainer, currentSegment.GetComponent<CorridorScript>().RightPoint);

            prevSegment = currentSegment;
        }

        //Устанавливаем правый край сцены
        LevelData.RightLimiter = prevSegment.GetComponent<CorridorScript>().RightPoint;
    }

    /// <summary>
    /// Инстанцируем двери
    /// </summary>
    /// <param name="name"></param>
    /// <param name="levelContainer"></param>
    /// <param name="currentSegment"></param>
    private void _InstDoor(string name, GameObject levelContainer, Transform currentSegment)
    {
        Transform door = Instantiate(DoorPrefab);
        door.name = name;
        door.parent = levelContainer.transform;
        door.transform.position = currentSegment.transform.position;

        if (name == "EndDoor")
            door.transform.rotation = new Quaternion(0, 180, 0, 0);
    }
}
