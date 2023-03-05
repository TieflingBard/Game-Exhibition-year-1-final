using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SingleUseCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam1;

    private void OnEnable()
    {
        CameraShift1.Register(cam1);
    }

    private void OnDisable()
    {
        CameraShift1.Unregister(cam1);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraShift1.SwitchCamera(cam1);
            this.enabled = false;
        }

    }
}