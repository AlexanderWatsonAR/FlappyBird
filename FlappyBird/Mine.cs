using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Mine
	{
		//Private variables.
		private SpriteUV 	mine;
		private TextureInfo	textureInfoMine;
		private float		width;
		private float		height;
		private Vector2		Pos;
		
		//Accessors.
		public float GetWidth { get{return width;} }
		public float GetRadius { get{return width / 2;} }
		public float GetX { get{return mine.Position.X;} }
		public float GetY { get{return mine.Position.Y;} }

		public Mine (float startX, float startY, Scene scene)
		{
			textureInfoMine  = new TextureInfo("/Application/textures/seamine.png");
			mine = new SpriteUV(textureInfoMine);
			mine.Quad.S = textureInfoMine.TextureSizef;
			
			scene.AddChild(mine);
			
			//Get sprite bounds.
			Bounds2 b = mine.Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			mine.Position = new Vector2(startX, startY);
		}
		
		public void SetPosition(float x, float y)
		{
			Pos.X = x;
			Pos.Y = y;
		}
		
		public void Dispose()
		{
			textureInfoMine.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			mine.Position = new Vector2(Pos.X, Pos.Y);
		}
	}
}

