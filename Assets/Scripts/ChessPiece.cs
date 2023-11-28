using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public enum Type
    {
        BlackKing, BlackQueen, BlackKnignt, BlackBishop, BlackRook, BlackPawn,
        WhiteKing, WhiteQueen, WhiteKnignt, WhiteBishop, WhiteRook, WhitePawn
    }


    public Type TypeChess;

    public GameObject Controller;
    public GameObject MovePossiblePrefab;

    [SerializeField] private SpriteRenderer _spriteRenderer;


    private int posBoardX = -1;
    private int posBoardY = -1;

    //private string player;

    public Sprite BlackKing, BlackQueen, BlackKnignt, BlackBishop, BlackRook, BlackPawn;
    public Sprite WhiteKing, WhiteQueen, WhiteKnignt, WhiteBishop, WhiteRook, WhitePawn;



    public void Initialiaze()
    {
        Controller = GameManager.Instance.gameObject;

        SetCoords();

        switch (TypeChess)
        {
            case Type.BlackKing:
                _spriteRenderer.sprite = BlackKing;
                break;
            case Type.BlackQueen:
                _spriteRenderer.sprite = BlackQueen;
                break;
            case Type.BlackKnignt:
                _spriteRenderer.sprite = BlackKnignt;
                break;
            case Type.BlackBishop:
                _spriteRenderer.sprite = BlackBishop;
                break;
            case Type.BlackRook:
                _spriteRenderer.sprite = BlackRook;
                break;
            case Type.BlackPawn:
                _spriteRenderer.sprite = BlackPawn;
                break;
            case Type.WhiteKing:
                _spriteRenderer.sprite = WhiteKing;
                break;
            case Type.WhiteQueen:
                _spriteRenderer.sprite = WhiteQueen;
                break;
            case Type.WhiteKnignt:
                _spriteRenderer.sprite = WhiteKnignt;
                break;
            case Type.WhiteBishop:
                _spriteRenderer.sprite = WhiteBishop;
                break;
            case Type.WhiteRook:
                _spriteRenderer.sprite = WhiteRook;
                break;
            case Type.WhitePawn:
                _spriteRenderer.sprite = WhitePawn;
                break;
            default:
                break;
        }



    }
    public void SetCoords()
    {
        float x = posBoardX;
        float y = posBoardY;

        x *= 0.76f;
        y *= 0.76f;
        x += -2.65f;
        y += -2.65f;

        transform.position = new Vector3(x, y, 0);
    }


    public int GetXBoard => posBoardX;
    public int GetYBoard => posBoardY;
    public void SetXBoard(int x)
    {
        posBoardX = x;
    }
    public void SetYBoard(int y)
    {
        posBoardY = y;
    }

    private void OnMouseUp()
    {
        DestroyMovePossibles();
        InitMovePossible();
    }


    private void DestroyMovePossibles()
    {
        //GameObject[] movePossible ;

        //for (int i = 0; i < movePossible.Length; i++)
        //{
        //    Destroy(movePossible[i]);
        //}

    }

    private void InitMovePossible()
    {
        switch (TypeChess)
        {
            case Type.BlackKing:
                break;
            case Type.BlackQueen:
                break;
            case Type.BlackKnignt:
                break;
            case Type.BlackBishop:
                break;
            case Type.BlackRook:
                break;
            case Type.BlackPawn:
                break;
            case Type.WhiteKing:
                break;
            case Type.WhiteQueen:
                break;
            case Type.WhiteKnignt:
                break;
            case Type.WhiteBishop:
                break;
            case Type.WhiteRook:
                break;
            case Type.WhitePawn:
                break;
            default:
                break;
        }
    }
    public void LineMovePossible(int xIncrement, int yIncrement)
    {
        var game = GameManager.Instance;

        int x = posBoardX + xIncrement;
        int y = posBoardY + yIncrement;

        while (game.PositionOnBoard(x, y) && game.GetPosition(x, y) == null)
        {
            MovePossibleSpawn(x, y, false);
            x += xIncrement;
            y += yIncrement;
        }

        if (game.PositionOnBoard(x, y) && game.GetPosition(x, y).GetComponent<ChessPiece>().TypeChess != TypeChess)
        {
            MovePossibleSpawn(x, y, true);
        }
    }

    public void LMovePossible()
    {
        PointMovePossible(posBoardX + 1, posBoardY + 2);
        PointMovePossible(posBoardX - 1, posBoardY + 2);
        PointMovePossible(posBoardX + 2, posBoardY + 1);
        PointMovePossible(posBoardX + 2, posBoardY - 1);
        PointMovePossible(posBoardX + 1, posBoardY - 2);
        PointMovePossible(posBoardX - 1, posBoardY - 2);
        PointMovePossible(posBoardX - 2, posBoardY + 1);
        PointMovePossible(posBoardX - 2, posBoardY - 1);
    }
    public void SurrondMovePossible()
    {
        PointMovePossible(posBoardX, posBoardY + 1);
        PointMovePossible(posBoardX, posBoardY - 1);
        PointMovePossible(posBoardX - 1, posBoardY - 1);
        PointMovePossible(posBoardX - 1, posBoardY);
        PointMovePossible(posBoardX - 1, posBoardY + 1);
        PointMovePossible(posBoardX + 1, posBoardY - 1);
        PointMovePossible(posBoardX + 1, posBoardY);
        PointMovePossible(posBoardX + 1, posBoardY + 1);
    }


    public void PointMovePossible(int x, int y)
    {
        var game = GameManager.Instance;

        if (game.PositionOnBoard(x, y))
        {
            GameObject cp = game.GetPosition(x, y);
            if (cp == null)
            {
                MovePossibleSpawn(x, y, false);
            }
            else if (cp.GetComponent<ChessPiece>().TypeChess != TypeChess)
            {
                MovePossibleSpawn(x, y, true);
            }
        }
    }


    public void PawnMovePlate(int x, int y)
    {
        var game = GameManager.Instance;

        if (game.PositionOnBoard(x, y))
        {
            if (game.GetPosition(x, y) == null)
            {
                MovePossibleSpawn(x, y, false);
            }

            if (game.PositionOnBoard(x + 1, y) && game.GetPosition(x + 1, y) != null
                && game.GetPosition(x + 1, y).GetComponent<ChessPiece>().TypeChess != TypeChess)
            {
                MovePossibleSpawn(x, y, true);
            }
            if (game.PositionOnBoard(x - 1, y) && game.GetPosition(x - 1, y) != null
                && game.GetPosition(x - 1, y).GetComponent<ChessPiece>().TypeChess != TypeChess)
            {
                MovePossibleSpawn(x, y, true);
            }
        }
    }
    public void MovePossibleSpawn(int x, int y, bool attack)
    {
        float xf = x;
        float yf = y;

        xf *= 0.76f;
        yf *= 0.76f;
        xf -= 2.65f;
        yf -= 2.65f;

        GameObject go = Instantiate(MovePossiblePrefab, new Vector3(xf, yf, 0), Quaternion.identity);
        MovePossible mp = go.GetComponent<MovePossible>();
        mp.Attack = attack;
        mp.SetReference(gameObject);
        mp.SetCoords(x, y);
    }
}
