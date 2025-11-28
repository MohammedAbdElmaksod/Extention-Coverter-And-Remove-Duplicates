using System.Security.Cryptography;
using static System.Console;
try
{

    WriteLine("Enter folder path: ");
    string folderPath = ReadLine() ?? "";

    if (string.IsNullOrEmpty(folderPath))
    {
        WriteLine("Folder path cannot be empty!");
        WriteLine("Please Enter Folder Path");
        folderPath = ReadLine() ?? "";
    }

    if (!Directory.Exists(folderPath))
    {
        WriteLine("Folder not found!");
        return;
    }

    WriteLine("Great! Folder Exists Press any key to continue...");
    ReadKey();

    WriteLine("\nChoose an option:");
    WriteLine("1. Convert File Extensions");
    WriteLine("2. Remove Duplicate Files");
    WriteLine("3. Both");
    string choice = ReadLine() ?? "";

    if (string.IsNullOrEmpty(choice))
    {
        WriteLine("Choice cannot be empty!");
        WriteLine("Please Enter Your Choice");
        choice = ReadLine() ?? "";
    }

    if (choice == "1")
    {
        ConvertFileExtensions(folderPath);
    }
    else if (choice == "2")
    {
        RemoveDuplicateFiles(folderPath);
    }
    else if (choice == "3")
    {
        ConvertFileExtensions(folderPath);
        RemoveDuplicateFiles(folderPath);
    }
    else
    {
        WriteLine("Invalid choice!");
    }

    WriteLine("\nPress any key to exit...");
    ReadKey();

}
catch (Exception ex)
{
    WriteLine($"\nAn error occurred: {ex.Message}");
}


// ------------------ FUNCTION: CONVERT EXTENSIONS ------------------
static void ConvertFileExtensions(string folderPath)
{
    WriteLine("\nEnter Current Extension (without dot): ");
    string currentExtention = ReadLine() ?? "";

    if (string.IsNullOrEmpty(currentExtention))
    {
        WriteLine("Current Extension cannot be empty!");
        currentExtention = ReadLine() ?? "";
    }

    WriteLine("Enter New Extension (without dot): ");
    string newExtention = ReadLine() ?? "";

    if (string.IsNullOrEmpty(newExtention))
    {
        WriteLine("New Extension cannot be empty!");
        newExtention = ReadLine() ?? "";
    }

    WriteLine("\nStarting conversion... Press any key to continue.");
    ReadKey();

    string[] files = Directory.GetFiles(folderPath, $"*.{currentExtention}");
    int count = 0;

    WriteLine($"\nRenaming {files.Length} files...");

    int spinnerIndex = 0;
    char[] spinner = { '|', '/', '-', '\\' };

    foreach (var file in files)
    {
        string newFile = Path.ChangeExtension(file, $".{newExtention}");

        try
        {
            File.Move(file, newFile, true);
            count++;
        }
        catch (Exception ex)
        {
            WriteLine($"\nError renaming {Path.GetFileName(file)}: {ex.Message}");
        }

        // Loader animation
        Write($"\rProcessing... {spinner[spinnerIndex]}   ({count}/{files.Length})");
        spinnerIndex = (spinnerIndex + 1) % spinner.Length;
        Thread.Sleep(100);
    }

    WriteLine($"\n\n✔ Done! {count} files renamed successfully.");
}


// ------------------ FUNCTION: REMOVE DUPLICATES ------------------
static void RemoveDuplicateFiles(string folderPath)
{
    var fileHashes = new Dictionary<string, string>();
    string[] files = Directory.GetFiles(folderPath);
    int removedCount = 0;

    WriteLine($"\nScanning {files.Length} files for duplicates...");

    int spinnerIndex = 0;
    char[] spinner = { '|', '/', '-', '\\' };

    int processed = 0;

    foreach (var file in files)
    {
        string hash = GetFileHash(file);

        if (fileHashes.ContainsKey(hash))
        {
            File.Delete(file);
            removedCount++;
        }
        else
        {
            fileHashes[hash] = file;
        }

        processed++;

        // Loader animation with counter
        Write($"\rProcessing... {spinner[spinnerIndex]}   ({processed}/{files.Length})");
        spinnerIndex = (spinnerIndex + 1) % spinner.Length;
        Thread.Sleep(100);
    }

    WriteLine($"\n\n✔ Done! {removedCount} duplicate files removed successfully.");
}


static string GetFileHash(string filePath)
{
    using var md5 = MD5.Create();
    using var stream = File.OpenRead(filePath);
    byte[] hash = md5.ComputeHash(stream);
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
}
