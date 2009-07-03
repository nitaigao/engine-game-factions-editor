using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataCompiler.Environment
{
    [Serializable]
    [XmlRoot( ElementName = "environment" )]
    public class Environment : Serializable
    {
        [XmlArray( ElementName = "colors" )]
        [XmlArrayItem( ElementName = "color" )]
        public List<EnvironmentColor> Colors { get; set; }

        public Environment( )
        {
            this.Colors = new List< EnvironmentColor >( );
        }
    }
}
