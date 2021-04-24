using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;
using System.Linq;

public class GameController : MonoBehaviour
{
    public class Grid
    {
        public Vector2Int Size;
        public Cell[] cells;
        
        public Grid(Vector2Int size)
        {
            Size = size;
            cells = new Cell[size.x * size.y];
        }
        public void Set(int x, int y, Cell cell)
        {
            cells[GetId(x,y)] = cell;
        }
        public int GetId(int x, int y)
        {
            return x + Size.x * y;
        }
    }
    public class Cell
    {
        public int Inhabitant = -1;
    }

    public Vector2Int GridSize;
    public bool DrawGizmos;
    public Character[] Characters;
    public Color[] Colors;
    public Block GameData;
    public int[] DefaultGridPositions;

    private Grid grid;
    private int FrameId = -1;

    private void Start()
    {
        //TEMP
        Init(GameData, -1);
    }
    private void Init(Block gamedata, int frame)
    {
        grid = new Grid(GridSize);
        for(int x = 0; x < grid.Size.x; x++)
        {
            for(int y = 0; y < grid.Size.y; y++)
            {   
                grid.Set(x,y, new Cell());
            }
        }
        Characters = gamedata.characters;
        for(int i = 0; i < Characters.Length; i++)
        {
            grid.cells[Characters[i].GridId].Inhabitant = Characters[i].Id;
            Characters[i].GridId = DefaultGridPositions[Characters[i].Id];
        }
        FrameId = -1;
        while(FrameId != frame)
        {
            Tick();
        }
    }
    private void Update()
    {
        //Validate all characters are in correct grid cells, for debug
        for(int i = 0; i < grid.cells.Length; i++)
        {
            grid.cells[i].Inhabitant = -1;
        }
        for(int i = 0; i < Characters.Length; i++)
        {
            grid.cells[Characters[i].GridId].Inhabitant = Characters[i].Id;
        }
        
        //TEMP
        if(Input.GetKeyDown(KeyCode.Space))
            Tick();
        if(Input.GetKeyDown(KeyCode.Q))
            Init(GameData, -1);
    }

    public void AppendInput(int character, InputFrame input)
    {
        //Note: dont mind how terrible this code is
        List<InputFrame> inputs = Characters[character].timeLine.ToList();
        if(FrameId != inputs.Count - 1)
        {
            Debug.LogError($"Cannot append input when not at present frame Frame:{FrameId}, Input length: {inputs.Count}");
            return;
        }
        inputs.Add(input);
        Characters[character].timeLine = inputs.ToArray();
        GameData.characters = Characters;
    }
    public void Tick()
    {
        FrameId++;
        for(int i = 0; i < Characters.Length; i++)
        {
            if(Characters[i].timeLine.Length <= FrameId)
            {
                //Character out of inputs;
                continue;
            }
            InputFrame input = Characters[i].timeLine[FrameId];
            //TEMP
            if(input.action == 0)   //Move action
            {
                Characters[i].GridId = input.cell;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(!DrawGizmos)
            return;
        if(grid == null)
        {
            Init(GameData, -1);
        }
        for(int x = 0; x < grid.Size.x; x++)
        {
            for(int y = 0; y < grid.Size.y; y++)
            {
                Cell cell = grid.cells[grid.GetId(x, y)];
                Gizmos.color = Color.white;
                Gizmos.DrawCube(new Vector3(x,y), Vector3.one * 0.95f);
                if(cell.Inhabitant != -1)
                {
                    Gizmos.color = Colors[Characters[cell.Inhabitant].color];
                    Gizmos.DrawCube(new Vector3(x,y,1), Vector3.one * 0.85f);
                }
            }
        }
    }
}
