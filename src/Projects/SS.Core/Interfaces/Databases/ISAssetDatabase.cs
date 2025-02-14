using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISAssetDatabase
    {
        void RegisterTexture(string identifier, Texture2D value);
        void RegisterSpriteFont(string identifier, SpriteFont value);
        void RegisterSong(string identifier, Song value);
        void RegisterSoundEffect(string identifier, SoundEffect value);
        void RegisterEffect(string identifier, Effect value);

        Texture2D GetTexture(string name);
        SpriteFont GetSpriteFont(string name);
        Song GetSong(string name);
        SoundEffect GetSoundEffect(string name);
        Effect GetEffect(string name);
    }
}
