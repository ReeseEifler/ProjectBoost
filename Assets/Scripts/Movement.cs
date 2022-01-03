using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrust = 2000;
    [SerializeField] float rotateSpeed = 400;
    [SerializeField] AudioClip boostSound;

    AudioSource audioSource;
    Rigidbody rb;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
            if (!audioSource.isPlaying) audioSource.PlayOneShot(boostSound);
        }
        else if (audioSource.isPlaying) audioSource.Stop(); 
        
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            ApplyRotation(-rotateSpeed);
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            ApplyRotation(rotateSpeed);
    }

    void ApplyRotation(float rotation)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.right * rotation * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
