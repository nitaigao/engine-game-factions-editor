using System;
using System.IO;
using System.Text;
using System.Xml;
using DataCompiler.System;
using System.Text.RegularExpressions;

namespace DataCompiler
{
    public class MaxParameters
    {
        public string sceneFilePath { get; set; }
        public string outputFilePath { get; set; }

        public string purgePath { get; set; }
        public string entitiesPath { get; set; }
    }

    public class MaxInterface
    {
        public static void PurgeLevelData( MaxParameters parameters )
        {
            var directoryInfo = new DirectoryInfo( parameters.purgePath );

            foreach ( var file in directoryInfo.GetFiles( ) )
            {
                File.Delete( file.FullName );
            }

            foreach ( var directory in directoryInfo.GetDirectories( ) )
            {
                Directory.Delete( directory.FullName, true );
            }
        }

        public static void SetGamePaths( MaxParameters parameters )
        {
            var entitiesDirInfo = new DirectoryInfo( parameters.entitiesPath );

            foreach ( var subDirectory in entitiesDirInfo.GetDirectories( ) )
            {
                foreach ( var file in subDirectory.GetFiles( ) )
                {
                    Encoding en1252 = Encoding.GetEncoding( 1252 );

                    var inputStream = File.Open( file.FullName, FileMode.Open );
                    var reader = new StreamReader( inputStream, en1252 );
                    string fileData = reader.ReadToEnd( );
                    reader.Close( );
                    inputStream.Close( );

                    fileData = Regex.Replace( fileData, @"meshFile=('|"")([A-Za-z0-9_\-\.]*\.mesh)('|"")", string.Format( "meshFile=\"{0}/$2\"", Paths.MeshesGamePath ) );
                    fileData = Regex.Replace( fileData, @"([A-Za-z0-9_\-\.]*)(jpg|tga|bmp|gif)", string.Format( "{0}/$&", Paths.TexturesGamePath ) );
                    fileData = Regex.Replace( fileData, @"([A-Za-z0-9_\-\.]*)(.skeleton)", string.Format( "{0}/$&", Paths.MeshesGamePath ) );

                    var outputStream = File.Open( file.FullName, FileMode.Truncate );
                    var writer = new StreamWriter( outputStream, en1252 );
                    writer.Write( fileData );
                    writer.Close( );
                }
            }
        }

        public static void CompileModels( MaxParameters parameters )
        {
            var sceneFileInfo = new FileInfo( parameters.sceneFilePath );
            Paths.WorkingDirectory = sceneFileInfo.Directory.FullName;

            var sceneFile = new XmlDocument( );
            sceneFile.Load( parameters.sceneFilePath );

            var modelCompiler = new ModelCompiler( );
            var models = modelCompiler.CompileModelsFromScene( sceneFile );

            foreach ( XmlNode model in models )
            {
                var rootNode = model.SelectSingleNode( "/node" );

                var xmlTextWriter = new XmlTextWriter( string.Format( "{0}/{1}.model", Paths.ModelsFullPath, rootNode.Attributes[ "name" ].Value ), Encoding.UTF8 );
                xmlTextWriter.Formatting = Formatting.Indented;
                
                model.WriteTo( xmlTextWriter );
                xmlTextWriter.Close( );
            }
        }

        public static void CompileLevel( MaxParameters parameters )
        {
            var inputFileInfo = new FileInfo( parameters.sceneFilePath );
            Paths.WorkingDirectory = inputFileInfo.Directory.FullName;

            var inputReader = File.OpenText( inputFileInfo.FullName );
            string inputXml = inputReader.ReadToEnd( ).Replace( "<![CDATA[", "" ).Replace( "]]>", "" );
            inputReader.Close( );

            var sceneFile = new XmlDocument( );
            sceneFile.LoadXml( inputXml );

            var compiler = new LevelCompiler( );
            var levelFile = compiler.CompileLevel( sceneFile );

            var writer = new XmlTextWriter( parameters.outputFilePath, Encoding.Default );
            writer.Formatting = Formatting.Indented;
            levelFile.WriteTo( writer );
            writer.Close( );
        }
    }
}
