using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    // Player Values
    private GameObject[] objects;
    private GameObject currentObject;
    private int currentIndex;
    public float speed; /*Editable in Editor*/
    private Rigidbody rigidBody;

    // Logic Variables
    public float TIMER_TIME; /*Editable in Editor*/
    private float timeLeft;
    private bool playable = true;
    
    // Turn Values
    public int totalTurns = 3; /*Editable in Editor*/
    private int[] turns;

    // UI Stuff
    private GameObject uiTurns;
    private Text[] turnsText;
    private Text timerText, currentObjectText;

    // Camera
    CameraController cameraController;
    
    /* Driver Functions */
    void Start()
    {
        // Declarations
        objects = new GameObject[4];
        turns = new int[objects.Length];
        turnsText = new Text[objects.Length];
        uiTurns = GameObject.Find("Turns Text");

        // Declare Texts
        GameObject tempObj = GameObject.Find("Current Object").gameObject;
        currentObjectText = tempObj.GetComponent<Text>() as Text;

        tempObj = GameObject.Find("Timer").gameObject;
        timerText = tempObj.GetComponent<Text>() as Text;
        timeLeft = TIMER_TIME;

        // Get Camera Controller
        tempObj = GameObject.Find("Main Camera").gameObject;
        cameraController = tempObj.GetComponent<CameraController>() as CameraController;
        
        // Iteratively Reference Game Objects and UI Text
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = transform.GetChild(i).gameObject;
            turns[i] = totalTurns;

            // Reference and Set Turns UI Elements
            tempObj = uiTurns.transform.GetChild(i).gameObject;
            turnsText[i] = tempObj.GetComponent<Text>() as Text;
            turnsText[i].text = turns[i].ToString();
        }

        // Initial Object Setup
        SetCurrentObject(0);
    }

    void Update()
    {
        // Only Runs if Game is Playable
        if (playable)
        {
            // Push Current Object
            float x = Input.GetAxis("Horizontal"),
                  y = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(x, 0.0f, y);
            rigidBody.AddForce(movement * speed);

            // Check for Out-of-Bounds
            if(currentObject.transform.position.y < -15)
            {
                // Disable Object
                turns[currentIndex] = 0;
                turnsText[currentIndex].text = "X";
                currentObject.transform.gameObject.SetActive(false);

                // Reset Timer
                timeLeft = TIMER_TIME;
                CycleObjects();
            }

            // Update Timer, Then Cycle
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = TIMER_TIME;
                CycleObjects();
            }
            timerText.text = Mathf.RoundToInt(timeLeft).ToString();
        }
        else
        {
            // Not Playable so Game is Over
            currentObjectText.text = "Game Over";
        }
    }

    public GameObject getCurrentObject() { return currentObject; }
     
    public void CycleObjects()
    {
        // Only do this if there are more turns left.
        if (IsMore())
        {
            // Get Random Index to Cycle
            int nextObjectIndex = Random.Range(0, 4);

            // Verify Cycle is Valid
            while (!IsValidObjectCycle(nextObjectIndex))
            { nextObjectIndex = Random.Range(0, 4); };

            // Setup Next Object
            SetCurrentObject(nextObjectIndex);
        }
    }

    private bool IsValidObjectCycle(int cycleIndex)
    {
        // Checks the validity of an object transaction
        if (currentObject == objects[cycleIndex] || turns[cycleIndex] == 0)
            return false;
        
        return true;
    }

    private bool IsMore()
    {
        // For Initial Setup
        if (currentObject == null)
            return true;

        // Checks if there exist more turns in any objects --> O(n)
        for(int i=0; i<turns.Length; i++)
            if (turns[i] > 0)
                return true;

        // End of Game
        playable = false;
        Debug.Log("No More Items - GAME OVER");
        return false;
    }

    public void SetCurrentObject(int index)
    {
        // Set Object and Change Text
        currentObject = objects[index];
        currentObjectText.text = currentObject.ToString();
        currentIndex = index;

        // Decrement Turns and Change Text
        turns[index]--;
        turnsText[index].text = turns[index].ToString();
        
        // Update Rigid Body
        rigidBody = currentObject.GetComponent<Rigidbody>();

        // Set Camera 
        cameraController.ResetRotation();
    }
}
