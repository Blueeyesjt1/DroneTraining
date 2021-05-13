using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMoves : MonoBehaviour
{
    public List<GameObject> walkingCharacters = new List<GameObject>();

    int peopleCounter = 0;
    int peopleCounterOpp = 0;

    private void Start() {

        for(int i = 0; i < walkingCharacters.Count; i++)
            renameAppendage(walkingCharacters[i], 0);

        StartCoroutine(PeopleCounter());

    }

    void renameAppendage(GameObject rootChild, int counter) {
        if (rootChild.transform.childCount > 0) {
            foreach (Transform child in rootChild.transform) {
                child.name = "C" + counter++ + "-" + child.childCount;
                counter++;
                renameAppendage(child.gameObject, counter++);
                counter++;
            }
        }
    }

    IEnumerator PeopleCounter() {
        yield return new WaitForSeconds((int)Random.Range(5, 20));
        if (peopleCounter < 20)
            StartCoroutine(newCharWalk(walkingCharacters[(int)Random.Range(0, walkingCharacters.Count)]));
        yield return new WaitForSeconds((int)Random.Range(1, 5));
        if (peopleCounterOpp < 20)
            StartCoroutine(newCharWalkOpp(walkingCharacters[(int)Random.Range(0, walkingCharacters.Count)]));
        StartCoroutine(PeopleCounter());
    }

    

    IEnumerator newCharWalk(GameObject character) {

        GameObject tempChar = GameObject.Instantiate(character);

        GameObject.Find("drone").GetComponent<movement>().dangers.Add(tempChar.gameObject);

        peopleCounter++;
        tempChar.name = "Character";

        tempChar.transform.position = new Vector3(166.62f, 0.242f, -16.61f);
        
        while(tempChar.transform.position.x > -187.9f) {
            tempChar.transform.Translate(-0.075f, 0, 0, Space.World);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(tempChar);
        peopleCounter--;
    }

    IEnumerator newCharWalkOpp(GameObject character)
    {

        GameObject tempChar = GameObject.Instantiate(character);

        tempChar.transform.Rotate(0, 180, 0);

        GameObject.Find("drone").GetComponent<movement>().dangers.Add(tempChar.gameObject);

        peopleCounterOpp++;
        tempChar.name = "Character";

        tempChar.transform.position = new Vector3(-166.62f, 0.242f, -41);

        while (tempChar.transform.position.x < 188f)
        {
            tempChar.transform.Translate(0.075f, 0, 0, Space.World);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(tempChar);
        peopleCounterOpp--;
    }
}
