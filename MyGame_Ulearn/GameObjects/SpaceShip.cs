using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame_Ulearn.Code;

public class SpaceShip
{
    private Texture2D Ship { get; }
    public float WindowWidth { get; }
    public float WindowHeight { get; }


    private Point SpriteSize { get; }
    private const float Speed = 20f;
    public readonly HashSet<Missile> BulletHashSet = new();
    private KeyboardState _keyboardState, _prevKeyboardState;
    public Vector2 Position;

    public Vector2 StartPosition;
    //private Rectangle ShipSprite;

    public SpaceShip(GameWindow window, Texture2D texture2D)
    {
        Ship = texture2D;
        Position = new Vector2(window.ClientBounds.Width / 2 - 100,
            window.ClientBounds.Height - 225);
        WindowWidth = window.ClientBounds.Width;
        WindowHeight = window.ClientBounds.Height;
        SpriteSize = new Point(Ship.Width, Ship.Height);
        StartPosition = Position;
    }

    public void Update()
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left) && Position.X > 1)
            Position.X -= Speed;
        if (keyboardState.IsKeyDown(Keys.Right) && Position.X < WindowWidth - SpriteSize.Y)
            Position.X += Speed;
        if (keyboardState.IsKeyDown(Keys.Up) && Position.Y >= 1)
            Position.Y -= Speed;
        if (keyboardState.IsKeyDown(Keys.Down) && Position.Y + SpriteSize.Y <= WindowHeight - 90)
            Position.Y += Speed;

        if (keyboardState.IsKeyDown(Keys.LeftControl) && _prevKeyboardState.IsKeyUp(Keys.LeftControl))
            BulletHashSet.Add(new Missile(Position));

        foreach (var bul in BulletHashSet)
        {
            if (bul.GetBulletPosition().Y <= 0)
                BulletHashSet.Remove(bul);
            bul.Update();
        }

        _prevKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Ship, Position, Color.White);

        foreach (var bul in BulletHashSet)
            bul.Draw(spriteBatch);
    }
}