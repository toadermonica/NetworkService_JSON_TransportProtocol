
namespace Server2
{
    public interface ICategory
    {
        void add(string name);

        void update(int index, string name);

        int GetCategoryId(string name);
        
        int GetCategoryId(int index);
        
        bool delete(string name);

    }
}