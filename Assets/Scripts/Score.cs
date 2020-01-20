using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int shotsFired;
    public int shotsHit;
    public double accuracy;
    public int enemiesKilled;
    public int damageDone;
    public int roundsSurvived;
    // consider time alive

    // Start is called before the first frame update
    void Start()
    {
        shotsFired = 0;
        shotsHit = 0;
        enemiesKilled = 0;
        damageDone = 0;
        roundsSurvived = 0;
    }

    public void CollectStats()
    {
        GameObject Auth = GameObject.FindWithTag("Auth");
        // calculate accuracy
        this.accuracy = this.shotsHit / this.shotsFired;
        Auth.GetComponent<Authentication>().SubmitScore(this.shotsFired, this.accuracy, this.enemiesKilled, this.damageDone, this.roundsSurvived);
    }
}
