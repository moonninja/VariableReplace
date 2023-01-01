//1.Create c VariableReplace/input folder
//2.Copy source code to input folder - manually
//3.Ask for variable name, or set default  [--X--].
//4.Ask for replace name
//4.a. Ask for replace file name, default replace name
//5. Create c VariableReplace/output folder
//6. Replace variable name with replace name, replace source file with replace file name (or replace name if none provided)
//6.a Copy each replace to VariableReplace/output folder

// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

string cpath = Path.GetPathRoot(Environment.SystemDirectory);
string inputpath = System.IO.Path.Combine(cpath, "VariableReplace", "input");
string outputpath = System.IO.Path.Combine(cpath, "VariableReplace", "output");

Console.WriteLine("Replace and script variables for source code.");

//create VariableReplace/input folder
bool exists = System.IO.Directory.Exists(inputpath);
if (exists) Directory.Delete(inputpath, true);
System.IO.Directory.CreateDirectory(inputpath);
Console.WriteLine("C:/VariableReplace/input created!");

Console.WriteLine("replace command format - string,string etc... : input path m if already copied, variable name or x if want to use default [--X--], replace name , file replace name if x then replace name will be used");
Console.WriteLine($"1st case : C:\\Users\\Alois\\Downloads\\VariableReplace,x,Test,x");
Console.WriteLine("2nd case - before entering command copy files over : m,x,Test,x");
string command = Console.ReadLine();
string[] commands = command.Split(',');

//copy all files from a folder to input folder if somewhere else and not done manually
if (commands[0].ToLower() != "m")
{
    string copySourcePath = System.IO.Path.Combine(commands[0]);
    var allFiles = Directory.GetFiles(copySourcePath, "*.*", SearchOption.AllDirectories);
    foreach (string newPath in allFiles)
    {
        File.Copy(newPath, newPath.Replace(copySourcePath, inputpath), true);
    }
}

//variable name
string variableName = commands[1].ToLower() == "x" ? "[--X--]" : commands[1];

//replace name + replace filename
string replaceName = commands[2];
string replaceFileName = commands[3].ToLower() == "x" ? commands[2] : commands[3];

//create output path
if (System.IO.Directory.Exists(outputpath)) Directory.Delete(outputpath, true);
System.IO.Directory.CreateDirectory(outputpath);
Console.WriteLine("C:/VariableReplace/output created!");

//variable name -> replace name
//copy to output path
string[] outputFiles = Directory.GetFiles(inputpath, "*.*", SearchOption.AllDirectories);
foreach (string file in outputFiles)
{
    //Create an object of FileInfo for specified path            
    FileInfo fi = new FileInfo(file);

    string filename = fi.Name;

    //Open a file for Read\Write
    FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

    //Create an object of StreamReader by passing FileStream object on which it needs to operates on
    StreamReader sr = new StreamReader(fs);

    //Use the ReadToEnd method to read all the content from file
    string fileContent = sr.ReadToEnd();

    //var -> replace
    string replacedFileContent = fileContent.Replace(variableName,replaceName);

    //Close the StreamReader object after operation
    sr.Close();
    fs.Close();

    //create new file
    string newFileName = $"{replaceFileName}{filename}";
    string newFilePath = Path.Combine(outputpath, newFileName);
    FileStream newfs = new FileStream(newFilePath, FileMode.OpenOrCreate, FileAccess.Write);
    StreamWriter m_streamWriter = new StreamWriter(newfs);
    // Write to the file using StreamWriter class
    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
    m_streamWriter.Write(replacedFileContent);
    m_streamWriter.Flush();

    m_streamWriter.Close();
    newfs.Close();
}

Console.WriteLine("Variables replaced");
Console.WriteLine("C:/VariableReplace/");
//Process.Start(outputpath);
Environment.Exit(0);

