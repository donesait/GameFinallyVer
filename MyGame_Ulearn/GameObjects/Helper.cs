using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame_Ulearn.Code;

public class Helper
{
    public static Texture2D Texture { get; set; }
    public Rectangle HelperSprite;
    private float Speed = 20f;
    public Vector2 Position;

    public static readonly Helper Zero = new(new Vector2(2000, 2000), 0.0f);


    public Helper(Vector2 position, float speed)
    {
        Speed = speed;
        Position = position;
    }

    public void Update()
    {
        Position.Y += Speed;
        HelperSprite = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width+20, Texture.Height+10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}