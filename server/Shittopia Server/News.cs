namespace Shittopia_Server
{
    internal class News
    {
        public int id { get; set; }

        public string title { get; set; }

        public string details { get; set; }

        public string imageURL { get; set; }

        public News(int _id, string _title, string _details, string _imageURL)
        {
            this.id = _id;
            this.title = _title;
            this.details = _details;
            this.imageURL = _imageURL;
        }
    }
}
