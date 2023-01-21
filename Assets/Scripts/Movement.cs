using UnityEngine;


enum FacingDirection {
    Horizontal,
    Vertical
}
enum MovementDirection {
    Up = 2,
    Down = 1,
    Right = 3,
    Left = 4,
    None = 0
}
public class Movement : MonoBehaviour
{
    private const float SPEED_SCALE = 350;
    [SerializeField]
    public float MoveSpeed;
    private FacingDirection facingDirection;
    Movement() {
        facingDirection = FacingDirection.Vertical;
    }
    public GameObject Player;
    public Animator animator;

    void Update()
    {
        if (Player.GetComponent<Health>().Dead) return;
        int vertical = 0, horzitonal = 0;
        if (Input.GetKey("w"))
        {
            facingDirection = FacingDirection.Vertical;
            vertical += 1;
            animator.SetInteger("MovingDirection", (int)MovementDirection.Up);
        }
        else if (Input.GetKey("s"))
        {
            facingDirection = FacingDirection.Vertical;
            vertical -= 1;
            animator.SetInteger("MovingDirection", (int)MovementDirection.Down);
        }
        if (Input.GetKey("a"))
        {
            facingDirection = FacingDirection.Horizontal;
            horzitonal -= 1;
            animator.SetInteger("MovingDirection", (int)MovementDirection.Left);
        }
        else if (Input.GetKey("d"))
        {
            facingDirection = FacingDirection.Horizontal;
            horzitonal += 1;
            animator.SetInteger("MovingDirection", (int)MovementDirection.Right);
        }
        animator.SetBool("FacingHorizontal", facingDirection == FacingDirection.Horizontal);
        Player.transform.Translate(new Vector3(horzitonal, vertical) * MoveSpeed / SPEED_SCALE);
        if (horzitonal == 0 && vertical == 0) {
            animator.SetInteger("MovingDirection", (int)MovementDirection.None);
        }
    }
}
