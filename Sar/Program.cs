using System;
using System.IO;

namespace Sar
{
    class Program
    {
        private static StringReplaceCollection? _operations = null!;
        private static string _confPath = null!;
        private static string _rootPath = null!;
        private static bool _readonly = true;

        public static int Main(string[] args)
        {
            if (args.Length is not (2 or 3))
                ExitError($"Invalid number of arguments.");

            _confPath = args[0];
            _rootPath = args[1];
            _readonly = args[2] == "readonly";
            
            if (!File.Exists(_confPath))
            {
                ExitError($"Path {_confPath} doesn't exist.");
            }

            if (!Directory.Exists(_rootPath))
            {
                ExitError($"Path {_rootPath} doesn't exist.");
            }

            _operations = StringReplaceCollection.GetCollectionFromFile(_confPath);
            if (_operations == null || _operations.Operations.Count == 0)
            {
                ExitError($"Path {_confPath} doesn't contain valid string replace operations.");
            }
            
            if (_readonly) Console.WriteLine("Read Only Mode");

            Replace(_rootPath, _readonly);

            WriteLineGood("Done");
            return 0;
        }

        public static void Replace(string root, bool readOnly)
        {
            if (_operations == null)
            {
                return;
            }

            // Work with sub-files.
            string[] files = Directory.GetFiles(root);
            for (int i = 0; i < files.Length; i++)
            {
                // Prep
                string file = files[i];
                string fileName = Path.GetFileName(file);
                string dirPath = Path.GetDirectoryName(file)!;

                // String replace contents of the file.
                string currentContents = File.ReadAllText(file);
                string newContents = _operations.GetNewString(currentContents);
                if (!string.Equals(newContents, currentContents, StringComparison.CurrentCulture))
                {
                    if (!readOnly)
                    {
                        File.Delete(file);
                        File.WriteAllText(file, newContents);
                    }

                    WriteLineGood($"Replace: '{file}'");
                }

                // Rename the file if necessary
                string newFileName = _operations.GetNewString(fileName);
                if (fileName != newFileName)
                {
                    string newFilePath = Path.Combine(dirPath, newFileName);

                    if (!File.Exists(newFilePath))
                    {
                        if (!readOnly)
                        {
                            File.Move(file, newFilePath);
                            fileName = newFileName;
                            files[i] = file = newFilePath;
                        }

                        WriteLineGood($"Rename:  '{file}' => '{newFilePath}'");
                    }
                    else
                    {
                        WriteLineBad($"Error:   {newFilePath} alraedy exists. Can't move {file}.");
                    }
                }
            }

            // Work with sub-directories.
            string[] directories = Directory.GetDirectories(root);
            for (int i = 0; i < directories.Length; i++)
            {
                // Prep
                string directory = directories[i];
                DirectoryInfo dirInfo = new DirectoryInfo(directory);

                // String replace contents of the directory.
                Replace(directory, readOnly);

                // Rename Directories if necessary
                string dirName = (new DirectoryInfo(directory)).Name;
                string parentDirPath = dirInfo.Parent!.FullName;
                string newDirName = _operations.GetNewString(dirName);

                if (!string.Equals(dirName, newDirName, StringComparison.CurrentCulture))
                {
                    string newDirPath = Path.Combine(parentDirPath, newDirName);

                    // Make sure not to overwrite other dirs in the event of naming conflict.
                    if (!Directory.Exists(newDirPath))
                    {
                        if (!readOnly)
                        {
                            Directory.Move(directory, newDirPath);
                            directories[i] = directory = newDirPath;
                            dirName = newDirName;
                        }

                        WriteLineGood($"Rename:  '{directory}' => '{newDirPath}'");
                    }
                    else
                    {
                        WriteLineBad($"Dir {newDirPath} alraedy exists. Can't move {directory}.");
                    }
                }
            }
        }

        public static void WriteLineBad(string message)
        {
            // Show console error.
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        public static void WriteLineGood(string message)
        {
            // Show console error.
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        public static void ExitError(string message)
        {
            // Show console error.
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;

            // Exit application by force with -1 error.
            Environment.Exit(-1);
        }
    }
}