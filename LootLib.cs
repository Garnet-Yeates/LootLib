using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;

namespace LootLib
{
    /// <summary>
    /// A <see cref="LootPredicate{R}"/> is the same as a regular predicate but the type param R must be an <see cref="LootExtensions.IItemDropRule"/>.
    /// Loot Predicates are used to find a specific rule that matches the supplied set of conditions.<br/>
    /// <br/>
    /// These predicates can be used inside any of the top-level recursive extension methods:<br/><br/>
    /// <see cref="LootExtensions.RemoveWhere{R}(ILoot, LootPredicate{R}, bool, int?, bool, ChainAttacher, bool)"/><br/>
    /// <see cref="LootExtensions.RemoveChildrenWhere{R}(IItemDropRule, LootPredicate{R}, int?, bool, ChainAttacher, bool)"/><br/>
    /// <see cref="LootExtensions.RemoveWhere{P, C, R}(ILoot, LootPredicate{R}, bool, int?, bool, ChainAttacher bool)"/><br/>
    /// <see cref="LootExtensions.RemoveChildrenWhere{P, C, R}(IItemDropRule, LootPredicate{R}, int?, bool, ChainAttacher bool)"/><br/>
    /// <see cref="LootExtensions.RemoveWhere{N, R}(ILoot, LootPredicate{R}, bool, bool, int?)"/><br/>
    /// <see cref="LootExtensions.RemoveChildrenWhere{N, R}(IItemDropRule, LootPredicate{R}, bool, int?)"/><br/>
    /// <br/>
    /// <see cref="LootExtensions.HasRuleWhere{R}(ILoot, LootPredicate{R}, bool, int?)"/><br/>
    /// <see cref="LootExtensions.FindRuleWhere{R}(ILoot, LootPredicate{R}, bool, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.FindRulesWhere{R}(ILoot, LootPredicate{R}, bool, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.HasChildWhere{R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildWhere{R}(IItemDropRule, LootPredicate{R}, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildrenWhere{R}(IItemDropRule, LootPredicate{R}, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.HasRuleWhere{P, C, R}(ILoot, LootPredicate{R}, bool, int?)"/><br/>
    /// <see cref="LootExtensions.FindRuleWhere{P, C, R}(ILoot, LootPredicate{R}, bool, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.FindRulesWhere{P, C, R}(ILoot, LootPredicate{R}, bool, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.HasChildWhere{P, C, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildWhere{P, C, R}(IItemDropRule, LootPredicate{R}, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildrenWhere{P, C, R}(IItemDropRule, LootPredicate{R}, ChainAttacher, int?)"/><br/>
    /// <see cref="LootExtensions.HasRuleWhere{N, R}(ILoot, LootPredicate{R}, bool, int?)"/><br/>
    /// <see cref="LootExtensions.FindRuleWhere{N, R}(ILoot, LootPredicate{R}, bool, int?)"/><br/>
    /// <see cref="LootExtensions.FindRulesWhere{N, R}(ILoot, LootPredicate{R}, bool, int?)"/><br/>
    /// <see cref="LootExtensions.HasChildWhere{N, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildWhere{N, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindChildrenWhere{N, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <br/>
    /// There are also special helper extension methods for <see cref="LootExtensions.IItemDropRule"/>s that can <i>only</i> be used within these predicates:<br/><br/>
    /// <see cref="LootExtensions.ParentRule(IItemDropRule, int)"/><br/>
    /// <see cref="LootExtensions.ImmediateParentRule(IItemDropRule)"/><br/>
    /// <see cref="LootExtensions.ChainFromImmediateParent(IItemDropRule)"/><br/>
    /// <see cref="LootExtensions.IsChained(IItemDropRule)"/><br/>
    /// <see cref="LootExtensions.IsNested(IItemDropRule)"/><br/>
    /// <see cref="LootExtensions.HasParentRuleWhere{R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindParentRuleWhere{R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.HasParentRuleWhere{P, C, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.FindParentRuleWhere{P, C, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.HasParentRuleWhere{N, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// <see cref="LootExtensions.LootExtensions.FindParentRuleWhere{N, R}(IItemDropRule, LootPredicate{R}, int?)"/><br/>
    /// </summary>
    public delegate bool LootPredicate<R>(R rule) where R : class, IItemDropRule;

    /// <summary>
    /// When reattachChains is set to true in any of the recursive removal methods, and a rule <paramref name="ruleToChain"/> is removed, it will reattach the child chains of the 
    /// currRule onto the parent or currRule. This is so currRule's children are not also lost. By default it will use whatever the ChainAttempt is between
    /// currRule => child. Supplying a ChainAttacher function will allow you to create a chain yourself between the parent of currRule and the child of
    /// currRule. The <paramref name="ruleToChain"/> parameter of this delegate is a reference to one of currRule's children (this function will be called for
    /// each of currRule's children)<br/><br/>
    /// <see cref="ChainAttacher"/> delegates are used by the <paramref name="chainReattacher"/> param (recursive removing extensions, when <paramref name="reattachChains"/> is true), or <paramref name="chainReplacer"/> param (recursive find extensions)
    /// </summary>
    public delegate IItemDropRuleChainAttempt ChainAttacher(IItemDropRule ruleToChain);

    public static class LootExtensions
    {


        #region Recursive Removing 

        #region General Recursive Removing

        // RECURSIVE REMOVING

        /// <summary>
        /// Performs the following processes/checks on each <see cref="IItemDropRule"/> "<paramref name="currRule"/>" within <paramref name="loot"/>.Get()  <br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        /// 
        /// Then it will remove currRule from this loot pool, and possibly re-attach currRule's children onto currRule's parent if <paramref name="reattachChains"/> is set to true.
        /// If <paramref name="reattachChains"/> is true but the rule has no parent (i.e it is directly inside this ILoot, as opposed to being the child of a rule inside this ILoot), the chains can't be reattached (they don't exist). If 
        /// <paramref name="reattachChains"/> is set to false (or there are no chains) it will terminate here because this means the loot chained after this rule is lost no matter what, so there is no reason to make modifications to this rule's children. 
        /// If <paramref name="reattachChains"/> is true, and <paramref name="stopAtFirst"/> is set to false, it will then repeat this same predicate-checking process on the children of the child (nested and chained children) recursively until it
        /// runs out of children to operate on. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// If <paramref name="chainReattacher"/> function is supplied, and reattachChains is set to true, it will re-attach the chains using the function provided as opposed to the default re-attaching mechanism (which is to add currRule's ChainedRules to the 
        /// parent that currRule is removed from). The rule being chained is the param of the function and it should return some type of <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        ///
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool RemoveWhere<R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null, bool reattachChains = false, ChainAttacher chainReattacher = null, bool stopAtFirst = false) where R : class, IItemDropRule
        {
            bool removedAny = false;

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                if (RecursiveRemoveEntryPoint(loot, predicate, rootRule, reattachChains, chainReattacher, stopAtFirst, 1, nthChild))
                {
                    removedAny = true;

                    if (stopAtFirst)
                    {
                        return true;
                    }
                }
            }

            return removedAny;
        }

        /// <summary>
        /// Performs the following processes/checks on each child rule of this <paramref name="rootRule"/><br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        /// 
        /// Then it will remove currRule from this loot pool, and possibly re-attach currRule's children onto currRule's parent if <paramref name="reattachChains"/> is set to true.
        /// If <paramref name="reattachChains"/> is set to false it will terminate here because this means the loot chained after this rule is lost no matter what, so there is no reason to make modifications to this rule's children. 
        /// If <paramref name="reattachChains"/> is true, and <paramref name="stopAtFirst"/> is set to false, it will then repeat this same predicate-checking process on the children of the child (nested and chained children) recursively until it
        /// runs out of children to operate on. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// If <paramref name="chainReattacher"/> function is supplied, and reattachChains is set to true, it will re-attach the chains using the function provided as opposed to the default re-attaching mechanism (which is to add currRule's ChainedRules to the 
        /// parent that currRule is removed from). The rule being chained is the param of the function and it should return some type of <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool RemoveChildrenWhere<R>(this IItemDropRule rootRule, LootPredicate<R> predicate = null, int? nthChild = null, bool reattachChains = false, ChainAttacher chainReattacher = null, bool stopAtFirst = false) where R : class, IItemDropRule
        {
            return RecursiveRemoveEntryPoint(null, predicate, rootRule, reattachChains, chainReattacher, stopAtFirst, 0, nthChild);
        }

        /// <summary>
        /// Main entry point into RecursiveRemoveMain. RecursiveRemoveMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveRemoveMain within RecursiveFind predicates without causing issues with the dictionary being modified during the predicates]) Note: can't think of many use cases
        /// for having RecursiveRemove within RecursiveFind predicates, but there are definitely some. It would be more common to use RecursiveFind within RecursiveRemove
        /// </summary>
        private static bool RecursiveRemoveEntryPoint<R>(ILoot loot, LootPredicate<R> predicate, IItemDropRule rootRule, bool reattachChains, ChainAttacher chainReattacher, bool stopAtFirst, int n, int? nthChild) where R : class, IItemDropRule
        {
            UseDictionary();
            predicate ??= (_) => true;
            bool result = RecursiveRemoveMain(loot, predicate, rootRule, reattachChains, chainReattacher, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return result;
        }

        #endregion

        // The following methods are syntax sugar and apply the following 4 checks to your predicate automatically: rule => rule.HasParentRule() && rule.IsChained() && rule.ChainFromImmediateParent() is C && rule.ImmediateParentRule() is P
        #region Chained Rule Overloads

        /// <summary>
        /// Performs the following processes/checks on each child rule, "<paramref name="currRule"/>" of this <paramref name="rootRule"/><br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/>, and <paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        /// Then it will remove currRule from this loot pool, and possibly re-attach currRule's children onto currRule's parent if <paramref name="reattachChains"/> is set to true.
        /// If <paramref name="reattachChains"/> is true but the rule has no parent (i.e it is directly inside this ILoot, as opposed to being the child of a rule inside this ILoot), the chains can't be reattached (they don't exist). If 
        /// <paramref name="reattachChains"/> is set to false (or there are no chains) it will terminate here because this means the loot chained after this rule is lost no matter what, so there is no reason to make modifications to this rule's children. 
        /// If <paramref name="reattachChains"/> is true, and <paramref name="stopAtFirst"/> is set to false, it will then repeat this same predicate-checking process on the children of the child (nested and chained children) recursively until it
        /// runs out of children to operate on. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// If <paramref name="chainReattacher"/> function is supplied, and reattachChains is set to true, it will re-attach the chains using the function provided as opposed to the default re-attaching mechanism (which is to add currRule's ChainedRules to the 
        /// parent that currRule is removed from). The rule being chained is the param of the function and it should return some type of <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>
        /// </summary>
        public static bool RemoveWhere<P, C, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null, bool reattachChains = false, ChainAttacher chainReattacher = null, bool stopAtFirst = false) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            bool removedAny = false;

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                if (RecursiveRemoveEntryPoint<P, C, R>(loot, predicate, rootRule, reattachChains, chainReattacher, stopAtFirst, 1, nthChild))
                {
                    removedAny = true;

                    if (stopAtFirst)
                    {
                        return true;
                    }
                }
            }

            return removedAny;
        }

        /// <summary>
        /// Performs the following processes/checks on each child rule, "<paramref name="currRule"/>" of this <paramref name="rootRule"/><br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/>, and <paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        /// Then it will remove currRule from this loot pool, and possibly re-attach currRule's children onto currRule's parent if <paramref name="reattachChains"/> is set to true.
        /// If <paramref name="reattachChains"/> is set to false it will terminate here because this means the loot chained after this rule is lost no matter what, so there is no reason to make modifications to this rule's children. 
        /// If <paramref name="reattachChains"/> is true, and <paramref name="stopAtFirst"/> is set to false, it will then repeat this same predicate-checking process on the children of the child (nested and chained children) recursively until it
        /// runs out of children to operate on. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// If <paramref name="chainReattacher"/> function is supplied, and reattachChains is set to true, it will re-attach the chains using the function provided as opposed to the default re-attaching mechanism (which is to add currRule's ChainedRules to the 
        /// parent that currRule is removed from). The rule being chained is the param of the function and it should return some type of <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool RemoveChildrenWhere<P, C, R>(this IItemDropRule rootRule, LootPredicate<R> predicate = null, int? nthChild = null, bool reattachChains = false, ChainAttacher chainReattacher = null, bool stopAtFirst = false) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return RecursiveRemoveEntryPoint<P, C, R>(null, predicate, rootRule, reattachChains, chainReattacher, stopAtFirst, 0, nthChild);
        }

        /// <summary>
        /// Main entry point into RecursiveRemoveMain. RecursiveRemoveMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveRemoveMain within RecursiveFind predicates without causing issues with the dictionary being modified during the predicates]) Note: can't think of many use cases
        /// for having RecursiveRemove within RecursiveFind predicates, but there are definitely some. It would be more common to use RecursiveFind within RecursiveRemove
        /// </summary>
        private static bool RecursiveRemoveEntryPoint<P, C, R>(ILoot loot, LootPredicate<R> predicate, IItemDropRule rootRule, bool reattachChains, ChainAttacher chainReattacher, bool stopAtFirst, int n, int? nthChild) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            UseDictionary();
            predicate ??= (_) => true;
            bool result = RecursiveRemoveMain<R>(loot, rule => rule.HasParentRule() && rule.IsChained() && rule.ImmediateParentRule() is P && rule.ChainFromImmediateParent() is C && predicate(rule), rootRule, reattachChains, chainReattacher, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return result;
        }

        #endregion

        // The following methods are syntax sugar and apply the following 3 checks to your predicate automatically: rule => rule.HasParentRule() && rule.IsNested() && rule.ImmediateParentRule() is N
        #region Nested Rule Overloads

        /// <summary>
        /// Performs the following processes/checks on each rule, "<paramref name="currRule"/>" of this <paramref name="loot"/><br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is nested inside of an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/>, and <paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        ///
        /// Then it will remove currRule from this loot pool. The function will stop on the first removed rule if <paramref name="stopAtFirst"/> is true. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool RemoveWhere<N, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, bool stopAtFirst = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                if (RecursiveRemoveEntryPoint<N, R>(loot, predicate, rootRule, stopAtFirst, 1, nthChild))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Performs the following processes/checks on each child rule, "<paramref name="currRule"/>" of this <paramref name="rootRule"/><br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is nested inside of an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/>, and <paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        ///
        /// Then it will remove currRule from this loot pool. The function will stop on the first removed rule if <paramref name="stopAtFirst"/> is true <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool RemoveChildrenWhere<N, R>(this IItemDropRule rootRule, LootPredicate<R> predicate = null, bool stopAtFirst = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return RecursiveRemoveEntryPoint<N, R>(null, predicate, rootRule, stopAtFirst, 0, nthChild);
        }

        /// <summary>
        /// Main entry point into RecursiveRemoveMain. RecursiveRemoveMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveRemoveMain within RecursiveFind predicates without causing issues with the dictionary being modified during the predicates]) Note: can't think of many use cases
        /// for having RecursiveRemove within RecursiveFind predicates, but there are definitely some. It would be more common to use RecursiveFind within RecursiveRemove
        /// </summary>
        private static bool RecursiveRemoveEntryPoint<N, R>(ILoot loot, LootPredicate<R> predicate, IItemDropRule rootRule, bool stopAtFirst, int n, int? nthChild) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            // They can use IItemDropRule for N to specify any nested parent, but otherwise N MUST inherit from INestedItemDropRule
            // I don't use generic constraint (for compile-time checking) for this because I want them to be able to use IItemDropRule as N to be non-specific
            if (typeof(N) != typeof(IItemDropRule) && !typeof(N).IsAssignableTo(typeof(INestedItemDropRule)))
            {
                throw new InvalidOperationException("N must be of type INestedItemDropRule to be used in <N, R> LootExtensions");
            }

            UseDictionary();
            predicate ??= (_) => true;
            bool result = RecursiveRemoveMain<R>(loot, rule => rule.HasParentRule() && rule.IsNested() && rule.ImmediateParentRule() is N && predicate(rule), rootRule, false, null, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return result;
        }

        #endregion

        // Main function
        #region Main Recursive Removal Method (all other recursive removal functions use this in some way)

        /// <summary>
        /// Performs the following processes/checks on <paramref name="currRule"/>:<br/><br/>
        ///
        /// <code>if (<paramref name="currRule"/> is of type <typeparamref name="R"/>, and <paramref name="currRule"/> matches the <paramref name="predicate"/>)</code>
        /// Then it will remove currRule from this loot pool / its parent, and possibly re-attach currRule's children onto currRule's parent if <paramref name="reattachChains"/> is set to true.
        /// If <paramref name="reattachChains"/> is true but the rule's parent is the ILoot that this extension method originated from, then the children cannot be reattached since the parent isn't an IItemDropRule (there'd be no rule to chain it onto). If 
        /// <paramref name="reattachChains"/> is set to false it will terminate here because this means the loot chained after this rule is lost no matter what, so there is no reason to make modifications to this rule's children. 
        /// If <paramref name="reattachChains"/> is true, and <paramref name="stopAtFirst"/> is set to false, it will then repeat this same predicate-checking process on the children of the child (nested and chained children) (both nested and chained rules)
        /// recursively until it runs out of children to operate on. <br/><br/>
        /// 
        /// If <paramref name="nthChild"/> is supplied, then the above if check will fail unless <paramref name="currRule"/> is the nth descendent of the <see cref="IItemDropRule"/> or <see cref="ILoot"/> that this extension method originated from.<br/><br/>
        ///
        /// If <paramref name="chainReattacher"/> function is supplied, and reattachChains is set to true, it will re-attach the chains using the function provided as opposed to the default re-attaching mechanism (which is to add currRule's ChainedRules to the 
        /// parent that currRule is removed from). The rule being chained is the param of the function and it should return some type of <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.
        ///
        /// <code>else</code>Then it will also repeat this process recursively on the children of the child (nested and chained children). The rule chained/nested onto currRule will be the new currRule on the next call and n goes up by 1 on the next call<br/><br/>
        /// 
        /// </summary>
        private static bool RecursiveRemoveMain<R>(ILoot loot, LootPredicate<R> predicate, IItemDropRule currRule, bool reattachChains, ChainAttacher chainReattacher, bool stopAtFirst, int n, int? nthChild) where R : class, IItemDropRule
        {
            // n is 0 when this method is entered through an IItemDropRule. It is 1 when it is entered through an ILoot. n == 0 and parent being null => loot is not null (loot is parent in this case)
            // n == 1 it is possible for parent to be the ILoot, also possible for the parent to be an IItemDropRule
            // n > 1 => we definitely have an IItemDropRule parent, meaning we're guaranteed to have either a ChainedParent or NestedParent
            if (n != 0 && currRule is R castedRule && predicate(castedRule) && (nthChild is null || nthChild == n))
            {
                if (currRule.ImmediateParentRule() is IItemDropRule parentRule)
                {
                    RemoveFromParent(currRule);

                    if (currRule.IsChained() && reattachChains && !stopAtFirst)
                    {
                        ContinueRecursion(newParent: parentRule);
                    }
                }
                else // If we get to this else => it is guaranteed that loot is not null (n == 1, first iteration, entered through an ILoot)
                {
                    loot.Remove(currRule);
                }

                return true;
            }
            else
            {
                return ContinueRecursion(newParent: currRule);
            }

            // Return true if any were removed. Returns immediately after finding one (preventing extra calls to RecursiveRemoveMain) if stopAtFirst is set to true
            bool ContinueRecursion(IItemDropRule newParent)
            {
                if (nthChild is not null && (n + 1) > nthChild) // Stop trying to search if they specified that it must be nth child (maxN) and we are gonna be further than maxN next iteration
                {
                    return false;
                }

                bool removedAny = false;

                // Try nested rules first
                if (currRule is INestedItemDropRule ruleThatExecutesOthers)
                {
                    foreach (IItemDropRule nestedRule in GetRulesNestedInsideThisRule(ruleThatExecutesOthers))
                    {
                        nestedRule.RegisterAsNestedChild(ruleThatExecutesOthers);

                        removedAny |= RecursiveRemoveMain(loot, predicate, nestedRule, reattachChains, chainReattacher, stopAtFirst, n + 1, nthChild);

                        if (removedAny && stopAtFirst)
                        {
                            return true;
                        }
                    }
                }

                // Then chained rules
                foreach (IItemDropRuleChainAttempt chainAttempt in new List<IItemDropRuleChainAttempt>(currRule.ChainedRules)) // iterate over shallow clone to stop concurrent modification
                {
                    IItemDropRule child = chainAttempt.RuleToChain;
                    child.RegisterAsChainedChild(newParent, chainAttempt);

                    removedAny |= RecursiveRemoveMain(loot, predicate, child, reattachChains, chainReattacher, stopAtFirst, n + 1, nthChild);

                    if (removedAny && stopAtFirst)
                    {
                        return true;
                    }
                }

                return removedAny;
            }

            void RemoveFromParent(IItemDropRule removing)
            {
                // Down here it is implied that the entry exists in the dictionary
                ParentChildRelationship relationship = ParentDictionary[removing];

                // If the rule is chained to a parent
                if (relationship.ChainedParent is not null)
                {
                    IItemDropRule parentOfRemoving = relationship.ChainedParent;
                    parentOfRemoving.ChainedRules.Remove(relationship.ChainAttempt);

                    if (reattachChains)
                    {
                        foreach (IItemDropRuleChainAttempt chainAttempt in removing.ChainedRules)
                        {
                            parentOfRemoving.ChainedRules.Add(chainReattacher is null ? chainAttempt : chainReattacher(chainAttempt.RuleToChain));
                        }
                    }

                }

                // If the rule is nested inside of a parent
                else
                {
                    RemoveNestedRuleFromParent(removing, (INestedItemDropRule)relationship.NestedParent);
                }
            }
        }

        private static IItemDropRule[] GetRulesNestedInsideThisRule(INestedItemDropRule rule)
        {
            IItemDropRule[] children = null;
            if (rule is OneFromRulesRule ofr)
            {
                children = ofr.options;
            }
            else if (rule is FewFromRulesRule ffr)
            {
                children = ffr.options;
            }
            else if (rule is SequentialRulesRule srr)
            {
                children = srr.rules;
            }
            else if (rule is SequentialRulesNotScalingWithLuckRule srrnl)
            {
                children = srrnl.rules;
            }
            else if (rule is AlwaysAtleastOneSuccessDropRule aos)
            {
                children = aos.rules;
            }
            else if (rule is DropBasedOnExpertMode dbem)
            {
                children = new IItemDropRule[] { dbem.ruleForExpertMode, dbem.ruleForNormalMode };
            }
            else if (rule is DropBasedOnMasterMode dbmm)
            {
                children = new IItemDropRule[] { dbmm.ruleForMasterMode, dbmm.ruleForDefault };
            }

            return children;
        }

        private static void RemoveNestedRuleFromParent(IItemDropRule rule, INestedItemDropRule parent)
        {
            if (parent is OneFromRulesRule ofr)
            {
                ofr.RemoveOption(rule);
            }
            else if (parent is FewFromRulesRule ffr)
            {
                ffr.RemoveOption(rule);
            }
            else if (parent is SequentialRulesRule srr)
            {
                srr.RemoveOption(rule);
            }
            else if (parent is SequentialRulesNotScalingWithLuckRule srrnl)
            {
                srrnl.RemoveOption(rule);
            }
            else if (parent is AlwaysAtleastOneSuccessDropRule aos)
            {
                aos.RemoveOption(rule);
            }
            else if (parent is DropBasedOnExpertMode dbem)
            {
                if (dbem.ruleForExpertMode == rule)
                {
                    dbem.ruleForExpertMode = ItemDropRule.DropNothing();

                }
                if (dbem.ruleForNormalMode == rule)
                {
                    dbem.ruleForNormalMode = ItemDropRule.DropNothing();
                }
            }
            else if (parent is DropBasedOnMasterMode dbmm)
            {
                if (dbmm.ruleForMasterMode == rule)
                {
                    dbmm.ruleForMasterMode = ItemDropRule.DropNothing();
                }
                if (dbmm.ruleForDefault == rule)
                {
                    dbmm.ruleForDefault = ItemDropRule.DropNothing();
                }
            }
        }

        #endregion

        #endregion

        #region Recursive Finding

        #region General Recursive Finding

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for any rules <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that match the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find more, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied and the found rule is chained (not nested), the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindRulesWhere<R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint(rootRule, predicate, chainReplacer, false, 1, nthChild));
            }

            return foundRules;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for the first <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindRuleWhere<R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint(rootRule, predicate, chainReplacer, true, 1, nthChild));

                if (foundRules.Any())
                {
                    return foundRules[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for the first <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindRuleWhere<R>(this ILoot loot, out R result, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            result = loot.FindRuleWhere(predicate, includeGlobalDrops, chainReplacer, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and sees if there is any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        /// 
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary>
        public static bool HasRuleWhere<R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where R : class, IItemDropRule
        {
            return loot.FindRuleWhere(predicate, includeGlobalDrops, null, nthChild) is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for any children <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that match the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find more, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied and the found rule is chained (not nested), the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindChildrenWhere<R>(this IItemDropRule root, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            return RecursiveFindEntryPoint(root, predicate, chainReplacer, false, 0, nthChild);
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for the first child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindChildWhere<R>(this IItemDropRule root, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            List<R> children = RecursiveFindEntryPoint(root, predicate, chainReplacer, true, 0, nthChild);
            return children.Any() ? children[0] : null;
        }


        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for the first child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindChildWhere<R>(this IItemDropRule root, out R result, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where R : class, IItemDropRule
        {
            result = root.FindChildWhere(predicate, chainReplacer, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and sees if there is any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool HasChildWhere<R>(this IItemDropRule root, LootPredicate<R> predicate = null, int? nthChild = null) where R : class, IItemDropRule
        {
            return root.FindChildWhere(predicate, null, nthChild) is not null;
        }

        /// <summary>
        /// Main entry point into RecursiveFindMain. RecursiveFindMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveFind within RecursiveRemoveWhere predicates without causing issues with the dictionary being modified during the predicates])
        /// </summary>
        private static List<R> RecursiveFindEntryPoint<R>(IItemDropRule root, LootPredicate<R> predicate, ChainAttacher chainReplacer, bool stopAtFirst, int n, int? nthChild) where R : class, IItemDropRule
        {
            UseDictionary();
            predicate ??= (_) => true;
            List<R> foundRules = RecursiveFindMain(new(), predicate, null, chainReplacer, root, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return foundRules;
        }

        #endregion

        // The following methods are syntax sugar and apply the following 3 checks to your predicate automatically: rule => rule.HasParentRule() && rule.IsChained() && rule.ChainFromImmediateParent() is C
        #region Chained Rule Overloads

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindRulesWhere<P, C, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint<P, C, R>(rootRule, predicate, chainReplacer, false, 1, nthChild));
            }

            return foundRules;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for the first <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindRuleWhere<P, C, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint<P, C, R>(rootRule, predicate, chainReplacer, true, 1, nthChild));

                if (foundRules.Any())
                {
                    return foundRules[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for the first <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindRuleWhere<P, C, R>(this ILoot loot, R result, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            result = loot.FindRuleWhere<P, C, R>(predicate, includeGlobalDrops, chainReplacer, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and sees if there is any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool HasRuleWhere<P, C, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return loot.FindRuleWhere<P, C, R>(predicate, includeGlobalDrops, null, nthChild) is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindChildrenWhere<P, C, R>(this IItemDropRule root, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return RecursiveFindEntryPoint<P, C, R>(root, predicate, chainReplacer, false, 0, nthChild);
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for the first child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindChildWhere<P, C, R>(this IItemDropRule root, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            List<R> foundRule = RecursiveFindEntryPoint<P, C, R>(root, predicate, chainReplacer, true, 0, nthChild);
            return foundRule.Any() ? foundRule[0] : null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for the first child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindChildWhere<P, C, R>(this IItemDropRule root, out R result, LootPredicate<R> predicate = null, ChainAttacher chainReplacer = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            result = root.FindChildWhere<P, C, R>(predicate, chainReplacer, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and sees if there is any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool HasChildWhere<P, C, R>(this IItemDropRule root, LootPredicate<R> predicate = null, int? nthChild = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return root.FindChildWhere<P, C, R>(predicate, null, nthChild) is not null;
        }

        /// <summary>
        /// Main entry point into RecursiveFindMain. RecursiveFindMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveFind within RecursiveRemoveWhere predicates without causing issues with the dictionary being modified during the predicates])
        /// </summary>
        public static List<R> RecursiveFindEntryPoint<P, C, R>(IItemDropRule root, LootPredicate<R> predicate, ChainAttacher chainReplacer, bool stopAtFirst, int n, int? nthChild) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            UseDictionary();
            predicate ??= (_) => true;
            List<R> foundRules = RecursiveFindMain<R>(new(), rule => rule.HasParentRule() && rule.IsChained() && rule.ImmediateParentRule() is P && rule.ChainFromImmediateParent() is C && predicate(rule), null, chainReplacer, root, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return foundRules;
        }

        #endregion

        // The following methods are syntax sugar and apply the following 3 checks to your predicate automatically: rule => rule.HasParentRule() && rule.IsNested() && rule.ImmediateParentRule() is N
        #region Nested Rule Overloads

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and looks for any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find more, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindRulesWhere<N, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint<N, R>(rootRule, predicate, false, 1, nthChild));
            }

            return foundRules;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and sees if there is any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindRuleWhere<N, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            List<R> foundRules = new();

            foreach (IItemDropRule rootRule in loot.Get(includeGlobalDrops))
            {
                foundRules.AddRange(RecursiveFindEntryPoint<N, R>(rootRule, predicate, true, 1, nthChild));

                if (foundRules.Any())
                {
                    return foundRules[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and sees if there is any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindRuleWhere<N, R>(this ILoot loot, out R result, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            result = loot.FindRuleWhere<N, R>(predicate, includeGlobalDrops, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="ILoot"/> instance and sees if there is any <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool HasRuleWhere<N, R>(this ILoot loot, LootPredicate<R> predicate = null, bool includeGlobalDrops = false, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return loot.FindRuleWhere<N, R>(predicate, includeGlobalDrops, nthChild) is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and looks for any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will add it to a list. Regardless of if a match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find more, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return an empty list.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static List<R> FindChildrenWhere<N, R>(this IItemDropRule root, LootPredicate<R> predicate = null, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return RecursiveFindEntryPoint<N, R>(root, predicate, false, 0, nthChild);
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and sees if there is any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return it. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static R FindChildWhere<N, R>(this IItemDropRule root, LootPredicate<R> predicate = null, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            List<R> foundRule = RecursiveFindEntryPoint<N, R>(root, predicate, true, 0, nthChild);
            return foundRule.Any() ? foundRule[0] : null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and sees if there is any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will set <paramref name="result"/> to the matched rule and return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool TryFindChildWhere<N, R>(this IItemDropRule root, out R result, LootPredicate<R> predicate = null, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            result = root.FindChildWhere<N, R>(predicate, nthChild);
            return result is not null;
        }

        /// <summary>
        /// Recursively loops through this <see cref="IItemDropRule"/>'s children and sees if there is any child <see cref="IItemDropRule"/> of type <typeparamref name="R"/> that is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/> that matches the given <paramref name="predicate"/>. If <paramref name="nthChild"/> is specified, then the rule must be the nth child of the loot pool.
        /// If a match is found it will return true. If no match is found, it will recursively loop through the children of that child (both nested and chained children) and try to find one, so on and so forth.
        /// If nothing was found after searching all children recursively, it will return false.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        public static bool HasChildWhere<N, R>(this IItemDropRule root, LootPredicate<R> predicate = null, int? nthChild = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return root.FindChildWhere<N, R>(predicate, nthChild) is not null;
        }

        /// <summary>
        /// Main entry point into RecursiveFindMain. RecursiveFindMain should only ever be called by this method or itself. This method marks the dictionary as in use and will clear
        /// it and unmark after (unless some other call earlier on the method call stack was using it first, in which case it will let that method unmark and clear it [this is so that you
        /// can use RecursiveFind within RecursiveRemoveWhere predicates without causing issues with the dictionary being modified during the predicates])
        /// </summary>
        private static List<R> RecursiveFindEntryPoint<N, R>(IItemDropRule root, LootPredicate<R> predicate, bool stopAtFirst, int n, int? nthChild) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            if (typeof(N) != typeof(IItemDropRule) && !typeof(N).IsAssignableTo(typeof(INestedItemDropRule)))
            {
                throw new InvalidOperationException("N must be of type INestedItemDropRule to be used in <N, R> LootExtensions");
            }

            UseDictionary();
            predicate ??= (_) => true;
            List<R> foundRules = RecursiveFindMain<R>(new(), rule => rule.HasParentRule() && rule.IsNested() && rule.ImmediateParentRule() is N && predicate(rule), null, null, root, stopAtFirst, n, nthChild);
            StopUsingDictionary();
            return foundRules;
        }

        #endregion

        #region Main Recursive Find Method

        // Pass LinkedList reference through the algorithm to add to (better than List here, since all we are doing is adding)
        /// <summary>
        /// Checks if currRule is of type <typeparamref name="R"/> and matches the given <paramref name="predicate"/>. If it does it will be returned. If not, it will recursively call this method
        /// again on the children of currRule. If nthChild is specified, then the currRule must also be the nth child of whatever ILoot or IItemDropRule that this
        /// recursive call originated with. If nothing was found after searching all children recursively, it will return null.<br/><br/>
        ///
        /// If <paramref name="chainReplacer"/> function is supplied, the found rule's chain from its parent will be replaced with the chain that the function returns. The found rule will be the param of the function and it should return some type of 
        /// <see cref="IItemDropRuleChainAttempt"/> where ChainAttempt.RuleToChain is set to the parameter.<br/><br/>
        ///        
        /// Note: A rule (a) is considered the chained parent to another rule (b) if (b) is an <see cref="IItemDropRuleChainAttempt"/>.RuleToChain in (a)'s <see cref="IItemDropRule.ChainedRules"/> array<br/>
        /// Note: A rule (a) is considered the nested parent to another rule (b) if (a) is an <see cref="INestedItemDropRule"/> that executes (b). Examples of <see cref="INestedItemDropRule"/> include (not limited to) <see cref="OneFromRulesRule"/>, <see cref="DropBasedOnExpertMode"/>, <see cref="SequentialRulesRule"/>        
        /// </summary> 
        private static List<R> RecursiveFindMain<R>(LinkedList<R> found, LootPredicate<R> predicate, int? indexOfChainToCurrRule, ChainAttacher chainReplacer, IItemDropRule currRule, bool stopAtFirst, int n, int? nthChild) where R : class, IItemDropRule
        {
            // n == 0 means this RecursiveFindMain call should not check currRule at all. This is used when an EntryPoint is first called on an IItemDropRule (not an ILoot) so that the rule
            // itself isn't queried and returned if it happens to match the <paramref name="predicate"/> (because we only want to predicate its children). n will be initially set to 1 when called on ILoot entry, 0 when called on IItemDropRule entry

            // n must not be 0, currRule must be of type T and match the <paramref name="predicate"/>. Also if nthChild is specified, n must be == nthChild
            if (n != 0 && currRule is R castedRule && predicate(castedRule) && (nthChild is null || nthChild == n))
            {
                // When we find a chained rule matching the predicate and chainReplacer is supplied, use it to replace chains
                if (chainReplacer is not null && indexOfChainToCurrRule is int i) // indexOfChainToCurrRule not null => rule has chained parent
                {
                    currRule.ImmediateParentRule().ChainedRules[i] = chainReplacer(currRule);
                }

                found.AddLast(castedRule);

                if (!stopAtFirst)
                {
                    ContinueRecursion();
                }
            }
            else
            {
                ContinueRecursion();
            }

            return found.ToList();

            void ContinueRecursion()
            {
                if (nthChild is not null && (n + 1) > nthChild) // Stop trying to search if they specified that it must be nth child and we are gonna be further than that specified n next iteration
                {
                    return;
                }

                // Try nested children first
                if (currRule is INestedItemDropRule ruleThatExecutesOthers)
                {
                    foreach (IItemDropRule nestedRule in GetRulesNestedInsideThisRule(ruleThatExecutesOthers))
                    {
                        nestedRule.RegisterAsNestedChild(ruleThatExecutesOthers);
                        RecursiveFindMain(found, predicate, null, chainReplacer, nestedRule, stopAtFirst, n + 1, nthChild);
                    }
                }

                // Then try chained children
                for (int i = 0; i < currRule.ChainedRules.Count; i++)
                {
                    IItemDropRuleChainAttempt chainAttempt = currRule.ChainedRules[i];
                    IItemDropRule child = chainAttempt.RuleToChain;
                    child.RegisterAsChainedChild(currRule, chainAttempt);

                    RecursiveFindMain(found, predicate, i, chainReplacer, child, stopAtFirst, n + 1, nthChild);
                }
            }
        }

        #endregion

        #endregion

        // Data structures used within recursive calls for parent tracking. Only use these within predicates, or else it will throw an InvalidOperationException
        #region Volatile Parent Dictionary (All the extensions used here can only be used in the context of RecursiveMain functions or in the context of the predicates that RecursiveMain functions execute)

        /// <summary>
        /// Used to temporarily store data about which IItemDropRule is the "parent" of another IItemDropRule, as well as storing the ChainAttempt 
        /// An IItemDropRule is considered a "child" to another IItemDropRule if the parent's ChainedRules array contains an IItemDropRuleChainAttempt
        /// where the chainAttempt.RuleToChain references the child.
        /// </summary>
        public struct ParentChildRelationship
        {
            // These two will be set if the child is chained to its parent (NestedParent will be null)
            public IItemDropRule ChainedParent { get; set; }
            public IItemDropRuleChainAttempt ChainAttempt { get; set; }

            // Or they will be null and NestedParent won't be 
            public IItemDropRule NestedParent { get; set; }

        }

        /// <summary>
        /// Used to temporarily store data about which IItemDropRule is the "parent" of another IItemDropRule. Keys in this dictionary
        /// represent children, and you can get their parent as well as the chain that leads to the child. This dictionary is built up
        /// on-the-fly during RecursiveRemoveMain and RecursiveFindMain, and is cleared after recursive methods are done using it, so it will only be
        /// accurate during recursion. This means that IItemDropRule.ParentRule(n) extension will only be valid during recursion and should only be used in
        /// predicates plugged into my recursive functions <br/>
        /// See <see cref="ParentRule(IItemDropRule, int)"/>, as well as <see cref="DictionaryPopulated"/>
        /// </summary> 
        private readonly static Dictionary<IItemDropRule, ParentChildRelationship> ParentDictionary = new();

        /// <summary>
        /// Used to determine if the dictionary is being used, and to control whether or not the dictionary should be cleared 
        /// (if a recursive function earlier on the stack used it first, then later calls that use the dictionary won't be responsible for
        /// clearing it and it will be left up to the earlier function). See
        /// <see cref="RecursiveFindEntryPoint{R}(IItemDropRule, LootPredicate{R}, int, int?)"/> and 
        /// <see cref="RecursiveRemoveEntryPoint{R}(ILoot, LootPredicate{R}, IItemDropRule, bool, ChainAttacher, bool, int, int?)"/>
        /// to see this behavior. It is also used in 
        /// <see cref="ParentRule(IItemDropRule, int)"/> to make sure it is being called in the correct context (Dictionary MUST bee in use by a 
        /// recursive method to be able to use ParentRule(n). This basically means that ParentRule(n) can only be used within predicates of my extension methods)
        /// </summary>
        private static readonly Stack<object> DictionaryUseStack = new();

        private static bool DictionaryPopulated => DictionaryUseStack.Any();

        private static void UseDictionary()
        {
            DictionaryUseStack.Push(null);
        }

        private static void StopUsingDictionary()
        {
            DictionaryUseStack.Pop(); // If exception is thrown here it means StopUsingDictionary() was called without calling UseDictionary() (my coding error)
            if (!DictionaryUseStack.Any())
            {
                ParentDictionary.Clear();
            }
        }

        private static void RegisterAsChainedChild(this IItemDropRule rule, IItemDropRule parent, IItemDropRuleChainAttempt chainAttempt)
        {
            ParentDictionary[rule] = new ParentChildRelationship { ChainedParent = parent, ChainAttempt = chainAttempt };
        }

        private static void RegisterAsNestedChild(this IItemDropRule rule, INestedItemDropRule parent)
        {
            ParentDictionary[rule] = new ParentChildRelationship { NestedParent = (IItemDropRule)parent };
        }

        /// <summary>
        /// Finds the immediate parent of this rule. This is equivalent to <see cref="ParentRule(IItemDropRule, int)"/> where nthChild is 1. <br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing<br/><br/>
        /// 
        /// Returns null if no parent is found
        /// </summary>
        public static IItemDropRule ImmediateParentRule(this IItemDropRule rule)
        {
            if (!DictionaryPopulated)
            {
                throw new InvalidOperationException("IItemDropRule parent referencing can only be done in the context of predicates within the Find, Has, and Remove extensions (and their generic overloads)");
            }

            return ParentDictionary.TryGetValue(rule, out ParentChildRelationship parentWithChain) ? (parentWithChain.ChainedParent ?? parentWithChain.NestedParent) : null;
        }

        /// <summary>
        /// Returns the nth parent of this IItemDropRule. n being 1 means the direct parent of this rule, n being 2 means the parent of ParentRule(1), etc<br/><br/>
        /// 
        /// Can only be used in the context of Remove calls, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove as a means of being more exact about what
        /// rule we are removing
        /// </summary>
        public static IItemDropRule ParentRule(this IItemDropRule rule, int nthParent)
        {
            if (nthParent == 0)
            {
                throw new InvalidOperationException("nth parent must be greater than 0");
            }

            for (int i = 0; i < nthParent; i++)
            {
                rule = rule.ImmediateParentRule();

                if (rule is null)
                {
                    return null;
                }
            }

            return rule;
        }

        /// <summary>
        /// Returns true if this rule has an <paramref name="nthParent"/>. nthParent = 1 is immediate parent, nthParent = 2 is immediate parent of immediate parent<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing
        /// </summary>
        public static bool HasParentRule(this IItemDropRule rule, int nthParent = 1)
        {
            return rule.ParentRule(nthParent) is not null;
        }

        /// <summary>
        /// Determines if this rule is Chained onto its parent via an IItemDropRuleChainAttempt. A false return means this rule is nested inside of an INestedItemDropRule.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing<br/><br/>
        /// Throws <see cref="NullReferenceException"></see> if the rule has no parent. This is to ensure that a false return means the rule is Nested
        /// </summary>
        public static bool IsChained(this IItemDropRule rule)
        {
            if (rule.ImmediateParentRule() is null)
            {
                throw new NullReferenceException("Cannot check if the current rule is chained/nested to its parent as it has no parent. Make sure rule.HasParentRule() returns true");
            }

            return ParentDictionary[rule].ChainAttempt is not null;
        }

        public static IItemDropRuleChainAttempt ChainFromImmediateParent(this IItemDropRule rule)
        {
            if (rule.ImmediateParentRule() is null)
            {
                throw new NullReferenceException("Cannot get chain from immediate parent as there is no parent. Make sure rule.HasParentRule() returns true");
            }

            if (!rule.IsChained())
            {
                throw new InvalidOperationException("Cannot get the chain from the immediate parent of this rule as this rule is not chained to its parent (it is nested inside its parent)");
            }

            return ParentDictionary[rule].ChainAttempt;
        }

        /// <summary>
        /// Determines if this rule is Nested inside an INestedItemDropRule. A false return means this rule is chained to its parent via an IItemDropRuleChain attempt, as opposed to being nested inside a parent.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing<br/><br/>
        /// Throws <see cref="NullReferenceException"></see> if the rule has no parent. This is to ensure that a false return means the rule is Chained as opposed to nested
        /// </summary>
        public static bool IsNested(this IItemDropRule rule)
        {
            return !rule.IsChained();
        }

        #endregion

        // These methods can also only be called inside predicates since they use the volatile dictionary. Their main use is within predicates to help narrow down the search, but they really shouldn't even be needed
        #region Recursive Parent Finding

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return true. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return false.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing
        /// </summary>
        public static bool HasParentRuleWhere(this IItemDropRule rule, LootPredicate<IItemDropRule> predicate, int? nthParent = null)
        {
            return rule.FindParentRuleWhere(predicate, nthParent) is not null;
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return it. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return null.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing        
        /// </summary>
        public static IItemDropRule FindParentRuleWhere(this IItemDropRule rule, LootPredicate<IItemDropRule> predicate, int? nthParent = null)
        {
            return rule.FindParentRuleWhere<IItemDropRule>(predicate, nthParent);
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return true. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return false.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing
        /// </summary>
        public static bool HasParentRuleWhere<R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where R : class, IItemDropRule
        {
            return rule.FindParentRuleWhere(predicate, nthParent) is not null;
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return it. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return null.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing        
        /// </summary>
        public static R FindParentRuleWhere<R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where R : class, IItemDropRule
        {
            IItemDropRule currParent = rule.ImmediateParentRule();
            int n = 1;
            while (currParent is not null)
            {
                if (currParent is R castedParent && predicate(castedParent) && (nthParent is null || nthParent == n))
                {
                    return castedParent;
                }

                n++;
                currParent = currParent.ImmediateParentRule();

            }
            return null;
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return true. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return false.<br/><br/>
        /// </summary>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing
        public static bool HasParentRuleWhere<P, C, R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return rule.FindParentRuleWhere<P, C, R>(predicate, nthParent) is not null;
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, is chained onto a parent rule of type <typeparamref name="P"/> via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="C"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return it. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return null.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing        
        /// </summary>
        public static R FindParentRuleWhere<P, C, R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where P : class, IItemDropRule where C : class, IItemDropRuleChainAttempt where R : class, IItemDropRule
        {
            return rule.FindParentRuleWhere<R>(rule => rule.HasParentRule() && rule.IsChained() && rule.ChainFromImmediateParent() is C && predicate(rule), nthParent);
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, is chained onto its parent via an <see cref="IItemDropRuleChainAttempt"/> of type <typeparamref name="N"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return true. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return false.<br/><br/>
        /// </summary>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing
        public static bool HasParentRuleWhere<N, R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return rule.FindParentRuleWhere<N, R>(predicate, nthParent) is not null;
        }

        /// <summary>
        /// Recursively looks at <see cref="IItemDropRule"/>'s parent <see cref="IItemDropRule"/> and sees if it is of type <typeparamref name="R"/>, is directly nested inside an <see cref="INestedItemDropRule"/> of type <typeparamref name="N"/>, and if it matches the given <paramref name="predicate"/>. If <paramref name="nthParent"/> is specified, then the rule must be the nth parent of the rule this was originally called on.
        /// If a match is found it will return it. If no match is found, it will recursively look at the parent of the last queried parent and do the same check, so on and so forth.
        /// If nothing was found after searching all parents recursively, it will return null.<br/><br/>
        /// 
        /// Can only be used in the context of RemoveWhere/FindWhere/Has recursive predicates, as it uses a Dictionary to find the parent, and the dictionary is only populated
        /// (accurate) during these calls and is cleared after. Mainly used inside predicates of Remove/Find as a means of being more exact about what
        /// rule we are predicateing        
        /// </summary>
        public static R FindParentRuleWhere<N, R>(this IItemDropRule rule, LootPredicate<R> predicate, int? nthParent = null) where N : class, IItemDropRule where R : class, IItemDropRule
        {
            return rule.FindParentRuleWhere<R>(rule => rule.HasParentRule() && rule.IsNested() && rule.ImmediateParentRule() is N && predicate(rule), nthParent);
        }

        #endregion

        #region Other ILoot Extensions

        /// <summary>Syntax sugar to clear this ILoot. What it does is calls RemoveWhere(_ => true) </summary>
        public static void Clear(this ILoot loot)
        {
            loot.RemoveWhere(_ => true);
        }

        #endregion

        #region Other IItemDropRule Extensions

        public static IItemDropRule MultipleOnSuccess(this IItemDropRule parent, bool hideLootReport, params IItemDropRule[] rulesToExecute)
        {
            foreach (IItemDropRule rule in rulesToExecute)
            {
                rule.OnSuccess(rule, hideLootReport);
            }
            return parent;
        }

        public static IItemDropRule MultipleOnFailure(this IItemDropRule parent, bool hideLootReport, params IItemDropRule[] rulesToExecute)
        {
            foreach (IItemDropRule rule in rulesToExecute)
            {
                rule.OnFailedRoll(rule, hideLootReport);
            }
            return parent;
        }

        public static IItemDropRule MultipleOnFailedConditions(this IItemDropRule parent, bool hideLootReport, params IItemDropRule[] rulesToExecute)
        {
            foreach (IItemDropRule rule in rulesToExecute)
            {
                rule.OnFailedRoll(rule, hideLootReport);
            }
            return parent;
        }

        #endregion

        #region OneFromOptionsDropRule Extensions

        public static void AddOption(this OneFromOptionsDropRule rule, int option)
        {
            List<int> asList = rule.dropIds.ToList();
            asList.Add(option);
            rule.dropIds = asList.ToArray();
        }

        public static bool RemoveOption(this OneFromOptionsDropRule rule, int removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this OneFromOptionsDropRule rule, params int[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool ContainsOption(this OneFromOptionsDropRule rule, int option)
        {
            return rule.dropIds.Contains(option);
        }

        public static bool FilterOptions(this OneFromOptionsDropRule rule, Predicate<int> predicate)
        {
            bool anyFiltered = false;
            List<int> newDropIds = new();
            foreach (int dropId in rule.dropIds)
            {
                if (predicate(dropId))
                {
                    newDropIds.Add(dropId);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.dropIds = newDropIds.ToArray();
            return anyFiltered;
        }


        #endregion

        #region OneFromOptionsNotScaledWithLuckDropRule Extensions

        public static void AddOption(this OneFromOptionsNotScaledWithLuckDropRule rule, int option)
        {
            List<int> asList = rule.dropIds.ToList();
            asList.Add(option);
            rule.dropIds = asList.ToArray();
        }

        public static bool RemoveOption(this OneFromOptionsNotScaledWithLuckDropRule rule, int removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this OneFromOptionsNotScaledWithLuckDropRule rule, params int[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool ContainsOption(this OneFromOptionsNotScaledWithLuckDropRule rule, int option)
        {
            return rule.dropIds.Contains(option);
        }

        public static bool FilterOptions(this OneFromOptionsNotScaledWithLuckDropRule rule, Predicate<int> predicate)
        {
            bool anyFiltered = false;
            List<int> newDropIds = new();
            foreach (int dropId in rule.dropIds)
            {
                if (predicate(dropId))
                {
                    newDropIds.Add(dropId);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.dropIds = newDropIds.ToArray();
            return anyFiltered;
        }

        #endregion

        #region FewFromOptionsDropRule Extensions

        public static void AddOption(this FewFromOptionsDropRule rule, int option)
        {
            List<int> asList = rule.dropIds.ToList();
            asList.Add(option);
            rule.dropIds = asList.ToArray();
        }

        public static bool RemoveOption(this FewFromOptionsDropRule rule, int removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this FewFromOptionsDropRule rule, params int[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool ContainsOption(this FewFromOptionsDropRule rule, int option)
        {
            return rule.dropIds.Contains(option);
        }

        public static bool FilterOptions(this FewFromOptionsDropRule rule, Predicate<int> predicate)
        {
            bool anyFiltered = false;
            List<int> newDropIds = new();
            foreach (int dropId in rule.dropIds)
            {
                if (predicate(dropId))
                {
                    newDropIds.Add(dropId);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.dropIds = newDropIds.ToArray();
            return anyFiltered;
        }

        #endregion

        #region FewFromOptionsNotScaledWithLuckDropRule Extensions

        public static void AddOption(this FewFromOptionsNotScaledWithLuckDropRule rule, int option)
        {
            List<int> asList = rule.dropIds.ToList();
            asList.Add(option);
            rule.dropIds = asList.ToArray();
        }

        public static bool RemoveOption(this FewFromOptionsNotScaledWithLuckDropRule rule, int removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this FewFromOptionsNotScaledWithLuckDropRule rule, params int[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool ContainsOption(this FewFromOptionsNotScaledWithLuckDropRule rule, int option)
        {
            return rule.dropIds.Contains(option);
        }

        public static bool FilterOptions(this FewFromOptionsNotScaledWithLuckDropRule rule, Predicate<int> predicate)
        {
            bool anyFiltered = false;
            List<int> newDropIds = new();
            foreach (int dropId in rule.dropIds)
            {
                if (predicate(dropId))
                {
                    newDropIds.Add(dropId);

                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.dropIds = newDropIds.ToArray();
            return anyFiltered;
        }

        #endregion

        #region OneFromRulesRule Extensions

        public static void AddOption(this OneFromRulesRule oneFromRulesRule, IItemDropRule option)
        {
            List<IItemDropRule> asList = oneFromRulesRule.options.ToList();
            asList.Add(option);
            oneFromRulesRule.options = asList.ToArray();
        }

        public static bool RemoveOption(this OneFromRulesRule rule, IItemDropRule removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this OneFromRulesRule rule, params IItemDropRule[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool RemoveOption(this OneFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            return rule.FilterOptions(rule => !predicate(rule));
        }

        public static bool ContainsOption(this OneFromRulesRule rule, IItemDropRule ruleOption)
        {
            return rule.options.Contains(ruleOption);
        }

        public static bool ContainsOption(this OneFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            foreach (IItemDropRule nestedRule in rule.options)
            {
                if (predicate(nestedRule))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterOptions(this OneFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            bool anyFiltered = false;
            List<IItemDropRule> newOptions = new();
            foreach (IItemDropRule nestedRule in rule.options)
            {
                if (predicate(nestedRule))
                {
                    newOptions.Add(nestedRule);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.options = newOptions.ToArray();
            return anyFiltered;
        }

        #endregion

        #region FewFromRulesRule Extensions

        public static void AddOption(this FewFromRulesRule rule, IItemDropRule option)
        {
            List<IItemDropRule> asList = rule.options.ToList();
            asList.Add(option);
            rule.options = asList.ToArray();
        }

        public static bool RemoveOption(this FewFromRulesRule rule, IItemDropRule removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this FewFromRulesRule rule, params IItemDropRule[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool RemoveOption(this FewFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            return rule.FilterOptions(rule => !predicate(rule));
        }

        public static bool ContainsOption(this FewFromRulesRule rule, IItemDropRule ruleOption)
        {
            return rule.options.Contains(ruleOption);
        }
        public static bool ContainsOption(this FewFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            foreach (IItemDropRule nestedRule in rule.options)
            {
                if (predicate(nestedRule))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterOptions(this FewFromRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            bool anyFiltered = false;
            List<IItemDropRule> newOptions = new();
            foreach (IItemDropRule nestedRule in rule.options)
            {
                if (predicate(nestedRule))
                {
                    newOptions.Add(nestedRule);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.options = newOptions.ToArray();
            return anyFiltered;
        }

        #endregion

        #region SequentialRulesRule Extensions

        public static void AddOption(this SequentialRulesRule rule, IItemDropRule option)
        {
            List<IItemDropRule> asList = rule.rules.ToList();
            asList.Add(option);
            rule.rules = asList.ToArray();
        }

        public static bool RemoveOption(this SequentialRulesRule rule, IItemDropRule removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this SequentialRulesRule rule, params IItemDropRule[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool RemoveOption(this SequentialRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            return rule.FilterOptions(rule => !predicate(rule));
        }

        public static bool ContainsOption(this SequentialRulesRule rule, IItemDropRule ruleOption)
        {
            return rule.rules.Contains(ruleOption);
        }

        public static bool ContainsOption(this SequentialRulesRule rule, Predicate<IItemDropRule> predicate)
        {
            foreach (IItemDropRule nestedRule in rule.rules)
            {
                if (predicate(nestedRule))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterOptions(this SequentialRulesRule sequentialRulesRule, Predicate<IItemDropRule> predicate)
        {
            bool anyFiltered = false;
            List<IItemDropRule> newRules = new();
            foreach (IItemDropRule nestedRule in sequentialRulesRule.rules)
            {
                if (predicate(nestedRule))
                {
                    newRules.Add(nestedRule);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            sequentialRulesRule.rules = newRules.ToArray();
            return anyFiltered;
        }

        #endregion

        #region SequentialRulesNotScalingWithLuckRule Extensions

        public static void AddOption(this SequentialRulesNotScalingWithLuckRule rule, IItemDropRule option)
        {
            List<IItemDropRule> asList = rule.rules.ToList();
            asList.Add(option);
            rule.rules = asList.ToArray();
        }

        public static bool RemoveOption(this SequentialRulesNotScalingWithLuckRule rule, IItemDropRule removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this SequentialRulesNotScalingWithLuckRule rule, params IItemDropRule[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool RemoveOption(this SequentialRulesNotScalingWithLuckRule rule, Predicate<IItemDropRule> predicate)
        {
            return rule.FilterOptions(rule => !predicate(rule));
        }

        public static bool ContainsOption(this SequentialRulesNotScalingWithLuckRule rule, IItemDropRule ruleOption)
        {
            return rule.rules.Contains(ruleOption);
        }
        public static bool ContainsOption(this SequentialRulesNotScalingWithLuckRule rule, Predicate<IItemDropRule> predicate)
        {
            foreach (IItemDropRule nestedRule in rule.rules)
            {
                if (predicate(nestedRule))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterOptions(this SequentialRulesNotScalingWithLuckRule rule, Predicate<IItemDropRule> predicate)
        {
            bool anyFiltered = false;
            List<IItemDropRule> newRules = new();
            foreach (IItemDropRule nestedRule in rule.rules)
            {
                if (predicate(nestedRule))
                {
                    newRules.Add(nestedRule);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.rules = newRules.ToArray();
            return anyFiltered;
        }

        #endregion

        #region AlwaysAtLeastOneSuccessDropRule Extensions

        public static void AddOption(this AlwaysAtleastOneSuccessDropRule rule, IItemDropRule option)
        {
            List<IItemDropRule> asList = rule.rules.ToList();
            asList.Add(option);
            rule.rules = asList.ToArray();
        }

        public static bool RemoveOption(this AlwaysAtleastOneSuccessDropRule rule, IItemDropRule removing)
        {
            return rule.RemoveMultipleOptions(removing);
        }

        public static bool RemoveMultipleOptions(this AlwaysAtleastOneSuccessDropRule rule, params IItemDropRule[] removing)
        {
            return rule.FilterOptions(option => !removing.Contains(option));
        }

        public static bool RemoveOption(this AlwaysAtleastOneSuccessDropRule rule, Predicate<IItemDropRule> predicate)
        {
            return rule.FilterOptions(rule => !predicate(rule));
        }

        public static bool ContainsOption(this AlwaysAtleastOneSuccessDropRule rule, IItemDropRule ruleOption)
        {
            return rule.rules.Contains(ruleOption);
        }

        public static bool ContainsOption(this AlwaysAtleastOneSuccessDropRule rule, Predicate<IItemDropRule> predicate)
        {
            foreach (IItemDropRule option in rule.rules)
            {
                if (predicate(option))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterOptions(this AlwaysAtleastOneSuccessDropRule rule, Predicate<IItemDropRule> predicate)
        {
            bool anyFiltered = false;
            List<IItemDropRule> newRules = new();
            foreach (IItemDropRule option in rule.rules)
            {
                if (predicate(option))
                {
                    newRules.Add(option);
                }
                else
                {
                    anyFiltered = true;
                }
            }
            rule.rules = newRules.ToArray();
            return anyFiltered;
        }

        #endregion
    }
}