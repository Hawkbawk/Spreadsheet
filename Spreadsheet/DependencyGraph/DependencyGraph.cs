// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System.Collections.Generic;

/// <summary>
/// <author>
/// Ryan Hawkins
/// </author>
/// <remarks>
/// Written on January 31, 2019.
/// </remarks>
/// </summary>
namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {

        /// <summary>
        /// Contains a mapping of every node and their associated dependents.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependentsList;

        /// <summary>
        /// Contains a mapping of every of every node and their associated dependees.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependeesList;

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            Size = 0;
            dependentsList = new Dictionary<string, HashSet<string>>();
            dependeesList = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        /// <param name="s">
        /// The string whose dependents status will be checked</param>
        /// <returns>
        /// True if s has dependents, otherwise returns false.</returns>
        public bool HasDependents(string s)
        {
            // Obtain the dependents for s. If the HashSet isn't null or if
            // it has some items in it, return true. Otherwise return false.
            ;
            if (dependentsList.TryGetValue(s, out HashSet<string> dependents)
                && dependents.Count != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        /// <param name="s">
        /// The string whose dependee status will be checked.</param>
        /// <returns>
        /// True if s has dependees, false otherwise.</returns>
        public bool HasDependees(string s)
        {
            // Obtain the dependees for s. If the HashSet isn't null or if
            // it has some items in it, return true. Otherwise return false.
            ;
            if (dependeesList.TryGetValue(s, out HashSet<string> dependees)
                && dependees.Count != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        /// <param name="s">
        /// The string whose list of dependents will be returned.</param>
        /// <returns>
        /// The dependents of s. If s has no dependents, returns an empty HashSet.</returns>
        public IEnumerable<string> GetDependents(string s)
        {
            // If s already has dependents, return those dependents.
            if (dependentsList.TryGetValue(s, out HashSet<string> dependents))
            {
                return new HashSet<string>(dependents);
            }
            // Otherwise return an empty HashSet.
            return new HashSet<string>();
        }

        /// <summary>
        ///  Enumerates dependees(s).  Requires s != null.
        /// </summary>
        /// <param name="s">
        /// The string whose list of dependees will be returned.</param>
        /// <returns>
        /// The dependees of s. If s has no dependees, returns an empty HashSet.</returns>
        public IEnumerable<string> GetDependees(string s)
        {
            // @TODO Make sure and copy the list instead of simply passing
            // the reference to the dependees HashSet.
            if (dependeesList.TryGetValue(s, out HashSet<string> dependees))
            {
                return new HashSet<string>(dependees);
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        /// <param name="s">
        /// The dependee
        /// </param>
        /// <param name="t">
        /// The dependent
        /// </param>
        public void AddDependency(string s, string t)
        {
            // Obtain the list of dependents for s.
            HashSet<string> dependents;
            if (dependentsList.TryGetValue(s, out dependents))
            {
            }
            else
            {
                // If s isn't in the dictionary, add it.
                dependents = new HashSet<string>();
                dependentsList.Add(s, dependents);
            }
            // Add the string t to dependents(s). Return if t is already there.
            if (!dependents.Add(t))
            {
                return;
            }

            // Obtain the list of dependees for t.
            HashSet<string> dependees;
            if (dependeesList.TryGetValue(t, out dependees))
            {
            }
            else
            {
                // If t isn't in the dictionary, add it.
                dependees = new HashSet<string>();
                dependeesList.Add(t, dependees);
            }

            // Add the string s to dependees(t). Return if s is already there.
            dependees.Add(s);
            // Adjust size accordingly.
            Size++;
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        /// <param name="s">
        /// The dependent
        /// </param>
        /// <param name="t">
        /// The dependee
        /// </param>
        public void RemoveDependency(string s, string t)
        {
            // Obtain the list of dependents for s. If the list doesn't exist, 
            // or doesn't contain t, simply return.
            HashSet<string> dependents;
            if (!dependentsList.TryGetValue(s, out dependents) || !dependents.Contains(t))
            {
                return;
            }
            dependents.Remove(t);

            // Obtain the list of dependees for t. If the list doesn't exist,
            // or doesn't contain s, simply return.
            HashSet<string> dependees;
            if (!dependeesList.TryGetValue(t, out dependees) || !dependees.Contains(s))
            {
                return;
            }
            dependees.Remove(s);
            // Adjust size accordingly.
            Size--;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        /// <param name="s">
        /// The string whose dependents will be replaced.
        /// </param>
        /// <param name="newDependents">
        /// A list of new dependents for s.
        /// </param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<string> dependents;
            if (dependentsList.TryGetValue(s, out dependents) && dependents != null)
            {
                // Copy over the list of dependents, so we can safely use them
                // without throwing a modification error.
                IEnumerable<string> dependentsArr = this.GetDependents(s);
                // Delete all of s's dependencies.
                foreach (string str in dependentsArr)
                {
                    RemoveDependency(s, str);
                }
            }
            else
            {
                // If s didn't exist in the dictionary, add it.
                dependentsList.Add(s, new HashSet<string>());
            }

            // Add the list of new dependencies.
            foreach (string dependent in newDependents)
            {
                AddDependency(s, dependent);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        /// <param name="t">
        /// The dependent whose dependees will be replaced.
        /// </param>
        /// <param name="newDependees">
        /// A list of t's new dependencies.
        /// </param>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            HashSet<string> dependees;
            if (dependeesList.TryGetValue(t, out dependees) && dependees != null)
            {
                // Obtain a copy of the list of dependees so we can iterate 
                // over it and change it at the same time.
                IEnumerable<string> dependeesArr = this.GetDependees(t);
                // Delete all of t's dependencies.
                foreach (string str in dependeesArr)
                {
                    RemoveDependency(str, t);
                }
            }
            else
            {
                // If t doesn't exist in the dictionary, add it in. 
                dependeesList.Add(t, new HashSet<string>());
            }

            // Add in all of the new dependencies.
            foreach (string dependee in newDependees)
            {
                AddDependency(dependee, t);
            }
        }
    }
}
