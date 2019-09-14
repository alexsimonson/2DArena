using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Test();
    }

    void Test()
    {
        if (Input.GetButtonDown("1"))
        {
            Debug.Log("What's up");
        }
    }

}
