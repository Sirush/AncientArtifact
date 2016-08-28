using UnityEngine;
using System.Collections;

public class CharacterMovemement : MonoBehaviour {

    public float MoveSpeed = .2f;

    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update() {
        transform.position += new Vector3(MoveSpeed, 0f) * Time.deltaTime;
    }

    void FixedUpdate() {
        var hit = new RaycastHit2D();
        var up = transform.TransformDirection(Vector2.up);

        hit = Physics2D.Raycast(transform.position, -up);
        transform.position = hit.point + new Vector2(0f, .25f);
    }
}