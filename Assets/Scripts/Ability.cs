using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public abstract class Ability : MonoBehaviour
    {
        public string Description;
        public int Damage;

        public class Berserk : Ability
        {
            public Berserk()
            {
                Description = "Björn will go berserk and do ... extra damage";
                Damage = 40;
            }
            
           

        }

        public class DrinkMead : Ability
        {
            public DrinkMead()
            {
                Description = "Björn will drink mead and ...";
                Damage = 30;
            }
        }

    }

