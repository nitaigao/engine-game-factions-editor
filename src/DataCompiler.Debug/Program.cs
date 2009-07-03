
namespace DataCompiler.Debug
{
    class Program
    {
        static void Main( string[ ] args )
        {
            //MaxInterface.CompileLevel( new MaxParameters( ) {sceneFilePath = args[ 0 ], outputFilePath = args[ 1 ]} );

            //MaxInterface.CompileModelsFromScene( new MaxParameters(  ) { sceneFilePath = args[ 0 ] } );

            MaxInterface.SetGamePaths( new MaxParameters(  ) { entitiesPath = args[ 0 ] } );
        }
    }
}
