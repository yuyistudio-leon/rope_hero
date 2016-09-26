using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Security.Permissions;

public class FileUtility
{
    public static string GetPath()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = Application.dataPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.dataPath;
            if (path.EndsWith("Assets"))
            {
                path += "/StreamingAssets/Storage";
            }
        }
        Debug.Log(path);
        return path;
    }
    public static void Test()
    {
        string path = GetPath();
        string content = LoadFile(path, "test.txt");
        if (content == "error")
        {
            CreateOrWriteTextFile(path, "test.txt", "TestContent");
            content = LoadFile(path, "test.txt");
        }
        Debug.Log(content);
    }

    #region STRING
    public static void CreateOrWriteTextFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
    public static void DeleteFile(string name)
    {
        var path = GetPath()+ "//" + name;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    #endregion
    #region BINARY
    public static bool Serialize(string filename, object obj)
    {
        if (filename == "" || filename == null)
        {
            Debug.LogError("filname cannot be empty");
            return false;
        }
        IFormatter formatter = new BinaryFormatter();
        Stream file = null;
        try
        {
            file = new FileStream(GetPath() + "/" + filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(file, obj);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
    public static object Deserialize(string filename)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream file = null;
        try
        {
            file = new FileStream(GetPath() + "/" + filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (Exception)
        {
            if (file != null)
            {
                file.Close();
            }
            return null;
        }
        try
        {
            object obj = formatter.Deserialize(file);
            return obj;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
        finally
        {
            file.Close();
        }
    }
    #endregion
    #region COMMON
    public static void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);
    }
    public static string LoadFile(string path, string name)
    {
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            return "error";
        }
        StreamReader sr = null;
        sr = File.OpenText(path + "//" + name);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            break;
        }
        sr.Close();
        sr.Dispose();
        return line;
    }
    #endregion
}
