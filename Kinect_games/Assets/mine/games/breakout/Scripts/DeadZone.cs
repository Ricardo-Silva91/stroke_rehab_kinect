using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.name == "Ball")
            GM.instance.LoseLife();
    }
}