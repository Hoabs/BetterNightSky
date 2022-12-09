
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework;


namespace BetterNightSky
{
	public enum CelestialObject
	{
		Mars = 8, Saturn, Jupiter, Mercury, CrabNebula, Andromeda, CatsEyeNebula, CarinaNebula, Triangulum,
		LargeMagellanicCloud, SmallMagellanicCloud, HelixNebula, Uranus
	}

	class BetterNightSky : Mod
	{
		public static Dictionary<CelestialObject, int> CelestialIndex = new Dictionary<CelestialObject, int>();
		public static int NewArraySize = 21;
		public static int NewStarCount = 300;

		//we use a new random object because Main.rand isn't initialized yet in Load()
		static Random temp_Rand = new Random();

		public BetterNightSky()
		{
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				int newSize = 21;
				Array.Resize<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>>(ref TextureAssets.Star, newSize);

				for (int i = 0; i < TextureAssets.Star.Length; i++)
				{
					TextureAssets.Star[i] = this.Assets.Request<Texture2D>("Textures/Star_" + i);
				}

				for (int i = 0; i < TextureAssets.Moon.Length; i++)
				{
					TextureAssets.Moon[i] = this.Assets.Request<Texture2D>("Textures/Moon1");
				}

				/*for(int i = 0; i < TextureAssets.Star.Length; i++)
				{
					Main.star[250 - (i)].type = i;
					Main.star[250 - (i)].twinkleSpeed *= 0;
					Main.star[250 - (i)].rotationSpeed *= 0;
					CelestialIndex.Add((CelestialObject)i, 250 - (i));
				}*/

			}
		}

		

		private TaskCompletionSource<bool> unloadTcs;

		public override void Unload()
		{
			CelestialIndex.Clear();
			

			/*if (!Main.dedServ)
			{
				// Unload runs on another thread. To ensure we don't replace textures as they are drawing, we use ModSceneEffect.IsSceneEffectActivec which will run on the next frame to actually swap the textures.
				var tcs = new TaskCompletionSource<bool>();
				unloadTcs = tcs;
				tcs.Task.Wait();
			}*/

			Main.numStars = 130;
			//Array.Resize<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>>(ref TextureAssets.Star, 4);
			//Array.Resize<Star>(ref Main.star, 130);

			/*Main.rand = new Terraria.Utilities.UnifiedRandom();
			Star.SpawnStars();*/
			for (int i = 0; i < 130; i++)
			{
				if (Main.star[i].type > 3)
				{
					Main.star[i].type = Main.rand.Next(4);
				}
			}
			for (int i = 0; i < 5; i++)
			{
				TextureAssets.Star[i] = ModContent.Request<Texture2D>("Terraria/Images/Star_" + i);
			}

			for (int i = 0; i < TextureAssets.Moon.Length; i++)
			{
				TextureAssets.Moon[i] = ModContent.Request<Texture2D>("Terraria/Images/Moon_" + i);
			}
				/*unloadTcs.SetResult(true);
				unloadTcs = null;*/
		}

		//Extracted from normal vanilla code because Main.numStars was hardcoded to 130 in there
		//so I just decided to be lazy and take this method out and set Main.numStars to whatever i want
		
	}

	class BetterNightSkyWorld : ModSystem
	{
		public static Dictionary<CelestialObject, int> CelestialIndex = new Dictionary<CelestialObject, int>();
		public static int NewArraySize = 21;
		public static int NewStarCount = 300;

			//we use a new random object because Main.rand isn't initialized yet in Load()
		static Random temp_Rand = new Random();
		public override void OnWorldUnload()
		{
			CelestialIndex.Clear();

		}
		public override void OnWorldLoad() 
		{
			if (!Main.dedServ)
			{
				CelestialIndex.Clear();
				Array.Resize<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>>(ref TextureAssets.Star, NewArraySize);
				

				/*for (int i = 0; i < TextureAssets.Star.Length; i++)
				{
					TextureAssets.Star[i] = Mod.Assets.Request<Texture2D>("Textures/Star_" + i);
				}

				for (int i = 0; i < TextureAssets.Moon.Length; i++)
				{
					TextureAssets.Moon[i] = Mod.Assets.Request<Texture2D>("Textures/Moon1");
				}*/

				SpawnNewStars();

				for (int i = 0; i < Main.star.Length; i++)
				{
					if (temp_Rand.Next(15) == 0)
					{
						Main.star[i].type = 5;
					}

					if (temp_Rand.Next(9) == 0)
					{
						Main.star[i].type = 6;
					}


					if (temp_Rand.Next(20) == 0)
					{
						Main.star[i].type = 7;
					}
				}

				for(int i = 0; i < TextureAssets.Star.Length; i++)
				{
					Main.star[250 - (i)].type = i;
					Main.star[250 - (i)].twinkleSpeed *= 0;
					Main.star[250 - (i)].rotationSpeed *= 0;
					CelestialIndex.Add((CelestialObject)i, 250 - (i));
				}
			}
			
			
			
			static void SpawnNewStars()
			{
				
				Array.Resize<Star>(ref Main.star, NewStarCount);
				Main.numStars = NewStarCount;
				
				for (int i = 0; i < Main.numStars; i++)
				{
					Main.star[i] = new Star();
					Main.star[i].position.X = (float)temp_Rand.Next(-12, Main.screenWidth + 1);
					Main.star[i].position.Y = (float)temp_Rand.Next(-12, Main.screenHeight);
					Main.star[i].rotation = (float)temp_Rand.Next(628) * 0.01f;
					Main.star[i].scale = (float)temp_Rand.Next(50, 120) * 0.01f;
					Main.star[i].type = temp_Rand.Next(0, 3);
					Main.star[i].twinkle = (float)temp_Rand.Next(101) * 0.01f;
					Main.star[i].twinkleSpeed = (float)temp_Rand.Next(40, 100) * 0.0001f;
					if (temp_Rand.Next(2) == 0)
					{
						Main.star[i].twinkleSpeed *= -1f;
					}
					Main.star[i].rotationSpeed = (float)temp_Rand.Next(10, 40) * 0.0001f;
					if (temp_Rand.Next(2) == 0)
					{
						Main.star[i].rotationSpeed *= -1f;
					}
				}
			}
		}
	}
}
