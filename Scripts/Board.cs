//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{

    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
            return new RectInt(position, this.boardSize);
        }   // ��Ʈ���� �� �߾��� ������ǥ(0,0) => ���ϱ����� �����ϴ� (-,-)���� �����ؾ��� 
            // �׷��� ���� Postion �̶�� ������ this(���罺ũ��Ʈ).boardsize�� ������ ���ΰ���.
    }
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }  
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();

    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePositon = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePositon, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePositon = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePositon, null);
        }
    }


    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds; //board Size�� �ǹ���.

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }


    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin; //x = -10 , y = -20

        while(row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                ScoreUp();
            }
            else
            {
                row++;
            }
        }

    }

   /// <summary> 2024-08-12 �����κ� �߰�
   
    public void ScoreUp()
    {
        GameManager.gm.score += 100;
    }
  
   /// <returns></returns>

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
                //�ش���ġ�� Ÿ���� �������� �ʴ´ٸ�, ���� �������� �ʾ����� �ǹ�
            {
                return false; // Ÿ���� ���� ���� �ʾ����� ���� ���� �����ɷ� �Ǹ�
            }
        }

        return true; // Ÿ���� �����迭�� ��á���� ���� �������� Ȯ��.
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col<bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
    
    
  
}
