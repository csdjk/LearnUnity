using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/// <summary>
/// 协程管理器
/// </summary>
public class CoroutineManager
{
    private static CoroutineManager _instance = null;
    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CoroutineManager();
            }
            return _instance;
        }
    }

    private LinkedList<IEnumerator> coroutineList = new LinkedList<IEnumerator>();

    public void StartCoroutine(IEnumerator ie)
    {
        coroutineList.AddLast(ie);
    }

    public void StopCoroutine(IEnumerator ie)
    {
        try
        {
            coroutineList.Remove(ie);
        }
        catch (Exception e) { Console.WriteLine(e.ToString()); }
    }

    public void UpdateCoroutine()
    {
        var node = coroutineList.First;
        while (node != null)
        {
            IEnumerator ie = node.Value;
            bool ret = true;
            if (ie.Current is IWait)
            {
                IWait wait = (IWait)ie.Current;
                //检测等待条件，条件满足，跳到迭代器的下一元素 （IEnumerator方法里的下一个yield）
                if (wait.Tick())
                {
                    ret = ie.MoveNext();
                }
            }
            else
            {
                ret = ie.MoveNext();
            }
            //迭代器没有下一个元素了，删除迭代器（IEnumerator方法执行结束）
            if (!ret)
            {
                coroutineList.Remove(node);
            }
            //下一个迭代器
            node = node.Next;
        }
    }
}