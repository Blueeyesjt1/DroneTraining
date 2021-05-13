using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerDetection : MonoBehaviour
{
    GameObject[] propellers = new GameObject[4];

    private void Start() {
        propellers[0] = transform.root.GetComponent<movement>().frontLeftFan;
        propellers[1] = transform.root.GetComponent<movement>().backLeftFan;
        propellers[2] = transform.root.GetComponent<movement>().frontRightFan;
        propellers[3] = transform.root.GetComponent<movement>().backRightFan;

        for (int i = 0; i < propellers.Length; i++)
            propellers[i].GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Danger") {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.red;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.red;
            }
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.tag == "Danger") {
            for (int i = 0; i < propellers.Length; i++) {
                propellers[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                propellers[i].GetComponent<MeshRenderer>().sharedMaterials[1].color = Color.blue;
            }
        }
    }
}
