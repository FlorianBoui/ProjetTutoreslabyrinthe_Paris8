using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Opendoor : MonoBehaviour
{
    private int rayLength = 1;
    public AnimationClip DoorOpen;
    public bool inRoom = false;
    string animName = "OpenDoor";
    string levelName = "room";

    void Update()
    {
        if (inRoom == false)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayLength))
            {
                if (hit.collider.gameObject.tag == "Door_C")
                {
                    if (Input.GetKeyDown("e"))
                    {
                        //for (int j = 0; j < 60; j++)
                        //{
                        //   hit.transform.gameObject.transform.rotation = Quaternion.Euler(hit.transform.gameObject.transform.rotation.x, hit.transform.gameObject.transform.rotation.y + (90/60), hit.transform.gameObject.transform.rotation.z);
                        //}
                        GameObject doorparent = hit.transform.gameObject.transform.parent.gameObject;
                        doorparent.GetComponent<Animation>().Play("OpenDoor");
                        StartCoroutine(LoadAfterAnim(doorparent));


                    }
                }
            }
        }
    }
    public IEnumerator LoadAfterAnim(GameObject Doortageted)
    {
        yield return new WaitForSeconds(DoorOpen.length);
        SceneManager.LoadScene("room", LoadSceneMode.Additive);
        inRoom = true;
    }

}