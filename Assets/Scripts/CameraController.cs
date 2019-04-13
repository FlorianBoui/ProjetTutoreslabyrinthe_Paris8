using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    public bool ingame = false;
    public bool gamedone = false;
	void Update () {
        if(ingame == false)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
            transform.rotation = player.transform.rotation;
        }

    }

}
