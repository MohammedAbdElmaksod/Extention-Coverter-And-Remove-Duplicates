using System.Security.Cryptography;
using static System.Console;

WriteLine("Enter folder path: ");
string folderPath = ReadLine() ?? "";

while (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
{
    WriteLine("Folder not found or empty. Please re-enter:");
    folderPath = ReadLine() ?? "";
}

WriteLine("\nFolder Found Press any key to continue...");
ReadKey();

WriteLine("\nChoose an option:");
WriteLine("1. Convert File Extensions");
WriteLine("2. Remove Duplicate Files");
WriteLine("3. Both");
string choice = ReadLine() ?? "";

switch (choice)
{
    case "1":
        ConvertFileExtensions(folderPath);
        break;
    case "2":
        RemoveDuplicateFiles(folderPath);
        break;
    case "3":
        ConvertFileExtensions(folderPath);
        RemoveDuplicateFiles(folderPath);
        break;
    default:
        WriteLine("Invalid choice!");
        break;
}

WriteLine("\nPress any key to exit...");
ReadKey();



// ------------------ FUNCTION: CONVERT EXTENSIONS ------------------
static void ConvertFileExtensions(string folderPath)
{
    WriteLine("\nEnter Current Extension (without dot): ");
    string currentExt = ReadLine() ?? "";

    while (string.IsNullOrEmpty(currentExt))
    {
        WriteLine("Extension cannot be empty! Try again:");
        currentExt = ReadLine() ?? "";
    }

    WriteLine("Enter New Extension (without dot): ");
    string newExt = ReadLine() ?? "";

    while (string.IsNullOrEmpty(newExt))
    {
        WriteLine("Extension cannot be empty! Try again:");
        newExt = ReadLine() ?? "";
    }

    WriteLine("\nStarting conversion... Press any key to continue.");
    ReadKey();

    string[] files = Directory.GetFiles(folderPath, $"*.{currentExt}", SearchOption.AllDirectories);
    int count = 0;

    WriteLine($"\nRenaming {files.Length} files...");

    int spinnerIndex = 0;
    char[] spinner = { '|', '/', '-', '\\' };

    foreach (var file in files)
    {
        string newFile = Path.ChangeExtension(file, newExt);

        try
        {
            File.Move(file, newFile, true);
            count++;
        }
        catch (Exception ex)
        {
            WriteLine($"\nError renaming {Path.GetFileName(file)}: {ex.Message}");
        }

        Write($"\rProcessing... {spinner[spinnerIndex]}   ({count}/{files.Length})");
        spinnerIndex = (spinnerIndex + 1) % spinner.Length;
    }

    WriteLine($"\n\n✔ Done! {count} files renamed successfully.");
}



// ------------------ FUNCTION: REMOVE DUPLICATES ------------------
static void RemoveDuplicateFiles(string folderPath)
{
    WriteLine("\nScanning files...\n");

    string backupFolder = Path.Combine(folderPath, "_DUPLICATES");
    Directory.CreateDirectory(backupFolder);

    var fileGroups = Directory
        .GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
        .GroupBy(f => new FileInfo(f).Length); // Group by file size first

    int removedCount = 0;
    int processed = 0;

    int spinnerIndex = 0;
    char[] spinner = { '|', '/', '-', '\\' };

    foreach (var group in fileGroups)
    {
        if (group.Count() < 2) continue; // Only check duplicates in groups with > 1 file

        var hashStore = new HashSet<string>();

        foreach (var file in group)
        {
            processed++;

            string hash;
            try
            {
                hash = GetFileHash(file);
            }
            catch (Exception ex)
            {
                WriteLine($"\n⚠ Unable to read {file}: {ex.Message}");
                continue;
            }

            if (hashStore.Contains(hash))
            {
                string duplicateDest = Path.Combine(backupFolder, Path.GetFileName(file));

                try
                {
                    File.Move(file, duplicateDest, true);
                    removedCount++;
                }
                catch (Exception ex)
                {
                    WriteLine($"\n Error moving duplicate file {file}: {ex.Message}");
                }
            }
            else
            {
                hashStore.Add(hash);
            }

            Write($"\rProcessing... {spinner[spinnerIndex]}  ({processed} checked)");
            spinnerIndex = (spinnerIndex + 1) % spinner.Length;
        }
    }

    WriteLine($"\n\n Done! {removedCount} duplicate files moved to folder: _DUPLICATES");
}



// ------------------ FUNCTION: HASH (SAFE) ------------------
static string GetFileHash(string filePath)
{
    using var sha256 = SHA256.Create();
    using var stream = File.OpenRead(filePath);
    byte[] hashBytes = sha256.ComputeHash(stream);
    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
}
