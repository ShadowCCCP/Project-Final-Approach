using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : GameObject
    {
        TiledLoader loader;
        string currentLevelName;

        // Use this to load level:
        // ((MyGame) game).LoadLevel("MainMenu.tmx");

        public Level(string filename)
        {
            currentLevelName = filename;
            loader = new TiledLoader(filename);
            CreateLevel();
        }

        private void CreateLevel(bool includeImageLayer = true)
        {
            try
            {
                loader.rootObject = this;
                loader.addColliders = false;
                loader.LoadImageLayers();
                loader.LoadTileLayers(0);
                loader.autoInstance = true;
                loader.LoadObjectGroups();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load level: " + e);
            }
        }
    }
}
