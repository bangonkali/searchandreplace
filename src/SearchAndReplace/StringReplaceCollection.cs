using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchAndReplace
{
    public class StringReplaceCollection
    {
        public List<StringReplaceOperation> Operations { get; set; }

        public void StringReplaceOperation()
        {
            Operations = new List<StringReplaceOperation>();
        }

        public string GetNewString(string current)
        {
            string buffer = current;
            foreach (StringReplaceOperation item in Operations)
            {
                buffer = item.GetNewString(buffer);
            }

            return buffer;
        }

        public static StringReplaceCollection GetCollectionFromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            string line;
            StringReplaceCollection c = new StringReplaceCollection();
            c.Operations = new List<StringReplaceOperation>();

            using (StreamReader sr = new StreamReader(File.OpenRead(path)))
            {
                int count = 0;
                int operationsCount = 0;

                StringReplaceOperation operation = new StringReplaceOperation();
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    switch (count % 3)
                    {
                        case 0:
                            operation = new StringReplaceOperation();
                            operation.RegexEnabled = line == "1";
                            break;
                        case 1:
                            operation.Pattern = line;
                            break;
                        case 2:
                            operation.New = line;
                            c.Operations.Add(operation);
                            operationsCount++;
                            break;
                        default:
                            break;
                    }

                    count++;
                }
            }

            return c;
        }
    }
}
