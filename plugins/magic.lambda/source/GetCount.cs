﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.source
{
    [Slot(Name = "get-count")]
    public class GetCount : ISlot, IMeta
    {
        public void Signal(Node input)
        {
            if (input.Value == null)
                throw new ApplicationException("No expression source provided for [count]");

            var src = input.Evaluate();
            input.Value = src.Count();
        }

        public IEnumerable<Node> GetArguments()
        {
            yield return new Node(":", "x");
        }
    }
}