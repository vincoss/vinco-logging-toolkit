using System.Text;


namespace Elmah.Everywhere
{
    public abstract class LogDetailBase
    {
        public abstract string Name { get; }
        public abstract void Append(StringBuilder sb);
    }
}