namespace SAT.Solver
{
    public enum ESaticfiction
    {
        All,
        Both,
        None
    }

    public class CSaticfiction
    {
        public static ESaticfiction OR(ESaticfiction a, ESaticfiction b)
        {
            if (a == ESaticfiction.All || b == ESaticfiction.All) return ESaticfiction.All;
            else if (a == ESaticfiction.None && b == ESaticfiction.None) return ESaticfiction.None;

            return ESaticfiction.Both;
        }

        public static ESaticfiction AND(ESaticfiction a, ESaticfiction b)
        {
            if (a == ESaticfiction.All && b == ESaticfiction.All) return ESaticfiction.All;
            else if (a == ESaticfiction.None || b == ESaticfiction.None) return ESaticfiction.None;

            return ESaticfiction.Both;
        }

        public static ESaticfiction NOT(ESaticfiction a)
        {
            if (a == ESaticfiction.All) return ESaticfiction.None;
            if (a == ESaticfiction.None) return ESaticfiction.All;

            return ESaticfiction.Both;
        }

        public static bool isSatisfiable(ESaticfiction a)
        {
            return a != ESaticfiction.None;
        }
    }
}
