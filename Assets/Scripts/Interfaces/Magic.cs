using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public enum UnitType
    {
        Worker,
        Warrior,
        Archer,
        Monk,
        General,
        Scout,
        Caravan
    }

    public enum VillageState
    {
        Active,
        Ruined
    }

    public enum Team
    {
        Neutral,
        Enemy,
        Our
    }

    public enum ResourceState
    {
        Build,
        Deactive,
        Active
    }

    public enum ResourceType
    {
        Sawmill,
        Mine,
        Farm
    }
}
