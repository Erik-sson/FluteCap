using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluteCap
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		//Player stuff
		Texture2D Player;
		Vector2 PlayerPos, PlayerSpeed;
		Rectangle PlayerRectangle;
		SoundEffect Flute;
		int FluteCounter;
		int Health;
		//PlayerBools
		bool PlayerImmobilized, HasFlute, PlaysTheFlute;
		bool PlayerAlive;
		//Universal
		int Level;
		bool OutOfArea;
		//Level one props
		Vector2 MountainWall, StonePos;
		Rectangle MountainRec, StoneRec;
		Texture2D MountainGray, Stone;
		Vector2 FencePos;
		Rectangle FenceRec;
		Texture2D Fence;
		Vector2 TreePos1, TreePos2, TreePos3;
		Rectangle TreeRec1, TreeRec2, TreeRec3;
		Texture2D Tree;
		//Level two props
		Rectangle BossRec;
		Texture2D Boss;
		Vector2 BossPos, BossDirection, BossSpeed, BossOrigin;
		float BossRotation;
		int BossHealth;
		bool BossAlive;
		

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			//Player
			PlayerPos.X = 400; PlayerPos.Y = 400;
			PlayerSpeed.X = 4; PlayerSpeed.Y = 4;
			PlayerImmobilized = false; HasFlute = true; PlaysTheFlute = false;
			PlayerAlive = true;
			FluteCounter = 0; Health = 6;
			//Level one Props
			MountainWall.X = 700;
			MountainWall.Y = 0; MountainRec = new Rectangle(700, 0, 100, 300);
			FencePos.X = 0; FencePos.Y = 0;
			FenceRec = new Rectangle(0, 0, 407, 62);
			TreePos1.X = 0; TreePos1.Y = 122; TreePos2.X = 0; TreePos2.Y = 302; // TreePos3.X = 0; TreePos3.Y = 402;
			TreeRec1 = new Rectangle(0, 122, 157, 170); TreeRec2 = new Rectangle(0, 302, 157, 170); // TreeRec3 = new Rectangle(0, 402, 157, 170);
			StonePos.X = 0; StonePos.Y = 62;
			StoneRec = new Rectangle(0, 62, 70, 70);
			Level = 0;
			//Level two Props
			BossPos.X = 400; BossPos.Y = 150;
			BossSpeed.X = 1; BossSpeed.Y = 1;
			BossRotation = 1f;
			BossHealth = 2;
			BossAlive = true;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Player = Content.Load<Texture2D>("Sprites/Katt");
			MountainGray = new Texture2D(GraphicsDevice, 1, 1);
			MountainGray.SetData(new[] { Color.White });
			Fence = Content.Load<Texture2D>("Sprites/Fence");
			Tree = Content.Load<Texture2D>("Sprites/Tree");
			Boss = Content.Load<Texture2D>("Sprites/Zombie1");
			Flute = Content.Load<SoundEffect>("Sounds/Kazoo-SoundBible.com-161921968");
			Stone = Content.Load<Texture2D>("Sprites/Rock");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
			MountainGray.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			KeyboardState UserKeyboard = Keyboard.GetState();
			if (PlayerAlive)
			{
				if (!PlayerImmobilized)
				{
					if (UserKeyboard.IsKeyDown(Keys.W)) { PlayerPos.Y -= PlayerSpeed.Y; }
					if (UserKeyboard.IsKeyDown(Keys.S)) { PlayerPos.Y += PlayerSpeed.Y; }
					if (UserKeyboard.IsKeyDown(Keys.A)) { PlayerPos.X -= PlayerSpeed.X; }
					if (UserKeyboard.IsKeyDown(Keys.D)) { PlayerPos.X += PlayerSpeed.X; }
				}
				PlayerRectangle = new Rectangle(Convert.ToInt32(PlayerPos.X), Convert.ToInt32(PlayerPos.Y), Player.Width, Player.Height);
				OutOfArea = CheckBounds(PlayerRectangle, Window.ClientBounds);
				if (OutOfArea == true)
				{ PlayerPos.X = 300; PlayerPos.Y = 300; Health--; Level = 1; }
			}
			switch (Level)
			{
				case 0:
					if (UserKeyboard.IsKeyDown(Keys.Enter)) { Level = 1; }
					break;
				case 1:
					//Collision with mountain
					if (PlayerRectangle.Right > MountainRec.Left && PlayerRectangle.Right < MountainRec.Left + 5 && PlayerRectangle.Intersects(MountainRec))
					{
						PlayerPos.X = MountainRec.Left - Player.Width - 1;
						PlayerRectangle = new Rectangle(Convert.ToInt32(PlayerPos.X), Convert.ToInt32(PlayerPos.Y), Player.Width, Player.Height);
					}
					else if (PlayerRectangle.Top < MountainRec.Bottom && PlayerRectangle.Intersects(MountainRec) == true)
					{
						PlayerPos.Y += PlayerSpeed.Y;
						PlayerRectangle = new Rectangle(Convert.ToInt32(PlayerPos.X), Convert.ToInt32(PlayerPos.Y), Player.Width, Player.Height);
						Level = 2;
					}
					if (PlayerRectangle.Intersects(FenceRec))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > FenceRec.Bottom - 4) { PlayerPos.Y += PlayerSpeed.Y; }
						//if (UserKeyboard.IsKeyDown(Keys.S)) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > FenceRec.Right - 4) { PlayerPos.X += PlayerSpeed.X; }
						//if (UserKeyboard.IsKeyDown(Keys.D)) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(TreeRec1))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > TreeRec1.Bottom - 4) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < TreeRec1.Top + 4) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > TreeRec1.Right - 4) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < TreeRec1.Left + 4) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(TreeRec2))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > TreeRec2.Bottom - 4) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < TreeRec2.Top + 4) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > TreeRec2.Right - 4) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < TreeRec2.Left + 4) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(TreeRec3))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > TreeRec3.Bottom - 4) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < TreeRec3.Top + 4) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > TreeRec3.Right - 4) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < TreeRec3.Left + 4) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(StoneRec))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > StoneRec.Bottom - 4) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < StoneRec.Top + 4) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > StoneRec.Right - 4) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < StoneRec.Left + 4) { PlayerPos.X -= PlayerSpeed.X; }
					}
					break;
				case 2:
					if (PlayerAlive && BossAlive)
					{ BossDirection = PlayerPos - BossPos; }
					else if (!PlayerAlive)
					{ BossDirection = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2) - BossPos; }
					BossDirection.Normalize();
					BossRotation = float.Parse(Convert.ToString(Math.Atan2(BossDirection.Y, BossDirection.X)));
					BossPos += BossDirection * BossSpeed;
					BossRec = new Rectangle(Convert.ToInt32(BossPos.X), Convert.ToInt32(BossPos.Y), Boss.Width, Boss.Height);
					BossOrigin = new Vector2(BossRec.Width / 2, BossRec.Height / 2);
					if (FluteCounter == 479)
					{ BossHealth--; }
					if (BossHealth == 0)
					{ BossAlive = false; }
					break;
				case 3:
					break;
				case 4:
					break;
			}
			if (HasFlute && PlayerAlive)
			{
				if (UserKeyboard.IsKeyDown(Keys.F) && !PlaysTheFlute)
				{
					PlaysTheFlute = true;
					PlayerImmobilized = true;
					Flute.Play();
				}
			}
			if (PlaysTheFlute )
			{ FluteCounter++; }
			switch (FluteCounter)
			{
				case 180:
					PlayerImmobilized = false;
					break;
				case 480:
					PlaysTheFlute = false;
					FluteCounter = 0;
					break;
			}
			if (Health <= 0) { PlayerAlive = false; Level = -1; }
			base.Update(gameTime);
		}

		private bool CheckBounds(Rectangle playerRectangle, Rectangle clientBounds)
		{
			if (playerRectangle.Left < 0) { return true; }
			else if (playerRectangle.Right > clientBounds.Width) { return true; }
			else if (playerRectangle.Bottom > clientBounds.Height) { return true; }
			else if (playerRectangle.Top < 0) { return true; }
			else { return false; }
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.ForestGreen);
			spriteBatch.Begin();
			switch (Level)
			{
				case 0:
					break;
				case 1:
					spriteBatch.Draw(MountainGray, MountainRec, Color.SlateGray);
					spriteBatch.Draw(Fence, FencePos, Color.White);
					spriteBatch.Draw(Tree, TreePos1, Color.White);
					spriteBatch.Draw(Tree, TreePos2, Color.White);
					//spriteBatch.Draw(Tree, TreePos3, Color.White);
					spriteBatch.Draw(Stone, StonePos, Color.White);

					break;
				case 2:
					if (BossAlive)
					{ spriteBatch.Draw(Boss, BossPos, null, Color.White, BossRotation, BossOrigin, 0.5f, SpriteEffects.None, 0); }
					break;
				case -1:
					break;
			}
			if (PlayerAlive && Level != 0) { spriteBatch.Draw(Player, PlayerPos, Color.White); }
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
