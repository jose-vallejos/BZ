using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boton : MonoBehaviour {

    private Control_principal controll;

    public void Start()
    {
        controll = GameObject.Find("Control").GetComponent<Control_principal>();
    }

    public void Destruccion()
    {
        controll.Destruirtodo();
    }

    public void Bzreaction(bool value)
    {
        Control_principal.bzreaction = GetComponent<Toggle>().isOn;
    }

    public void Animar(bool value)
    {
        Control_principal.activar = GetComponent<Toggle>().isOn;
    }
     public void generarCubos()
    {
        controll.generarCubos();
    }
}
