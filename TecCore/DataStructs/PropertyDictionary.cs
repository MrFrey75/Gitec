namespace TecCore.DataStructs;

public class PropertyDictionary : Dictionary<string, Type>
{
    public string Name { get; set; } = string.Empty;
    public  void AddProperty(string name, Type type)
    {
        //ensure that the property name is unique
        if (ContainsKey(name))
        {
            Console.WriteLine($"Property {name} already exists.");
            return;
        }
        Add(name, type);
    }
    
    public  void RemoveProperty(string name)
    {
        Remove(name);
    }
    
    public  void UpdateProperty(string name, Type type)
    {
        Remove(name);
        Add(name, type);
    }
    
    public  Type GetPropertyType(string name)
    {
        return this[name];
    }
    
    public  bool ContainsProperty(string name)
    {
        return ContainsKey(name);
    }
    
    public  bool ContainsPropertyType(Type type)
    {
        return ContainsValue(type);
    }
    
    
}