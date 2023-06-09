using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame_Ulearn.Code;
using MyGame_Ulearn.Objects;

namespace MyGame_Ulearn;

public class Level
{
    private Texture2D BackgroundGame { get; }

    private float MinMonsterSpeed { get; }
    private float MaxMonsterSpeed { get; }


    private int HealthCount { get; set; }
    private int HealthCountTemp { get; }

    private int MaxMonsterCount { get; }
    private Texture2D MobSprite { get; }
    private int _killsForWin;
    private readonly int _killsForWinTemp;

    private readonly SpaceShip _shuttle;
    private readonly HashSet<Monster> _monstersHashSet = new();
    private Helper _helper = Helper.Zero;
    private readonly Random _randomGenerator = new();

    public Level(int killsForWin, int healthCount, int maxMonsterCount, SpaceShip shuttle,
        float minMonsterSpeed, Texture2D backgroundGame, float maxMonsterSpeed, Texture2D mobSprite)
    {
        HealthCount = healthCount;
        MaxMonsterCount = maxMonsterCount;
        _killsForWin = killsForWin;
        _shuttle = shuttle;
        MinMonsterSpeed = minMonsterSpeed;
        MaxMonsterSpeed = maxMonsterSpeed;
        _killsForWinTemp = _killsForWin;
        HealthCountTemp = HealthCount;
        BackgroundGame = backgroundGame;
        MobSprite = mobSprite;
    }

    public void Update(Action<State> changeState, State state, GameTime gameTime, ref List<Component> gameComponents)
    {
        if (state != State.Game)
            return;

        GenerateMonster();
        GenerateHelper();

        foreach (var component in gameComponents)
            component.Update(gameTime);

        #region LevelUpdate

        _shuttle.Update();
        _helper.Update();

        foreach (var monster in _monstersHashSet)
        {
            if (monster.Position.Y >= _shuttle.WindowHeight)
            {
                if (HealthCount <= 1)
                {
                    _monstersHashSet.Clear();
                    _shuttle.Position = _shuttle.StartPosition;
                    _killsForWin = _killsForWinTemp;
                    HealthCount = HealthCountTemp;
                    changeState(State.Reload);
                }
                else
                {
                    _monstersHashSet.Remove(monster);
                    HealthCount--;
                }
            }

            if (_helper.Position.Y >= _shuttle.WindowHeight)
                _helper = Helper.Zero;

            foreach (var bullet in _shuttle.BulletHashSet)
            {
                if (monster.MonsterSprite.Intersects(bullet.MissileSprite))
                {
                    _monstersHashSet.Remove(monster);
                    _shuttle.BulletHashSet.Remove(bullet);
                    _killsForWin--;
                }

                if (!bullet.MissileSprite.Intersects(_helper.HelperSprite)) continue;
                HealthCount++;
                _helper = Helper.Zero;
                _shuttle.BulletHashSet.Remove(bullet);
            }


            if (_killsForWin == 0)
            {
                _monstersHashSet.Clear();
                _shuttle.Position = _shuttle.StartPosition;
                _killsForWin = _killsForWinTemp;
                HealthCount = HealthCountTemp;
               // Thread.Sleep(500);
                changeState(State.Congratulation);
            }

            monster.Update();
        }

        #endregion
    }

    #region GenerateMonster

    private void GenerateMonster()
    {
        if (_randomGenerator.Next(0, 10) != 6 || _monstersHashSet.Count >= MaxMonsterCount) return;

        var possibleMonster = new Monster(new Vector2(_randomGenerator.Next(0, (int)_shuttle.WindowWidth - 80),
                _randomGenerator.Next(0, 30)), _randomGenerator.Next((int)MinMonsterSpeed, (int)MaxMonsterSpeed),
            MobSprite);
        
        if (_monstersHashSet.Count < 1)
        {
            _monstersHashSet.Add(possibleMonster);
            return;
        }

        if (_monstersHashSet.All(m => m.MonsterSprite.Intersects(possibleMonster.MonsterSprite))) return;
        _monstersHashSet.Add(possibleMonster);
    }

    #endregion

    #region GenerateHelper
    private void GenerateHelper()
    {
        if (_killsForWinTemp - _killsForWin > 5 && _randomGenerator.Next(0, 500) == 3 && _helper == Helper.Zero)
            _helper = new Helper(new Vector2(_randomGenerator.Next(0, (int)_shuttle.WindowWidth - 80),
                _randomGenerator.Next(0, 30)), 0.8f);
    }

    #endregion

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, State state, GameTime gameTime,
        ref List<Component> gameComponents)
    {
        if (state != State.Game)
            return;

        spriteBatch.Draw(BackgroundGame, Vector2.Zero, Color.White);

        foreach (var monster in _monstersHashSet)
            monster.Draw(spriteBatch);

        if (_helper != Helper.Zero)
            _helper.Draw(spriteBatch);

        _shuttle.Draw(spriteBatch);

        foreach (var component in gameComponents)
            component.Draw(gameTime, spriteBatch);

        spriteBatch.DrawString(font, $"{HealthCount}", new Vector2(380, 827), Color.White);
        spriteBatch.DrawString(font, $"{_killsForWinTemp - _killsForWin}", new Vector2(685, 827), Color.White);
    }
}