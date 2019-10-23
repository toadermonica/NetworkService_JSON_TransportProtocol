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

        public void update(int index, string name)
        {
            categories[index-1].Name = name;
        }
        
        public int GetCategoryId(string name)
        {
            Console.WriteLine("getcat");
            // return GetCategory(find(name));
            return find(name);

        }

        public List<Category> GetCategories()
        {
            Console.WriteLine("getcats");
            // return GetCategory(find(name));
            return categories;

        }

        public Category GetCategory(int index)
        {
            Console.WriteLine("getcats");
            // return GetCategory(find(name));
            return categories[index-1];

        }

        public int GetCategoryId(int index)
        {
            return findid(index);
        }

        private int findid(int index)
        {
            foreach (var category in categories)
            {
                Console.WriteLine("getfind1");
                if (category.Id.Equals(index))
                {
                    Console.WriteLine("getfind2");
                    return category.Id;
                }
            }
            // -1 means that the given name is NOT in the category list 
            return -1;
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

        public bool delete(int id)
        {
            i--;
            categories.RemoveAt(id-1);
            return true;
        }

        private int find(string name)
        {
            foreach (var category in categories)
            {
                Console.WriteLine("getfind1");
                if (category.Name.Equals(name))
                {
                    Console.WriteLine("getfind2");
                    return category.Id;
                }
            }
// -1 means that the given name is NOT in the category list 
            return -1;
        }

    }
}