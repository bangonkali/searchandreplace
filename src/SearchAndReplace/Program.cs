using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SearchAndReplace
{
    public class Program
    {
        private static StringReplaceCollection _operations;
        private static string _confPath;
        private static string _rootPath;

        public static int Main(string[] args)
        {
            if (args.Length != 2)
                ExitError($"Invalid number of arguments.");

            _confPath = args[0];
            _rootPath = args[1];

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
           
            Replace(_rootPath);

            WriteLineGood("Done");
            return 0;
        }

        public static void Replace(string root)
        {
            // Work with sub-files.
            string[] files = Directory.GetFiles(root);
            for (int i = 0; i < files.Length; i++)
            {
                // Prep
                string file = files[i];
                string fileName = Path.GetFileName(file);
                string dirPath = Path.GetDirectoryName(file);

                // String replace contents of the file.
                string currentContents = File.ReadAllText(file);
                string newContents = _operations.GetNewString(currentContents);
                if (!string.Equals(newContents, currentContents, StringComparison.CurrentCulture))
                {
                    File.Delete(file);
                    File.WriteAllText(file, newContents);
                }

                // Rename the file if necessary
                string newFileName = _operations.GetNewString(fileName);
                if (fileName != newFileName)
                {
                    string newFilePath = Path.Combine(dirPath, newFileName);

                    if (!File.Exists(newFilePath))
                    {
                        File.Move(file, newFilePath);
                        fileName = newFileName;
                        files[i] = file = newFilePath;
                    }
                    else
                    {
                        WriteLineBad($"File {newFilePath} alraedy exists. Can't move {file}.");
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
                Replace(directory);

                // Rename Directories if necessary
                string dirName = (new DirectoryInfo(directory)).Name;
                string parentDirPath = dirInfo.Parent.FullName;
                string newDirName = _operations.GetNewString(dirName);
                                
                if (!string.Equals(dirName, newDirName, StringComparison.CurrentCulture))
                {
                    string newDirPath = Path.Combine(parentDirPath, newDirName);

                    // Make sure not to overwrite other dirs in the event of naming conflict.
                    if (!Directory.Exists(newDirPath))
                    {
                        Directory.Move(directory, newDirPath);
                        directories[i] = directory = newDirPath;
                        dirName = newDirName;
                    } else
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
