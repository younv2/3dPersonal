using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        stat = CharacterManager.Instance.player.stat;
    }

    public void EquipNew(ItemData data)
    {
        curEquip = data;
    }

    public void UnEquip()
    {
        if(curEquip != null)
        {
            curEquip = null;
        }
    }
}
