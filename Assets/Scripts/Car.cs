using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private const float IN_RANGE_OF_PLAYER = 2;
    private const float SPEED_SCALE = 350;
    [SerializeField]
    public Vector2 DirectionVector;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public GameObject RayCaster;
    private RoadScript carController;

    public Vector2 DeleteCarAt;
    private bool collisionOccured = false;
    private GameObject player;

    public void Awake() {
        player =  GameObject.FindWithTag("Player");
        carController = GameObject.FindWithTag("TrafficController").GetComponent<RoadScript>();
    }

    void DebugPlayerLines()
    {
        float distance = Vector2.Distance(RayCaster.transform.position, player.transform.position);
        if (distance <= IN_RANGE_OF_PLAYER)
        {
            Debug.DrawLine(RayCaster.transform.position, player.transform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(RayCaster.transform.position, player.transform.position, Color.black);
        }
    }

    bool CheckInRangeOfPlayer()
    {
        float distance = Vector2.Distance(RayCaster.transform.position, player.transform.position);
        if (distance <= IN_RANGE_OF_PLAYER)
        {
            Debug.DrawLine(RayCaster.transform.position, player.transform.position, Color.red);
        }
        else
        {
            Debug.DrawLine(RayCaster.transform.position, player.transform.position, Color.black);
        }
        return distance <= IN_RANGE_OF_PLAYER;
    }

    void Update()
    {
        DebugPlayerLines();
        if (!collisionOccured)
        {
            this.transform.Translate(DirectionVector * Speed / SPEED_SCALE);
        }
        if (DirectionVector.y > 0 && this.transform.position.y >= DeleteCarAt.y || DirectionVector.y < 0 && this.transform.position.y <= DeleteCarAt.y)
        {
            Object.Destroy(gameObject);
        }
    }

    private IEnumerator WaitForPlayer() {
        yield return new WaitForSeconds(0.2f);
        this.collisionOccured = this.CheckInRangeOfPlayer();
        if (this.collisionOccured) {
            StartCoroutine(WaitForPlayer());
        } else {
            yield return new WaitForSeconds(1f);
            carController.PlayerLeft();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !this.collisionOccured)
        {
            this.collisionOccured = true;
            carController.PlayerCollided();
            StartCoroutine(WaitForPlayer());
            this.transform.Translate(-DirectionVector * Speed / SPEED_SCALE);
            player.GetComponent<Health>().TakePercentageDamage(0.75f); // 75% of the player's health for this
        }
    }
}
