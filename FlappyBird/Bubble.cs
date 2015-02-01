using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Bubble
	{
		private SpriteUV 	bubble;
		private TextureInfo	textureInfoBubble;
		private float 		width;
		private float 		height;
		private float		radius;
		private Vector2		Pos;
		
		public float GetX { get{return bubble.Position.X;} }
		public float GetY { get{return bubble.Position.Y;} }
	
		public float GetRadius { get{return radius;} }
		
		public Bubble(float x, float y, float size, Scene scene)
		{
			textureInfoBubble  = new TextureInfo("/Application/textures/bubble.png");	
			bubble 			   = new SpriteUV(textureInfoBubble);
			bubble.Quad.S 	   = textureInfoBubble.TextureSizef;	
			
			scene.AddChild(bubble);
			
			//Get sprite bounds.
			bubble.Scale 	= new Vector2(size, size);
			
			Bounds2 b = bubble.Quad.Bounds2();	
			
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			radius = (float)(width * size) / 2;			

			bubble.Position = new Vector2(x, y);

		}
		
		public void Dispose()
		{
			textureInfoBubble.Dispose();
		}
		
		public void SetPosition(float x, float y)
		{
			Pos.X = x;
			Pos.Y = y;
			bubble.Position = new Vector2(Pos.X, Pos.Y);
		}
		
		public void Update(float deltaTime)
		{
			bubble.Position 	= new Vector2(bubble.Position.X - 3.0f, bubble.Position.Y + 5.0f);
			
			if(bubble.Position.X < - Director.Instance.GL.Context.GetViewport().Width)
			{
				bubble.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width, bubble.Position.Y);
			}
			
			if(bubble.Position.Y >  Director.Instance.GL.Context.GetViewport().Height)
			{
				bubble.Position = new Vector2(bubble.Position.X, 0.0f - (RandomFloat() * 15));
			}
		}
		
		private float RandomFloat()
		{
			Random rand = new Random();
			float randomFloat = (float)rand.NextDouble();
			return randomFloat;
		}
	}
}

