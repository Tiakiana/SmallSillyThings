// See https://aka.ms/new-console-template for more information

using Zipper;

Console.WriteLine("Plox gib a path to the folder");
string path = Console.ReadLine();

Directory.CreateDirectory(path+"\\results");
string putThemTherePath = path+"\\results";
string[] files =Directory.GetFiles(path);
int number = 1;
for (int i = 0; i < files.Length; i+=2)
{
    //MerchantID_0001.zip
    List<string> filesToWrite = new List<string>() { files[i], files[i + 1] };
    string numberstring = number.ToString();
    numberstring = numberstring.PadLeft(4, '0');
    ZipFileCreator.CreateZipFile(putThemTherePath+"\\" + "MerchantID_" + numberstring + ".zip", filesToWrite );
    
    number++;
}


