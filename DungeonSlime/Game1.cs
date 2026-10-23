using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime;

public class Game1 : Core
{

    // texture region that defines slime sprite in atlas
    private TextureRegion _slime;

    // texture region that defines slime sprite in atlas
    private TextureRegion _bat;

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
        // Create the atlas texture from the XML config file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        // Retrieve slime region from atlas
        _slime = atlas.GetRegion("slime");

        // Retrive bat region from atlas
        _bat = atlas.GetRegion("bat");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin sprite batch to prepare for rendering
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp); // SamplerState.PointClamp allows scaled pixel art to stay sharp.

        // Draw the slime texture region at a scale of 4.0
        _slime.Draw(SpriteBatch, Vector2.Zero, Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 1.0f);

        // Draw the bat texture region 10px to the right of the slime at a scale of 4.0
        _bat.Draw(SpriteBatch, new Vector2(_slime.Width * 4.0f + 10.0f, 0), Color.White, 0.0f, Vector2.One, 4.0f, SpriteEffects.None, 1.0f);


        // Always end the sprite batch when finished
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}