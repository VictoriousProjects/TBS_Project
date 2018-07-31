using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKController : MonoBehaviour {
    public OptionsManager OptionsManager;
    public float moveSpeed;
    public float sprintMoveMultiplierSpeed;

    private float sprintMoveSpeed = 1.0f;
    // Use this for initialization
    void Start ()
    {
        moveSpeed = (float)OptionsManager.GetComponent<OptionsManager>().moveSpeed;
        sprintMoveMultiplierSpeed = (float)OptionsManager.GetComponent<OptionsManager>().sprintMoveMultiplierSpeed;
    }

	// Update is called once per frame
	void Update ()
    {

        Vector3 move = new Vector3 (Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));

       
        this.transform.Translate(move * moveSpeed* sprintMoveSpeed * Time.deltaTime, Space.World );


        sprintMoveSpeed = 1f;

    }
}
