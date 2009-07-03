using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataCompiler.Entities
{
    [Serializable]
    [XmlRoot( ElementName = "entity" )]
    public class Entity : Serializable
    {
        [XmlArray( ElementName = "components", IsNullable = false )]
        [XmlArrayItem( ElementName = "component", Type = typeof( EntityComponent ) )]
        public List<EntityComponent> components { get; set; }

        [XmlAttribute( AttributeName = "name" )]
        public string Name { get; set; }

        [XmlAttribute( AttributeName = "src" )]
        public string Source { get; set; }

        public Entity( )
            : this( string.Empty )
        {
            
        }

        public Entity( string name )
        {
            this.Name = name;
            this.components = new List<EntityComponent>( );
        }
    }
}