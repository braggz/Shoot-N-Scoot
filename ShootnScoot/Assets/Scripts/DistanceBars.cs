using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceBars : MonoBehaviour
{
    public GameObject player1;
    public GameObject player1Start;
    public GameObject player2;
    public GameObject player2Start;
    public GameObject endGoal;

    public Image p1Slider;
    public Image p2Slider;

    public GameObject p1Particle;
    public GameObject p2Particle;

    private float distanceForP1;
    private float distanceForP2;
    private float distanceTillGoalForP1;
    private float distanceTillGoalForP2;
    private Vector2 particle1OriginalPosition;
    private Vector2 particle2OriginalPosition;
    private float particle1TravelDistance;
    private float particle2TravelDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        distanceForP1 = Vector3.Distance(endGoal.transform.position, player1Start.transform.position);
        distanceForP2 = Vector3.Distance(endGoal.transform.position, player2Start.transform.position);
        particle1OriginalPosition = p1Particle.transform.position;
        particle2OriginalPosition = p2Particle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Weird constant is added because player can't actually reach the position of endGoal
        distanceTillGoalForP1 = Vector2.Distance(player1.transform.position, endGoal.transform.position) - .9f;
        distanceTillGoalForP2 = Vector2.Distance(player2.transform.position, endGoal.transform.position) - .9f;

        p1Slider.fillAmount = distanceTillGoalForP1 / distanceForP1;
        p2Slider.fillAmount = distanceTillGoalForP2 / distanceForP2;

        particle1TravelDistance = -(p1Slider.rectTransform.rect.width * p1Slider.transform.localScale.x * p1Slider.fillAmount);
        p1Particle.transform.position = new Vector2(particle1OriginalPosition.x + particle1TravelDistance, particle1OriginalPosition.y);

        particle2TravelDistance = (p2Slider.rectTransform.rect.width * p2Slider.transform.localScale.x * p2Slider.fillAmount);
        p2Particle.transform.position = new Vector2(particle2OriginalPosition.x + particle2TravelDistance, particle2OriginalPosition.y);
    }
}
