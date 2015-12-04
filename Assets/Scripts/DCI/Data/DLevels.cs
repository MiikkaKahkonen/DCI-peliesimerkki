using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DLevels : Data,RGameLevels
{
    Dictionary<string, RGameGraph> levels;

	public DLevels() : base()
	{
        levels = new Dictionary<string, RGameGraph>();
    }

    #region RGameLevelList implementation
    Dictionary<string, RGameGraph> RGameLevels.levels
    {
        get
        {
            return levels;
        }

        set
        {
            levels = value;
        }
    }
    #endregion
}
