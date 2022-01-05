using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    public GameObject allCubes, vfx, restartButton;
    public GameObject[] canvasStartPage, cubesToCreate;
    private Rigidbody allCubesRb;
    private bool isLose, isFirstCube = true;
    private float camMoveToYPos, comeMoveSpeed = 2;
    public Color[] bgColors;
    private Color toCameraColor;
    public Text scoreTxt;

    private List<Vector3> allCubesPositions = new List<Vector3>{
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1)
    };
    private int prevMaxCountHorizontal;
    private Coroutine showCubePlace;
    private Transform mainCam;
    private List<GameObject> possibleCubesToCreate = new List<GameObject>();

    private void Start()
    {
        showCubePlace = StartCoroutine(ShowCubePlace());
        allCubesRb = allCubes.GetComponent<Rigidbody>();

        mainCam = Camera.main.transform;
        camMoveToYPos = 5.9f + nowCube.y - 1;
        toCameraColor = Camera.main.backgroundColor;

        int countOfCubes;

        if (PlayerPrefs.GetInt("score") < 5)
        {
            countOfCubes = 1;
        }
        else if (PlayerPrefs.GetInt("score") < 10)
        {
            countOfCubes = 2;
        }
        else if (PlayerPrefs.GetInt("score") < 20)
        {
            countOfCubes = 3;
        }
        else if (PlayerPrefs.GetInt("score") < 30)
        {
            countOfCubes = 4;
        }
        else if (PlayerPrefs.GetInt("score") < 45)
        {
            countOfCubes = 5;
        }
        else if (PlayerPrefs.GetInt("score") < 60)
        {
            countOfCubes = 6;
        }
        else if (PlayerPrefs.GetInt("score") < 80)
        {
            countOfCubes = 7;
        }
        else if (PlayerPrefs.GetInt("score") < 100)
        {
            countOfCubes = 8;
        }
        else
        {
            countOfCubes = 9;
        }

        for (int i = 0; i < countOfCubes; i++)
        {
            possibleCubesToCreate.Add(cubesToCreate[i]);
        }

        scoreTxt.text = "<size=15>BEST:</size> " + PlayerPrefs.GetInt("score") + "\n<size=12>NOW: </size> " + 0;
    }
    private void Update()
    {
        // Input.GetKeyDown(KeyCode.Space) || 
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        && cubeToPlace != null && allCubes != null && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began) {
                return;
            }
#endif

            if (isFirstCube)
            {
                isFirstCube = false;
                foreach (GameObject obj in canvasStartPage)
                {
                    Destroy(obj);
                }
            }

            GameObject creatingCube = null;
            if (possibleCubesToCreate.Count == 1)
            {
                creatingCube = possibleCubesToCreate[0];
            }
            else
            {
                creatingCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];
            }

            GameObject newCube = Instantiate(creatingCube,
                cubeToPlace.position,
                Quaternion.identity) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            GameObject newVfx = Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;
            Destroy(newVfx, 1.5f);

            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (allCubesRb.velocity.magnitude > 0.1f && !isLose)
        {
            mainCam.localPosition = new Vector3(mainCam.localPosition.x, 9f, mainCam.localPosition.z - 5f);
            restartButton.SetActive(true);
            isLose = true;
            StopCoroutine(showCubePlace);
            Destroy(cubeToPlace.gameObject);
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, camMoveToYPos, mainCam.localPosition.z),
            comeMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z))
            && nowCube.x + 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z))
            && nowCube.y + 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1))
            && nowCube.z - 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }

        if (positions.Count > 1)
        {
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        }
        else if (positions.Count == 0)
        {
            isLose = true;
        }
        else
        {
            cubeToPlace.position = positions[0];
        }
    }

    private bool IsPositionEmpty(Vector3 targetVector)
    {
        if (targetVector.y == 0)
        {
            return false;
        }

        foreach (Vector3 vector in allCubesPositions)
        {
            if (targetVector.x == vector.x && targetVector.y == vector.y && targetVector.z == vector.z)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHorizontal = 0;

        foreach (Vector3 vector in allCubesPositions)
        {
            if (Mathf.Abs(vector.x) > maxX)
            {
                maxX = Convert.ToInt32(vector.x);
            }
            if (vector.y > maxY)
            {
                maxY = Convert.ToInt32(vector.y);
            }
            if (Mathf.Abs(vector.z) > maxZ)
            {
                maxZ = Convert.ToInt32(vector.z);
            }
        }

        if (PlayerPrefs.GetInt("score") < maxY - 1)
        {
            PlayerPrefs.SetInt("score", maxY - 1);
        }

        scoreTxt.text = "<size=15>BEST:</size> " + PlayerPrefs.GetInt("score") + "\n<size=12>NOW: </size> " + (maxY - 1);

        camMoveToYPos = 5.9f + nowCube.y - 1;
        maxHorizontal = maxX > maxZ ? maxX : maxZ;

        if (maxHorizontal % 3 == 0 && maxHorizontal != prevMaxCountHorizontal)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2f);
            prevMaxCountHorizontal = maxHorizontal;
        }

        if (maxY >= 10)
        {
            toCameraColor = bgColors[2];
        }
        else if (maxY >= 5)
        {
            toCameraColor = bgColors[1];
        }
        else if (maxY >= 2)
        {
            toCameraColor = bgColors[0];
        }
    }
}

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 vector)
    {
        x = Convert.ToInt32(vector.x);
        y = Convert.ToInt32(vector.y);
        z = Convert.ToInt32(vector.z);
    }
}