using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataCompiler.Entities
{
    [Serializable]
    public class EntityComponent
    {
        [XmlAttribute ( AttributeName = "type" )]
        public string Type { get; set; }

        [XmlAttribute( AttributeName = "system" )]
        public string System { get; set; }

        [XmlArray( ElementName = "attributes", IsNullable = false )]
        [XmlArrayItem( ElementName = "attribute", Type = typeof( EntityAttribute ) )]
        public List< EntityAttribute > Attributes;

        public EntityComponent( )
            : this( string.Empty )
        {

        }

        public EntityComponent( string system )
        {
            this.System = system;
            this.Attributes = new List<EntityAttribute>( );
            this.Type = "default";
        }
    }
}