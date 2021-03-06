using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // for TMP text
//using UnityEngine.UI; //for normal text
public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Transform spawnPoint;
    public float timeFactor;
    public float swayDistance;
    public GameObject scoreboard;
    public TextMeshProUGUI score_TMP;
    public TextMeshProUGUI highscore_TMP;



    List<GameObject> cubeList = new List<GameObject>();
    int freeze = -5;
    bool isGameOver = false;

    void Start()
    {
        scoreboard.SetActive(false);
    }


    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        

        // make the spawner sway left and right
        Vector3 newPos = spawnPoint.localPosition;
        newPos.x  = Mathf.Sin(Time.time * timeFactor) * swayDistance;
        spawnPoint.localPosition = newPos;

        //check if the tower is falling
        if (cubeList.Count >= 2)
        {
            GameObject lastCube = cubeList[cubeList.Count - 1];
            GameObject lastCube2 = cubeList[cubeList.Count - 2];
            if (lastCube.transform.position.y < lastCube2.transform.position.y)
            {
                // cubes are falling
                foreach(GameObject cube in cubeList)
                {
                    cube.GetComponent<Rigidbody>().isKinematic = true;
                }

                isGameOver = true;
                scoreboard.SetActive(true);

                // get the current score and display it
                score_TMP.text = cubeList.Count.ToString();
                int currentHighscore = PlayerPrefs.GetInt("highscore");
                highscore_TMP.text = currentHighscore.ToString();
                if (cubeList.Count > currentHighscore)
                {
                    PlayerPrefs.SetInt("highscore", cubeList.Count);
                    highscore_TMP.text = currentHighscore.ToString();

                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) { 
           
            GameObject c = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
            transform.Translate(0, 0.5f, 0);

            cubeList.Add(c);
            freeze++;

            if (freeze >= 0)
            {
                cubeList[freeze].GetComponent<Rigidbody>().isKinematic = true;
            }

            // c.GetComponent<Rigidbody>().isKinematic = true;

        }
    }
}
