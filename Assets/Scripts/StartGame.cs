using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private int rayLength = 5;
    public GameObject Maincamera;

    void Start()
    {
        Scene gameScene = SceneManager.GetSceneByName("room");
        SceneManager.SetActiveScene(gameScene);
    }

    void Update()
    {

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 rayposition = new Vector3(transform.position.x, transform.position.y - 0.65f, transform.position.z);
        Debug.DrawRay(rayposition, forward, Color.green);

        if (Physics.Raycast(rayposition, transform.TransformDirection(Vector3.forward), out hit, rayLength))
        {
            if (Maincamera.GetComponent<CameraController>().gamedone == false)
            {
                if (hit.collider.gameObject.tag == "Table_top")
                {
                    if (Input.GetKeyDown("e"))
                    {
                        Maincamera.GetComponent<CameraController>().ingame = true;
                        Maincamera.transform.position = new Vector3(-13.40f, -25f, 19.40f);
                        Maincamera.transform.rotation = Quaternion.Euler(90, -90, 0);
                    }
                }
            }
            else
            {
                if (hit.collider.gameObject.tag == "Door_B")
                {
                    if (Input.GetKeyDown("e"))
                    {
                        
                        Scene gameScene2 = SceneManager.GetSceneByName("SampleScene");
                        SceneManager.SetActiveScene(gameScene2);
                        GameObject.Find("player").GetComponent<Opendoor>().inRoom = false;

                        SceneManager.UnloadSceneAsync("room");
                    }
                }
            }
        }
        

    }
}