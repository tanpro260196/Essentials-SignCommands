﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TShockAPI;

namespace Essentials
{
	public static class esUtils
	{
		/* Search IDs */
    public static Dictionary<string, int> ItemIdSearch(string search)  
		{
			try
			{
        var found = new Dictionary<string, int>();
				for (int i = -48; i < Main.maxItemTypes; i++)
				{
					Item item = new Item();
					item.netDefaults(i);
					if (item.Name.ToLower().Contains(search.ToLower()))
						found.Add(item.Name, item.netID);
				}
				return found;
			}
      catch { return new Dictionary<string, int>(); }
		}
    public static Dictionary<string, int> NPCIdSearch(string search)
		{
			try
			{
        var found = new Dictionary<string, int>();
				for (int i = -65; i < Main.maxNPCTypes; i++)
				{
					NPC npc = new NPC();
					npc.SetDefaults(i);
					if (npc.FullName.ToLower().Contains(search.ToLower()))
						found.Add(npc.FullName, npc.netID);
				}
				return found;
			}
      catch { return new Dictionary<string, int>(); }
		}
    public static Dictionary<string, int> TileIdSearch(string search) {
      Dictionary<string, int> results = new Dictionary<string, int>();

      foreach (var fi in typeof(Terraria.ID.TileID).GetFields()) {
        var name = fi.Name;
        var sb = new StringBuilder();
        for (int i = 0; i < name.Length; i++) {
          if (Char.IsUpper(name[i]))
            sb.Append(" ").Append(Char.ToLower(name[i]));
          else
            sb.Append(name[i]);
        }

        if (sb.ToString(1, sb.Length - 1).ToLower().Contains(search.ToLower()))
          results.Add(sb.ToString(1, sb.Length - 1), (ushort)fi.GetValue(null));
      }

      return results;
    }
    public static Dictionary<string, int> WallIdSearch(string search) {
      Dictionary<string, int> results = new Dictionary<string, int>();

      foreach (var fi in typeof(Terraria.ID.WallID).GetFields()) {
        var name = fi.Name;
        var sb = new StringBuilder();
        for (int i = 0; i < name.Length; i++) {
          if (Char.IsUpper(name[i]))
            sb.Append(" ").Append(Char.ToLower(name[i]));
          else
            sb.Append(name[i]);
        }

        if (sb.ToString(1, sb.Length - 1).ToLower().Contains(search.ToLower()))
          results.Add(sb.ToString(1, sb.Length - 1), (ushort)fi.GetValue(null));
      }

      return results;
    }

    public static void DisplaySearchResults(TSPlayer Player, string type, Dictionary<string, int> Results, int Page) {
      Player.SendInfoMessage(type + " Search:");
      var sb = new StringBuilder();
      if (Results.Count > (8 * (Page - 1))) {
        for (int j = (8 * (Page - 1)); j < (8 * Page); j++) {
          if (sb.Length != 0)
            sb.Append(" | ");
          sb.Append(Results.ElementAt(j).Key).Append(": ").Append(Results.ElementAt(j).Value);
          if (j == Results.Count - 1) {
            Player.SendSuccessMessage(sb.ToString());
            break;
          }
          if ((j + 1) % 2 == 0) {
            Player.SendSuccessMessage(sb.ToString());
            sb.Clear();
          }
        }
      }
      if (Results.Count > (8 * Page)) {
        Player.SendInfoMessage("Type /spage {0} for more Results.", Page + 1);
      }
    }

    ///* Sethome - how many homes can a user set. */
    //public static int NoOfHomesCanSet(TSPlayer ply)
    //{
    //  if (ply.Group.HasPermission("essentials.home.set.*"))
    //    return -1;

    //  List<int> maxHomes = new List<int>();
    //  foreach (string p in ply.Group.TotalPermissions)
    //  {
    //    if (p.StartsWith("essentials.home.set.") && p != "essentials.home.set." && !ply.Group.negatedpermissions.Contains(p))
    //    {
    //      int m = 0;
    //      if (int.TryParse(p.Remove(0, 20), out m))
    //        maxHomes.Add(m);
    //    }
    //  }

    //  if (maxHomes.Count < 1)
    //    return 1;
    //  else
    //    return maxHomes.Max();
    //}

    ///* Sethome - get next home */
    //public static string NextHome(List<string> homes)
    //{
    //  List<int> intHomes = new List<int>();
    //  foreach (string h in homes)
    //  {
    //    int m = 0;
    //    if (int.TryParse(h, out m))
    //      intHomes.Add(m);
    //  }
    //  if (intHomes.Count < 1)
    //    return "1";
    //  else
    //    return (intHomes.Max() + 1).ToString();
    //}

		/* Top, Up and Down Methods */
		public static int GetTop(int TileX)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				if (WorldGen.SolidTile(TileX, y))
					return y - 3;
			}
			return -1;
		}
		public static int GetUp(int TileX, int TileY)
		{
			for (int y = TileY - 1; y > 0; y--)
			{
				if (WorldGen.SolidTile(TileX, y + 1) && !WorldGen.SolidTile(TileX, y) && !WorldGen.SolidTile(TileX, y - 1) && !WorldGen.SolidTile(TileX, y - 2))
					return y - 2;
			}
			return -1;
		}
		public static int GetDown(int TileX, int TileY)
		{
			for (int y = TileY + 3; y < Main.maxTilesY; y++)
			{
				if (WorldGen.SolidTile(TileX, y + 1) && !WorldGen.SolidTile(TileX, y) && !WorldGen.SolidTile(TileX, y - 1) && !WorldGen.SolidTile(TileX, y - 2))
					return y - 2;
			}
			return -1;
		}

		/* Left and Right */
		public static int GetLeft(int TileX, int TileY)
		{
			bool FoundSolid = false;
			for (int x = TileX; x > 0; x--)
			{
				if (!FoundSolid)
				{
					if (!WorldGen.SolidTile(x, TileY))
						continue;
					else
						FoundSolid = true;
				}
				else if (!WorldGen.SolidTile(x - 1, TileY) &&
					!WorldGen.SolidTile(x, TileY + 1) && !WorldGen.SolidTile(x - 1, TileY + 1) &&
					!WorldGen.SolidTile(x, TileY + 2) && !WorldGen.SolidTile(x - 1, TileY + 2))
					return x - 1;
				else
					FoundSolid = true;
			}
			return -1;
		}
		public static int GetRight(int TileX, int TileY)
		{
			bool FoundSolid = false;
			for (int x = TileX + 1; x < Main.maxTilesX; x++)
			{
				if (!FoundSolid)
				{
					if (!WorldGen.SolidTile(x, TileY))
						continue;
					else
						FoundSolid = true;
				}
				else if (!WorldGen.SolidTile(x + 1, TileY) &&
					!WorldGen.SolidTile(x, TileY + 1) && !WorldGen.SolidTile(x + 1, TileY + 1) &&
					!WorldGen.SolidTile(x, TileY + 2) && !WorldGen.SolidTile(x + 1, TileY + 2))
					return x + 1;
			}
			return -1;
		}

		#region ParseParameters
		public static List<String> ParseParameters(string str)
		{
			var ret = new List<string>();
			var sb = new StringBuilder();
			bool instr = false;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (instr)
				{
					if (c == '\\')
					{
						if (i + 1 >= str.Length)
							break;
						c = GetEscape(str[++i]);
					}
					else if (c == '"')
					{
						ret.Add(sb.ToString());
						sb.Clear();
						instr = false;
						continue;
					}
					sb.Append(c);
				}
				else
				{
					if (IsWhiteSpace(c))
					{
						if (sb.Length > 0)
						{
							ret.Add(sb.ToString());
							sb.Clear();
						}
					}
					else if (c == '"')
					{
						if (sb.Length > 0)
						{
							ret.Add(sb.ToString());
							sb.Clear();
						}
						instr = true;
					}
					else
					{
						sb.Append(c);
					}
				}
			}
			if (sb.Length > 0)
				ret.Add(sb.ToString());

			return ret;
		}
		private static char GetEscape(char c)
		{
			switch (c)
			{
				case '\\':
					return '\\';
				case '"':
					return '"';
				case 't':
					return '\t';
				default:
					return c;
			}
		}
		private static bool IsWhiteSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\n';
		}
		#endregion
	}
}
