using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        oldPos = this.transform.position;

    }
    Vector3 oldPos;

	// Update is called once per frame
	void Update () {
        // todo: click n drag
        //Wasd
        //Zoom
        CameraIsMoved(); //check camera move

    }

    public void PanHex ( Hexagon Hex)
    {
        //move camera to hex
    }

    HexagonComponent[] hexagons;

    void CameraIsMoved()
    {
        if(this.oldPos != this.transform.position)
        {
            //algo ha movido la camara
            this.oldPos = this.transform.position;

            if(hexagons ==null)
                hexagons = GameObject.FindObjectsOfType<HexagonComponent>();

            foreach( HexagonComponent h in hexagons)
            {
                h.UpdatePosition();
            }

        }
    }
}
