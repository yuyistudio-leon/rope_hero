using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LyLib
{
    public class Recipe
    {
        public bool available = true;
        public string information = "NO INFO";
        public string resultant = "";
        public int resultant_count = 1;
        public Dictionary<string, int> materials;
        public Recipe(string resultant, int resultant_count, Dictionary<string, int> materials, string information)
        {
            this.resultant_count = resultant_count;
            this.resultant = resultant;
            this.materials = materials;
            this.information = information;
        }
    }
    public class CraftClass
    {
        public CraftClass(List<Recipe> recipes, string info)
        {
            this.recipes = recipes;
            this.information = info;
        }
        public List<Recipe> recipes;
        public string information = "";
    }
    public class CraftManager
    {
        public static CraftManager instance;
        Dictionary<string, CraftClass> craft_class = new Dictionary<string, CraftClass>();
        Dictionary<string, Recipe> craft_items = new Dictionary<string, Recipe>();
        public CraftManager()
        {
            instance = this;
        }
        public void AddCraftClass(string class_name, string info, params string[] lines)
        {
            for (int i_line = 0; i_line + 1 < lines.Length; i_line += 2)
            {
                var line = lines[i_line];
                var f = line.Split(' ');
                if (f.Length < 4)
                {
                    Debug.LogError("error recipe: " + line);
                    return;
                }
                var item_name = f[0];
                var item_count = int.Parse(f[1]);
                Dictionary<string, int> materials = new Dictionary<string, int>();
                int i_material = 2;
                for (; i_material + 1 < f.Length; i_material += 2)
                {
                    materials[f[i_material]] = int.Parse(f[i_material + 1]);
                }
                if (this.craft_items.ContainsKey(item_name))
                {
                    Debug.LogError("duplicated item_name: " + item_name);
                    return;
                }
                var recipe = new Recipe(item_name, item_count, materials, lines[i_line + 1]);
                craft_items[item_name] = recipe;
                this.Add(class_name, info, recipe);
            }
        }
        public void Add(string cls, string information, Recipe recipe)
        {
            if (!craft_class.ContainsKey(cls))
            {
                craft_class[cls] = new CraftClass(new List<Recipe>(), information);
            }
            craft_class[cls].recipes.Add(recipe);
        }
        public Recipe GetRecipe(string name)
        {
            if (craft_items.ContainsKey(name) == false)
            {
                foreach (var key in craft_items.Keys)
                {
                    Debug.Log(" => " + key);
                }
                return null;
            }
            return craft_items[name];
        }
        public List<Recipe> GetClass(string cls)
        {
            if (craft_class.ContainsKey(cls))
            {
                return craft_class[cls].recipes;
            }
            else
            {
                return null;
            }
        }
    }
}