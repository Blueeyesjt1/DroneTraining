using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public Text droneLogs;
    public GameObject frontLeftFan;
    public GameObject frontRightFan;
    public GameObject backLeftFan;
    public GameObject backRightFan;
    public GameObject drone;

    public GameObject[] propellers = new GameObject[4];
    public List<GameObject> dangers = new List<GameObject>();

    String nearestThreat;

    public bool closeThreat = false;

    public AudioClip hitSound;

    float power = 0;

    private void Start() {
        propellers[0] = frontLeftFan;
        propellers[1] = frontRightFan;
        propellers[2] = backLeftFan;
        propellers[3] = backRightFan;
        drone = backRightFan.transform.root.gameObject;
    }

    void Update()
    {
        if (Mathf.Abs(dangers[0].transform.position.x - drone.transform.position.x) < 10 && Mathf.Abs(dangers[0].transform.position.y - drone.transform.position.y) < 10 && Mathf.Abs(dangers[0].transform.position.z - drone.transform.position.z) < 10) {
            nearestThreat = "<color=red>" + new Vector3(Mathf.Abs(dangers[0].transform.position.x - drone.transform.position.x), Mathf.Abs(dangers[0].transform.position.y - drone.transform.position.y), Mathf.Abs(dangers[0].transform.position.z - drone.transform.position.z)) + "</color>";
            closeThreat = true;
        }
        else {
            nearestThreat = (new Vector3(Mathf.Abs(dangers[0].transform.position.x - drone.transform.position.x), Mathf.Abs(dangers[0].transform.position.y - drone.transform.position.y), Mathf.Abs(dangers[0].transform.position.z - drone.transform.position.z))).ToString();
            closeThreat = false;
        }
            droneLogs.text =
                    "Position = " + gameObject.transform.position +
            "\n" + "Rotation = " + gameObject.transform.localEulerAngles +
            "\n" + "Velocity = " + Math.Round(gameObject.GetComponent<Rigidbody>().velocity.magnitude * 1.75f * 0.68f, 3) + " mph" +
            "\n" + "Turning = " + Input.GetAxisRaw("Mouse X") + ", " + Input.GetAxisRaw("Mouse Y") +
            "\n" + "Upward power = " + power +
            "\n" + "Nearest threat = " + nearestThreat.ToString() +
            "\n" +
            "\n" +
            "\n" +
            "\n";

        StartCoroutine(propellorSpeed());

        Movement();

        DangerUpdates();
    }

    void DangerUpdates() {
        for(int i = 0; i < dangers.Count; i++) {

            if (i != dangers.Count - 1) {
                float posX = Mathf.Abs(dangers[i].transform.position.x - drone.transform.position.x);
                float posY = Mathf.Abs(dangers[i].transform.position.y - drone.transform.position.y);
                float posZ = Mathf.Abs(dangers[i].transform.position.z - drone.transform.position.z);

                float posXP = Mathf.Abs(dangers[i + 1].transform.position.x - drone.transform.position.x);
                float posYP = Mathf.Abs(dangers[i + 1].transform.position.y - drone.transform.position.y);
                float posZP = Mathf.Abs(dangers[i + 1].transform.position.z - drone.transform.position.z);


                if ((posX + posY + posZ) / 3 > (posXP + posYP + posZP) / 3) {

                    GameObject tempNow = dangers[i];

                    dangers[i] = dangers[i + 1];
                    dangers[i + 1] = tempNow;
                }
            }
        }

        Debug.DrawLine(drone.transform.position, dangers[0].transform.position, Color.red);
    }

    private void OnCollisionEnter(Collision collision) {
        print("Collision impact velocity: " + collision.relativeVelocity.magnitude);

        if (collision.relativeVelocity.x > .5f || collision.relativeVelocity.y > .5f || collision.relativeVelocity.z > .5f) {
            StartCoroutine(DeletesoundEmpty(collision));
        }
    }

    IEnumerator DeletesoundEmpty(Collision col) {
        GameObject emptySoundClip = GameObject.Instantiate(new GameObject("test"), drone.transform.position, drone.transform.rotation);
        emptySoundClip.AddComponent<AudioSource>();
        emptySoundClip.GetComponent<AudioSource>().clip = hitSound;
        emptySoundClip.GetComponent<AudioSource>().volume = .1f * (col.relativeVelocity.x + col.relativeVelocity.y + col.relativeVelocity.z);
        emptySoundClip.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(1f, 1.5f);
        emptySoundClip.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2);     //Wait for sound to finish
        Destroy(emptySoundClip);
    }

    void Movement() {

        StartCoroutine(floatDown());

        gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, power, 0, ForceMode.Force);

        if (Input.GetKey(KeyCode.W)) {
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.x) < 100) {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(-200 * Time.deltaTime, 0, 0, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, .2f, ForceMode.Force);
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.x) < 100) {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(200 * Time.deltaTime, 0, 0, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, -.2f, ForceMode.Force);
            }

        }

        if (Input.GetKey(KeyCode.A)) {
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.z) < 100) {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -200 * Time.deltaTime, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().AddRelativeTorque(-.2f, 0, 0, ForceMode.Force);
            }

        }
        else if (Input.GetKey(KeyCode.D)) {
            if (Mathf.Abs(gameObject.GetComponent<Rigidbody>().velocity.z) < 100) {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 200 * Time.deltaTime, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().AddRelativeTorque(.2f, 0, 0, ForceMode.Force);
            }
        }

        if (Input.GetAxisRaw("Mouse X") != 0) {
            gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, 5 * Input.GetAxisRaw("Mouse X"), 0, ForceMode.Force);
        }

        if (Input.GetAxisRaw("Mouse Y") != 0) {
            gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 5 * Input.GetAxisRaw("Mouse Y"), ForceMode.Force);
        }

        if ((backLeftFan.transform.position.y + frontLeftFan.transform.position.y) / 2 != (backRightFan.transform.position.y + frontRightFan.transform.position.y) / 2)
            gameObject.GetComponent<Rigidbody>().AddRelativeTorque(-1f * (((backLeftFan.transform.position.y + frontLeftFan.transform.position.y) / 2) - ((backRightFan.transform.position.y + frontRightFan.transform.position.y) / 2)), 0, 0, ForceMode.Force);

        if ((frontRightFan.transform.position.y + frontLeftFan.transform.position.y) / 2 != (backRightFan.transform.position.y + backLeftFan.transform.position.y) / 2)
            gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, .1f * (((frontRightFan.transform.position.y + frontLeftFan.transform.position.y) / 2) - ((backRightFan.transform.position.y + backLeftFan.transform.position.y) / 2)), ForceMode.Force);

        if (Input.GetKey(KeyCode.C))
            gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
    }

    IEnumerator floatDown() {
        if (!Input.GetKey(KeyCode.Space) && power > 0) {
            while (power > 0 && !Input.GetKey(KeyCode.Space)) {
                power -= 20f * Time.deltaTime;
                yield return new WaitForSeconds(.01f);
            }
        }
        else if (Input.GetKey(KeyCode.Space)) {
            if (power < 20 & Input.GetKey(KeyCode.Space)) {
                power += 20f * Time.deltaTime;
                yield return new WaitForSeconds(.01f);
            }
        }

    }

    IEnumerator propellorSpeed() {
        if (power > 0 || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Mouse X") != 0) || gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude > .1f) {
            for(int i = 0; i < propellers.Length; i++) {
                if (propellers[i].transform.GetComponent<Animator>().speed < 1f) {
                    while (propellers[i].transform.GetComponent<Animator>().speed < 1f && Input.GetKey(KeyCode.Space)) {
                        propellers[i].transform.GetComponent<Animator>().speed += .01f;
                        yield return new WaitForSeconds(.01f);
                    }
                }
            }
        }
        else {
            for (int i = 0; i < propellers.Length; i++) {
                if (propellers[i].transform.GetComponent<Animator>().speed > 0 && !Input.GetKey(KeyCode.Space)) {
                    while (propellers[i].transform.GetComponent<Animator>().speed > 0) {
                        propellers[i].transform.GetComponent<Animator>().speed -= .01f;
                        if (propellers[i].transform.GetComponent<Animator>().speed < 0)
                            propellers[i].transform.GetComponent<Animator>().speed = 0;
                        yield return new WaitForSeconds(.01f);
                    }
                }
            }
        }


        gameObject.transform.GetComponent<AudioSource>().volume = .1f * gameObject.GetComponent<Rigidbody>().velocity.magnitude;
    }
}
