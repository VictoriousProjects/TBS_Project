using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;
    public GameObject hexagPrefab;

    public Material[] HexagMaterials;

    public readonly int nRows = 30; //80- 128 mapa enorme en cv - test 66-106
    public readonly int nColumns = 60;

    void Start ()
    {
        GenMap();
    }

    public void GenMap()
    {      
        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {
                Hexagon h = new Hexagon(column, row);

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

                GO.GetComponent<HexagonComponent>().Hex = h;
                GO.GetComponent<HexagonComponent>().HexMap = this;

                GO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);


                MeshRenderer mr = GO.GetComponentInChildren<MeshRenderer>();
                mr.material = HexagMaterials[Random.Range(0, HexagMaterials.Length)];
            }
        }
     //   StaticBatchingUtility.Combine(this.gameObject);
    }
    
}
