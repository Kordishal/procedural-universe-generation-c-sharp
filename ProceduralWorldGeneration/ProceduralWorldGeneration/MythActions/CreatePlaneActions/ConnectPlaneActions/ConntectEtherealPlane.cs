﻿using ProceduralWorldGeneration.DataStructure;
using ProceduralWorldGeneration.Generator;
using ProceduralWorldGeneration.MythObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralWorldGeneration.MythActions.CreatePlaneActions.ConnectPlaneActions
{
    class ConntectEtherealPlane : ConnectPlanes
    {
        public override bool checkPrecondition(ActionTakerMythObject taker)
        {
            if (taker.PlaneConstruction.PlaneType.isAttachedTo != null)
                return true;
            else
                return false;
        }

        public override void Effect(ActionTakerMythObject taker)
        {
            taker.PlaneConstruction.connectPlane(searchPlaneType(taker.PlaneConstruction.PlaneType.isAttachedTo));
            taker.PlaneConstruction.PlaneSize = taker.PlaneConstruction.NeighbourPlanes[0].PlaneSize;
            taker.CurrentCreationState.isConnected = true;
        }
    }
}