using System;
using System.IO;
using System.Collections.Generic;
using Pipliz;
using Pipliz.Chatting;
using Pipliz.JSON;
using Pipliz.Threading;
using Pipliz.APIProvider.Recipes;
using Pipliz.APIProvider.Jobs;
using NPC;

namespace ScarabolMods
{
  [ModLoader.ModManager]
  public static class GemCuttersModEntries
  {
    private static string JOB_ITEM_KEY = "mods.scarabol.notenoughblocks.ColonyEmpire.gemcutter";
    private static string AssetsDirectory;

    [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "scarabol.gemcutters.assemblyload")]
    public static void OnAssemblyLoaded(string path)
    {
      AssetsDirectory = Path.Combine(Path.GetDirectoryName(path), "assets");
      ModLocalizationHelper.localize(Path.Combine(AssetsDirectory, "localization"), "", false);
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterStartup, "scarabol.gemcutters.registercallbacks")]
    public static void AfterStartup()
    {
      Pipliz.Log.Write("Loaded Gem Cutters Mod 1.0 by Scarabol");
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterDefiningNPCTypes, "scarabol.gemcutters.registerjobs")]
    [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
    public static void AfterDefiningNPCTypes()
    {
      BlockJobManagerTracker.Register<GemCutterJob>(JOB_ITEM_KEY);
    }

    [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, "scarabol.gemcutters.loadrecipes")]
    [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.registerrecipes")]
    public static void AfterItemTypesDefined()
    {
      RecipeManager.LoadRecipes("scarabol.gemcutter", Path.Combine(AssetsDirectory, "craftinggems.json"));
    }
  }

  public class GemCutterJob : CraftingJobBase, IBlockJobBase, INPCTypeDefiner
  {
    public override string NPCTypeKey { get { return "scarabol.gemcutter"; } }

    public override float TimeBetweenJobs { get { return 5f; } }

    public override int MaxRecipeCraftsPerHaul { get { return 7; } }

    public override List<string> GetCraftingLimitsTriggers ()
    {
      return new List<string>()
      {
        "mods.scarabol.notenoughblocks.ColonyEmpire.gemcutterx+",
        "mods.scarabol.notenoughblocks.ColonyEmpire.gemcutterx-",
        "mods.scarabol.notenoughblocks.ColonyEmpire.gemcutterz+",
        "mods.scarabol.notenoughblocks.ColonyEmpire.gemcutterz-"
      };
    }

    // TOOD add job tool?
//    public override InventoryItem RecruitementItem { get { return new InventoryItem(ItemTypes.IndexLookup.GetIndex("mods.scarabol.construction.buildtool"), 1); } }

    NPCTypeSettings INPCTypeDefiner.GetNPCTypeDefinition ()
    {
      NPCTypeSettings def = NPCTypeSettings.Default;
      def.keyName = NPCTypeKey;
      def.printName = "GemCutter";
      def.maskColor1 = new UnityEngine.Color32(30, 120, 30, 255);
      def.type = NPCTypeID.GetNextID();
      return def;
    }
  }

}
