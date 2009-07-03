using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCompiler.Environment
{
    [Serializable]
    [XmlRootAttribute( ElementName = "color" )]
    public class EnvironmentColor : Serializable
    {
        [XmlAttribute( AttributeName = "type" )]
        public string Type { get; set; }

        [XmlAttribute( AttributeName = "r" )]
        public float R { get; set; }

        [XmlAttribute( AttributeName = "g" )]
        public float G { get; set; }

        [XmlAttribute( AttributeName = "b" )]
        public float B { get; set; }

        public static EnvironmentColor FromXML( XmlNode xmlNode )
        {
            var color = new EnvironmentColor( );

            color.Type = xmlNode.Name;
            color.R = float.Parse( xmlNode.Attributes[ "r" ].Value );
            color.G = float.Parse( xmlNode.Attributes[ "g" ].Value );
            color.B = float.Parse( xmlNode.Attributes[ "b" ].Value );

            return color;
        }
    }
}