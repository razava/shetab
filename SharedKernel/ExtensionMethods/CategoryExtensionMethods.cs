using Domain.Models.Relational;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.ExtensionMethods;

public static class CategoryExtensionMethods
{
    public static Category Structure(
        this List<Category> categories,
        StructureType type = StructureType.BiDirectional)
    {
        foreach (var category in categories)
        {
            var children = categories.Where(c => c.ParentId == category.Id).ToList();
            if(type == StructureType.BiDirectional || type == StructureType.TopDown)
            {
                category.Categories = children;
            }
            if(type == StructureType.BiDirectional || type == StructureType.BottomUp)
            {
                children.ForEach(c => c.Parent = category);
            }
        }
        var root = categories.Where(c => c.ParentId == null).FirstOrDefault();
        if (root is null)
            throw new Exception("No root found.");

        return root;
    }
    public enum StructureType
    {
        BiDirectional,
        TopDown,
        BottomUp
    }


    public static Category Compress(
        this Category root)
    {
        var queue = new Queue<Category>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var category = queue.Dequeue();

            if(category.Categories.Count == 1)
            {
                var child = category.Categories.First();
                category.Categories = child.Categories;
                child.Categories.ToList().ForEach(c => { c.Parent = category; c.ParentId = category.Id; });
                child.Categories.ToList().ForEach(c => queue.Enqueue(c));
            }
            else
            {
                category.Categories.ToList().ForEach(c => queue.Enqueue(c));
            }
        }

        return root;
    }
}
