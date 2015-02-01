using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Foreground
	{
		private SpriteTile		seawead;
		private TextureInfo		seaweedInfo;
		private float			width;
		private float			height;
		private int				count;
		
		
		public Foreground (float x, Scene scene)
		{		
			seaweedInfo  	   = new TextureInfo(new Texture2D("/Application/textures/seaweed.png", false),
			                                     new Vector2i(3,1), TRS.Quad0_1);
			seawead 		   = new SpriteTile(seaweedInfo);
			seawead.Quad.S 	   = seaweedInfo.TileSizeInPixelsf;
			
			scene.AddChild(seawead);
			
			seawead.TileIndex2D = new Vector2i(0, 0);
			
			//Get sprite bounds.
			Bounds2 b = seawead.Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			seawead.Position 	= new Vector2(x, -20.0f);
			count = 0;
		}
		
		public void Dispose()
		{
			seaweedInfo.Dispose();
		}
			
		public void Update(float deltaTime)
		{			
			seawead.Position 	 = new Vector2(seawead.Position.X - 16.0f, seawead.Position.Y);
			
			if(seawead.Position.X < - Director.Instance.GL.Context.GetViewport().Width)
			{
				seawead.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width, seawead.Position.Y);
				seawead.TileIndex2D = new Vector2i(count, 0);
				count++;
				if(count == 3)
				{
					count = 0;
				}
			}

		
		}
	}
}

