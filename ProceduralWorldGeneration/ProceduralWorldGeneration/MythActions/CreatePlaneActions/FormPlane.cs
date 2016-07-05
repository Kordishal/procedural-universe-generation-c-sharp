﻿using ProceduralWorldGeneration.DataStructure;
using ProceduralWorldGeneration.MythActions.CreatePlaneActions.FormPlaneActions;
using ProceduralWorldGeneration.MythObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralWorldGeneration.MythActions.CreatePlaneActions
{
    class FormPlane : MythAction
    {

        public FormPlane() : base()
        {
            _is_primitve = false;
        }

        public override bool checkPrecondition(CreationMythState state, BaseMythObject taker)
        {
            PrimordialForce _taker = (PrimordialForce)taker;
            if (_taker.PlaneConstructionState.hasCreator)
            {
                if (!_taker.PlaneConstructionState.hasType || !_taker.PlaneConstructionState.hasSize || !_taker.PlaneConstructionState.hasElement)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override void Effect(CreationMythState state, BaseMythObject taker)
        {
            throw new NotImplementedException();
        }
    }
}