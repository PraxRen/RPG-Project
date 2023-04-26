using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [System.Serializable]
        class Predicate
        {
            [SerializeField] private string _predicate;
            [SerializeField] private string[] _parameters;
            [SerializeField] private bool _negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (IPredicateEvaluator evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(_predicate, _parameters);

                    if (result == null)
                        continue;

                    if (result == _negate)
                        return false;
                }

                return true;
            }
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] private Predicate[] _or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in _or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [SerializeField] private Disjunction[] _and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in _and)
            {
                if (!dis.Check(evaluators))
                    return false;
            }

            return true;
        }
    }
}
