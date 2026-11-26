using System;
using System.IO;
using System.IO.Compression;

public class ZipHelper
{
    public void AddFilesToArchive(string archiveName, string filePath, string fileFilter)
    {
        DirectoryInfo di = new DirectoryInfo(filePath);
        using (ZipArchive archive = ZipFile.Open(archiveName, ZipArchiveMode.Update))
        {
            foreach (var file in di.GetFiles(fileFilter))
            {
                try
                {
                    if (archive.GetEntry(file.Name) == null)
                    {
                        archive.CreateEntryFromFile(filePath + file.Name, file.Name);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
    public void AddFileToArchive(string archiveName, string filePath, string fileName)
    {
        try
        {
            using (ZipArchive archive = ZipFile.Open(archiveName, ZipArchiveMode.Update))
            {
                if (archive.GetEntry(fileName) == null)
                {
                    archive.CreateEntryFromFile(filePath + fileName, fileName);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public bool AddFileToArchiveB(string archiveName, string filePath, string fileName)
    {
        using (ZipArchive archive = ZipFile.Open(archiveName, ZipArchiveMode.Update))
        {
            if (archive.GetEntry(fileName) == null)
            {
                archive.CreateEntryFromFile(filePath + fileName, fileName);
                return true;
            }
            else
                return false;
        }
    }
    public bool IsFileInZip(string zipPath, string fileName)
    {
        using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
        {
            return archive.GetEntry(fileName) != null;
        }
    }
}
