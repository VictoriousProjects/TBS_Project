using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;

    public GameObject hexagPrefab;

    public Mesh M_Ocean;
    public Mesh M_Coast;
    public Mesh M_Land;
    public Mesh M_Mountain;
    public Mesh M_Hill;

    public Material MatOcean;
    public Material MatMountains;
    public Material MatLand;
    public Material MatWater;
    public Material MatPlains;
    public Material MatDirt;
    public Material MatSnow;


    public float A_Mountain = 1.3f;
    public float A_Hill = 0.8f;
    public float A_Plains = 0.1f;
    public float A_Coast = -0.5f;
 
    public readonly int nRows = 30; //80- 128 mapa enorme en cv - test 66-106
    public readonly int nColumns = 60;


    [Header("Do not touch")]
    public bool allow_Wrap_EW;
    public bool allow_Wrap_NS;

    void Start ()
    {
        A_Mountain = OptionsManager.GetComponent<OptionsManager>().A_Mountain;
        A_Hill = OptionsManager.GetComponent<OptionsManager>().A_Hill;
        A_Plains = OptionsManager.GetComponent<OptionsManager>().A_Plains;
        A_Coast = OptionsManager.GetComponent<OptionsManager>().A_Coast;

        allow_Wrap_EW = OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest;
        allow_Wrap_NS = OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth;

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
            {
                x += nColumns;
            }

        }
        if (allow_Wrap_NS)
        {
            y = y % nRows;

            if (y < 0)
            {
                y += nRows;
            }
        }


        try
        {
            return hexagons[x, y];
        }
        catch
        {
            Debug.LogError("GetHexagonAt: " + x + "," + y);
            return null;
        }
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
                
                MeshRenderer mr = GO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mfr = GO.GetComponentInChildren<MeshFilter>();

                if (h.Altitude >= A_Mountain)
                {
                    mr.material = MatMountains;
                    mfr.mesh = M_Mountain;
                }
                else if (h.Altitude > A_Hill)
                {
                    mr.material = MatLand;
                    mfr.mesh = M_Hill;
                }                    
                else if (h.Altitude >= A_Plains)
                {
                    mr.material = MatPlains;
                    mfr.mesh = M_Land;
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
}

