﻿using System;
using System.Linq;
using System.Collections.Generic;
using CodingSeb.ExpressionEvaluator;

namespace DndCore
{
	[Tooltip("Gives the specified magic to the specified target with the specified parameters")]
	[Param(1, typeof(string), "magicItem", "The name of the magic item to give.", ParameterIs.Required)]
	[Param(2, typeof(object), "data1", "Data parameter 1 for the magic item.", ParameterIs.Optional)]
	[Param(3, typeof(object), "data2", "Data parameter 2 for the magic item.", ParameterIs.Optional)]
	[Param(4, typeof(object), "data3", "Data parameter 3 for the magic item.", ParameterIs.Optional)]
	[Param(5, typeof(object), "data4", "Data parameter 4 for the magic item.", ParameterIs.Optional)]
	[Param(6, typeof(object), "data5", "Data parameter 5 for the magic item.", ParameterIs.Optional)]
	[Param(7, typeof(object), "data6", "Data parameter 6 for the magic item.", ParameterIs.Optional)]
	[Param(8, typeof(object), "data7", "Data parameter 7 for the magic item.", ParameterIs.Optional)]
	[Param(9, typeof(object), "data8", "Data parameter 8 for the magic item.", ParameterIs.Optional)]

	public class GiveMagic : DndFunction
	{
		public override string Name { get; set; } = "GiveMagic";

		public override object Evaluate(List<string> args, ExpressionEvaluator evaluator, Character player, Target target = null, CastedSpell spell = null, DiceStoppedRollingData dice = null)
		{
			ExpectingArguments(args, 2, 9);
			if (player != null)
			{
				string magicItemName = args[0];

				object data1 = null;
				object data2 = null;
				object data3 = null;
				object data4 = null;
				object data5 = null;
				object data6 = null;
				object data7 = null;
				object data8 = null;

				if (args.Count > 1)
					data1 = Expressions.Get(args[1], player, target, spell);
				if (args.Count > 2)
					data2 = Expressions.Get(args[2], player, target, spell);
				if (args.Count > 3)
					data3 = Expressions.Get(args[3], player, target, spell);
				if (args.Count > 4)
					data4 = Expressions.Get(args[4], player, target, spell);
				if (args.Count > 5)
					data5 = Expressions.Get(args[5], player, target, spell);
				if (args.Count > 6)
					data6 = Expressions.Get(args[6], player, target, spell);
				if (args.Count > 7)
					data7 = Expressions.Get(args[7], player, target, spell);
				if (args.Count > 8)
					data8 = Expressions.Get(args[8], player, target, spell);

				if (target == null)
					target = player.ActiveTarget;
				player.GiveMagic(magicItemName, target, data1, data2, data3, data4, data5, data6, data7, data8);
			}

			return null;
		}
	}
}
