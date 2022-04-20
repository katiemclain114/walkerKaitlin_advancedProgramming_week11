using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameHandler : MonoBehaviour
{
    public GameObject ballPrefab;
    public AllBalls allBalls = new AllBalls();

    private List<GameObject> ballGameObjects;
    private void Awake()
    {
        LoadBallsFromJSON("/saveFile.json");
    }

    private void Start()
    {
        ballGameObjects = new List<GameObject>();
        if (allBalls.ballsList.Count == 0)
        {
            Debug.Log("shes empty");
            SpawnNewBall();
        }
        else
        {
            for (int i = 0; i < allBalls.ballsList.Count; i++)
            {
                var ballsList = allBalls.ballsList;
                var ballGO = Instantiate(ballPrefab);
                ballGO.transform.position = ballsList[i].position;
                ballGO.transform.localScale = ballsList[i].scale;
                ballGO.GetComponent<MeshRenderer>().material.color = ballsList[i].color;
                ballGameObjects.Add(ballGO);
                allBalls.ballsList = ballsList;
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNewBall();
        }
    }

    private void SpawnNewBall()
    {
        var ballGO = Instantiate(ballPrefab);
        ballGameObjects.Add(ballGO);
        Ball newBall = new Ball();
        newBall.position = ballGO.transform.position;
        newBall.scale = ballGO.transform.localScale;
        newBall.color = ballGO.GetComponent<MeshRenderer>().material.color;
        allBalls.ballsList.Add(newBall);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("application quit");
        var ballsList = allBalls.ballsList;
        for (int i = 0; i < ballGameObjects.Count; i++)
        {
            ballsList[i].position = ballGameObjects[i].transform.position;
            ballsList[i].scale = ballGameObjects[i].transform.localScale;
            ballsList[i].color = ballGameObjects[i].GetComponent<MeshRenderer>().material.color;
        }
        allBalls.ballsList = ballsList;

        WriteBallsToJSON("/saveFile.json");
    }

    public void WriteBallsToJSON(string path)
    {
        string json = JsonUtility.ToJson(allBalls);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + path, json);
    }

    public void LoadBallsFromJSON(string path)
    {
        string json = File.ReadAllText(Application.dataPath + path);
        allBalls = JsonUtility.FromJson<AllBalls>(json);
    }

}

[System.Serializable]
public class Ball
{
    public Vector3 position;
    public Vector3 scale;
    public Color color;
}

[System.Serializable]
public class AllBalls
{
    public List<Ball> ballsList = new List<Ball>();
}
