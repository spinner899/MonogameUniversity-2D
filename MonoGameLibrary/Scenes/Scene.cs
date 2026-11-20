using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    /// <summary>
    /// Gets ContentManager used for loading scene-specific assets.
    /// </summary>
    /// <remarks>
    /// Assests loaded through this Content Manager will automatically be unloaded when this scene ends.
    /// </remarks>
    protected ContentManager Content { get; }

    /// <summary>
    /// Gets value that indicates if scene has been disposed of.
    /// </summary>
    public bool IsDisposed { get; private set; }


    public Scene()
    {
        // Create content manager for the scene
        Content = new ContentManager(Core.Content.ServiceProvider);

        // Set root directory for content to same as root directory for game's content
        Content.RootDirectory = Core.Content.RootDirectory;
    }

    // Finalizer, called when object is cleaned up by garbage collector
    ~Scene() => Dispose(false);

    /// <summary>
    /// Initialized scene.
    /// </summary>
    /// <remarks>
    /// When overrided this derived class, ensre base,Initialize() still called as this is when LoadContent is called.
    /// </remarks>
    public virtual void Initialize()
    {
        LoadContent();
    }

    /// <summary>
    /// Override to provide logic to load content for scene.
    /// </summary>
    public virtual void LoadContent() { }

    /// <summary>
    /// Unloads scene-specific content.
    /// </summary>
    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    /// <summary>
    /// Updates this scene.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for current frame.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Draws this scene.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing valeusfor the current frame.</param>
    public virtual void Draw(GameTime gameTime) { }

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    /// <param name="disposing">
    /// Indicated whether managed resources should be disposed. This is only true when called from main Dispose method.
    /// When called from the finalized, this fill be false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnloadContent();
            Content.Dispose();
        }
        IsDisposed = true;
    }
}