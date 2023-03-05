using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn instance;
    
    public Vector3 respawnPoint;


    // Start is called before the first frame update


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        respawnPoint = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Respawn")
        {
            transform.position = respawnPoint;
        }
    }








}




