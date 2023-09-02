namespace Sample.PluginApp.ConsoleApp
{
    internal class QueueAttribute : Attribute
    {
        public QueueAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}