🔧 File Extension Renamer & Duplicate Cleaner (C# Console App)

This project is a simple and efficient .NET Core console application designed for:

✔️ Bulk renaming file extensions (e.g., .tmp → .jpg)
✔️ Automatically detecting and removing duplicate files based on content
✔️ Displaying a smooth loading animation for better user experience

It is useful when working with exported raw files where file extensions are incorrect or missing.

🛠 Features

📝 Prompt user for folder path and file extensions

🔄 Change extensions for all matching files

🗑 Detects and deletes duplicate files using SHA256 hashing

⏱ Shows a loading spinner during processing

📊 Displays success summary with renamed and deleted counts

🚀 How It Works

The program performs the following steps:

Reads user input (folder path, current extension, new extension)

Validates folder existence

Renames all files with the specified extension to the new one

Scans renamed files and removes duplicates

Shows progress animation while processing

Prints results

📦 Requirements

.NET Core 6.0 or later

Windows, macOS, or Linux

▶️ How to Run

Clone the repository:

git clone https://github.com/MohammedAbdElmaksod/Extention-Coverter-And-Remove-Duplicates.git


Build the project:

dotnet build


Run the executable:

dotnet run

💡 Example Usage
Enter folder path:
C:\Images

Enter current extension:
tmp

Enter new extension:
jpg

Processing... 

 128 files renamed  
 5 duplicate files removed  

⚙️ How Duplicate Removal Works

Duplicate detection is done by:

Reading file content

Generating a SHA-256 hash

If two files have the same hash → one is deleted

This ensures duplicates are removed even if filenames are different.

📝 License

This project is licensed under the MIT License — feel free to modify and use it.

🤝 Contribution

Pull requests are welcome!
For major changes, please open an issue first to discuss what you'd like to change.
