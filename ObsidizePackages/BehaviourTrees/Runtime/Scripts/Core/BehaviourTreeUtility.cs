using System.Text.RegularExpressions;

namespace Obsidize.BehaviourTrees
{
    public static class BehaviourTreeUtility
    {

        public static string WithoutNodeSuffix(this string input)
		{
            return Regex.Replace(input, "Node$", "");
		}
    }
}
