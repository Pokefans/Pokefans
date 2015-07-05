// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Some.Namespace.Here;
using Pokefans; // Our and 3rd Party namespaces come after ALL system namespaces
using Nintendo.Pokemon;


// Pokefans coding guidelines
// ----------------------------------------------
//
// Indentation:
// I know this is a very strongly discussed topic, but for convenience sake, let's keep it on VS defaults.
// I also know that tabs are better (!!!111) but, to be honest, I care more about contributions than about whitespace.
// So, in Pokefans, we use 4 Spaces for indentation. NO. EXCEPTION.
// "But I want to use tabs!!1111" - then do: http://stackoverflow.com/questions/2316677/can-git-automatically-switch-between-spaces-and-tabs
// the repository will always contain spaces, no matter what.


// Structs: structs are data havens, nothing else.
// Want a member function or constructor? Make a class instead.
// Struct Names: CamelCase

struct CompletelyUselsessPokedexInfo {
    public int UselessId;      //!< Public variables are CamelCase.
    public IPokedexInfo info;  //!< Interfaces are an exception, they're named ICamelCase.
    public string Røfëlkøptør; //!< Unicode is totally fine too! And that includes that you have to save your files as UTF8.
}


// Namespaces should be named CamelCase
// Exception: Areas, which are lowercase
// Also, they do push the indentation level
namespace Pokemon {
    // Classes: complex thingies.
    // Naming: CamelCase
    public class RelaxoIsAHeavvyPokemon : IPokemon
    {                               // Braces deserve their own lines.
        ///<summary>
        /// Everything that isn't private needs to be xml-doc'ed
        ///</summary>
        public RelaxoWeight Weight { get; set; } // {} can be condensed for properties like this

        private bool isBaby = false; // private members are camelCase. Also, explicit visibility declaration is mandatory.

//^ Add a newline after a declaration. This applies to Properties, Variables, or generally everything present in the class scope.
        // Long method signatures get wrapped. Align with spaces.
        // Constructors can get wrapped too.
        /// <summary>
        /// Initializes a new instance of the <see cref="RealaxoIsAHeavvyPokemon"/> class.
        /// </summary>
        /// <param name="really">Bool that represents if relaxo really is a heavvy pokemon</param>
        /// <param name="isAlreadyWokenWithPokeFlute">Bool that represents if relaxo is already woken with pokeflute</param>
        /// <param name="caughtWith">informations about how relaxo was caught</param>
        /// <param name="favoritePokeBall">This relaxos favorite pokeball</param>
        public RelaxoIsAHeavvyPokemon(bool really, bool isAlreadyWokenWithPokeFlute, CaughtInfo caughtWith, 
        	                          PokeBall favoritePokeBall)
        {
            if(!isAlreadyWokenWithPokeFlute)
            {
            	favoritePokeBall.MakeDescision();
            }
            else // Else deserves it's own line too
            {
                if(CaughtInfo.Pokeball == favoritePokeBall)
                {
                    // Notice the spaces:
                    //                 |  |
                    //                 v  v
                    Pokedex.Awesomeness += 1337;
                }
            }

            this.Weight = new RelaxoWeight() // Use this.<something> when reffering to class members
            { // Use the {} initialization wherever possible. Also, {} deserve their own lines.
                Weight = 9001,
                OverweightLevel = OverweightLevel.Obese,
                OverweightBadness = 42
            }; // don't forget the semicolon.
        }
        /// <summary>
        /// Formats the Mass of Relaxo nicely.
        /// </summary>
        /// <returns>Mass of Relaxo as string</returns>
        public string printMass()
        {
            // Write ToDos like this:
            // TODO: make better formatting
            return String.Format("{0}", this.Weight.Weight);
        }

        /// <summary>
        /// The Yawn routine. For extra performance, async.
        /// </summary>
        public async Task Yawn()
        {
            // Notice the spaces:
            //                   | |
            //                   v v
            if(this.Weight.Weight < 21)
            {
                // Always use {} for conditional code blocks. Even if it's only one statement.
                if(!isBaby)
                {
                    // Don't throw generic Exceptions.
                    // Altough InvalidOperationException might be OK here.
                    throw new RelaxoTooHungryException("Give me some Pokéblocks!");
                }
            }
            
            int yawing = 42;
            while(yawning-- > 0)
            {
                // Reduce indentation where possible with clever if-statements. In loops, instead of nested checks, check the opposite and use continue.
                if(Pokedex.Awesomeness != 2345)
                {
                    continue;
                }
                if(Pokedex.SomeArbitraryValueBecauseICouldNotThinkOfAnythingBetter == ArbitraryValue.Random)
                    continue; // This is the ONLY place where you may omit {}

                await YawingGenerator.DoTheAcutalWork(this);
            }
        }
    }
}
