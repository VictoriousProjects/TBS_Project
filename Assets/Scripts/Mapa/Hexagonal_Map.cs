using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;

    public GameObject hexagPrefab;

    public Mesh M_Water;
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

    public float A_Hill = 0.9f;

    public float A_Plains = 0.3f;

    public float A_Coast = -0.5f;

    //public Vector2 A_Mountain = new Vector2(0.7f, 1f);
    //public Vector2 A_Plains = new Vector2(0.7f, 1f);
    //public Vector2 A_Forest = new Vector2(0.7f, 1f);
    //public Vector2 A_dirt = new Vector2(0.0f, 0.2f);


    public readonly int nRows = 30; //80- 128 mapa enorme en cv - test 66-106
    public readonly int nColumns = 60;

    void Start ()
    {
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

        if (OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest)
        {
            x = x % nColumns;

            if(x < 0)
            {
                x += nColumns;
            }

        }
        if (OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth)
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
                Hexagon h = new Hexagon(column, row);
                h.Altitude = -0.5f;
                hexagons[column,row ] = h;

                

                Vector3 pos = h.PositionFCamera(
                    Camera.main.transform.position,
                    nRows,
                    nColumns, 
                    OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest, 
                    OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth 
                    );

                GameObject GO = (GameObject)Instantiate(
                    hexagPrefab,
                    pos,
                    Quaternion.identity,
                    this.transform
                    );

                Hex2GOMap[h] = GO;

                GO.name = string.Format("HEX: {0},{1}", column, row);

                GO.GetComponent<HexagonComponent>().Hex = h;
                GO.GetComponent<HexagonComponent>().HexMap = this;

                if (OptionsManager.GetComponent<OptionsManager>().activateCoordCells)
                {
                    //if (GO.GetComponentInChildren<Renderer>().enabled == true)
                    //   GO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
                    if (GO.GetComponentInChildren<Renderer>().enabled == true)
                        GO.GetComponentInChildren<TextMesh>().text = string.Format("{0}", h.Altitude);


                }


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

              
                if (h.Altitude >= A_Mountain)
                    mr.material = MatMountains;
                else if (h.Altitude < A_Hill && h.Altitude > A_Plains)
                    mr.material = MatPlains;
                    //if (row < nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentBottomPole ||
                    //   row > nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentTopPole)
                    //{
                    //    mr.material = MatSnow;
                    //}

                else if (h.Altitude >= A_Hill && h.Altitude < A_Mountain)
                    mr.material = MatLand;
                    //if (row < nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentBottomPole ||
                    //   row > nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentTopPole)
                    //{
                    //    mr.material = MatSnow;
                    //}

                else if (h.Altitude >= A_Coast && h.Altitude < A_Plains)
                    mr.material = MatWater;
                    //if (row < nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentBottomPole ||
                    //   row > nRows * (float)OptionsManager.GetComponent<OptionsManager>().percentTopPole)
                    //{
                    //    mr.material = MatSnow;
                else
                    mr.material = MatOcean;

                MeshFilter mfr = GO.GetComponentInChildren<MeshFilter>();
                mfr.mesh = M_Water;

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

