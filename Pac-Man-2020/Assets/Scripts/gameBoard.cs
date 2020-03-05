﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gameBoard : MonoBehaviour
{
    // board dimensions
    private static int boardWidth = 30; 
    private static int boardHeight = 30;
    public static int MULTIPLIER = 10; //Score added per pill.
    private static float time = 0;
    //String Names of Game Characters for various uses. 
    public string Ghost1 = "Blinky";
    public string Ghost2 = "Inky";
    public string Ghost3 = "Clyde";
    public string PacManName = "Pac-Man-Node";
    //Point Tracker
    public int points = 0;
    //Delay before game starts again after Pac-Man hits a ghost.
    public static int DEATH_DELAY = 5;

    //Array of type GameObject initialized with board width and height
    //We are getting the positions of the game objects and then storing them at that position in this array.
    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    // Start is called before the first frame update
    void Start()
    {
        //Create an array of objects containing every objects in the scene
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
		{
            //Get the positions: 
            Vector2 pos = o.transform.position; // we use "position" (instead of "localposition") which is in the global space of Unity. 

            // we only want to store the objects in the array (pills, walls, etc.) not PacMan itself. 
            if (o.name != "Clyde" && o.name != "Pac-Man-Node" && o.name != "Game" && o.name != "Maze" && o.name != "Pills" && o.name != "Nodes" && o.name != "Background" &&  o.name != "NonNodes" && o.name != "Overlay" && o.name != "Blinky" && o.name != "Inky")
			{
                //if (o.GetComponent<Pills>() != null) {
                //    if (o.GetComponent<Pills>().isPellet || o.GetComponent<Pills>().isLargePellet) {
                //        totalPellets++;
                //    }
                //}
                Debug.Log("X: " + (int)pos.x + " Y: " + (int)pos.y + " " + o.name);
                board[(int)pos.x, (int)pos.y] = o;
                //Debug.Log(board[(int)pos.x, (int)pos.y]);
			} else
			{
                //just print this in case PacMan is found. 
                Debug.Log("Found " + o.name + " at " + pos);
			}
		}
    }
    public void score()
    {
        points += MULTIPLIER;
    }

    public void addTime(float seconds)
    {
        if (time != 0)
        {
            time += seconds;
        }
        else time += .5f;//Prevents glitching.

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(time != 0)
        {
            time -= (float)(1.0/24.0);//Every frame we substract one frame second from time bank. When bank hits zero, stop the munching sound.
            if(time < 0)
            {
                time = 0;
            }
        }
        if (time == 0)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    public void Die() //Put the death logic here.
    {
        StartCoroutine(RepositionCharactersAndDelay());
    }

    IEnumerator RepositionCharactersAndDelay()
    {
        GameObject Inky = GameObject.Find(Ghost1);
        GameObject Blinky = GameObject.Find(Ghost2);
        GameObject Clyde = GameObject.Find(Ghost3);
        GameObject PacMan = GameObject.Find(PacManName);
        //Disable Scripts for death delay.
        Inky.SetActive(false);
        Blinky.SetActive(false);
        Clyde.SetActive(false);
        PacMan.SetActive(false);
        //Reposition the character and reset all temp variables to original conditions.
        Inky.GetComponent<GhostController>().refresh();
        Blinky.GetComponent<GhostController>().refresh();
        Clyde.GetComponent<GhostController>().refresh();
        PacMan.GetComponent<PacManController>().refresh();

        //Add ready sprite here.
        yield return new WaitForSeconds(DEATH_DELAY); //Death Delay
        //Remove ready sprite here. 

        //GO -- reactivate scripts.
        Inky.SetActive(true);
        Blinky.SetActive(true);
        Clyde.SetActive(true);
        PacMan.SetActive(true);
    }

    private void Update()
    {

    }
}
