﻿using System;
using System.Linq;
using System.ComponentModel;

namespace DndCore
{
	[TypeConverter("DndCore.EnumDescriptionTypeConverter")]
	public enum PlayerProperty
	{
		None,
		Alignment,
		ArmorClass,
		Charisma,
		Constitution,
		DeathSaves,
		Dexterity,
		ExperiencePoints,
		GoldPieces,
		HitDice,
		HitPoints,
		TempHitPoints,
		Initiative,
		Inspiration,
		Intelligence,
		Level,
		Load,
		[Description("Name/Headshot")]
		NameHeadshot,
		Perception,
		ProficiencyBonus,
		Race,
		SavingCharisma,
		SavingConstitution,
		SavingDexterity,
		SavingIntelligence,
		SavingStrength,
		SavingWisdom,
		SkillsAcrobatics,
		SkillsAnimalHandling,
		SkillsArcana,
		SkillsAthletics,
		SkillsDeception,
		SkillsHistory,
		SkillsInsight,
		SkillsIntimidation,
		SkillsInvestigation,
		SkillsMedicine,
		SkillsNature,
		SkillsPerception,
		SkillsPerformance,
		SkillsPersuasion,
		SkillsReligion,
		SkillsSleightOfHand,
		SkillsStealth,
		SkillsSurvival,
		WalkingSpeed,
		Strength,
		Weight,
		Wisdom
	}
}
