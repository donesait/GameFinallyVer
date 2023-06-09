using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame_Ulearn.Code;

public class Missile
{
    public static Texture2D Bullet { get; set; }
    private Vector2 _position;
    private const float Speed = 30f;
    public Vector2 GetBulletPosition() => _position;
    public Rectangle MissileSprite;
    
    public Missile(Vector2 positionShip)
    {
        _position.X = positionShip.X + 70;
        _position.Y = positionShip.Y - 35;
    }

    public void Update()
    {
        _position.Y -= Speed;
        MissileSprite = new Rectangle((int)_position.X, (int)_position.Y, Bullet.Width, Bullet.Height);
    } 

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Bullet,
            _position, Color.White);
    }
}