using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRespawnController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam1;
    [SerializeField] CinemachineVirtualCamera cam2;

    
    [SerializeField] Transform respawnPoint1;
    [SerializeField] Transform respawnPoint2;
  
     





    // Start is called before the first frame update
    private void Awake()
    {
 
    }

    private void OnEnable()
    {
        CameraShift1.Register(cam1);
        CameraShift1.Register(cam2);
        CameraShift1.SwitchCamera(cam1);
     
    }


    private void OnDisable()
    {
        CameraShift1.Unregister(cam1);
        CameraShift1.Unregister(cam2);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            if (CameraShift1.isActiveCamera(cam1))
            {

                CameraShift1.SwitchCamera(cam2);
                PlayerRespawn.instance.respawnPoint = respawnPoint2.position;
              
            }
            else if (CameraShift1.isActiveCamera(cam2))
            {
                CameraShift1.SwitchCamera(cam1);
                PlayerRespawn.instance.respawnPoint = respawnPoint1.position;

            }
        }
    }




}


