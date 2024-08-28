using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //---------Deklarace promìnnıch---------
    Rigidbody rb;
    Animator objectWithAnim;
    bool running;
    public float movementSpeed = 50f;
    //--------------------------------------

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        objectWithAnim = GameObject.FindGameObjectWithTag("Animobject").GetComponent<Animator>();
        Cursor.visible = false; //Skryje kurzor myši pøi spuštìní scény nebo hry
    }

    void Update()
    {


        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && //Kontroluje, zda jsou souèasnì stisknuty klávesy W a LeftShift. Tato podmínka zajišuje, e postava bude bìet vpøed pouze tehdy, kdy jsou tyto dvì klávesy stisknuty.
            !objectWithAnim.GetBool("Aim") && //Kontroluje, zda postava neaimuje. Pokud není aim aktivní (hodnota false), pak podmínka projde.
            !objectWithAnim.GetCurrentAnimatorStateInfo(0).IsName("Inspect")) //Tato èást kontroluje, zda postava nevykonává animaci s názvem "Inspect". Pokud tato animace není aktivní (hodnota false), pak podmínka projde.
        {
            //Pokud jsou všechny tyto podmínky splnìny, tedy postava bìí vpøed, jsou provedeny následující akce:
            rb.AddRelativeForce(new Vector3(0, 0, (movementSpeed + 5) * Time.deltaTime));//Pøidá relativní sílu (AddRelativeForce) k Rigidbody postavy tak, aby se pohybovala vpøed. Síla závisí na hodnotì movementSpeed a èase Time.deltaTime (co zajišuje plynulost pohybu).
            objectWithAnim.SetBool("Run", true);//Nastaví parametr "Run" v Animatoru na hodnotu true, co mùe spustit bìhovou animaci postavy.
            running = true;
        }
        else//V pøípadì, e podmínka v if neplatí (tj. hráè nepøidruje klávesu W a/nebo LeftShift, aimuje nebo vykonává animaci "Inspect"), pak se provedou akce uvedené ve "else" èásti:
        {
            objectWithAnim.SetBool("Run", false);//Nastaví parametr "Run" v Animatoru na hodnotu false, co mùe zastavit bìhovou animaci postavy.
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
    //volá vdy, kdy ubìhne pevnı èasovı krok urèenı pro fyzikální vıpoèty. Je vhodná pro manipulaci s fyzikou, protoe její volání nezávisí na rychlosti snímání obrazovky (FPS), co mùe bıt nepravidelné.
    private void FixedUpdate()//Deklaruje metodu, která bude volána automaticky kadı fyzikální krok.
    {
        rb.AddForce(Physics.gravity);// Pøidává sílu gravitace k objektu, kterı je ovládán danım Rigidbody (rb).
                                     //Physics.gravity: Jedná se o vıchozí hodnotu gravitace v Unity.
                                     //Je to vektor, kterı udává smìr a velikost gravitaèní síly.
                                     //Vìtšinou smìøuje dolù ve smìru osi Y s velikostí odpovídající normální hodnotì gravitace na Zemi (cca -9.81 m/s^2).
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