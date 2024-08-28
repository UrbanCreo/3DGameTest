using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    RaycastHit hitInfo;
    Animator objectWithAnim;
    public float dostrel;
    public float sila;
    public float poskozeni;
    public GameObject efektTrefy;

    Image crosshair;
    ParticleSystem efektVystrelu;
    AudioSource audioSource;

    Text nabojeText;
    public int naboje;
    public int maxNaboje;
    bool reloading;
    bool pistol;
    bool canShoot = true;

    bool slider;

    void Start()
    {
        objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
        efektVystrelu = GameObject.FindGameObjectWithTag("EffectShoot").GetComponent<ParticleSystem>();
        audioSource = transform.GetComponent<AudioSource>();

        nabojeText = GameObject.FindGameObjectWithTag("ammo").GetComponent<Text>();
        maxNaboje = naboje;
        nabojeText.text = naboje.ToString() + "/" + maxNaboje.ToString();
        if (transform.tag != "ak")
        {
            pistol = true;
        }
    }

    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();
            efektVystrelu = GameObject.FindGameObjectWithTag("EffectShoot").GetComponent<ParticleSystem>();
        }

        if (Input.GetMouseButton(1) && canShoot && Input.GetMouseButtonDown(0) && pistol && !objectWithAnim.GetBool("Run") && !objectWithAnim.GetBool("Holster") && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Inspect") && naboje > 0 && !reloading && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
        {
            canShoot = false;
            objectWithAnim.SetTrigger("Shoot");
            Invoke("ShootBullet", 0);
            Invoke("CheckIfFired", 0.7f);
        }

        if (Input.GetMouseButtonDown(0) && !objectWithAnim.GetBool("Run") && !objectWithAnim.GetBool("Holster") && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Inspect") && naboje > 0 && !reloading && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
        {
            if (pistol && canShoot)
            {
                canShoot = false;
                objectWithAnim.SetTrigger("Shoot");
                Invoke("ShootBullet", 0);
                Invoke("CheckIfFired", 0.7f);
            }
            if (!pistol)
            {
                objectWithAnim.SetBool("Shoot", true);
                InvokeRepeating("ShootBullet", 0, 0.25f);
            }

        }

        if (objectWithAnim.GetBool("Aim") || objectWithAnim.GetBool("Holster"))
        {
            crosshair.enabled = false;
        }
        else
        {
            crosshair.enabled = true;
        }

        if (naboje <= 0 || reloading)
        {
            CancelInvoke("ShootBullet");
            objectWithAnim.SetBool("Shoot", false);
        }

        if (naboje <= 0 && pistol && !slider && !objectWithAnim.GetBool("Out Of Ammo Slider"))
            objectWithAnim.SetBool("Out Of Ammo Slider", true);

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("ShootBullet");
            objectWithAnim.SetBool("Shoot", false);
        }

        if (efektVystrelu.isPlaying && efektVystrelu.time >= 0.15f)
        {
            efektVystrelu.Stop();
        }
        if (objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left") || objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo"))
        {
            CancelInvoke("ShootBullet");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("test1");
            Invoke("ReturnSlider", 2);
            StartCoroutine(Reload());
        }
    }

    void ShootBullet()
    {
        efektVystrelu.Stop();
        efektVystrelu.Play();
        audioSource.Stop();
        audioSource.Play();
        naboje -= 1;
        nabojeText.text = naboje.ToString() + "/" + maxNaboje.ToString();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, dostrel))
        {
            GameObject trefa = Instantiate(efektTrefy, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(trefa, 1);
            if (hitInfo.transform.GetComponent<Rigidbody>())
            {
                hitInfo.transform.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * sila);
            }
            if (hitInfo.transform.GetComponent<EnemyHealth>())
            {
                hitInfo.transform.GetComponent<EnemyHealth>().TakeDamage(poskozeni);
            }
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        if (naboje > 0)
        {
            objectWithAnim.SetTrigger("Reload");
            yield return new WaitUntil(() => objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"));
            yield return new WaitUntil(() => !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"));
            naboje = maxNaboje;

        }
        else
        {
            objectWithAnim.SetTrigger("ReloadNoAmmo");
            yield return new WaitUntil(() => objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo"));
            yield return new WaitUntil(() => !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo"));
            naboje = maxNaboje;
        }
        nabojeText.text = naboje.ToString() + "/" + maxNaboje.ToString();
        reloading = false;
    }

    void CheckIfFired()
    {
        canShoot = true;
    }
    void ReturnSlider()
    {
        slider = true;
        objectWithAnim.SetBool("Out Of Ammo Slider", false);
    }
}