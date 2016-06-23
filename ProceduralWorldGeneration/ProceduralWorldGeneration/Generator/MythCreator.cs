﻿using ProceduralWorldGeneration.DataStructure;
using ProceduralWorldGeneration.Input;
using ProceduralWorldGeneration.Input.ParserDefinition;
using ProceduralWorldGeneration.MythObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProceduralWorldGeneration.Generator
{
    class MythCreator : INotifyPropertyChanged
    {

        private MythObjectReader myth_object_reader;
        private Parser myth_object_parser;


        private CreationMyth _creation_myth;
        public CreationMyth CreationMyths
        {
            get
            {
                return _creation_myth;
            }

            set
            {
                if (_creation_myth != value)
                {
                    _creation_myth = value;
                    NotifyPropertyChanged("CreationMyths");

                }
            }
        }

        private Random rnd;

        private int _current_year = 0;

        private int _end_year = 1000;

        public MythCreator()
        {
           
        }

        public void InitializeMythCreation(WorldGenerationConfig config)
        {
            rnd = new Random(config.RandomSeed.GetHashCode());
            myth_object_reader = new MythObjectReader();
            myth_object_reader.readMythObjects();
            myth_object_parser = new Parser();
            myth_object_parser.generateExpressionTree(myth_object_reader.Tokens);
            myth_object_parser.generateMythObjects();

            _creation_myth.MythObjectData = myth_object_parser.MythObjects;

            _creation_myth.MythObjects = new List<BaseMythObject>();
            _creation_myth.ActionableMythObjects = new Queue<IAction>();

            _creation_myth.PrimordialForces = new List<PrimordialForce>();
        }



        public void creationLoop()
        {
            int action_queue_count, counter;
            IAction current_myth_object;
            // first the primordial powers are created. This strongly shapes what type of universe will spawn.
            createPrimordialPowers();
            
            // each tick is one year. Each myth object that can take actions can take one action per year at most.
            while (_current_year < _end_year)
            {

                // go through action queue once.
                counter = 0;
                action_queue_count = _creation_myth.ActionableMythObjects.Count;
                while (counter < action_queue_count)
                {
                    current_myth_object = _creation_myth.ActionableMythObjects.Dequeue();

                    // Restore some points for the next round.
                    current_myth_object.regenerateActionPoints(rnd);

                    // only take action if the myth object has at least 1 action point.
                    if (current_myth_object.ActionPoints > 0)
                    {
                        current_myth_object.takeAction(_creation_myth, _current_year, rnd);
                    }

                    _creation_myth.ActionableMythObjects.Enqueue(current_myth_object);
                    counter = counter + 1;
                }

                _current_year += 1;
            }

            // END OF CREATION
        }


        private void createPrimordialPowers()
        {
            List<PrimordialForce> all_primordial_forces = _creation_myth.MythObjectData.PrimordialForces;
            int count = all_primordial_forces.Count;
            int total_weight = 0;

            // accumulate total weight
            foreach (PrimordialForce primordial_force in all_primordial_forces)
            {
                total_weight += primordial_force.SpawnWeight;
            }

            // pick first primordial power. There needs to be always at least one as otherwise nothing will be created.
            int chance = rnd.Next(total_weight);
            int current_weight = 0;
            foreach (PrimordialForce primordial_force in all_primordial_forces)
            {
                current_weight += primordial_force.SpawnWeight;
                if (chance < current_weight)
                {
                    _creation_myth.PrimordialForces.Add(primordial_force);
                    break;
                }
            }

            // remove primordial force from list to not spawn it twice
            all_primordial_forces.Remove(_creation_myth.PrimordialForces[0]);
            count = count - 1;

            // chance for a second primordial force.
            chance = rnd.Next(100);
            
            // 50% chance of an opposing force appearing.
            if (chance < 50 && _creation_myth.PrimordialForces[0].Opposing != null)
            {
                _creation_myth.PrimordialForces.Add(opposingForce(_creation_myth.PrimordialForces[0]));
            }
            // 20% chance of any other force appearing.
            else if (chance < 70)
            {
                _creation_myth.PrimordialForces.Add(all_primordial_forces[rnd.Next(count)]);
                all_primordial_forces.Remove(_creation_myth.PrimordialForces[1]);
                count = count - 1;
                // 2% chance of a third force appearing
                if (chance < 52)
                {
                    _creation_myth.PrimordialForces.Add(all_primordial_forces[rnd.Next(count)]);
                }
            }

            // Add them to the complete list for viewing
            foreach (PrimordialForce primordial_force in _creation_myth.PrimordialForces)
            {
                _creation_myth.MythObjects.Add(primordial_force);
                _creation_myth.ActionableMythObjects.Enqueue(primordial_force);
            }

        }


        public void ClearMythCreation()
        {
            _creation_myth.MythObjects.Clear();
            _creation_myth.PrimordialForces.Clear();
        }

        private PrimordialForce opposingForce(PrimordialForce primordial_force)
        {
            foreach (PrimordialForce current_primordial_force in _creation_myth.MythObjectData.PrimordialForces)
            {
                if (current_primordial_force.Opposing == primordial_force.Name)
                {
                    return current_primordial_force;
                }
            }
            return null;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
