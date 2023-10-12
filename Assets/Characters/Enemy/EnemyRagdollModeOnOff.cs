using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdollModeOnOff : MonoBehaviour
{
    public BoxCollider MainCollider;
    public GameObject EnemyRig;
    public Animator EnemyAnimator;

    void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
        Debug.Log("Enemy script working!");
    }


    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "swordhitbox")
        {
            Debug.Log("Hit by sword!");
            RagdollModeOn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "swordhitbox")
        {
            Debug.Log("Detected sword it inside OnTriggerEnter!");
            RagdollModeOn();
        }
    }


    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollBits()
    {
        ragdollColliders = EnemyRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = EnemyRig.GetComponentsInChildren<Rigidbody>();
    }


    void RagdollModeOn()
    {
        EnemyAnimator.enabled = false;

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody body in limbsRigidbodies)
        {
            body.isKinematic = false;
        }


        MainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void RagdollModeOff()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody body in limbsRigidbodies)
        {
            body.isKinematic = true;
        }

        EnemyAnimator.enabled = true;
        MainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
