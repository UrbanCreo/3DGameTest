using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{

    float osa_x;
    float osa_y;
    float mousesensitivi = 12;
    GameObject ak;
    GameObject pistol;
    Text ammoText;
    Animator objectWithAnim;

    void Start()
    {
        ak = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
        pistol = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        ammoText = GameObject.FindGameObjectWithTag("ammo").GetComponent<Text>();
        objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        osa_x -= Input.GetAxis("Mouse Y") * mousesensitivi;
        osa_y += Input.GetAxis("Mouse X") * mousesensitivi;

        osa_x = Mathf.Clamp(osa_x, -45, 45);

        Camera.main.transform.localEulerAngles = new Vector3(osa_x, 0, 0);
        transform.localEulerAngles = new Vector3(0, osa_y, 0);

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ChangeWeapon();
        }
    }
    void ChangeWeapon()
    {
        if (ak.activeSelf)
        {
            ak.SetActive(false);
            pistol.SetActive(true);
            ammoText.text = pistol.GetComponent<Shoot>().naboje.ToString() + "/" + pistol.GetComponent<Shoot>().maxNaboje.ToString();
        }
        else
        {
            pistol.SetActive(false);
            ak.SetActive(true);
            ammoText.text = ak.GetComponent<Shoot>().naboje.ToString() + "/" + ak.GetComponent<Shoot>().maxNaboje.ToString();
        }
        objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();

        GetComponent<Move>().ChangeWeapon(objectWithAnim);
    }
}