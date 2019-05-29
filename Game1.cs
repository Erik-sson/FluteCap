using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluteCap
{

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
		Texture2D Heart;
		Vector2 Heart1, Heart2, Heart3;
		//PlayerBools
		bool PlayerImmobilized, HasFlute, PlaysTheFlute;
		bool PlayerAlive;
		//Universal
		int Level;
		bool OutOfArea;
		SpriteFont Font;
		//Level one props
		Vector2 MountainWall, StonePos;
		Rectangle MountainRec, StoneRec;
		Texture2D MountainGray, Stone;
		Vector2 FencePos;
		Rectangle FenceRec;
		Texture2D Fence;
		Vector2 TreePos1, TreePos2;
		Rectangle TreeRec1, TreeRec2;
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

		protected override void Initialize()
		{
			//Player
			PlayerPos.X = 400; PlayerPos.Y = 400;
			PlayerSpeed.X = 4; PlayerSpeed.Y = 4;
			PlayerImmobilized = false; HasFlute = false; PlaysTheFlute = false;
			PlayerAlive = true;
			FluteCounter = 0; Health = 3;
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
			//Flute = Content.Load<SoundEffect>("Sounds/Flute_tone");				//Changing to this will not be the accurate music for the game
																				//but it might be a nicer sound
			Stone = Content.Load<Texture2D>("Sprites/Rock");
			Heart = Content.Load<Texture2D>("Sprites/Heart");
			Font = Content.Load<SpriteFont>("Font/Font");
		}


		protected override void UnloadContent()
		{
			MountainGray.Dispose();
		}


		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			KeyboardState UserKeyboard = Keyboard.GetState();
			//Player controlls and check if the player tries to escape.
			if (PlayerAlive && Level != 0)
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
				{ PlayerPos.X = 300; PlayerPos.Y = 300; Health--; Level = 1;
				PlayerRectangle = new Rectangle(Convert.ToInt32(PlayerPos.X), Convert.ToInt32(PlayerPos.Y), Player.Width, Player.Height);
				}
			}
			Heart1.X = PlayerPos.X; Heart1.Y = PlayerPos.Y - 20;
			Heart2.X = PlayerPos.X + 20 ; Heart2.Y = PlayerPos.Y - 20;
			Heart3.X = PlayerPos.X + 40 ; Heart3.Y = PlayerPos.Y - 20;
			switch (Level)
			{
				case 0:
					if (UserKeyboard.IsKeyDown(Keys.Enter)) { Level = 1; }
					break;
					//Level 1 with multiple props but no other entities(creatures) and most of the code is so the player wont walk through what should be solid
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
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > FenceRec.Bottom - 5) { PlayerPos.Y += PlayerSpeed.Y; }
						//if (UserKeyboard.IsKeyDown(Keys.S)) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > FenceRec.Right - 5) { PlayerPos.X += PlayerSpeed.X; }
						//if (UserKeyboard.IsKeyDown(Keys.D)) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(TreeRec1))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > TreeRec1.Bottom - 5) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < TreeRec1.Top + 5) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > TreeRec1.Right - 5) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < TreeRec1.Left + 5) { PlayerPos.X -= PlayerSpeed.X; }
					}
					if (PlayerRectangle.Intersects(TreeRec2))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > TreeRec2.Bottom - 5) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < TreeRec2.Top + 5) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > TreeRec2.Right - 5) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < TreeRec2.Left + 5) { PlayerPos.X -= PlayerSpeed.X; }
					}
					//The stone has some hidden features ;]
					if (PlayerRectangle.Intersects(StoneRec))
					{
						if (UserKeyboard.IsKeyDown(Keys.W) && PlayerPos.Y > StoneRec.Bottom - 5) { PlayerPos.Y += PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.S) && PlayerRectangle.Bottom < StoneRec.Top + 5) { PlayerPos.Y -= PlayerSpeed.Y; }
						if (UserKeyboard.IsKeyDown(Keys.A) && PlayerPos.X > StoneRec.Right - 5) { PlayerPos.X += PlayerSpeed.X; }
						if (UserKeyboard.IsKeyDown(Keys.D) && PlayerRectangle.Right < StoneRec.Left + 5) { PlayerPos.X -= PlayerSpeed.X; }
						if (!HasFlute) { HasFlute = true; }
						if (PlaysTheFlute) { Health = 3; }
						if (!BossAlive) { Level = 3; }
					}
					break;
					//Level 2 one boss, no other props. All this is handling boss movement and death for player & boss.
				case 2:
					if (PlayerAlive && BossAlive)
					{
						BossDirection = PlayerPos - BossPos;
						if (PlayerRectangle.Intersects(BossRec))
						{
							Level = 1;
							Health--;
							PlayerPos.X = 300; PlayerPos.Y = 300;
							BossPos.X = 400; BossPos.Y = 150;
						}
					}
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
					//Level 3, You have the monster as your own follower who will try and not get to close
				case 3:
					BossDirection = PlayerPos - BossPos;
					BossDirection.Normalize();
					BossRotation = float.Parse(Convert.ToString(Math.Atan2(BossDirection.Y, BossDirection.X)));
					if (!PlayerRectangle.Intersects(BossRec))
					{ BossPos += BossDirection * BossSpeed; }
					BossRec = new Rectangle(Convert.ToInt32(BossPos.X)-40, Convert.ToInt32(BossPos.Y)-40, Boss.Width+80, Boss.Height+80);
					BossOrigin = new Vector2(BossRec.Width / 2, BossRec.Height / 2);
			break;
				case 4:
					break;
			}
			//Flute mechanics for standing still while playing and charming of creatures.
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
			//Death of player
			if (Health <= 0) { PlayerAlive = false; Level = -1; }
			base.Update(gameTime);
		}

		private bool CheckBounds(Rectangle playerRectangle, Rectangle clientBounds)
		{
			if (playerRectangle.Left < 10) { return true; }
			else if (playerRectangle.Right > clientBounds.Width - 10) { return true; }
			else if (playerRectangle.Bottom > clientBounds.Height - 10) { return true; }
			else if (playerRectangle.Top < 10) { return true; }
			else { return false; }
		}

	
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.ForestGreen);
			spriteBatch.Begin();
			//A switch case determining which level to draw
			switch (Level)
			{
				case 0:
					spriteBatch.DrawString(Font, "Press Enter to start Game", PlayerPos, Color.Black);
					break;
				case 1:
					spriteBatch.Draw(MountainGray, MountainRec, Color.SlateGray);
					spriteBatch.Draw(Fence, FencePos, Color.White);
					spriteBatch.Draw(Tree, TreePos1, Color.White);
					spriteBatch.Draw(Tree, TreePos2, Color.White);
					spriteBatch.Draw(Stone, StonePos, Color.White);

					break;
				case 2:
					if (BossAlive)
					{ spriteBatch.Draw(Boss, BossPos, null, Color.White, BossRotation, BossOrigin, 1f, SpriteEffects.None, 0); }
					else
					{
						spriteBatch.DrawString(Font, "You have put the monster to sleep", new Vector2(400, 400), Color.Black);
					}
					break;
				case 3:
					spriteBatch.Draw(Boss, BossPos, null, Color.White, BossRotation, BossOrigin, 1f, SpriteEffects.None, 0);
					break;
				case -1:
					spriteBatch.DrawString(Font, "You Died", new Vector2(400, 400), Color.Black);
					break;
			}
			//Draw that are not dependant to a specific level
			if (PlayerAlive && Level != 0)
			{
				spriteBatch.Draw(Player, PlayerPos, Color.White);
				switch (Health)
				{
					case (3):
						spriteBatch.Draw(Heart, Heart3, Color.White);
						spriteBatch.Draw(Heart, Heart2, Color.White);
						spriteBatch.Draw(Heart, Heart1, Color.White);
						break;
					case (2):
						spriteBatch.Draw(Heart, Heart2, Color.White);
						spriteBatch.Draw(Heart, Heart1, Color.White);
						break;
					case (1):
						spriteBatch.Draw(Heart, Heart1, Color.White);
						break;
					default: //Blinking hearts if you get more than 3 health
						if (Convert.ToInt32(gameTime.TotalGameTime.Seconds) % 2 == 0)
						{
							spriteBatch.Draw(Heart, Heart3, Color.White);
							spriteBatch.Draw(Heart, Heart2, Color.White);
							spriteBatch.Draw(Heart, Heart1, Color.White);
						}
						break;
				}
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
