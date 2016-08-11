﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProceduralWorldGeneration.DataStructure;
using ProceduralWorldGeneration.MythObjects;
using ProceduralWorldGeneration.Generator;
using ProceduralWorldGeneration.MythActions.CreatePlaneActions;

namespace ProceduralWorldGeneration.MythActions.CreateDeityActions
{
    class SetPersonality : MythAction
    {
        public override bool checkPrecondition(ActionTakerMythObject taker)
        {
            return true;
        }

        public override void Effect(ActionTakerMythObject taker)
        {
            taker.DeityCreation.Personality = CreationMythState.MythObjectData.Personalities[ConfigValues.RandomGenerator.Next(CreationMythState.MythObjectData.Personalities.Count)];
        }
    }
}
