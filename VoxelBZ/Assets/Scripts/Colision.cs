using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour {
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "cubo")
        {
            Objeto_principal.getVecinos(gameObject, collision.gameObject);

        }
    }
}
