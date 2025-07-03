using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseETC
{
    public void UseETCItem(int id);
}


public class UseETC : IUseETC
{

    //기타 아이템 아이디에 따라 효과들 나눔
    public void UseETCItem(int id)
    {
        if (id == 3002)
        {
            TextManager.Instance.MemoPopUpText($"{id}");
        }
        else if (id == 0090)
        {

        }
    }
}
