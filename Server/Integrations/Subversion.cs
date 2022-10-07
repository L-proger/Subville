using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Integrations {
    public class Subversion {
        public string RepositoriesRoot => @"E:/Repositories";
        public string FileSchemeUrlPrefix => @"file:///";
        public string FileSchemeRepositoriesRoot => FileSchemeUrlPrefix + RepositoriesRoot;

        public string[] GetRepositories()
        {
            return Directory.GetDirectories(RepositoriesRoot).Where(v=> IsRepository(v)).Select(v=> Path.GetFileName(v)).ToArray();
        }

        public bool IsRepository(string name)
        {
            var result = SystemProcess.Execute("svnlook", false, $"info {Path.Combine(RepositoriesRoot, name)}");
            return result.exitCode == 0;
        }


        [Serializable]
        public class SvnCommit {
            [XmlAttribute("revision")]
            public string Revision { get; set; }

            [XmlElement("author")]
            public string Author { get; set; }

            [XmlElement("date", DataType = "dateTime")]
            public DateTime Date { get; set; }
        }

        [Serializable]
        public class SvnEntry {
            [XmlAttribute("kind")]
            public string Kind { get; set; }

            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("size")]
            public int? Size { get; set; }


            [XmlElement("commit")]
            public SvnCommit Commit { get; set; }
        }


        [Serializable]
        public class SvnList {
            [XmlAttribute("path")]
            public string Path { get; set; }

            [XmlElement("entry")]
            public SvnEntry[] Entry { get; set; }
        }


        [Serializable]
        [XmlRootAttribute("lists")]
        public class SvnLists {
            [XmlElement("list")]
            public SvnList[] Lists { get; set; }
        }


        private T DeserializeXml<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

      
           MemoryStream s = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(data));
            T result = (T)serializer.Deserialize(s);
            s.Close();
            return result;
        }

        public string Cat(string repoName, string path)
        {
            var repoPath = RepositoriesRoot + "/" + repoName;
            var subPath =  "/" + path;

            var result = SystemProcess.Execute("svnlook", false, $"cat {repoPath} {subPath}");
            if (result.exitCode != 0)
            {
                throw new Exception(string.Join("\r\n", result.errBuffer));
            }
            return  string.Join("\r\n", result.outBuffer);
        }

        public SvnList List(string repoName, string path)
        {
            var listPath = FileSchemeRepositoriesRoot + "/" + repoName + "/" + path;

            var result = SystemProcess.Execute("svn", false, $"list  --xml \"{listPath}\"");
            var combinedResult = string.Join("\r\n", result.outBuffer);


            var lists = DeserializeXml<SvnLists>(combinedResult);
            return lists.Lists[0];

        }

        public int GetFileSize(string repoName, string relativeFilePath)
        {
            var repoPath = RepositoriesRoot + "/" + repoName;
            var subPath = "/" + relativeFilePath;

            var result = SystemProcess.Execute("svnlook", false, $"filesize {repoPath} {subPath}");
            if (result.exitCode != 0) {
                throw new Exception(string.Join("\r\n", result.errBuffer));
            }


            return int.Parse(result.outBuffer[0]); // string.Join("\r\n", result.outBuffer);
            //svnlook filesize  E:/Repositories/TestRepo1 /trunk/InspectorButton.cs
        }

        void Mkdir(string repoName, params string[] subpath) {
            var request = $"mkdir -m \"Create initial repo layout\" {string.Join(" ", subpath.Select(v => FileSchemeRepositoriesRoot + "/" + repoName + "/" + v))}";
            var result = SystemProcess.Execute("svn", false, request);
            if (!result.Success) {
                throw new Exception(string.Join("\r\n", result.errBuffer));
            }
        }

        public void CreateRepository(string name, bool createLayout) {
            var result = SystemProcess.Execute("svnadmin", false, $"create {RepositoriesRoot + "/" + name}");
            if(!result.Success) {
                throw new Exception(string.Join("\r\n", result.errBuffer));
            }


            if (createLayout) {
                Mkdir(name, "branches", "tags", "trunk");
            }
        }


        /*func isRepository(path string) bool {
            cmd := exec.Command("svnlook", "info", path)
            var out bytes.Buffer
            cmd.Stdout = &out
            err := cmd.Run()

            if err != nil {
                return false
            } else {
                return true
            }

        }*/
    }
}