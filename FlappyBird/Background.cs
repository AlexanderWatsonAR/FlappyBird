using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Background
	{	
		//Private variables.
		private SpriteUV[] 		sea;
		private SpriteUV[] 		sand;
		private TextureInfo		seaInfo;
		private TextureInfo[]	sandInfo;	
		
		private float			sandWidth;
		private float			seaWidth;
		
		//Public functions.
		public Background (Scene scene)
		{
			sea				= new SpriteUV[3];
			sand			= new SpriteUV[2];
			
			sandInfo		= new TextureInfo[2];
			seaInfo  		= new TextureInfo("/Application/textures/underwater.png");
			sandInfo[0]		= new TextureInfo("/Application/textures/sand.png");
			sandInfo[1]		= new TextureInfo("/Application/textures/sand2.png");
			
			//Left
			sea[0] 			= new SpriteUV(seaInfo);
			sea[0].Quad.S 	= seaInfo.TextureSizef;
			//Middle
			sea[1] 			= new SpriteUV(seaInfo);
			sea[1].Quad.S 	= seaInfo.TextureSizef;
			//Right
			sea[2] 			= new SpriteUV(seaInfo);
			sea[2].Quad.S 	= seaInfo.TextureSizef;
			
			//Left
			sand[0] 		= new SpriteUV(sandInfo[0]);
			sand[0].Quad.S 	= sandInfo[0].TextureSizef;
			//Right
			sand[1] 		= new SpriteUV(sandInfo[1]);
			sand[1].Quad.S 	= sandInfo[1].TextureSizef;
			
			//Colour Blending
			//sprites.BlendMode.BlendFunc.Set(BlendFuncMode.Add, BlendFuncFactor.SrcColor, BlendFuncFactor.DstColor);
			
			//Get sprite bounds.
			Bounds2 b = sand[0].Quad.Bounds2();
			sandWidth     = b.Point10.X;
			
			Bounds2 bSea 			= sea[0].Quad.Bounds2();
			seaWidth     		= bSea.Point10.X;
			
			//Position background.
			sea[0].Position  = new Vector2(0.0f, 0.0f);
			
			sea[1].Position  = new Vector2(sea[0].Position.X+seaWidth, 0.0f);
			
			sea[2].Position  = new Vector2(sea[1].Position.X+seaWidth, 0.0f);
			
			sand[0].Position = new Vector2(0.0f, 0.0f);
			
			sand[1].Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width, 0.0f);
			
			//Add to the current scene.
			foreach(SpriteUV s in sea)
			{
				scene.AddChild(s);
			}
			foreach(SpriteUV sa in sand)
			{
				scene.AddChild(sa);
			}
		}	
		
		public void Dispose()
		{
			seaInfo.Dispose();
			sandInfo[0].Dispose();
			sandInfo[1].Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			sand[0].Position = new Vector2(sand[0].Position.X - 2.4f, sand[0].Position.Y);
			sand[1].Position = new Vector2(sand[1].Position.X - 2.4f, sand[1].Position.Y);
			
			//Move the background.
			//Left
			if(sand[0].Position.X < -sandWidth)
			{
				sand[0].Position = new Vector2(sand[0].Position.X + (sandWidth * 2), 0.0f);
			}
			//Right
			if(sand[1].Position.X < -sandWidth)
			{
				sand[1].Position = new Vector2(sand[1].Position.X + (sandWidth * 2), 0.0f);
			}
			
			sea[0].Position = new Vector2(sea[0].Position.X - 0.3f, sea[0].Position.Y);
			sea[1].Position = new Vector2(sea[1].Position.X - 0.3f, sea[1].Position.Y);
			sea[2].Position = new Vector2(sea[2].Position.X - 0.3f, sea[2].Position.Y);
			
			//Move the background.
			//Left
			if(sea[0].Position.X < -seaWidth)
				sea[0].Position = new Vector2(sea[2].Position.X+seaWidth, 0.0f);
			else
				sea[0].Position = new Vector2(sea[0].Position.X-1, 0.0f);	
			
			//Middle
			if(sea[1].Position.X < -seaWidth)
				sea[1].Position = new Vector2(sea[0].Position.X+seaWidth, 0.0f);
			else
				sea[1].Position = new Vector2(sea[1].Position.X-1, 0.0f);	
			
			//Right
			if(sea[2].Position.X < -seaWidth)
				sea[2].Position = new Vector2(sea[1].Position.X+seaWidth, 0.0f);
			else
				sea[2].Position = new Vector2(sea[2].Position.X-1, 0.0f);
			
		}
	}
}

