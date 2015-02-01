using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Bird
	{
		//Private variables.
		private static SpriteUV 	bird;
		private static TextureInfo	textureInfo;
		private static int			pushAmount = 100;    
		private static float		yPositionBeforePush;
		private static bool			rise;
		private static float		angle;
		private static bool			alive;
		private static float		radius;
		private static float		fallSpeed;
		private const float			ACCELERATION = 0.1f;
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		public float GetX { get{return bird.Position.X;} }
		public float GetY { get{return bird.Position.Y;} }
		public float GetRadius { get{return radius;} }
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Bird (Scene scene)
		{
			fallSpeed = 3.0f;
			textureInfo  	= new TextureInfo("/Application/textures/bird.png");
			bird 			= new SpriteUV(textureInfo);	
			bird.Quad.S 	= textureInfo.TextureSizef;
			bird.Position 	= new Vector2(50.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			bird.Pivot 		= new Vector2(0.0f,0.0f);
			
			angle = 0.05f;
			rise  = false;
			alive = true;
			Bounds2 b = bird.Quad.Bounds2();
			radius = b.Point10.X / 2;
			
			//Add to the current scene.
			scene.AddChild(bird);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			//adjust the push
			if(rise)
			{
				if( (bird.Position.Y-yPositionBeforePush) < pushAmount)
				{
					bird.Position = new Vector2(bird.Position.X, bird.Position.Y + 3);
					fallSpeed = 3.0f;
				}
				else
				{
					rise = false;
				}
			}
			else
			{	fallSpeed = fallSpeed + ACCELERATION;
				bird.Position = new Vector2(bird.Position.X, bird.Position.Y - fallSpeed);
			}
		}	
		
		public void Tapped()
		{
			if(!rise)
			{
				rise = true;
				yPositionBeforePush = bird.Position.Y;
			}
		}
	}
}

