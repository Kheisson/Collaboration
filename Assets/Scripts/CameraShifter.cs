using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShifter : MonoBehaviour
{
    public GameObject virtualCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.SetActive(false);
    }
}
