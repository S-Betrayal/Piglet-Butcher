using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public bool alive = true;
    public float hp = 100.0f;

    public List<Collider> ragdollColliders;
    public List<Rigidbody> ragdollRigidbodies;
    public GameObject body;
    
    
    void Start()
    {
        ragdollColliders = body.GetComponentsInChildren<Collider>().ToList();
        ragdollRigidbodies = body.GetComponentsInChildren<Rigidbody>().ToList();
    }
    
    void Update()
    {
        
    }

    public void GetHit(float damage)
    {
        hp -= damage;
        if (hp <= 0 && alive)
        {
            Death();
        }
    }

    private void Death()
    {
        alive = false;
        Debug.Log(gameObject.name + " was murdered.");
        RagdollEnabled(true);
    }

    public void RagdollEnabled(bool ragdollEnabled)
    {
        foreach (Collider col in ragdollColliders) col.enabled = ragdollEnabled;
        foreach (Rigidbody rb in ragdollRigidbodies) rb.isKinematic = !ragdollEnabled;
        this.GetComponent<Collider>().enabled = !ragdollEnabled;
        body.GetComponent<Animator>().enabled = !ragdollEnabled;
    }
}
