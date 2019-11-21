using UnityEngine;
using System;
using System.Collections;

public class SplitScreen : MonoBehaviour {

	/*Reference both the transforms of the two players on screen.
	Necessary to find out their current positions.*/
	public Transform player1;
	public Transform player2;

	//The distance at which the splitscreen will be activated.
	public float splitDistance = 5;

	//The two cameras, both of which are initalized/referenced in the start function.
	private GameObject camera1;
	private GameObject camera2;

    float start = .1f;
    float end = 1f;
    float t = .5f;

    void Start () {
		//Referencing camera1 and initalizing camera2.
		camera1 = Camera.main.gameObject;
		camera2 = new GameObject ();
		camera2.AddComponent<Camera> ();
		//Setting up the culling mask of camera2 to ignore the layer "TransparentFX" as to avoid rendering the split and splitter on both cameras.
		camera2.GetComponent<Camera> ().cullingMask = ~(1 << LayerMask.NameToLayer ("TransparentFX"));
        camera2.GetComponent<Camera>().orthographic = true;
	}

	void LateUpdate () {
		//Gets the z axis distance between the two players and just the standard distance.
		float zDistance = player1.position.z - player2.transform.position.z;
		float distance = Vector3.Distance (player1.position, player2.transform.position);

		//Sets the angle of the player up, depending on who's leading on the x axis.
		float angle;
		if (player1.transform.position.x <= player2.transform.position.x) {
			angle = Mathf.Rad2Deg * Mathf.Acos (zDistance / distance);
		} else {
			angle = Mathf.Rad2Deg * Mathf.Asin (zDistance / distance) - 90;
		}

		//Gets the exact midpoint between the two players.
		Vector3 midPoint = new Vector3 ((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, (player1.position.z + player2.position.z) / 2); 

		//Waits for the two cameras to split and then calcuates a midpoint relevant to the difference in position between the two cameras.
		if (distance > splitDistance) {
			Vector3 offset = midPoint - player1.position; 
			offset.x = Mathf.Clamp(offset.x,-splitDistance/2,splitDistance/2);
			offset.y = Mathf.Clamp(offset.y,-splitDistance/2,splitDistance/2);
			offset.z = Mathf.Clamp(offset.z,-splitDistance/2,splitDistance/2);
			midPoint = player1.position + offset;

			Vector3 offset2 = midPoint - player2.position; 
			offset2.x = Mathf.Clamp(offset.x,-splitDistance/2,splitDistance/2);
			offset2.y = Mathf.Clamp(offset.y,-splitDistance/2,splitDistance/2);
			offset2.z = Mathf.Clamp(offset.z,-splitDistance/2,splitDistance/2);
			Vector3 midPoint2 = player2.position - offset;

            midPoint2 = new Vector3(midPoint2.x, 0, -10);
			camera2.transform.position = Vector3.Lerp(camera2.transform.position,midPoint2,Time.deltaTime*5);
            camera1.GetComponent<Camera>().rect = new Rect(0, 0, .5f, 1);
            camera2.GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, 1);

        }
        else
        {
            //Deactivates the splitter and camera once the distance is less than the splitting distance (assuming it was at one point).
            camera1.GetComponent<Camera>().rect = new Rect(0, 0, Mathf.Lerp(start, end, t),1);
            t += 1f * Time.deltaTime;
            if(t > 1.0f)
            {
                t = end; 
            }
		}

        /*Lerps the first cameras position and rotation to that of the second midpoint, so relative to the first player
		or when both players are in view it lerps the camera to their midpoint.*/
        midPoint = new Vector3(midPoint.x, 0, -10);
		camera1.transform.position = Vector3.Lerp(camera1.transform.position,midPoint,Time.deltaTime*5);
		
	}
}
