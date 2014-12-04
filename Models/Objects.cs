using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S9APIUploadDemo.Models
{
    public class License
    {
        public License() { }

        public string AuthServer { get; set; }
        public DateTime DateAccessed { get; set; }
        public DateTime DateCreated { get; set; }
        public string Domain { get; set; }
        public string IPAddress { get; set; }
        public int Reg { get; set; }
        public string Token { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
    }

    public class FieldItem
    {
        public List<String> MVAL;
        public int ID { get; set; }
        public string VAL { get; set; }

        public FieldItem(int id, string val)
        {
            ID = id;
            VAL = val;
        }
    }

    public class Indexer
    {
        public List<Field> fields { get; set; }
        public List<File> files { get; set; }
        public Indexer()
        {
            fields = new List<Field>();
            files = new List<File>();
        }
    }

    public class File
    {
        public String name;
        public File() { }
        public File(String Name)
        {
            name = Name;
        }
    }

    public class Field
    {
        public String name;
        public String value;
        public Field() { }
        public Field(String Name, String Value)
        {
            this.name = Name;
            this.value = Value;
        }
    }

    public class UploadedFile
    {
        public String name { get; set; }
        public Boolean isEmail { get; set; }
        public Dictionary<string, string> oEmailData { get; set; }
        public String test { get; set; }
    }

    public class UploadedFileList
    {
        public List<UploadedFile> files { get; set; }
    }
}
