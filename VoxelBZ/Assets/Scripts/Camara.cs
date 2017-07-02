using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class Camara : MonoBehaviour
{
    private float velocidad = 0.3f;

    public static Vector3 poscuboinvisible;

    public int estado;
    public int contador;
    public int x_value;
    public int y_value;
    public int z_value;

    public InputField valorx;
    public InputField valory;
    public InputField valorz;

    private int layer;

    private Vector3 vectorref = Vector3.zero;

    private GameObject camara;

    // Use this for initialization
    void Start () {
        camara = GameObject.Find("Camera");
        /*
        layer = LayerMask.GetMask("Terreno");

        if (valorx == null) { valorx = GameObject.Find("X_valor").GetComponent<InputField>(); }
        if (valory == null) { valory = GameObject.Find("Y_valor").GetComponent<InputField>(); }
        if (valorz == null) { valorz = GameObject.Find("Z_valor").GetComponent<InputField>(); }
        x_value = int.Parse(valorx.text);
        y_value = int.Parse(valory.text);
        z_value = int.Parse(valorz.text);
        poscuboinvisible = new Vector3((x_value / 2) + Control_principal.xinicio, 500, (y_value / 2) + Control_principal.zinicio);
        RaycastHit rayocuboinvisible;
        if (Physics.Raycast(poscuboinvisible, Vector3.down, out rayocuboinvisible, 300, layer))
        {
            poscuboinvisible.y = rayocuboinvisible.point.y + 0.5f;
        }
        */
        estado = 1;
        contador = 0;
	}

    // Update is called once per frame
    void Update() {

    }

    private void LateUpdate()
    {
            /*
            moverArriba();
            moverAbajo();
            moverDerecha();
            moverIzquierda();
            */

    }

    public void moverAdelante()
    {
        transform.Translate(Vector3.forward * 0.1f);
        transform.Translate(Vector3.up * 0.1f);
    }

    public void moverArriba()
    {
        if (estado == 1)
        {
            transform.Translate(Vector3.up * velocidad);
            transform.Rotate(velocidad, 0, 0);
            if(transform.eulerAngles.x > 89)
            {
                contador++;
                estado = 2;
            }
        }
    }

    public void moverAbajo()
    {
        if (estado == 2)
        {
            transform.Translate(Vector3.down * velocidad);
            transform.Rotate(-velocidad, 0, 0);
            if(transform.eulerAngles.x < 1)
            {
                contador++;
                estado = 1;
            }
            else if(contador > 4 && transform.eulerAngles.x > 44 && transform.eulerAngles.x < 45)
            {
                estado = 3;
            }
        }
    }

    public void moverDerecha()
    {
        if(estado == 3)
        {
            transform.Translate(Vector3.right * velocidad);
            transform.Rotate(0,(-velocidad), velocidad);
            if (transform.eulerAngles.y < 1)
            {
                contador++;
                estado = 4;
            }
        }
    }


    public void moverIzquierda()
    {
        if (estado == 4)
        {
            transform.Translate(Vector3.left * velocidad);
            transform.Rotate(0, (velocidad), -velocidad);
            if (transform.eulerAngles.y > 89)
            {
                contador++;
                estado = 3;
            }
            else if (contador > 8 && transform.eulerAngles.y > 44 && transform.eulerAngles.y < 45)
            {
                estado = 1;
                contador = 0;
            }
        }
    }
}
