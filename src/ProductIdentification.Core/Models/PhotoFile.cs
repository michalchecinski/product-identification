namespace ProductIdentification.Core.Models
{
    public class PhotoFile
    {
        public PhotoFile(string name, string path)
        {
            Name = name;
            Path = path;
        }
        
        public string Name { get; set; }
        public string Path { get; set; }
    }
}