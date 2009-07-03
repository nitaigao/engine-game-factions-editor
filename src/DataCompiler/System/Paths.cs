

using System;
using System.IO;

namespace DataCompiler.System
{
    public abstract class Paths
    {
        public static string BaseGamePath
        {
            get { return "/data/entities"; }
        }

        public static string BodiesGamePath
        {
            get { return string.Format( "{0}/bodies", Paths.BaseGamePath ); }
        }

        public static string TexturesGamePath
        {
            get { return string.Format( "{0}/textures", Paths.BaseGamePath ); }
        }

        public static string MeshesGamePath
        {
            get { return string.Format( "{0}/meshes", Paths.BaseGamePath ); }
        }

        public static string ModelsGamePath
        {
            get { return string.Format( "{0}/models", Paths.BaseGamePath ); }
        }

        public static string ScriptsGamePath
        {
            get { return string.Format( "{0}/scripts", Paths.BaseGamePath ); }
        }

        public static string AnimationsGamePath
        {
            get { return string.Format( "{0}/animations", Paths.BaseGamePath ); }
        }

        public static string WorkingDirectory { get; set; }

        public static string ModelsFullPath
        {
            get { return string.Format( "{0}/models", Paths.WorkingDirectory ); }
        }
    }
}
