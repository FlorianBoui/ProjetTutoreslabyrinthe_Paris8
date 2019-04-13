using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGame : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> tab = new List<GameObject>();
    List<GameObject> tabrandomize = new List<GameObject>();
    private System.Random rnd = new System.Random();
    public Camera MainCamera;
    GameObject firstSelect = null;
    GameObject secondSelect = null;
    Vector3 mem;
    private int swap1, swap2;

    void Start()
    {

        int random_piece = 0;
        GameObject ptype = null;
        int inc = 8;
        int f2;
        for(int f = 0; f < 9; f++)
        {
            f2 = f + 1;
            string imagename = "penta" + f2.ToString();
            ptype = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Renderer rend = ptype.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load(imagename) as Texture;
            ptype.tag = "piece" + f.ToString();   
            tabrandomize.Add(ptype);
        }
        for (float i = -0.52f; i < 1; i += 0.52f)
        {
            for (float j = -0.52f; j < 1; j += 0.52f)
            {
                random_piece = rnd.Next(inc);
                tabrandomize[random_piece].transform.position = new Vector3(transform.position.x + i, transform.position.y, transform.position.z + j);
                tabrandomize[random_piece].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                tabrandomize[random_piece].transform.rotation = Quaternion.Euler(0, 90, 0);
                tab.Add(tabrandomize[random_piece]);
                tabrandomize.Remove(tabrandomize[random_piece]);
                inc--;
            }
        }
    

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag.Substring(0, 5) == "piece") { 
                    if (firstSelect == null)
                    {
                        firstSelect = hit.transform.gameObject;
                    }
                    else
                    {
                        secondSelect = hit.transform.gameObject;
                        mem = hit.transform.gameObject.transform.position;
                        secondSelect.transform.position = firstSelect.transform.position;
                        firstSelect.transform.position = mem;
                        for (int swap = 0; swap < 9; swap++)
                        {
                            if (tab[swap] == firstSelect)
                                swap1 = swap;
                            if (tab[swap] == secondSelect)
                                swap2 = swap;
                        }
                        tab[swap1] = secondSelect;
                        tab[swap2] = firstSelect;
                        firstSelect = null;
                        secondSelect = null;
                        for (int check = 0; check < 9; check++)
                        {
                            if (tab[check].tag != "piece" + check.ToString())
                            {
                                break;
                            }
                            if(check == 8)
                            {
                                MainCamera.GetComponent<CameraController>().ingame = false;
                                MainCamera.GetComponent<CameraController>().gamedone = true;
                            }
                        }
                    }
                    }
                // the object identified by hit.transform was clicked
                // do whatever you want
            }
        }
    }
}
