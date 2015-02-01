using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;



namespace FlappyBird
{
	public class AppMain
	{
		private const int OBSTACLE_COUNT = 2;
		
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label				airLabel;
		
		private static Chain[]			chain;
		private static Mine[]			seamine;	
		private static Bird				bird;
		private static Background		background;
		private static List <Bubble>	bubble;	
		private static Timer 			airLoss;
		
		private static float 			X;
		private static float 			Y;
		private static float 			windowWidth;
		private static float 			windowHeight;
		private static int				air;
		private static int				score;
		
				
		public static void Main (string[] args)
		{
			air = 20;
			Initialize();
			score = 0;
			
			airLoss = new Timer();
			airLoss.Reset();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			bird.Dispose();
			foreach(Chain c in chain)
			{
				c.Dispose();
			}
			
			foreach(Bubble b in bubble)
			{
				b.Dispose();
			}
			
			background.Dispose(); 
			
			for(int i = 0; i < OBSTACLE_COUNT; i++)
			{
				seamine[i].Dispose();
				chain[i].Dispose();
			}
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Left;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			
			scoreLabel.TextShadow = new TextShadowSettings();
			scoreLabel.TextShadow.Color = new UIColor(0.0f,0.0f,0.0f,1.0f);
			scoreLabel.TextShadow.HorizontalOffset = 2.0f;
			scoreLabel.TextShadow.VerticalOffset = 2.0f;

			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Width*0.1f - scoreLabel.Height/2);
			
			scoreLabel.Text = "Score: " + score.ToString();
			
			airLabel = new Sce.PlayStation.HighLevel.UI.Label();
			airLabel.HorizontalAlignment = HorizontalAlignment.Right;
			airLabel.VerticalAlignment = VerticalAlignment.Top;
			
			airLabel.TextShadow = new TextShadowSettings();
			airLabel.TextShadow.Color = new UIColor(0.0f,0.0f,0.0f,1.0f);
			airLabel.TextShadow.HorizontalOffset = 2.0f;
			airLabel.TextShadow.VerticalOffset = 2.0f;
			
			airLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - airLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Width*0.1f - airLabel.Height/2);
			airLabel.Text = "Air: " + air.ToString();
			
			windowWidth = Director.Instance.GL.Context.GetViewport().Width;
			windowHeight = Director.Instance.GL.Context.GetViewport().Height;
			
			panel.AddChildLast(scoreLabel);
			panel.AddChildLast(airLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the flappy guy
			bird	 = new Bird(gameScene);
			
			//Create some chains.
			chain    = new Chain[OBSTACLE_COUNT];
			chain[0] = new Chain(windowWidth*0.5f, gameScene);	
			chain[1] = new Chain(windowWidth, gameScene);
			
			//Create seamines and attach to chain
			seamine    = new Mine[OBSTACLE_COUNT];
			seamine[0] = new Mine((X = chain[0].GetX + chain[0].GetMaxX) - 80 , Y = chain[0].GetY + chain[0].GetMaxY, gameScene);	
			seamine[1] = new Mine((X = chain[1].GetX + chain[1].GetMaxX) - 80, Y = chain[1].GetY + chain[1].GetMaxY, gameScene);
			
			//Create Bubbles
			bubble = new List<Bubble>();
			bubble.Add ( new Bubble(windowWidth / 4, -70.0f, 0.75f, gameScene));
			bubble.Add ( new Bubble(windowWidth / 2, -42.0f, 0.25f, gameScene));
			bubble.Add ( new Bubble((windowWidth / 4) * 3, -103.0f, 0.5f, gameScene));
			bubble.Add ( new Bubble(windowWidth, -129.0f, 0.5f, gameScene));
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Collision()
		{
			float distance;
			for(int i = 0; i < OBSTACLE_COUNT; i++)
			{
				//Creates bounding circle and checks if the bird and the seamines have collided.
				distance = ((seamine[i].GetX - bird.GetX) * (seamine[i].GetX - bird.GetX) +
							(seamine[i].GetY - bird.GetY) * (seamine[i].GetY - bird.GetY));
				
				
				if(distance <= (bird.GetRadius + seamine[i].GetRadius) *
				   			   (bird.GetRadius + seamine[i].GetRadius))
				{
					bird.Alive = false;
				}

				//Incriments Score.
				if(chain[i].GetX <  -chain[i].GetMaxX)
				{
					score++;
					scoreLabel.Text = "Score: " + score.ToString();
				}
				
			}
			// The bird perishes should it leave the screen.
			if(bird.GetY < 0 || bird.GetY > windowHeight)
			{
				bird.Alive = false;
			}
			//Creates bounding circle and checks if the bird and the bubbles have collided.
			foreach(Bubble b in bubble)
			{
				
				distance = ((b.GetX - bird.GetX) * (b.GetX - bird.GetX) +
							(b.GetY - bird.GetY) * (b.GetY - bird.GetY));
				
				if(distance <= (bird.GetRadius + b.GetRadius) *
				   			   (bird.GetRadius + b.GetRadius))
				{
					
					b.SetPosition(b.GetX, b.GetY -windowHeight);
					air += 2;
				}
			}

		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen.
			var touches = Touch.GetData(0);
			//If tapped, inform the bird.
			if(touches.Count > 0)
				bird.Tapped();
			
			// timer that decreases air supply of bird by one every 2 seconds.
			if(airLoss.Seconds() > 2)
			{
				air--;
				airLabel.Text = "Air: " + air.ToString();
				airLoss.Reset();
			}
			
			//Update the bird.
			bird.Update(0.0f);
			
			//Update the bubble.
			foreach(Bubble b in bubble)
			{
				b.Update(0.0f);
			}
			
			if(bird.Alive)
			{
				//Move the background.
				background.Update(0.0f);
				
				//Update chains and mines.
				for(int i = 0; i < OBSTACLE_COUNT; i++)
				{
					chain[i].Update(0.0f);
					seamine[i].SetPosition((chain[i].GetX - 60), (chain[i].GetY + chain[i].GetMaxY) - 20);
					seamine[i].Update(0.0f);				
				}
			}
			
			Collision();
		}
	}
}
