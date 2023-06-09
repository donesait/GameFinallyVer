using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn.Menu2;

public class Congratulation
{
    private readonly List<Texture2D> _texture2D;
    private readonly SpriteFont _font = null;
    private int _curLevelValue;


    public Congratulation(List<Texture2D> texture2D)
    {
        _texture2D = texture2D;
    }

    public void LoadContentCongratulation(Action<State> changeState, ref List<Component> congratulationComponents,
        Action<int> changeLevelValue)
    {
        var nextLevelButton = new Button(_texture2D[1], _font)
        {
            Position = new Vector2(854f, 305f),
        };
        var returnSelectLevelButton = new Button(_texture2D[0], _font)
        {
            Position = new Vector2(452f, 305f),
        };

        nextLevelButton.Click += (_, _) =>
        {
            if (_curLevelValue + 1 < 6)
            {
                changeLevelValue(_curLevelValue + 1);
                changeState(State.Game);
            }
            else
                changeState(State.LevelSelect);
        };
        returnSelectLevelButton.Click += (_, _) => { changeState(State.LevelSelect); };
        congratulationComponents = new List<Component> { nextLevelButton, returnSelectLevelButton };
    }

    public void Update(State state, ref List<Component> congratulationComponents, GameTime gameTime, int levelValue)
    {
        _curLevelValue = levelValue;
        if (state != State.Congratulation)
            return;
        foreach (var component in congratulationComponents)
            component.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, State state, ref List<Component> congratulationComponents,
        GameTime gameTime)
    {
        if (state != State.Congratulation)
            return;
        spriteBatch.Draw(_texture2D[^1], Vector2.Zero, Color.White);

        foreach (var component in congratulationComponents)
            component.Draw(gameTime, spriteBatch);
    }
}