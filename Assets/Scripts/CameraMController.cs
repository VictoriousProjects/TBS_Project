using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMController : MonoBehaviour
{

	void Start () {
  

    }

    bool dragCam = false;
    Vector3 LastMPos;

	void Update ()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitcoods = mouseRay.origin + (mouseRay.direction * (mouseRay.origin.y / mouseRay.direction.y)/*Length*/ );

        if (Input.GetMouseButtonDown(0)) // down, so drag
        {
            dragCam = true;
            LastMPos = hitcoods;

        }
        else if (Input.GetMouseButtonUp(0))// up, so musnt drag
        {
            dragCam = false;
        }

        if(dragCam==true)
        {
            Vector3 dff = LastMPos - hitcoods;
            Camera.main.transform.Translate(dff, Space.World);
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            LastMPos = mouseRay.origin - (mouseRay.direction * (mouseRay.origin.y / mouseRay.direction.y));

        }
            
	}
}
