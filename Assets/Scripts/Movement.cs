using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //---------Deklarace prom�nn�ch---------
    Rigidbody rb;
    Animator objectWithAnim;
    bool running;
    public float movementSpeed = 50f;
    //--------------------------------------

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();
        Cursor.visible = false; //Skryje kurzor my�i p�i spu�t�n� sc�ny nebo hry
    }

    void Update()
    {


        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && //Kontroluje, zda jsou sou�asn� stisknuty kl�vesy W a LeftShift. Tato podm�nka zaji��uje, �e postava bude b�et vp�ed pouze tehdy, kdy� jsou tyto dv� kl�vesy stisknuty.
            !objectWithAnim.GetBool("Aim") && //Kontroluje, zda postava neaimuje. Pokud nen� aim aktivn� (hodnota false), pak podm�nka projde.
            !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Inspect")) //Tato ��st kontroluje, zda postava nevykon�v� animaci s n�zvem "Inspect". Pokud tato animace nen� aktivn� (hodnota false), pak podm�nka projde.
        {
            //Pokud jsou v�echny tyto podm�nky spln�ny, tedy postava b�� vp�ed, jsou provedeny n�sleduj�c� akce:
            rb.AddRelativeForce(new Vector3(0, 0, (movementSpeed + 5) * Time.deltaTime));//P�id� relativn� s�lu (AddRelativeForce) k Rigidbody postavy tak, aby se pohybovala vp�ed. S�la z�vis� na hodnot� movementSpeed a �ase Time.deltaTime (co� zaji��uje plynulost pohybu).
            objectWithAnim.SetBool("Run", true);//Nastav� parametr "Run" v Animatoru na hodnotu true, co� m��e spustit b�hovou animaci postavy.
            running = true;
        }
        else//V p��pad�, �e podm�nka v if neplat� (tj. hr�� nep�idr�uje kl�vesu W a/nebo LeftShift, aimuje nebo vykon�v� animaci "Inspect"), pak se provedou akce uveden� ve "else" ��sti:
        {
            objectWithAnim.SetBool("Run", false);//Nastav� parametr "Run" v Animatoru na hodnotu false, co� m��e zastavit b�hovou animaci postavy.
            running = false;
        }

        if (!running)
        {
            if (Input.GetMouseButtonDown(0) && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Inspect") && !objectWithAnim.GetBool("Holster") && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Draw"))
            {
                objectWithAnim.SetBool("Run", false);
            }
            if (objectWithAnim.GetBool("Shoot"))
            {
                objectWithAnim.SetBool("Walk", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddRelativeForce(new Vector3(movementSpeed * Time.deltaTime, 0, 0));
                CheckAndSetWalkingAnimation();
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddRelativeForce(new Vector3(-movementSpeed * Time.deltaTime, 0, 0));
                CheckAndSetWalkingAnimation();
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddRelativeForce(new Vector3(0, 0, movementSpeed * Time.deltaTime));
                CheckAndSetWalkingAnimation();
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddRelativeForce(new Vector3(0, 0, -movementSpeed * Time.deltaTime));
                CheckAndSetWalkingAnimation();
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                objectWithAnim.SetBool("Walk", false);
            }

            if (Input.GetMouseButton(1) && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left") && !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo"))
            {
                objectWithAnim.SetBool("Aim", true);
            }
            else
            {
                objectWithAnim.SetBool("Aim", false);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                objectWithAnim.SetTrigger("Inspect");

            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                objectWithAnim.SetBool("Holster", !objectWithAnim.GetBool("Holster"));
            }
        }
    }
    //vol� v�dy, kdy� ub�hne pevn� �asov� krok ur�en� pro fyzik�ln� v�po�ty. Je vhodn� pro manipulaci s fyzikou, proto�e jej� vol�n� nez�vis� na rychlosti sn�m�n� obrazovky (FPS), co� m��e b�t nepravideln�.
    private void FixedUpdate()//Deklaruje metodu, kter� bude vol�na automaticky ka�d� fyzik�ln� krok.
    {
        rb.AddForce(Physics.gravity);// P�id�v� s�lu gravitace k objektu, kter� je ovl�d�n dan�m Rigidbody (rb).
                                     //Physics.gravity: Jedn� se o v�choz� hodnotu gravitace v Unity.
                                     //Je to vektor, kter� ud�v� sm�r a velikost gravita�n� s�ly.
                                     //V�t�inou sm��uje dol� ve sm�ru osi Y s velikost� odpov�daj�c� norm�ln� hodnot� gravitace na Zemi (cca -9.81 m/s^2).
    }

    void CheckAndSetWalkingAnimation()
    {
        if (!objectWithAnim.GetBool("Shoot"))
        {
            objectWithAnim.SetBool("Walk", true);
        }
    }

    public void ChangeWeapon(Animator objectWithAnim)
    {
        this.objectWithAnim = objectWithAnim;
    }
}