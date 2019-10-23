using System;
using System.Collections.Generic;
using Server;

namespace Server2
{
    /**
     * The purpose of this class is to provide storage and management of the different categories.
     */
    public class CategoryManager : ICategory
    {
        private int i;
        public List<Category> categories = new List<Category>();

        public CategoryManager()
        {
            add("Beverages");
            add("Condiments");
            add("Confections");
        }

        public void add(string name)
        {
            i++;
            categories.Add(new Category {Id = i, Name = name});
        }

        public Category GetCategory(string name)
        {
            return GetCategory(find(name));
        }
        
        public Category GetCategory(int index)
        {
            return categories[index];
        }

        public bool delete(string name)
        {
            int indexOfElement = find(name);
            if (indexOfElement == -1)    //-1 indicates that the element is not in the categories.
            {
                return false;    
            }
            categories.RemoveAt(indexOfElement);
            return true;
        }
        
        private int find(string name)
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