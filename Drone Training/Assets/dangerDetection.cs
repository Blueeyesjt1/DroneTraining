using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerDetection : MonoBehaviour
{
    GameObject[] propellers = new GameObject[4];
    public GameObject[] dangers;

    public GameObject drone;

    bool nearDangerCol = false;

    private void Start() {
        propellers[0] = transform.root.GetComponent<movement>().frontLeftFan;
        propellers[1] = transform.root.GetComponent<movement>().backLeftFan;
        propellers[2] = transform.root.GetComponent<movement>().frontRightFan;
        propellers[3] = transform.root.GetComponent<movement>().backRightFan;

        dangers = GameObject.FindGameObjectsWithTag("Danger");

        for (int i = 0; i < propellers.Length; i++) {
            propellers[i].GetComponent<MeshRenderer>().material.color = Color.blue;
            propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.blue;
        }

        for(int i = 0; i < dangers.Length; i++) {
            if(dangers[i].gameObject != null)
                drone.GetComponent<movement>().dangers.Add(dangers[i].gameObject);            
        }
    }

    public void Update() {
        if (drone.GetComponent<movement>().closeThreat) {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.red;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.red;
            }
        }
        else if(!nearDangerCol) {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.blue;
            }
        }

    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Danger") {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.red;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.red;
            }
            nearDangerCol = true;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.tag == "Danger") {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.blue;
            }
            nearDangerCol = false;
        }
    }
}
