using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trespassing : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Player has trespassed");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Player is no longer trespassing");
        }
    }
}
