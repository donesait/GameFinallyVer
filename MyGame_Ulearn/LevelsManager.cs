using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Code;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn;

public class LevelsManager
{
    public List<Level> Levels;
    private List<SpaceShip> _players;
    private readonly List<Texture2D> _gameBackground;
    private SpriteFont _font = null;
    private List<Texture2D> _monstersSprite = new();

    public LevelsManager(List<SpaceShip> players, List<Texture2D> gameBackground, List<Texture2D> monstersSprite)
    {
        _players = players;
        _gameBackground = gameBackground;
        _monstersSprite = monstersSprite;
    }
    
    public void LoadContentMenu(Texture2D texture2DButton, Action<State> changeState, ref List<Component> gameComponents)
    {
        var homeButton = new Button(texture2DButton, _font)
        {
            Position = new Vector2(19f, 830f),
        };
        
        homeButton.Click += (_, _) => { changeState(State.LevelSelect); };
        gameComponents = new List<Component> { homeButton};
    }

    public void CreateLevels()
    {
        Levels = new List<Level>
        {
            new(15, 1, 5, _players[0], 1f, _gameBackground[0], 1f, _monstersSprite[0]),
            new(15, 2, 8, _players[0], 2f, _gameBackground[0], 5f, _monstersSprite[0]),
            new(25, 3, 12, _players[1], 2.5f, _gameBackground[2], 6f, _monstersSprite[0]),
            new(25, 4, 15, _players[1], 2.5f, _gameBackground[2], 7f, _monstersSprite[0]),
            new(35, 4, 15, _players[2], 3f, _gameBackground[1], 8f, _monstersSprite[1]),
            new(45, 5, 15, _players[2], 5f, _gameBackground[1], 8f, _monstersSprite[1])
        };
    }
}