using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileSystem
{
    static List<string> list = new List<string>();//定义list变量，存放获取到的路径

    // Start is called before the first frame update
    public static List<string> GetFiles(string path, string head = "", string searchPattern = "*.lua")
    {

        getPath(path, head, searchPattern);
        // 
        foreach (var item in list)
        {
            Debug.Log("文件:" + item);
        }
        return list;
    }

    public static List<string> getPath(string path, string head, string searchPattern)
    {
        DirectoryInfo dir = new DirectoryInfo(path);

        // 文件
        FileInfo[] fil = dir.GetFiles(searchPattern);
        // 目录
        DirectoryInfo[] dii = dir.GetDirectories();

        // Debug.Log("文件夹:" + dir.Name);
        var dirName = dir.Name;

        foreach (FileInfo f in fil)
        {
            // Debug.Log("文件:" + f.Name);
            list.Add(head + "." + dirName + "." + Path.GetFileNameWithoutExtension(f.Name));//添加文件的路径到列表
        }
        //获取子文件夹内的文件列表
        foreach (DirectoryInfo d in dii)
        {
            getPath(d.FullName, head + "." + dirName, searchPattern);
        }
        return list;
    }

    // 获取所有文件
    public static void GetAllFileByPath(string path, ref List<FileInfo> res, string searchPattern)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        // 文件
        FileInfo[] fil = dir.GetFiles(searchPattern);
        // 目录
        DirectoryInfo[] dii = dir.GetDirectories();
        // 遍历当前文件夹下的所有文件
        foreach (FileInfo f in fil)
        {
            res.Add(f);
        }
        //获取子文件夹内的文件列表
        foreach (DirectoryInfo d in dii)
        {
            GetAllFileByPath(d.FullName, ref res, searchPattern);
        }
    }

}
