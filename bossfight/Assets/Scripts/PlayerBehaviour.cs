using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections;
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 direction;

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    void Start()
    {
        rigidbody2D.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void Move()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0f, 180f);
        }
        else if (direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0f, 0f);
        }

    }
    void OnMove()
    {
        rigidbody2D.linearVelocity = direction.normalized * speed;
    }

}
