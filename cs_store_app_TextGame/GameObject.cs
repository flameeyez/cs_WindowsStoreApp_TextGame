using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "GameObject", Namespace = "cs_store_app_TextGame")]
    public class GameObject
    {
        [DataMember]
        public int NID { get; set; }
    }
}
