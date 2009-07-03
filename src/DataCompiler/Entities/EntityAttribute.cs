using System;
using System.Xml.Serialization;

namespace DataCompiler.Entities
{
    [Serializable]
    public class EntityAttribute
    {
        [XmlAttribute( "key" )]
        public string Key { get; set; }

        [XmlAttribute( "v1" )]
        public string V1 { get; set; }

        [XmlAttribute( "v2" )]
        public string V2 { get; set; }

        [XmlAttribute( "v3" )]
        public string V3 { get; set; }

        [XmlAttribute( "v4" )]
        public string V4 { get; set; }

        public EntityAttribute( )
            : this( string.Empty )
        {

        }

        public EntityAttribute( string key )
        {
            this.Key = key;
        }
    }
}
