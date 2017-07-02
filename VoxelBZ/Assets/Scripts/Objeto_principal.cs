using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Objeto_principal : UnityEngine.Object {

    public List<Objeto_principal> _vecinos;
    //public static Dictionary<int, Objeto_principal> _ObjetosCargados;

    private GameObject cubofisico;
    public static GameObject cubofisicoprefab;

    public static int idAhora;
    public int id;

    public Vector3 position;

    public Color nuevo_color;
    public Color _color;
    public Color _colorahora;

    // Use this for initialization
    public Objeto_principal( Color color, Vector3 posicion)
    {
        this.id = idAhora++;
        this.position = posicion;
        //cubofisico = Instantiate(cubofisicoprefab, posicion, Quaternion.identity);
        //cubofisico.name = string.Format("{0}.fisico", this.id);
        cambiarColor(color);
        this._colorahora = color;
        Control_principal.Objprincipal.Add(this.id, this);
    }

    public void cambiarColor(Color color)
    {
        this._color = color;
    }

    public Color Colorahora
    {
        get
        {
            return _colorahora;
        }
        set
        {
            _colorahora = value;
        }
    }

    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            nuevo_color = value;
        }
    }
    
    public void setColor()
    {
        cambiarColor(nuevo_color);
    }
    /*
    public static Dictionary<int, Objeto_principal> Objetoscar
    {
        get
        {
            if(_ObjetosCargados == null)
            {
                _ObjetosCargados = new Dictionary<int, Objeto_principal>();
            }
            return _ObjetosCargados;
        }
    }
    */

    public List<Objeto_principal> Vecinos
    {
        get
        {
            if(_vecinos == null)
            {
                _vecinos = new List<Objeto_principal>();
            }
            return _vecinos;
        }
    }

    public static void getVecinos(GameObject objeto1,GameObject objeto2)
    {
        string nombre1 = objeto1.name.Split('.')[0];
        string nombre2 = objeto2.name.Split('.')[0];

        int id1;
        int id2;

        int.TryParse(nombre1, out id1);
        int.TryParse(nombre2, out id2);

        Objeto_principal original = Control_principal.Objprincipal[id1];
        Objeto_principal vecinos = Control_principal.Objprincipal[id2];

        //Objeto_principal original = Objetoscar[id1];
        //Objeto_principal vecinos = Objetoscar[id2];
        /*
        Objeto_principal original = Objetoscargados.Find(m => m.id == int.Parse(objeto1.name.Split('.')[0]));
        Objeto_principal vecinos = Objetoscargados.Find(m => m.id == int.Parse(objeto2.name.Split('.')[0]));
        */

        if (!original.Vecinos.Contains(vecinos))
        {
            original.Vecinos.Add(vecinos);
        }
        if (!vecinos.Vecinos.Contains(original))
        {
            vecinos.Vecinos.Add(original);
        }
    }

    public void killObj()
    {
        Destroy(this);
        this.Vecinos.Clear();
        //Objetoscar.Remove(this.id);
    }

    public static void destruirtodoaca()
    {
        /*
        foreach(Objeto_principal objeto in Objetoscar.Values)
        {
            objeto.Vecinos.Clear();
            Destroy(objeto);
        }
        Objetoscar.Clear();
        */
        idAhora = 0;
    }
}
