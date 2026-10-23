using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{

    // Texture2D that holds a image
    private Texture2D _logo;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _logo = Content.Load<Texture2D>("images/logo");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        var elapsed = gameTime.ElapsedGameTime.TotalSeconds;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Creating rectangles to select icon and words in logo
        Rectangle iconSourceRect = new Rectangle(0, 0, 128, 128);

        Rectangle wordmarkSourceRect = new Rectangle(150, 34, 458, 58);

        // Being sprite batch to prepare for rendering
        SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);


        // Draws the Icon only
        SpriteBatch.Draw(
            // texture
            _logo,
            // position vector
            new Vector2(
                Window.ClientBounds.Width * 0.5f, // x position of sprite
                Window.ClientBounds.Height * 0.5f // y position of sprite
                ),
            // source rectangle (portion of sprite being drawn, null is whole sprite)
            iconSourceRect,
            // color and opacity
            Color.White,
            // rotation
            0.0f,
            // set origin on sprite
            new Vector2(
                iconSourceRect.Width * 0.5f, // x shift of origin from top left corner
                iconSourceRect.Height * 0.5f // y shift of origin from top left corner
                ),
            // scales sprite
            new Vector2(
                1.0f, // scales in x direction
                1.0f // scales in y direction
                ),
            // sprite effects
            SpriteEffects.None,
            // layer depth
            1.0f
            );


        // Draws work mark of logo
        SpriteBatch.Draw(
            _logo,
            new Vector2(
                Window.ClientBounds.Width * 0.5f,
                Window.ClientBounds.Height * 0.5f
                ),
            wordmarkSourceRect,
            Color.White,
            0.0f,
            new Vector2(
                wordmarkSourceRect.Width * 0.5f,
                wordmarkSourceRect.Height * 0.5f
                ),
            1.0f,
            SpriteEffects.None,
            0.0f
            );
             

        // Always end the sprite batch when finished
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}