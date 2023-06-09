using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn.Menu2;

public class LevelSelect
{
    private readonly List<Button> _levelButton = new();
    private readonly List<Texture2D> _texture2DPerson;


    public LevelSelect(List<Texture2D> texture2DPerson)
    {
        _texture2DPerson = texture2DPerson;
    }

    public void LevelSelectLoadContent(ref List<Component> selectLvlComponents, Action<State> changeState,
        Action<int> changeLevelValue)
    {
        var tempButtonPosX = 294f;
        var tempButtonPosY = 166f;


        for (var i = 0; i < 6; i++)
        {
            var i1 = i;
            if (i == 3)
            {
                tempButtonPosY = 166f;
                tempButtonPosX = 914f;
            }

            _levelButton.Add(CreateButton(tempButtonPosX, tempButtonPosY, _texture2DPerson[i]));

            _levelButton[i].Click += (_, _) =>
            {
                changeLevelValue(i1);
                changeState(State.Game);
                // добавлено, чтобы сделать паузу при переключении с выбора уровня на саму игру (можно убрать)
                System.Threading.Thread.Sleep(500);
            };

            tempButtonPosY += 167f;
        }

        _levelButton.Add(new Button(_texture2DPerson[^2], null) { Position = new Vector2(294f, 688f), });

        _levelButton[^1].Click += (_, _) => { changeState(State.Menu); };

        selectLvlComponents = new List<Component>(_levelButton);
    }

    private static Button CreateButton(float x, float y, Texture2D texture2D) =>
        new(texture2D, null)
        {
            Position = new Vector2(x, y),
        };

    public void Update(ref List<Component> _selectLvlComponents, GameTime gameTime, State state)
    {
        if (state == State.LevelSelect)
        {
            foreach (var component in _selectLvlComponents)
                component.Update(gameTime);
        }
    }

    public void Draw(ref List<Component> _selectLvlComponents, SpriteBatch spriteBatch, GameTime gameTime, State state)
    {
        if (state != State.LevelSelect)
            return;

        spriteBatch.Draw(_texture2DPerson[^1]
            , new Vector2(0, 0),
            Color.FromNonPremultiplied(255, 255, 255, 300));
        foreach (var component in _selectLvlComponents)
            component.Draw(gameTime, spriteBatch);
    }
}