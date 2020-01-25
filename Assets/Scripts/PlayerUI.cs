using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public GameObject player;
    public GameObject hudAmmo;
    public GameObject ammoLoadedCount;
    public GameObject ammoReserveCount;
    public GameObject ammoTitle;
    static public string loggedInUsername;
    static public bool loggedIn;

    void Start () {
        player = GameObject.Find ("PlayerSprite");
        hudAmmo = GameObject.Find ("hudCanvas/hudAmmo");
        ammoLoadedCount = GameObject.Find ("hudCanvas/hudAmmo/ammoLoadedCount");
        ammoReserveCount = GameObject.Find ("hudCanvas/hudAmmo/ammoReserveCount");
        ammoTitle = GameObject.Find ("hudCanvas/hudAmmo/ammoTitle");
    }

    public void SetLoggedInUsername (string setUsername) {
        loggedInUsername = setUsername;
    }

    public void SetLoggedIn (bool state) {
        loggedIn = state;
    }

    public void ToggleAmmoUI (int type) {
        if (type == 0) {
            hudAmmo.SetActive (false);
        } else {
            hudAmmo.SetActive (true);
        }
    }

    public void UpdateAmmoHud (int numBullets = 1, bool shooting = true) {
        int loadedCount = Convert.ToInt32 (ammoLoadedCount.GetComponent<TMP_Text> ().text);
        if (shooting) {
            loadedCount -= numBullets;
        } else {
            int numNeeded = numBullets - loadedCount;
            loadedCount = numBullets;
            SubtractAmmoReserve (numNeeded);
        }
        ammoLoadedCount.GetComponent<TMP_Text> ().text = loadedCount.ToString ();
    }

    public void ReloadAmmoHud (int reserveAmmo, int loadedAmmo) {
        SetAmmoReserve (reserveAmmo);
        SetAmmoLoaded (loadedAmmo);
    }

    public void SetAmmoLoaded (int magazineSize) {
        ammoLoadedCount.GetComponent<TMP_Text> ().text = magazineSize.ToString ();
    }

    public void SetAmmoReserve (int bullets) {
        ammoReserveCount.GetComponent<TMP_Text> ().text = bullets.ToString ();
    }

    public void AddAmmoLoaded (int numBullets) {
        ammoLoadedCount.GetComponent<TMP_Text> ().text = (Convert.ToInt32 (ammoLoadedCount.GetComponent<TMP_Text> ().text) + numBullets).ToString ();
    }

    public void SubtractAmmoLoaded (int numBullets) {
        ammoLoadedCount.GetComponent<TMP_Text> ().text = (Convert.ToInt32 (ammoLoadedCount.GetComponent<TMP_Text> ().text) - numBullets).ToString ();
    }

    public void AddAmmoReserve (int numBullets) {
        ammoReserveCount.GetComponent<TMP_Text> ().text = (Convert.ToInt32 (ammoReserveCount.GetComponent<TMP_Text> ().text) + numBullets).ToString ();
    }

    public void SubtractAmmoReserve (int numBullets) {
        ammoReserveCount.GetComponent<TMP_Text> ().text = (Convert.ToInt32 (ammoReserveCount.GetComponent<TMP_Text> ().text) - numBullets).ToString ();
    }
}