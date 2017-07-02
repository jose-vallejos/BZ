using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using EZObjectPools;

public class Control_principal : MonoBehaviour
{
    public static List<float> _rays;
    public static Dictionary<Vector3, GameObject> _objetosposicion;
    public static Dictionary<int, Objeto_principal> _Objprincipalposicion;

    private const float espacio_cubo = 1f;
    private const float timepoejecucion = 0.2f;

    public GameObject cubovirtual;
    private Objeto_principal Instanciaobjetoprinciapl;
    private Objeto_principal destruir;

    public static int idAhora;
    private int id;
    public static int tamañoX;
    public static int tamañoY;
    public static int tamañoZ;
    public static int tamañoXremovido;
    public static int tamañoYremovido;
    public static int tamañoZremovido;

    public int inicioX;
    public int inicioY;
    public int inicioZ;
    public int inicio;
    public int tamaño;
    public const float xinicio = 100;
    public const float zinicio = 100;
    public const float yinicio = 2500;
    private static int iteraciones;
    public static int estado;

    public static bool bzreaction = false;
    public static bool activar = false;

    public Slider K;
    public Slider treshold_G;
    public Slider treshold_Gabajo;

    public InputField valorx;
    public InputField valory;
    public InputField valorz;
    public int x_value;
    public int y_value;
    public int z_value;
    public InputField int1;
    public InputField int2;
    public InputField int3;
    public InputField int4;
    public InputField int5;
    private int int1_value;
    private int int2_value;
    private int int3_value;
    private int int4_value;
    private int int5_value;

    private int layer;

    private MaterialPropertyBlock prop;

    private EZObjectPool objectPool;

    private Vector3 pos;
    private Vector3 randomVector;
    public static GameObject cuboInvisible;

    public static Camera camara;

    private void Awake()
    {
        objectPool = EZObjectPool.CreateObjectPool(cubovirtual, "Objetos", 300, true, true, false);
        layer = LayerMask.GetMask("Terreno");

        if (valorx == null) { valorx = GameObject.Find("X_valor").GetComponent<InputField>(); }
        if (valory == null) { valory = GameObject.Find("Y_valor").GetComponent<InputField>(); }
        if (valorz == null) { valorz = GameObject.Find("Z_valor").GetComponent<InputField>(); }
        x_value = int.Parse(valorx.text);
        y_value = int.Parse(valory.text);
        z_value = int.Parse(valorz.text);
        if (int1 == null) { int1 = GameObject.Find("primerint").GetComponent<InputField>(); }
        if (int2 == null) { int2 = GameObject.Find("segundoint").GetComponent<InputField>(); }
        if (int3 == null) { int3 = GameObject.Find("tercerint").GetComponent<InputField>(); }
        if (int4 == null) { int4 = GameObject.Find("cuartoint").GetComponent<InputField>(); }
        if (int5 == null) { int5 = GameObject.Find("quintoint").GetComponent<InputField>(); }
        int1_value = int.Parse(int1.text);
        int2_value = int.Parse(int2.text);
        int3_value = int.Parse(int3.text);
        int4_value = int.Parse(int4.text);
        int5_value = int.Parse(int5.text);

        /*
        //cubo invisible que esta al medio por si lo necesito
        poscuboinvisible = new Vector3((x_value / 2) + xinicio, 500, (y_value / 2) + zinicio);
        RaycastHit rayocuboinvisible;
        if (Physics.Raycast(poscuboinvisible, Vector3.down, out rayocuboinvisible, 300, layer))
        {
            poscuboinvisible.y = rayocuboinvisible.point.y + 0.5f;
        }

        cuboInvisible = Instantiate(cuboinvisible, poscuboinvisible, Quaternion.identity);//el y es arbitrario
        */
    }

    // Use this for initialization
    void Start()
    {
        if (K == null) { K = GameObject.Find("K").GetComponent<Slider>(); }
        if (treshold_G == null) { treshold_G = GameObject.Find("thresholdG").GetComponent<Slider>(); }
        if (treshold_Gabajo == null) { treshold_Gabajo = GameObject.Find("tresholdGabajo").GetComponent<Slider>(); }

        camara = Camera.main;
        prop = new MaterialPropertyBlock();

        generarCubos();

        randomVector = Random.insideUnitSphere.normalized;
        //Invoke("BZreaction", timepoejecucion);
    }

    // Update is called once per frame
    void Update()
    {
        BZreaction();

    }
 
    private void FixedUpdate()
    {
        moverCamara();
        if (bzreaction)
        {
            if (iteraciones % 10 == 0 && iteraciones != 0 && estado < 20)
            {
                moverX();
                estado++;
            }
            else if (iteraciones % 10 == 0 && iteraciones != 0 && estado < 40)
            {
                moverZ();
                estado++;
            }
            else if (iteraciones % 10 == 0 && iteraciones != 0 && estado < 60)
            {
                removeX();
                estado++;
            }
            else if (iteraciones % 10 == 0 && iteraciones != 0 && estado < 80)
            {
                removeZ();
                estado++;
            }
            else if (estado >= 80)
            {
                estado = 0;
            }
        }
    }

    public void moverCamara()
    {
        if (activar)
        {
            //Vector3 posrandom = new Vector3(objeto.transform.position.x, objeto.transform.position.y, objeto.transform.position.z );
            //camara.transform.position = Vector3.Slerp(camara.transform.position, posrandom, (Time.deltaTime +Time.smoothDeltaTime) * 0.3f / 2f);
            //arreglar como encontrar el del medio con respecto al terreno 3*
            Vector3 posicion = new Vector3(((tamañoX + tamañoXremovido) / 2), tamañoY / 2, ((tamañoZ + tamañoZremovido) / 2));
            GameObject objeto = Posicion[posicion];
            Vector3 posreferencia = (objeto.transform.position - camara.transform.position);
            Quaternion _look = Quaternion.LookRotation(posreferencia.normalized);
            float distance = Vector3.Distance(objeto.transform.position, camara.transform.position);
            if (distance > 50)
            {
                randomVector = new Vector3(Random.value + Random.Range(0, 2), Random.value, Random.value + Random.Range(0, 2)).normalized;
            }

            if (camara.transform.position.z > objeto.transform.position.z)
            {
                randomVector = new Vector3(-Random.Range(-1, 1), Random.Range(-1, 1), -Random.value);
            }
            else if (camara.transform.position.x > objeto.transform.position.x)
            {
                randomVector = new Vector3(-Random.value, Random.Range(-1, 1), -Random.Range(-1, 1));
            }
            if (camara.transform.position.y < objeto.transform.position.y + 10)
            {
                randomVector = new Vector3(Random.Range(-1, 1), Random.value, Random.Range(-1, 1));
            }
            camara.transform.Translate(randomVector * 0.1f);
            camara.transform.rotation = Quaternion.Slerp(camara.transform.rotation, _look, (Time.deltaTime + Time.smoothDeltaTime) * 1.9f / 2f);
        }
        else
        {
            RaycastHit hit;
            /*
            float y = objeto.transform.position.y;
            if (Physics.Raycast(new Vector3(camara.transform.position.x, camara.transform.position.y, camara.transform.position.z), Vector3.up, out hit, 300, layer))
            {
                y = hit.point.y;
            }
            else if (Physics.Raycast(new Vector3(camara.transform.position.x, camara.transform.position.y, camara.transform.position.z), Vector3.down, out hit, 300, layer))
            {
                y = hit.point.y;
            }
        */
            //arreglar como encontrar el del medio
            Vector3 posicion = new Vector3(((tamañoX + tamañoXremovido) / 2), tamañoY / 2, ((tamañoZ + tamañoZremovido) / 2));
            GameObject objeto = Posicion[posicion];
            Vector3 posreferencia = (objeto.transform.position - camara.transform.position);
            Quaternion _look = Quaternion.LookRotation(posreferencia.normalized);
            float distance = Vector3.Distance(objeto.transform.position, camara.transform.position);
            Vector3 posrandom = new Vector3(objeto.transform.position.x - (((tamañoX + tamañoXremovido) / 2) * 3 + 50), objeto.transform.position.y + 35, objeto.transform.position.z - (((tamañoZ + tamañoZremovido) / 2) * 3 + 50));
            //camara.transform.position = Vector3.Slerp(camara.transform.position, posrandom, (Time.deltaTime + Time.smoothDeltaTime) * 1.9f / 2f);
            camara.transform.rotation = Quaternion.Slerp(camara.transform.rotation, _look, (Time.deltaTime + Time.smoothDeltaTime) * 1.9f / 2f);
        }
    }
 
    public static List<float> Listarayos
    {
        get
        {
            if (_rays == null)
            {
                _rays = new List<float>();
            }
            return _rays;
        }
    }

    public static Dictionary<Vector3, GameObject> Posicion
    {
        get
        {
            if (_objetosposicion == null)
            {
                _objetosposicion = new Dictionary<Vector3, GameObject>();
            }
            return _objetosposicion;
        }
    }

    public static Dictionary<int, Objeto_principal> Objprincipal
    {
        get
        {
            if (_Objprincipalposicion == null)
            {
                _Objprincipalposicion = new Dictionary<int, Objeto_principal>();
            }
            return _Objprincipalposicion;
        }
    }

    public void crearObjeto(int x, int y, int z)
    {
        Vector3 posicion = new Vector3(espacio_cubo * x + xinicio, 0, espacio_cubo * z + zinicio);
        Vector3 posicionfacil = new Vector3(espacio_cubo * x, espacio_cubo * y, espacio_cubo * z);
        RaycastHit hit_arribaizquierda;
        RaycastHit hit_arribaderecha;
        RaycastHit hit_abajoizquierda;
        RaycastHit hit_abajoderecha;
        Listarayos.Clear();
        if (Physics.Raycast(new Vector3(espacio_cubo * x + xinicio - 0.5f, espacio_cubo * y + yinicio, espacio_cubo * z + zinicio + 0.5f), Vector3.down, out hit_arribaizquierda, 600, layer))
        {
            Listarayos.Add(hit_arribaizquierda.point.y);
            //posicion.y = hit.point.y +y*espacio_cubo + 1.5f;
        }
        if (Physics.Raycast(new Vector3(espacio_cubo * x + xinicio + 0.5f, espacio_cubo * y + yinicio, espacio_cubo * z + zinicio + 0.5f), Vector3.down, out hit_arribaderecha, 600, layer))
        {
            Listarayos.Add(hit_arribaderecha.point.y);
        }
        if (Physics.Raycast(new Vector3(espacio_cubo * x + xinicio - 0.5f, espacio_cubo * y + yinicio, espacio_cubo * z + zinicio - 0.5f), Vector3.down, out hit_abajoizquierda, 600, layer))
        {
            Listarayos.Add(hit_abajoizquierda.point.y);
        }
        if (Physics.Raycast(new Vector3(espacio_cubo * x + xinicio + 0.5f, espacio_cubo * y + yinicio, espacio_cubo * z + zinicio - 0.5f), Vector3.down, out hit_abajoderecha, 600, layer))
        {
            Listarayos.Add(hit_abajoderecha.point.y);
        }
        posicion.y = Listarayos.Min() + y * espacio_cubo + 0.5f;
        Color color = new Color(Random.value, Random.value, Random.value);
        GameObject outobject;
        if (objectPool.TryGetNextObject(posicion, Quaternion.identity, out outobject))
        {
            //instanciacubovirtual[x, y, z] = outobject;
            //instanciacubovirtual[x, y, z].name = string.Format("{0}.virtual", this.id);
            //setPropertyColor(color, instanciacubovirtual[x, y, z]);

            Instanciaobjetoprinciapl = new Objeto_principal(color, posicionfacil);
            //Objprincipal.Add(Instanciaobjetoprinciapl.id, Instanciaobjetoprinciapl);
            this.id = idAhora++;
            /*
            if(idAhora > 1200 && Objeto_principal.idAhora > 1200)
            {
                idAhora = 0;
                Objeto_principal.idAhora = 0;
            }
            */
            outobject.name = string.Format("{0}.virtual", this.id);
            setPropertyColor(color, outobject);
            Posicion.Add(posicionfacil, outobject);
            outobject.GetComponent<MeshRenderer>().enabled = true;
        }
    }



    public void moverObjeto(int x, int y, int z)
    {
        //saco mi cubo de escena
        pos = new Vector3(x, y, z);
        Posicion[pos].GetComponent<MeshRenderer>().enabled = false;
        Posicion[pos].transform.position = new Vector3(-1.5f * x - 100, -100 - 1.5f * y, -1.5f * z - 100);
        int id = int.Parse(Posicion[pos].name.Split('.')[0]);
        Objprincipal[id].killObj();
        Objprincipal[id] = null;
        Objprincipal.Remove(id);
        objectPool.AddToAvailableObjects(Posicion[pos]);
        Posicion.Remove(pos);
    }

    /*
    //Funcion que esconde los objetos principales del otro script, lo elimino por que no los instancio
    public void esconderObjetoprincipal()
    {
        for(int x = 0; x <tamañoX; x++)
        {
            for (int y = 0; y < tamañoY; y++)
            {
                for (int z = 0; z < tamañoZ; z++)
                {
                    Instanciaobjetoprinciapl[x, y, z].desactivar();
                }
            }
        }
    }
    */

    public void generarCubos()
    {
        inicio = 0;
        tamaño = 1;

        tamañoX = tamaño;
        tamañoY = tamaño;
        tamañoZ = tamaño;

        tamañoXremovido = 0;
        tamañoZremovido = 0;

        inicioX = inicio;
        inicioY = inicio;
        inicioZ = inicio;

        crearObjeto(inicioX, inicioY, inicioZ);

        for (int i = 1; i < x_value; i++)
        {
            addX();
        }
        for (int i = 1; i < z_value; i++)
        {
            addY();
        }
        for (int i = 1; i < y_value; i++)
        {
            addZ();
        }

    }

    //agrega al eje x una fila de cubos
    public void addX()
    {
        tamañoX++;
        for (int x = tamañoX - 1; x < tamañoX; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = inicioZ; z < tamañoZ; z++)
                {

                    crearObjeto(x, y, z);
                }
            }
        }
    }

    public void moverX()
    {
        tamañoX++;
        for (int x = tamañoX - 1; x < tamañoX; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = tamañoZremovido; z < tamañoZ; z++)
                {

                    crearObjeto(x, y, z);
                }
            }
        }
    }

    //quita un pedazo del principio del eje X
    public void removeX()
    {
        tamañoXremovido++;
        for (int x = tamañoXremovido - 1; x < tamañoXremovido; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = tamañoZremovido; z < tamañoZ; z++)
                {
                    moverObjeto(x, y, z);
                }
            }
        }
    }

    //agrega al eje y una fila de cubos
    public void addY()
    {
        tamañoY++;
        for (int x = inicioX; x < tamañoX; x++)
        {
            for (int y = tamañoY - 1; y < tamañoY; y++)
            {
                for (int z = inicioZ; z < tamañoZ; z++)
                {

                    crearObjeto(x, y, z);
                }
            }
        }
    }


    //agrega al eje z una fila de cubos
    public void addZ()
    {
        tamañoZ++;
        for (int x = inicioX; x < tamañoX; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = tamañoZ - 1; z < tamañoZ; z++)
                {

                    crearObjeto(x, y, z);
                }
            }
        }
    }

    public void moverZ()
    {
        tamañoZ++;
        for (int x = tamañoXremovido; x < tamañoX; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = tamañoZ - 1; z < tamañoZ; z++)
                {

                    crearObjeto(x, y, z);
                }
            }
        }
    }

    public void removeZ()
    {
        tamañoZremovido++;
        for (int x = tamañoXremovido; x < tamañoX; x++)
        {
            for (int y = inicioY; y < tamañoY; y++)
            {
                for (int z = tamañoZremovido - 1; z < tamañoZremovido; z++)
                {
                    moverObjeto(x, y, z);
                }
            }
        }

    }

    public void Destruirtodo()
    {
        idAhora = 0;
        Objeto_principal.destruirtodoaca();
        objectPool.ClearPool();
        Posicion.Clear();
        Objprincipal.Clear();
        Listarayos.Clear();
        objectPool.InstantiatePool();
        iteraciones = 0;
        estado = 0;
        if (valorx == null) { valorx = GameObject.Find("X_valor").GetComponent<InputField>(); }
        if (valory == null) { valory = GameObject.Find("Y_valor").GetComponent<InputField>(); }
        if (valorz == null) { valorz = GameObject.Find("Z_valor").GetComponent<InputField>(); }
        x_value = int.Parse(valorx.text);
        y_value = int.Parse(valory.text);
        z_value = int.Parse(valorz.text);
        if (int1 == null) { int1 = GameObject.Find("primerint").GetComponent<InputField>(); }
        if (int2 == null) { int2 = GameObject.Find("segundoint").GetComponent<InputField>(); }
        if (int3 == null) { int3 = GameObject.Find("tercerint").GetComponent<InputField>(); }
        if (int4 == null) { int4 = GameObject.Find("cuartoint").GetComponent<InputField>(); }
        if (int5 == null) { int5 = GameObject.Find("quintoint").GetComponent<InputField>(); }
        int1_value = int.Parse(int1.text);
        int2_value = int.Parse(int2.text);
        int3_value = int.Parse(int3.text);
        int4_value = int.Parse(int4.text);
        int5_value = int.Parse(int5.text);
        generarCubos();

    }

    public void setPropertyColor(Color color, GameObject objeto)
    {
        prop.Clear();
        prop.SetColor("_Color", color);
        objeto.GetComponent<Renderer>().SetPropertyBlock(prop);
    }

    public Color dibujarRojo(Color color)
    {
        Color32 color32 = color;
        if(color32.r >= int1_value && color32.r < int2_value)
        {
            //color32 = new Color32(255, 255, 255, 255);
        }
        else if(color32.r >= int2_value && color32.r < int3_value)
        {
            //color32 = new Color32(255, 182, 182, 255);
        }
        else if(color32.r >= int3_value && color32.r < int4_value)
        {
            //color32 = new Color32(255, 63, 63, 255);
        }
        else if(color32.r >= int4_value && color32.r <= int5_value)
        {
            //color32 = new Color32(255, 0, 0, 255);
        }
        Color nuevo_color = color32;
        return nuevo_color;
    }

    private void BZreaction()
    {
        if (bzreaction)
        {

            //esconderObjetoprincipal();
            foreach (Objeto_principal objeto in Objprincipal.Values)
            {
                objeto.Colorahora = objeto.Color;
                float Rmedia = 0;
                float Gmedia = 0;
                float Bmedia = 0;

                int contador = 0;

                objeto.Vecinos.ForEach(o =>
                {
                    Rmedia += o.Colorahora.r;
                    Gmedia += o.Colorahora.g;
                    Bmedia += o.Colorahora.b;
                    contador++;
                });
                Rmedia /= contador;
                Gmedia /= contador;
                Bmedia /= contador;
                contador = 0;

                float R_nuevo = (Rmedia + K.value * Rmedia * (Gmedia - Bmedia));
                if (R_nuevo > 1) R_nuevo = 1;
                if (R_nuevo < 0) R_nuevo = 0;
                float G_nuevo = (Gmedia + K.value * Gmedia * (Bmedia - Rmedia));
                if (G_nuevo > 1) G_nuevo = 1;
                if (G_nuevo < 0) G_nuevo = 0;
                float B_nuevo = (Bmedia + K.value * Bmedia * (Rmedia - Gmedia));
                if (B_nuevo > 1) B_nuevo = 1;
                if (B_nuevo < 0) B_nuevo = 0;

                Color nuevo_color = new Color(R_nuevo, G_nuevo, B_nuevo);
                objeto.Color = nuevo_color;
                objeto.setColor();

                setPropertyColor(dibujarRojo(nuevo_color), Posicion[objeto.position]);

                float g = objeto.Color.g;
                float mult = g * 100;
                if (mult > treshold_G.value || mult < treshold_Gabajo.value)
                {
                    Posicion[objeto.position].GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    Posicion[objeto.position].GetComponent<MeshRenderer>().enabled = true;
                }
            }
            iteraciones++;
        }
    }
    //Invoke("BZreaction", timepoejecucion);
}
