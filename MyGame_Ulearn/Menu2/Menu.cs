using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn.Menu2;

public class Menu
{
    private readonly List<Texture2D> _texture2D;
    private readonly SpriteFont _font = null;

    public Menu(List<Texture2D> texture2DMenu)
    {
        _texture2D = texture2DMenu;
    }

    public void LoadContentMenu(Action exit, Action<State> changeState, ref List<Component> gameComponents)
    {
        var startButton = new Button(_texture2D[0], _font)
        {
            Position = new Vector2(458f, 305f),
        };
        var exitButton = new Button(_texture2D[1], _font)
        {
            Position = new Vector2(588f, 485f),
        };
        exitButton.Click += (_, _) => exit();
        startButton.Click += (_, _) => { changeState(State.LevelSelect); };
        gameComponents = new List<Component> { startButton, exitButton };
    }

    public void Update(GameTime gameTime, ref List<Component> gameComponents, State state)
    {
        if (state == State.Menu)
        {
            foreach (var component in gameComponents)
                component.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, ref List<Component> gameComponents, State state)
    {
        if (state != State.Menu)
            return;

        _spriteBatch.Draw(_texture2D[^1], new Vector2(0, 0), Color.FromNonPremultiplied(255, 255, 255, 300));
        
        foreach (var component in gameComponents)
            component.Draw(gameTime, _spriteBatch);
    }
}