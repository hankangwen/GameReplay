using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMgr : MonoSingleton<UnitMgr>
{
    public int curUnitId = 1001;
    Unit curUnit;
    Dictionary<int, Unit> dictUnit = new Dictionary<int, Unit>();

    private void Start()
    {
        curUnit = GetUnit(curUnitId);
    }

    public void FixedUpdate()
    {
        if (curUnit == null)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        curUnit.Move(x, z);
    }

    public void Register(int id, Unit unit)
    {
        if (dictUnit.ContainsKey(id))
        {
            MonoHelper.Error($"unit {id} Id重复");
            return;
        }
        dictUnit.Add(id, unit);
    }

    public Unit GetUnit(int id)
    {
        if (!dictUnit.ContainsKey(id))
            return null;
        return dictUnit[id];
    }

    public Dictionary<int, Unit> GetAllUnit()
    {
        return dictUnit;
    }
}
