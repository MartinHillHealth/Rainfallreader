namespace RainfallReader
{
    internal class Device
    {
        private string id;

        private string name;

        private string location;

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Location
        {
            get
            {
                return location;
            }
        }

        public Device(string id, string name, string location)
        { 
            // Explicit assignment in case we want to do validation or conversion later.
           this.id = id;
           this.name = name;
           this.location = location;
        }
    }
}
