using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SectionManager : MonoBehaviour {

    public static SectionManager Instance { get; private set; }

    List<Section> sections;

    public List<Section> enableSections;
    public Section[,] walkableSections;
	
	void Awake ()
    {
        Instance = this;
        sections = transform.ChildrenWithComponent<Section>().Where(x => x != null).ToList();
        enableSections = sections.Where(x => x.sectionType != Section.SectionType.Empty).ToList();
	}

    public int MoveForSections(int index, int input)
    {
        if(index == 0)
        {
            if (input == 3)
                return 2;
            if (input == -3)
                return enableSections.Count() - 1;
        }
        if (index + input == enableSections.Count())
            return 0;
        else if (index + input > enableSections.Count())
            return enableSections.Count() - 1;
        else if (index + input == -1 && input == -1)
            return enableSections.Count() - 1;
        else if (index + input <= -1)
            return 0;
        return index + input;
    }
	
	public Vector3 ChangePosition(int index)
    {
        return enableSections[index].transform.position;
    }

    public int SelectCharacter(int index)
    {
        Section selectedSection = enableSections[index];
        if (index == 0)
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
