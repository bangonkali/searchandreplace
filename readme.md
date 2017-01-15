# Search and Replace

Search and replace tool that allows changing the folder names, file names, and contents of files through regex and simple string comparison.

## Example

### Configuration file

Create a configuration file.

```
[0/1]
[Pattern]
[ReplacementString]
[
	[0/1]
	[Pattern]
	[ReplacementString]
]...
```

### Root Directory

Determine the root directory.

### Execute command

```
searchandreplace <configuration_path> <root_dir_path>
```