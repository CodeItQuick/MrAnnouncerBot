﻿using System;
using System.Linq;

namespace DndCore
{
	public class DndExhaustionLevelsVariable : DndVariable
	{
		public override bool Handles(string tokenName)
		{
			return Enum.TryParse(tokenName, out ExhaustionLevels result);
		}

		public override object GetValue(string variableName, Character player)
		{
			if (Enum.TryParse(variableName, out ExhaustionLevels result))
				return result;
			return null;
		}
	}
}
