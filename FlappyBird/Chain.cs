using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Chain
	{
		const float kGap = 100.0f;
		
		//Private variables.
		private SpriteUV 	chain;
		private TextureInfo	textureInfoChain;
		private float		width;
		private float		maxY;
		private float		maxX;
		
		
		//Accessors.
		public float GetMaxY { get{return maxY;} }
		public float GetMaxX { get{return maxX;} }
		
		public float GetX { get{return chain.Position.X;} }
		public float GetY { get{return chain.Position.Y;} }
		
		//Public functions.
		public Chain (float startX, Scene scene)
		{
			textureInfoChain  = new TextureInfo("/Application/textures/chain.png");

			chain = new SpriteUV();
			
			//Bottom
			chain			= new SpriteUV(textureInfoChain);	
			chain.Quad.S 	= textureInfoChain.TextureSizef;		
			//Add to the current scene.
			scene.AddChild(chain);
			
			//Get sprite bounds.
			Bounds2 b = chain.Quad.Bounds2();
			width  = b.Point11.X;
			maxY  = b.Point01.Y;
			maxX  = (b.Point01.X + b.Point11.X) / 2;
			
			//Position chains.
			chain.Position = new Vector2(startX, RandomPosition() * -1);
			
		}
		
		public void Dispose()
		{
			textureInfoChain.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			chain.Position = new Vector2(chain.Position.X - 3, chain.Position.Y);
			
			//If off the left of the viewport, loop them around.
			if(chain.Position.X < -width)
			{
				chain.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              	 RandomPosition() * -1);
			}		
		}
		
		private int RandomPosition()
		{
			Random rand = new Random();
			int randomPosition = rand.Next(155, 255);
		
			return randomPosition;
		}
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
			return false;
		}
	}
}

