using Domain.Models.Relational;

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
}
