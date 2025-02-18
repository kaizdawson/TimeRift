using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 camStartPos;
    private Vector2 travel => (Vector2)cam.transform.position - camStartPos;

    private void Awake() {
        cam = Camera.main;
    }

    private void Start() {
        startPos = transform.position;
        camStartPos = cam.transform.position;
    }

    private void FixedUpdate() {
        transform.position = startPos + travel * parallaxOffset;
    }
}
