using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MyGame_Ulearn.Code;
using MyGame_Ulearn.Menu2;
using MyGame_Ulearn.Objects;


namespace MyGame_Ulearn;

#region Enums

public enum State
{
    Menu,
    LevelSelect,
    Game,
    Congratulation,
    Reload
}

public enum LevelValue
{
    First = 0,
    Second = 1,
    Third = 2,
    Fourth = 3,
    Fifth = 4,
    Six = 5
}

#endregion

#region EnumExtensions

static class LevelValueExtensions
{
    public static LevelValue From(int valueLevel)
    {
        return valueLevel switch
        {
            0 => LevelValue.First,
            1 => LevelValue.Second,
            2 => LevelValue.Third,
            3 => LevelValue.Fourth,
            4 => LevelValue.Fifth,
            5 => LevelValue.Six,
            _ => throw new ArgumentOutOfRangeException(nameof(valueLevel), valueLevel, "Error")
        };
    }
}

#endregion

public class Game1 : Game
{
    #region classFields

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backGroundMenu;
    private List<Component> _backGroundSelectLevel;
    private LevelValue _levelValue;
    private State _state = State.Menu;
    private SpriteFont _font;
    private List<Component> _gameComponents;
    private List<Component> _selectLvlComponents;
    private List<SpaceShip> _players;
    private LevelsManager _levelsManager;
    private Congratulation _congratulation;
    private LevelSelect _levelSelect;
    private Menu _menu;
    private List<Component> _congratulationComponents;
    private Reload _reload;
    private List<Component> _reloadComponents;
    private Texture2D _backGroundGame;
    private List<Texture2D> GameBackground;
    private List<Component> _gameBComponents;
    private List<Texture2D> _monstersSprite;

    #endregion

    //private Song song;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1600;
        _graphics.PreferredBackBufferHeight = 900;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        //song = Content.Load<Song>("White_Punk_-_Vampir_60075944");
       // MediaPlayer.Play(song);

        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("TextForCountHealth");
        //Monster.Mob = Content.Load<Texture2D>("sprite mon/mon2");
        Missile.Bullet = Content.Load<Texture2D>("Game/bulllll");
        Helper.Texture = Content.Load<Texture2D>("Game/hilka");

        #region Congratulation

        var textureCongratulation = new List<Texture2D>
        {
            Content.Load<Texture2D>("Congratulation/homeButton"),
            Content.Load<Texture2D>("Congratulation/next"),
            Content.Load<Texture2D>("Congratulation/Congratulation")
        };
        _congratulation = new Congratulation(textureCongratulation);
        _congratulation.LoadContentCongratulation(ChangeState, ref _congratulationComponents, ChangeLevelValue);

        #endregion

        #region Reload

        var textrureReload = new List<Texture2D>
        {
            Content.Load<Texture2D>("Reload/homeBut"),
            Content.Load<Texture2D>("Reload/resetBut"),
            Content.Load<Texture2D>("Reload/reload"),
        };
        _reload = new Reload(textrureReload);
        _reload.LoadContentReload(ChangeState, ref _reloadComponents);

        #endregion

        #region shuttle

        _players = new List<SpaceShip>
        {
            new(Window, Content.Load<Texture2D>("Спрайты персонажа/1person")),
            new(Window, Content.Load<Texture2D>("Спрайты персонажа/2person")),
            new(Window, Content.Load<Texture2D>("Спрайты персонажа/3person"))
        };

        #endregion

        _monstersSprite = new List<Texture2D>
        {
            Content.Load<Texture2D>("sprite mon/mon2"),
            Content.Load<Texture2D>("sprite mon/mon3"),
        };

        GameBackground = new List<Texture2D>
        {
            Content.Load<Texture2D>("GameWin/gameWin1"),
            Content.Load<Texture2D>("GameWin/gameWin2"),
            Content.Load<Texture2D>("GameWin/gameWin3")
        };

        #region levelsManager

        _levelsManager = new LevelsManager(_players, GameBackground, _monstersSprite);
        _levelsManager.LoadContentMenu(Content.Load<Texture2D>("GameWin/homeff"), ChangeState, ref _gameBComponents);
        _levelsManager.CreateLevels();

        #endregion

        #region Menu

        var textureMenu = new List<Texture2D>
        {
            Content.Load<Texture2D>("MenuSpr/start1"),
            Content.Load<Texture2D>("MenuSpr/exit1"),
            Content.Load<Texture2D>("MenuSpr/backgroundMenu12")
        };
        _menu = new Menu(textureMenu);
        _menu.LoadContentMenu(Exit, ChangeState, ref _gameComponents);

        #endregion

        #region LevelsSelect

        var textureLevelSelect = new List<Texture2D>()
        {
            Content.Load<Texture2D>("SelectLevel/1lvl"),
            Content.Load<Texture2D>("SelectLevel/2lvl"),
            Content.Load<Texture2D>("SelectLevel/3lvl"),
            Content.Load<Texture2D>("SelectLevel/4lvl"),
            Content.Load<Texture2D>("SelectLevel/5lvl"),
            Content.Load<Texture2D>("SelectLevel/6lvl"),
            Content.Load<Texture2D>("SelectLevel/backtothemenu"),
            Content.Load<Texture2D>("SelectLevel/selectLevel1")
        };

        _levelSelect = new LevelSelect(textureLevelSelect);
        _levelSelect.LevelSelectLoadContent(ref _selectLvlComponents, ChangeState, ChangeLevelValue);

        #endregion
    }

    private void ChangeLevelValue(int i) => _levelValue = LevelValueExtensions.From(i);
    private void ChangeState(State state) => _state = state;

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _congratulation.Update(_state, ref _congratulationComponents, gameTime, (int)_levelValue);
        _reload.Update(_state, ref _reloadComponents, gameTime);

        _levelsManager.Levels[(int)_levelValue].Update(ChangeState, _state, gameTime, ref _gameBComponents);

        _levelSelect.Update(ref _selectLvlComponents, gameTime, _state);

        _menu.Update(gameTime, ref _gameComponents, _state);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();

        _congratulation.Draw(_spriteBatch, _state, ref _congratulationComponents, gameTime);
        _reload.Draw(_spriteBatch, _state, ref _reloadComponents, gameTime);


        _levelsManager.Levels[(int)_levelValue].Draw(_spriteBatch, _font, _state, gameTime, ref _gameBComponents);

        _menu.Draw(_spriteBatch, gameTime, ref _gameComponents, _state);

        _levelSelect.Draw(ref _selectLvlComponents, _spriteBatch, gameTime, _state);


        _spriteBatch.End();
        base.Draw(gameTime);
    }
}