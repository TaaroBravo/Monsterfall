using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SectionManager : MonoBehaviour {

    public static SectionManager Instance { get; private set; }

    public List<Section> sections;
    Matrix<Section> _board;
    List<Section> enableSections;

    public int sizeX;
    public int sizeY;
	
	void Awake ()
    {
        Instance = this;
        enableSections = sections.Where(x => x.sectionType != Section.SectionType.Empty).ToList();
        CreateBoard();

    }

    void CreateBoard()
    {
        _board = new Matrix<Section>(sizeX, sizeY);
        int internalCount = 0;
        for (int y = 0; y < _board.height; y++)
        {
            for (int x = 0; x < _board.width; x++)
            {
                _board[x, y] = sections[internalCount];
                internalCount++;
            }
        }
    }

    public Vector2 MoveForSections(Vector2 currentPos, int x, int y)
    {
        Vector2 newPos = currentPos;
        if (newPos.x + x < sizeX && newPos.x + x >= 0)
            newPos.x += x;
        if (newPos.y + y < sizeY && newPos.y + y >= 0)
            newPos.y += y;

        if (_board[(int)newPos.x, (int)newPos.y].sectionType != Section.SectionType.Empty)
            currentPos = newPos;

        return currentPos;
    }
	
	public Vector3 ChangePosition(Vector2 pos)
    {
        return _board[(int)pos.x, (int)pos.y].transform.position;
    }

    public int SelectCharacter(Vector2 pos)
    {
        Section selectedSection = _board[(int)pos.x, (int)pos.y];
        if (selectedSection.sectionType == Section.SectionType.Random)
            return 99;
        string[] myChar = selectedSection.gameObject.name.Split('-');
        int character = int.Parse(myChar[1]);
        return character;
    }

    public int RandomCharacter()
    {
        return Random.Range(0, enableSections.Count - 1);
    }
}
