using Server;

namespace Server2
{
    public interface ICategory
    {
        void add(string name);

        void update(int index, string name);

        Category GetCategory(string name);
        
        Category GetCategory(int index);
        
        bool delete(string name);

    }
}