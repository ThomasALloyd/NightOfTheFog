using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    GameObject playerWeapon;
    public int shadehealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerWeapon = GameObject.FindWithTag("HoldWeap");
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(1)) { // right-click
        //     Debug.Log("Pressed right-click.");
        //     // ATTACK do animation
        // }

    }

    // Detects sword collision and does damage
    private void OnTriggerEnter(Collider other){
        Debug.Log("Attack collision detected");
        //sword hits stalker or pumpkin
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("if enemy");
            Destroy(other.gameObject);
            Debug.Log("enemy destroyed");
            
        }
        //sword hits shade enemy
        else if (other.gameObject.CompareTag("Shade")) {
            Debug.Log("if shade enemy");
            //removes health
            shadehealth--;
            if (shadehealth==0) {
                Destroy(other.gameObject);
                Debug.Log("destroyed");
            }
        }
    }
}
