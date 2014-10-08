using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityAttributes", Namespace = "cs_store_app_TextGame")]
    public class EntityAttributes
    {
        [DataMember]
        public int Strength { get; set; }
        [DataMember]
        public int Intelligence { get; set; }
        [DataMember]
        public int Vitality { get; set; }

        #region Health/Magic
        [DataMember]
        private int _currenthealth;
        public int CurrentHealth
        {
            get
            {
                return _currenthealth;
            }
            set
            {
                _currenthealth = value;

                if(_currenthealth > MaximumHealth)
                {
                    _currenthealth = MaximumHealth;
                }
            }
        }
        [DataMember]
        public int MaximumHealth { get; set; }
        public string HealthString
        {
            get
            {
                return CurrentHealth.ToString() + "/" + MaximumHealth.ToString();
            }
        }

        [DataMember]
        private int _currentmagic;
        public int CurrentMagic
        {
            get
            {
                return _currentmagic;
            }
            set
            {
                _currentmagic = value;

                if (_currentmagic > MaximumMagic)
                {
                    _currentmagic = MaximumMagic;
                }
            }
        }
        [DataMember]
        public int MaximumMagic { get; set; }
        public string MagicString
        {
            get
            {
                return CurrentMagic.ToString() + "/" + MaximumMagic.ToString();
            }
        }
        #endregion

        public EntityAttributes()
        {
            Strength = 20;
            Intelligence = 20;
            Vitality = 20;
            
            MaximumHealth = 20;
            CurrentHealth = 20;
            MaximumMagic = 20;
            CurrentMagic = 20;
        }
        public EntityAttributes(XElement element)
        {
            Strength = int.Parse(element.Element("strength").Value);
            Intelligence = int.Parse(element.Element("intelligence").Value);
            Vitality = int.Parse(element.Element("vitality").Value);
            MaximumHealth = int.Parse(element.Element("maximum-health").Value);
            CurrentHealth = MaximumHealth;
            MaximumMagic = int.Parse(element.Element("maximum-magic").Value);
            CurrentMagic = MaximumMagic;
        }
    }
}
