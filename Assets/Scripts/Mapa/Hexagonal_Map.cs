using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;

    public GameObject hexagPrefab;

    [Header("Forests GameObjects")]

    public GameObject Prefap_Forest_Alpine_Mountain;
    public GameObject Prefap_Forest_Alpine_Hill;
    public GameObject Prefap_Forest_Alpine;

    [Header("Tiles Meshes")]
    public Mesh M_Ocean;
    public Mesh M_Coast;
    public Mesh M_Plains;
    public Mesh M_Mountain;
    public Mesh M_Hill;

    [Header("Materials")]
    public Material MatOcean;
    public Material MatMountains;
    public Material MatGrassLand;
    public Material MatWater;
    public Material MatPlains;
    public Material MatDesert;
    public Material MatSnow;
    public Material MatTropical;
    public Material MatTundre;
    public Material MatTest;

    [Header("Do not touch")]
    public int nRows = 0; 
    public int nColumns = 0;

    public bool allow_Wrap_EW;
    public bool allow_Wrap_NS;


    //Altitude
    public float A_Mountain = 0.0f;
    public float A_Hill = 0.0f;
    public float A_Plains = 0.0f;
    public float A_Coast = 0.0f;
    public float A_Tropical = 0.0f;

    //Humidty
    public float H_RainForest = 0.0f;
    public float H_Forest = 0.0f;
    public float H_GrassPlains = 0.0f;
    public float H_Plains = 0.0f;

    //Temperature
    public float T_tropical = 0.0f;
    public float T_Desert = 0.0f;
    public float T_Ice = 0.0f;
    public float T_Tundre = 0.0f;

    //Probality gen forest
    public float P_ForestMountain = 0.0f;
    public float P_ForestHill = 0.0f;
    public float P_Forest = 0.0f;


    public float MapTemperature;

    void Start ()
    {
        GetOptionsValors();

        GenMap();
    }

    private Hexagon[,] hexagons;
    private Dictionary<Hexagon, GameObject> Hex2GOMap;
    private Dictionary<GameObject, Hexagon> Go2HexMap;

    public Hexagon GetHexagonAt(int x, int y)
    {
        if(hexagons == null)
        {
            throw new UnityException("hexgons not instanciated");
            return null;
        }

        if (allow_Wrap_EW)
        {
            x = x % nColumns;

            if(x < 0)
                x += nColumns;
            
        }
        if (allow_Wrap_NS)
        {
            y = y % nRows;

            if (y < 0)
                y += nRows;
        }
        return hexagons[x, y];        
    }


   virtual public void GenMap()
   {

        hexagons = new Hexagon[nColumns, nRows];
        Hex2GOMap = new Dictionary<Hexagon, GameObject>();
        Go2HexMap = new Dictionary<GameObject, Hexagon>();
        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {
                Hexagon h = new Hexagon(this,column, row);
                h.Altitude = -0.5f;
                hexagons[column,row ] = h;

                

                Vector3 pos = h.PositionFCamera(
                    Camera.main.transform.position,
                    nRows,
                    nColumns,
                    allow_Wrap_EW,
                    allow_Wrap_NS
                    );

                //pos.y = +0.8355f;

                    GameObject GO = (GameObject)Instantiate(
                    hexagPrefab,
                    pos,
                    Quaternion.identity,
                    this.transform
                    );

                Hex2GOMap[h] = GO;
                Go2HexMap[GO] = h;
    
                GO.name = string.Format("HEX: {0},{1}", column, row);

                GO.GetComponent<HexagonComponent>().Hex = h;
                GO.GetComponent<HexagonComponent>().HexMap = this;
            }
        }
        UpdateHVisual();

     //   StaticBatchingUtility.Combine(this.gameObject);
    }
    public void UpdateHVisual()
    {
        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {

                Hexagon h = hexagons[column, row];
                GameObject GO = Hex2GOMap[h];
                GO.transform.SetPositionAndRotation(new Vector3(h.Position().x, 0f, h.Position().z), GO.transform.rotation);

                MeshRenderer mr = GO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mfr = GO.GetComponentInChildren<MeshFilter>();

                //- -------------------------- Altitude

                if (h.Altitude >= A_Mountain)
                {
                    mr.material = MatMountains;
                    mfr.mesh = M_Mountain;
                }
                else if (h.Altitude > A_Hill)
                {
                    mr.material = MatPlains;
                    mfr.mesh = M_Hill;
                }                    
                else if (h.Altitude >= A_Plains)
                {
                    mr.material = MatPlains;
                    mfr.mesh = M_Plains;
                }                    
                else if (h.Altitude >= A_Coast)
                {
                    mr.material = MatWater;
                    mfr.mesh = M_Coast;
                }                    
                else
                {
                    mr.material = MatOcean;
                    mfr.mesh = M_Ocean;
                }


                //- -------------------------- humidify & temperature
                if (h.Altitude >= A_Plains && h.Altitude < A_Mountain)
                {
                    if (h.Humidify >= H_RainForest)
                    {
                        mr.material = MatGrassLand;
                    }
                    else if (h.Humidify >= H_GrassPlains)
                    {                      
                        if(h.Altitude <= A_Tropical && h.Temperature > T_tropical)
                            mr.material = MatTropical;                        
                        else
                            mr.material = MatGrassLand;                        
                    }
                    else if (h.Humidify > H_Forest)
                    {
                        mr.material = MatPlains;
                        //mr.material = MatTest;
                        float temp = Random.Range(0.0f, 1.0f);

                        if (temp < P_ForestHill  /**/ && h.Altitude < A_Hill) // añadir un enum a los hexagonos, y a la vez que se reparten las alturas asignarlo como la tile que es( hill, plain, mountain etc.)
                        {
                            GameObject.Instantiate(Prefap_Forest_Alpine_Hill, GO.transform.position, Quaternion.identity, GO.transform);
                        }
                        else if (temp < P_Forest  /**/ && h.Altitude > A_Plains) // añadir un enum a los hexagonos, y a la vez que se reparten las alturas asignarlo como la tile que es( hill, plain, mountain etc.)
                        {
                            GameObject.Instantiate(Prefap_Forest_Alpine, GO.transform.position, Quaternion.identity, GO.transform);
                        }

                    }                    
                    else if (h.Humidify >= H_Plains)
                    {
                        mr.material = MatPlains;
                    }
                    else
                    {
                        if (h.Temperature > T_Desert)
                            mr.material = MatDesert;
                        else if (h.Temperature < T_Ice)
                            mr.material = MatSnow;
                        else if (h.Temperature < T_Tundre)
                            mr.material = MatTundre;
                        else
                            mr.material = MatTropical;
                    }
                }
                else if (h.Altitude > A_Mountain && h.Humidify > H_Forest)
                {
                    float temp = Random.Range(0f,1f);

                    if(temp < P_ForestMountain)
                    {
                       GameObject.Instantiate(Prefap_Forest_Alpine_Mountain,GO.transform.position, Quaternion.identity, GO.transform);
                    }
                }
            }
        }
    }
    public Hexagon[] GetHexagonsInRange(Hexagon centerH, int range)
    {
        List<Hexagon> ret = new List<Hexagon>();
        
        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = Mathf.Max((-range + 1),( -dx - range)); dy < (Mathf.Min(range,( -dx + range - 1))); ++dy)
            {
               // ret.Add(hexagons[centerH.C + dx, centerH.R + dy]);
                ret.Add(GetHexagonAt(centerH.C + dx, centerH.R + dy) );
            }
        }

        return ret.ToArray();

    }

    private void GetOptionsValors()
    {
        //Row Columns
        nRows = OptionsManager.GetComponent<OptionsManager>().map_Rows;
        nColumns = OptionsManager.GetComponent<OptionsManager>().map_Columns;
        //Altitudes
        A_Mountain = OptionsManager.GetComponent<OptionsManager>().A_Mountain;
        A_Hill = OptionsManager.GetComponent<OptionsManager>().A_Hill;
        A_Plains = OptionsManager.GetComponent<OptionsManager>().A_Plains;
        A_Coast = OptionsManager.GetComponent<OptionsManager>().A_Coast;
        A_Tropical = OptionsManager.GetComponent<OptionsManager>().A_Tropical;
        // Humidity
        H_RainForest = OptionsManager.GetComponent<OptionsManager>().H_RainForest;
        H_Forest = OptionsManager.GetComponent<OptionsManager>().H_Forest;
        H_GrassPlains = OptionsManager.GetComponent<OptionsManager>().H_GrassPlains;
        H_Plains = OptionsManager.GetComponent<OptionsManager>().H_Plains;
        //Temperatures
        MapTemperature = OptionsManager.GetComponent<OptionsManager>().mapGlobalTemperature;

        T_tropical = OptionsManager.GetComponent<OptionsManager>().T_tropical;
        T_Desert = OptionsManager.GetComponent<OptionsManager>().T_Desert;
        T_Ice = OptionsManager.GetComponent<OptionsManager>().T_Ice;
        T_Tundre = OptionsManager.GetComponent<OptionsManager>().T_Tundre;

        //Wrap
        allow_Wrap_EW = OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest;
        allow_Wrap_NS = OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth;

        //Forest prob

        P_ForestMountain = OptionsManager.GetComponent<OptionsManager>().P_ForestMountain;
        P_ForestHill = OptionsManager.GetComponent<OptionsManager>().P_ForestHill;
        P_Forest = OptionsManager.GetComponent<OptionsManager>().P_Forest;

    }
}

