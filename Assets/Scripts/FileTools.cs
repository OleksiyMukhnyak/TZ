
using File = System.IO.File;


public static class FileTools
{

    public static string Read(string path)
    {

        string result = "";
        result = File.ReadAllText(path);

        return result;
    }


    public static void Write(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    public static bool Exists(string path)
    {
        return File.Exists(path);
    }
}
