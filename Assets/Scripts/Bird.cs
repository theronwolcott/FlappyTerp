using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private const float JUMP_FORCE = 100f;

    private Rigidbody2D birdBody;

    private void Awake() {
        birdBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Jump();
        }
    }

    private void Jump() {
        birdBody.velocity = Vector2.up * JUMP_FORCE;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Hit!");
    }
}
