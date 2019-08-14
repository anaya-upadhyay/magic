﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    [Slot(Name = "switch")]
    public class Switch : ISlot, IMeta
    {
        readonly ISignaler _signaler;

        public Switch(IServiceProvider services)
        {
            _signaler = services.GetService(typeof(ISignaler)) as ISignaler;
        }

        public void Signal(Node input)
        {
            if (!input.Children.Any(x => x.Name == "case"))
                throw new ApplicationException("[switch] must have one at least one [case] child");

            if (input.Children.Any((x) => x.Name != "case" && x.Name != "default"))
                throw new ApplicationException("[switch] can only handle [case] and [default] children");

            if (input.Children.Any(x => x.Name == "case" && x.Value == null))
                throw new ApplicationException("[case] with null value found");

            if (input.Children.Any(x => x.Name == "default" && x.Value != null))
                throw new ApplicationException("[default] with non-null value found");

            var result = input.Get();

            var executionNode = input.Children
                .Where((x) => x.Name == "case" && x.Value.Equals(result))
                .FirstOrDefault() ?? input.Children.Where((x) => x.Name == "default").FirstOrDefault();

            if (executionNode != null)
                _signaler.Signal(executionNode.Name, executionNode);
        }

        public IEnumerable<Node> GetArguments()
        {
            yield return new Node(":", "*");
            yield return new Node("*", 1);
            yield return new Node("case", "*");
            yield return new Node("default", 1);
        }
    }
}
