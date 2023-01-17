using UnityEngine;

public class Movement : MonoBehaviour
{
    private const float SPEED_SCALE = 350;
    [SerializeField]
    public float MoveSpeed;
    public GameObject Player;

    void Update()
    {
        if (Player.GetComponent<Health>().Dead) return;
        int vertical = 0, horzitonal = 0;
        if (Input.GetKey("w"))
        {
            vertical += 1;
        }
        else if (Input.GetKey("s"))
        {
            vertical -= 1;
        }
        if (Input.GetKey("a"))
        {
            horzitonal -= 1;
        }
        else if (Input.GetKey("d"))
        {
            horzitonal += 1;
        }
        Player.transform.Translate(new Vector3(horzitonal, vertical) * MoveSpeed / SPEED_SCALE);
    }
}
