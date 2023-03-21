Search and replace tool that allows changing the folder names, file names, and contents of files through regex and simple string comparison. This tool is built to migrate large Visual Studio, Android Studio, and Xcode projects to a new naming scheme.

## Why?

This was built to migrate large **Visual Studio**, **Android Studio**, and **Xcode** projects to a new naming scheme. Sometimes namespaces/branding are declared within files and the file and folder names also contain these namespaces/branding. When the project requires new branding or a new namespace, it is tedious to change all of these instances manually. This tool allows you to change all of these instances in one go. 

## Caution

Please make sure that you have a backup of your project before using this tool. This tool is not responsible for any damage caused to your project.

## Cross Platform Support

This application supports any platform that supports `Dotnet 7`. This includes Windows, Linux, and Mac.


### Instructions

1. Prepare your project for string replace.

	1. Perform a clean build of your project. Example: `dotnet clean`. This ensure we work with less meta data stuff and not doing needless work against generate or build files.

    1. Determine the string that needs to be replaced.

    1. Check whether you require regex replacement or not.

    1. Determine the string that will be used to replace the string that needs to be replaced.

1. Create a configuration file.
	

	```
	[0/1: 1 = enable regex, 0 = disable regex]
	[Pattern: regex or simple string to search for]
	[ReplacementString: string to replace with]
	... or more ...
	```

	1. The first line of the configuration file is the mode. This can be either `0` or `1`. `0` is regex and `1` is simple string.

	1. The second line of the configuration file is the pattern to search for. This can be a regex or a simple string.
	
	1. The third line of the configuration file is the replacement string. This is the string that will replace the pattern.
	
	1. __Make sure that the configuration file has sets of 3 lines.__

1. **Backup your project!** This tool is not responsible for any damage caused to your project.

1. Run the tool in readonly mode first! Study the output!

	```bash
	Sar.exe "<configuration_path>" "<root_dir_path>" readonly
	```

1. Possible Issues

	1. An issue can occur if a file that needs to be renamed is open in another application. Restart your computer and ensuring that the base directory files are not part of your computer's startup sequence.

	1. An issue can occur if the files new file name already exists.

1. Run the application with the configuration file and the root directory path of your project as arguments.

	```bash
	Sar.exe "<configuration_path>" "<root_dir_path>"
	```

1. Test if your project still builds and all tests passed!

### Example

> I want to change all instances of `"foo"` to `"bar"` in the `"C:/Users/username/Desktop/myproject"` directory. I also want to change all instances of `"Foo"` to `"Bar"` and all instances of `"FOO"` to `"BAR"`.

Configuration file: `"C:/config.txt"` and Contents:

```
0
foo
bar
0
Foo
Bar
0
FOO
BAR
```

Command to execute:

```bash
Sar.exe "C:/config.txt" "C:/Users/username/Desktop/myproject"
```

# To Do List

1. Use a better command prompt library. The current one is not very good.

   1. [System.CommandLine](https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/march/net-parse-the-command-line-with-system-commandline)

   1. [CommandLineParser](https://github.com/commandlineparser/commandline) 

1. Add support for `'.gitignore'` files.