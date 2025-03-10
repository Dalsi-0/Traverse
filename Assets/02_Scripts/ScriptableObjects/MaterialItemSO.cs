using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Object/Item/Material", order = int.MaxValue)]
public class MaterialItemSO : ItemSO
{
    [SerializeField] private MATERIAL_TYPE materialType;
    public MATERIAL_TYPE MaterialType => materialType;
}
