using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame_Ulearn.Code;

public class Monster
{
    private Texture2D Mob { get; }
    public Vector2 Position;
    private readonly float _speed;
    public Rectangle MonsterSprite;

    public Monster(Vector2 position, float speed, Texture2D mobSprite)
    {
        _speed = speed;
        Position = position;
        Mob = mobSprite;
    }

    public void Update()
    {
        Position.Y += _speed;
        MonsterSprite = new Rectangle((int)Position.X, (int)Position.Y, Mob.Width, Mob.Height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Mob, Position, Color.White);
    }
}