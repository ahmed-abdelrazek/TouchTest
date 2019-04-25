using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TouchTest.Web.Models
{
    [DataContract]
    public class Client
    {
        [DataMember]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [DataMember]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("Address")]
        public string Address { get; set; }

        [DataMember]
        [JsonProperty("Phone")]
        public string Phone { get; set; }
    }
}