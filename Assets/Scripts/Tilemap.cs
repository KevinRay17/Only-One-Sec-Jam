using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class Tilemap : MonoBehaviour
{
    public Transform Camera;
    public GameObject selectedUnit;
    public TileType[] tileTypes;

    private int[,] tiles;

    public int mapSizeX = 3;

    public int mapSizeY = 3;
    
    public int UnitX = 0;
    public int UnitY = 4;

    public bool falling = false;

    public GameObject MapGenObj;

    public GameObject StartBlock;
    private GameObject Clone;

    private bool restarting = false;
    [ColorUsage(true,true)]
    public Color myColor = new Vector4(0,0,.7f,1);
    public Color endColor = new Vector4(.7f,0,0,1);
    
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene ();
 
        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
 
        if (sceneName == "Menu")
        {
            mapSizeX = 10;
            mapSizeY = 10;
            tiles = new int[mapSizeX, mapSizeY];
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    tiles[x, y] = 1;
                }
            }

            //Map Pathing
            tiles[0, 3] = 4;
            tiles[0, 4] = 4;
            tiles[0, 5] = 4;
            tiles[1, 2] = 4;
            tiles[2, 1] = 4;
            tiles[3, 0] = 4;
            tiles[4, 0] = 4;
            tiles[5, 0] = 4;
            tiles[6, 0] = 4;
            tiles[7, 1] = 4;
            tiles[8, 2] = 4;
            tiles[9, 3] = 4;
            tiles[9, 4] = 4;
            tiles[9, 5] = 4;
            tiles[8, 6] = 4;
            tiles[8, 7] = 4;
            tiles[7, 8] = 4;
            tiles[6, 9] = 4;
            tiles[5, 9] = 4;
            tiles[4, 9] = 4;
            tiles[3, 9] = 4;
            tiles[2, 8] = 4;
            tiles[1, 7] = 4;
            tiles[1, 6] = 4;
            tiles[2, 4] = 4;
            tiles[3, 3] = 4;
            tiles[4, 2] = 4;
            tiles[5, 2] = 4;
            tiles[6, 3] = 4;
            tiles[7, 4] = 4;
            tiles[3, 6] = 4;
            tiles[3, 7] = 4;
            tiles[6, 6] = 4;
            tiles[6, 7] = 4;
        }
        else
        {
            mapSizeX = 4;
            mapSizeY = 4;
            tiles = CreateLevel(mapSizeX, mapSizeY);
        }
        
        Physics.gravity = new Vector3(0,0,9.8f);
        
       /* tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 1;
            }
        }

        //Map Pathing
        tiles[0, 4] = 2;
        tiles[1, 4] = 0;
        tiles[2, 4] = 0;
        tiles[3, 4] = 0;
        tiles[4, 4] = 0;
        tiles[5, 4] = 0;
        tiles[6, 4] = 0;
        tiles[7, 4] = 0;
        tiles[8, 4] = 0;
       */
        
        
        UnitX = 0;
        UnitY = mapSizeY/2;
        GenerateMap();
       MoveSelectedUnitTo(UnitX,UnitY);

    }
 

    void GenerateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
               Clone = Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }  
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, -.75f);
    }
    
    public void MoveSelectedUnitTo(int x, int y)
    {
      // selectedUnit.transform.position = Vector3.Lerp(selectedUnit.transform.position, TileCoordToWorldCoord(x,y), Time.deltaTime);
        selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
    }


    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = new Vector3(mapSizeX/2,mapSizeY/2, -10);

        if (Score.instance.secondsLeft <= 0 && !restarting)
        {
            StartCoroutine(GameOver());
        }
        
       
        if (!falling)
        {
            if (Input.anyKeyDown)
            {

                if (UnitX < mapSizeX -1)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        UnitX = UnitX + 1;
                    }
                }

                if (UnitX > 0)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        UnitX = UnitX - 1;
                    }
                }

                if (UnitY < mapSizeY -1)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        UnitY = UnitY + 1;
                    }
                }

                if (UnitY > 0)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        UnitY = UnitY - 1;
                    }
                }

                MoveSelectedUnitTo(UnitX, UnitY);
            }
        }
        else
        {
            if(!restarting)
            StartCoroutine(NewLevelFail());
        }


        //Raycast floor check
        if (Physics.Raycast(selectedUnit.transform.position, transform.TransformDirection(Vector3.forward), out hit, .25f))
        {
            if (hit.collider.gameObject.CompareTag("StartPath"))
            {
                StartBlock = hit.collider.gameObject;
            }
            
            if (hit.collider.gameObject.CompareTag("OffPath"))
            {
                falling = true;
            }
            else if (hit.collider.gameObject.CompareTag("Path") && !restarting)
            { 
               hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", myColor); 
                hit.collider.gameObject.GetComponent<PathObjects>().boom = true;
                StartBlock.GetComponent<PathObjects>().boom = true;
                
                //Score
                if (!hit.collider.gameObject.GetComponent<PathObjects>().gavePoints)
                {
                    StartCoroutine(Score.instance.AddPoints());
                    hit.collider.gameObject.GetComponent<PathObjects>().gavePoints = true;
                }
            }
          if (hit.collider.gameObject.CompareTag("StartPath"))
            {
                hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", myColor);
            }

            if (hit.collider.gameObject.CompareTag("EndPath"))
            {
                hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", endColor);
                if (!restarting)
                StartCoroutine(NewLevelWin());
            }
            
            Debug.Log("Did Hit");
        }
       
    }

    //Makes an array of gameobjects on layer
    GameObject[] FindGameObjectsByLayer(int layer)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].gameObject.layer == layer)
            {
                validTransforms.Add(objs[i].gameObject);
            }
        }
        return validTransforms.ToArray();
    }
    
    void DeleteGameObjectsByLayer(int layer)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].gameObject.layer == layer)
            {
                Destroy(objs[i].gameObject);
            }
        }
    }

    IEnumerator NewLevelWin()
    {
        
        restarting = true;
        falling = true;
        GameObject[] MapObjects = FindGameObjectsByLayer(9);
      // Color StartColor =MapObjects[0].gameObject.GetComponent<Renderer>().sharedMaterial.GetColor("_EmissionColor");
      /* float colUp = 1f;
        while (colUp >= 0f)
        {
           Debug.Log("Ah");
          
           // foreach (var c in MapObjects)
            //{
                Color intensity = MapObjects[0].gameObject.GetComponent<Renderer>().sharedMaterial.GetColor("_EmissionColor");
                MapObjects[0].gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor",
                    new Color(intensity.r, intensity.g, intensity.b + 1, intensity.a));
            //} 
            
            colUp -= .2f;
            yield return 0;
        }*/

        float col = 1f;
        StartCoroutine(Score.instance.AddTime());
        yield return new WaitForSeconds(.5f);
        
       /* while (col >= 0f)
        {
            //foreach (var c in MapObjects)
            //{
                Color intensity = MapObjects[0].gameObject.GetComponent<Renderer>().sharedMaterial.GetColor("_EmissionColor");
                MapObjects[0].gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor",
                    new Color(intensity.r, intensity.g, intensity.b - 1, intensity.a));
            //}

            col -= .1f;
            yield return 0;
        }*/ 
        DeleteGameObjectsByLayer(9);
        if (mapSizeX < 16)
        {
            mapSizeX = mapSizeX + 1;
        }

        if (mapSizeY < 9)
        {
            mapSizeY = mapSizeY + 1;
        }

        tiles = CreateLevel(mapSizeX, mapSizeY);
        UnitX = 0;
        UnitY = mapSizeY/2;
        GenerateMap();
        
        
       
        StartCoroutine(LightFade.instance.FadeIn());
        yield return new WaitForSeconds(.5f);
        selectedUnit.transform.position = new Vector3(UnitX,UnitY,-10f);
        yield return new WaitForSeconds(.7f);
        falling = false;
        restarting = false;
    }
    
    IEnumerator NewLevelFail()
    {
        restarting = true;
        float col = 1f;
        yield return new WaitForSeconds(.5f);
      
        GameObject[] MapObjects = FindGameObjectsByLayer(9);
        /*while (col >= 0f)
        {
            foreach (var c in MapObjects)
            {
               
                Color intensity = c.gameObject.GetComponent<Renderer>().material.GetColor("_EmissionColor");
                c.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                    new Color(intensity.r, intensity.g, intensity.b - 1, intensity.a));
            }

            col -= .1f;
            yield return 0;
        }*/

        DeleteGameObjectsByLayer(9);
        tiles = CreateLevel(mapSizeX, mapSizeY);
        UnitX = 0;
        UnitY = mapSizeY/2;
        GenerateMap();
       
        StartCoroutine(LightFade.instance.FadeIn());
        yield return new WaitForSeconds(.5f);
        selectedUnit.transform.position = new Vector3(UnitX,UnitY,-10f);
        yield return new WaitForSeconds(.7f);
        falling = false;
        restarting = false;
    }

    IEnumerator GameOver()
    {
        restarting = true;
        falling = true;
        Clone.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        yield return 0;
    }
    
    
    
    
    
    //Random Map Gen, Left To Right
    
     public void PrintLevel(int [,] map)
    {
        int width = map.GetUpperBound(0) + 1;
        int height = map.GetUpperBound(1) + 1;

        string row = "";

        for (int y=0; y < height; y++)
        {
            for (int x=0; x < width; x++)
            {
                row += string.Format("{0} ", map[x, y]);
            }
            row += string.Format("\n");
        }
        Debug.Log(row);
    }



    

    
    
    public bool FreePosition(int[,] map, int posX, int posY)
    {
        int width = map.GetUpperBound(0) + 1;
        int height = map.GetUpperBound(1) + 1;

        // Can't go off map
        if (posX < 0 || posY < 0 || posX > width-1 || posY > height-1)
            return false;

        // Is position already used?
        if (map[posX, posY] != 1)
            return false;
        
        // Can only have one neighbor with value 0 (where we are coming from)
        int neighbors = 0;

        if (posY-1 >= 0 && map[posX, posY-1] == 0)
            neighbors++;

        if (posX-1 >= 0 && map[posX-1, posY] == 0)
            neighbors++;
        
        if (posY+1 < height && map[posX, posY+1] == 0)
            neighbors++;

        if (posX+1 < width && map[posX+1, posY] == 0)
            neighbors++;
        
        return neighbors == 1;
    }


    public int PickDirection(int[,] map, int posX, int posY)
    {
        // -1=no move, 0=up(-y), 1=right(+x), 2=down(+y), 3=left(-x)
        List<int> availableDirections = new List<int>();

        // Can we move up (-y)?
        if (FreePosition(map, posX, posY-1))
            availableDirections.Add(0);

        // Can we move right (+x)?
        if (FreePosition(map, posX+1, posY))
            availableDirections.Add(1);

        // Can we move down (+y)?
        if (FreePosition(map, posX, posY+1))
            availableDirections.Add(2);

        // Can we move left (-x)?
        if (FreePosition(map, posX-1, posY))
            availableDirections.Add(3);
        
        // No moves available
        if (availableDirections.Count == 0)
            return -1;
        
        // Randomly pick an available move
        return availableDirections[Random.Range(0,availableDirections.Count)];
    }


    public bool Walk(ref int[,] map, ref int posX, ref int posY)
    {
        int width = map.GetUpperBound(0) + 1;

        bool done = false;
        int direction = -1; // -1=no move, 0=up(-y), 1=right(+x), 2=down(+y), 3=left(-x)

        while(!done)
        {
            direction = PickDirection(map, posX, posY);

            // Cant move
            if (direction == -1)
                return false;

            switch (direction)
            {
                case 0:
                    posY--;
                    break;
                case 1:
                    posX++;
                    break;
                case 2:
                    posY++;
                    break;
                case 3:
                    posX--;
                    break;
            }

            map[posX, posY] = 0;

            // Are we done?
            done = (posX == width-1);
        }

        map[posX, posY] = 2;
        
        return true;
    }

    public int [,] CreateLevel(int width, int height)
    {
        // Random.InitState(1);

        int StartX = 0;
        int StartY = height/2;

        int [,] map = null;

        bool done = false;
        while (!done)
        {
            map = new int[width, height];
            for (int y=0; y < height; y++)
                for (int x=0; x < width; x++)
                    map[x, y] = 1;

            map[StartX, StartY] = 0;

            int currentX = StartX;
            int currentY = StartY;

            done = Walk(ref map, ref currentX, ref currentY);
            map[StartX, StartY] = 3;
        }

        return map;
    }
    
}
