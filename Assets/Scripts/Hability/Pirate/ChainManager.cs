using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkGoat.ObjectPool;

public class ChainManager {

    private Hook _hook;

    private List<ChainPart> _chainParts;
    private List<ChainSection> _chainSections;

	public ChainManager(Func<ChainPart> factoryMethod, Hook hook)
    {
        ObjectPoolManager.Instance.AddObjectPool<ChainPart>(factoryMethod, ChainPart.Init, ChainPart.Finit, 30);
        _hook = hook;
        _hook.OnInitHook += (x, y) => InitEvent();
        _hook.OnEndHook += (x, y) => DestroyLastSectionEvent();
        _hook.OnTeleport += (x, y) => TelepGoEvent(y, x);
        _hook.OnTelepWithHookFired += () => Clean();

        _chainParts = new List<ChainPart>();
        _chainSections = new List<ChainSection>();
    }

    #region Update
    public void Update(Vector3 characterPosition, Vector3 pointPosition)
    {
        var currentIndex = _chainParts.Count-1;

        for (int i = 0; i < _chainSections.Count; i++)
        {
            var part = _chainSections[i];
            var distanceMadeInThisSection = 0f;
            var distanceToDo = GetDistanceSection(part, characterPosition, pointPosition);
            switch (part.part)
            {
                case ChainSection.Type.Initial:
                    while (distanceMadeInThisSection < distanceToDo)
                    {
                        if (part.to == Vector3.zero)
                        {
                            var direction = (characterPosition - pointPosition).normalized;
                            var position = pointPosition + direction * distanceMadeInThisSection;
                            currentIndex = PlacePart(position, direction, currentIndex);
                        }
                        else
                        {
                            var direction = (characterPosition- part.to).normalized;
                            var position = part.to + direction * distanceMadeInThisSection;
                            currentIndex = PlacePart(position, direction, currentIndex);
                        }

                        distanceMadeInThisSection += ChainPart.CHAIN_LENGTH;
                        currentIndex--;
                    }
                    break;

                case ChainSection.Type.OtherPart:
                    while (distanceMadeInThisSection < distanceToDo)
                    {
                        if (part.to == Vector3.zero)
                        {
                            var direction = (part.from- pointPosition).normalized;
                            var position = pointPosition + direction * distanceMadeInThisSection;
                            currentIndex = PlacePart(position, direction, currentIndex);
                        }
                        else
                        {
                            var direction = (part.from- part.to).normalized;
                            var position = part.to + direction * distanceMadeInThisSection;
                            currentIndex = PlacePart(position, direction, currentIndex);
                        }

                        distanceMadeInThisSection += ChainPart.CHAIN_LENGTH;
                        currentIndex--;
                    }
                    break;
            }
        }
        
        ClearUnnecesaryParts(currentIndex);
    }
    #endregion

    private void ClearUnnecesaryParts(int index)
    {
        for (int i = 0; i < index; i++)
        {
            ObjectPoolManager.Instance.ReturnObject<ChainPart>(_chainParts[i]);
        }
        for (int i = index - 1; i >= 0; i--)
        {
            _chainParts.RemoveAt(i);
        }
    }

    private int PlacePart(Vector3 pos, Vector3 forward, int partIndex)
    {
        if (partIndex < 0)
        {
            _chainParts.Insert(0, ObjectPoolManager.Instance.GetObject<ChainPart>());
            partIndex = 0;
        }
        var part = _chainParts[partIndex];
        part.transform.position = pos;
        part.transform.up = forward;
        return partIndex;
    }

    private float GetDistanceSection(ChainSection c, Vector3 characterPosition, Vector3 pointPosition)
    {
        switch(c.part)
        {
            case ChainSection.Type.Initial:
                if (c.to == Vector3.zero)
                    return Vector3.Distance(characterPosition, pointPosition);
                else
                    return Vector3.Distance(characterPosition, c.to);

            case ChainSection.Type.OtherPart:
                if (c.to == Vector3.zero)
                    return Vector3.Distance(c.from, pointPosition);
                else
                    return Vector3.Distance(c.from, c.to);
        }
        return 0;
    }

    private void InitEvent()
    {
        Clean();
        _chainSections.Add(new ChainSection(ChainSection.Type.Initial, Vector3.zero, Vector3.zero));
    }

    private void DestroyLastSectionEvent()
    {
        _chainSections.RemoveAt(_chainSections.Count - 1);
        if (_chainSections.Count > 0)
            _chainSections[_chainSections.Count - 1].to = Vector3.zero;
        else
            Clean();
    }

    private void TelepGoEvent(Vector3 position, Vector3 from)
    {
        _chainSections.Add(new ChainSection(ChainSection.Type.OtherPart, position, Vector3.zero));
        _chainSections[_chainSections.Count - 2].to = from;
    }

    private void Clean()
    {
        _chainSections.Clear();
        foreach(var item in _chainParts)
        {
            ObjectPoolManager.Instance.ReturnObject<ChainPart>(item);
        }
        _chainParts.Clear();
    }
}

public class ChainSection
{
    public Type part;
    public Vector3 from;
    public Vector3 to;

    public ChainSection(Type p, Vector3 f, Vector3 t)
    {
        part = p;
        from = f;
        to = t;
    }

    public enum Type
    {
        Initial,
        OtherPart
    }
}
