using SAT.Req;

namespace SAT.Req
{
    public class Requirement 
    {
        public sealed class None : Requirement;
        public sealed class NotPossible : Requirement;

        public sealed class Complex : Requirement
        {
            public RequirementMap map;
        };

        public sealed class Linear : Requirement
        {
            public Dictionary<HashSet<char>, HashSet<uint>> map;

            public Linear Invert()
            {
                Dictionary<HashSet<char>, HashSet<uint>> invertedMap = new();

                foreach (var pair in map)
                {
                    HashSet<uint> newSet = new HashSet<uint>();

                    for (uint i = 0; i <= pair.Key.Count; i++)
                    {
                        if (!pair.Value.Contains(i))
                        {
                            newSet.Add(i);
                        }
                    }

                    invertedMap.Add(pair.Key, newSet);
                }

                return new Linear()
                {
                    map = invertedMap
                };
            }
        }
    }
}
