using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] GameObject SoundObject;
    void Start()
    {
        StartCoroutine(GenerateSoundObject());
    }

    IEnumerator GenerateSoundObject()
    {
        GameObject NewSoundObject = Instantiate(SoundObject);
        NewSoundObject.transform.position = transform.position;

        yield return new WaitForSeconds(7f);

        StartCoroutine(GenerateSoundObject());
    }

}
