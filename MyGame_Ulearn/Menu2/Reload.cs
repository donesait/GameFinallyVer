using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn.Menu2;

public class Reload
{
    private readonly List<Texture2D> _texture2D;
    private readonly SpriteFont _font = null;


    public Reload(List<Texture2D> texture2D)
    {
        _texture2D = texture2D;
    }

    public void LoadContentReload(Action<State> changeState, ref List<Component> reloadComponents)
    {
        var reloadLevelButton = new Button(_texture2D[1], _font)
        {
            Position = new Vector2(854f, 305f),
        };
        var returnSelectLevelButton = new Button(_texture2D[0], _font)
        {
            Position = new Vector2(452f, 305f),
        };

        reloadLevelButton.Click += (_, _) => { changeState(State.Game); };
        returnSelectLevelButton.Click += (_, _) => { changeState(State.LevelSelect); };
        reloadComponents = new List<Component> { reloadLevelButton, returnSelectLevelButton };
    }

    public void Update(State state, ref List<Component> reloadComponents, GameTime gameTime)
    {
        if (state != State.Reload)
            return;

        foreach (var component in reloadComponents)
            component.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, State state, ref List<Component> reloadComponents,
        GameTime gameTime)
    {
        if (state != State.Reload)
            return;

        spriteBatch.Draw(_texture2D[^1], Vector2.Zero, Color.White);

        foreach (var component in reloadComponents)
            component.Draw(gameTime, spriteBatch);
    }
}