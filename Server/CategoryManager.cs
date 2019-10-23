using System;
using System.Collections.Generic;
using Server;

namespace Server2
{
    /**
     * The purpose of this class is to provide storage and management of the different categories.
     */
    public static class CategoryManager
    {
        private static int i;
        public static List<Category> categories = new List<Category>();

        static CategoryManager()
        {
            add("Beverages");
            add("Condiments");
            add("Confections");
        }

        public static void add(string name)
        {
            i++;
            categories.Add(new Category {Id = i, Name = name});
        }

        public static Category GetCategory(string name)
        {
            return GetCategory(find(name));
        }
        
        public static Category GetCategory(int index)
        {
            return categories[index];
        }

        public static bool delete(string name)
        {
            int indexOfElement = find(name);
            if (indexOfElement == -1)    //-1 indicates that the element is not in the categories.
            {
                return false;    
            }
            categories.RemoveAt(indexOfElement);
            return true;
        }
        
        private static int find(string name)
        {
            foreach (var category in categories)
            {
                if (category.Name.Equals(name))
                {
                    return category.Id;
                }
            }
// -1 means that the given name is NOT in the category list 
            return -1;
        }

    }
}