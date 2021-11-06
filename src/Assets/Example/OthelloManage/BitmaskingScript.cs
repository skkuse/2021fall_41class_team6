
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BitmaskingScript : UdonSharpBehaviour
{
    public int getStatus(int index, ulong f1, ulong f2)
    {
        int outer = index/32;
        int inner = index%32;

        ulong retval = outer == 0 ? f1 : f2; //statusField[outer];
        retval = retval << (inner*2);
        retval = retval >> 62;

        return (int)retval;
    }

    public void setStatus(int index, ulong f1, ulong f2, int targetData)
    {
        int outer = index/32;
        int inner = index%32;

        // statusField[outer] init
        ulong initializer = ~((ulong)3 << ((31-inner)*2));

        if(outer == 0)
        {
            f1 = f1 & initializer;
            f1 = f1 | ((ulong)targetData << ((31-inner)*2));
        }
        else
        {
            f2 = f2 & initializer;
            f2 = f2 | ((ulong)targetData << ((31-inner)*2));
        }
    }
}
