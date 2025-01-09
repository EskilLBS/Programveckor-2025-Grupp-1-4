using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Flyttar spelaren till h�ger
            rigidbody.velocity = new Vector2(1, rigidbody.velocity.y).normalized * speed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Flyttar spelaren till v�nster
            rigidbody.velocity = new Vector2(-1, rigidbody.velocity.y).normalized * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Flyttar spelaren upp�t
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 1).normalized * speed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Flyttar spelaren ned�t
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, - 1).normalized * speed;
        }
    }
}