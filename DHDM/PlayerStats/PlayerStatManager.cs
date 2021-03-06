﻿//#define profiling
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using DndCore;

namespace DHDM
{
	public class PlayerStatManager
	{
		public string LatestCommand { get; set; }
		public string LatestData { get; set; }
		public bool RollingTheDiceNow { get; set; }
		public bool HideSpellScrolls { get; set; }
		public int ActiveTurnCreatureID { get; set; }  // Negative numbers are for in-game creatures (not players)
		

		List<PlayerStats> players;
		public List<PlayerStats> Players
		{
			get
			{
				if (players == null)
					players = new List<PlayerStats>();
				return players;
			}

			set
			{
				players = value;
			}
		}

		public PlayerStatManager()
		{

		}

		public bool AnyoneIsReadyToRoll => Players.FirstOrDefault(x => x.ReadyToRollDice) != null;
		

		public PlayerStats GetPlayerStats(int playerId)
		{
			PlayerStats foundPlayer = Players.FirstOrDefault(x => x.PlayerId == playerId);
			if (foundPlayer != null)
				return foundPlayer;

			PlayerStats playerState = new PlayerStats(playerId);
			Players.Add(playerState);
			return playerState;
		}

		public void ToggleReadyRollD20(int playerId)
		{
			PlayerStats playerStats = GetPlayerStats(playerId);
			playerStats.DiceStack.Clear();
			playerStats.AddD20();

			if (playerStats.Vantage == VantageKind.Normal || !playerStats.ReadyToRollDice)
				playerStats.ReadyToRollDice = !playerStats.ReadyToRollDice;
			else
				playerStats.Vantage = VantageKind.Normal;
		}

		public void ToggleCondition(int playerId, Conditions conditions)
		{
			PlayerStats playerStats = GetPlayerStats(playerId);
			if (playerStats.Conditions.HasFlag(conditions))  // Bit is set.
				playerStats.Conditions &= ~conditions;  // clear the bit
			else
				playerStats.Conditions |= conditions;
		}

		public void ClearConditions(int playerId)
		{
			PlayerStats playerStats = GetPlayerStats(playerId);
			playerStats.Conditions = Conditions.None;
		}

		public void SetReadyRollDice(int playerId, bool newValue, DieRollDetails dieRollDetails)
		{
			PlayerStats playerState = GetPlayerStats(playerId);
			playerState.ReadyToRollDice = newValue;
			playerState.ClearDiceStack();
			foreach (Roll roll in dieRollDetails.Rolls)
				playerState.AddRoll(roll);
		}

		public void ReadyRollVantage(int playerId, VantageKind vantage)
		{
			PlayerStats playerState = GetPlayerStats(playerId);

			playerState.ReadyToRollDice = true;
			if (playerState.DiceStack.Count == 0)
				playerState.AddD20();
			playerState.Vantage = vantage;
		}

		public List<int> GetReadyToRollPlayerIds()
		{
			return Players.Where(x => x.ReadyToRollDice).Select(x => x.PlayerId).ToList();
		}
		
		public void ClearReadyToRollState()
		{
			foreach (PlayerStats playerStats in Players)
			{
				playerStats.ReadyToRollDice = false;
				playerStats.Vantage = VantageKind.Normal;
			}
		}
		
		public void ReadyRollDice(string data)
		{
			bool newReadyState = false;
			if (data == "All")
				newReadyState = true;
			foreach (PlayerStats playerStats in Players)
			{
				playerStats.ClearDiceStack();
				playerStats.Vantage = VantageKind.Normal;
				playerStats.ReadyToRollDice = newReadyState;
				if (playerStats.ReadyToRollDice)
					playerStats.AddD20();
			}
		}

		public void ClearAllActiveTurns()
		{
			ActiveTurnCreatureID = -1;
		}

		public void ClearAll()
		{
			ClearAllActiveTurns();
			Players = null;
		}

		public void ToggleTarget(int playerId)
		{
			PlayerStats playerStats = GetPlayerStats(playerId);
			if (playerStats == null)
				return;
			playerStats.IsTargeted = !playerStats.IsTargeted;
		}

		public void ClearAllTargets()
		{
			foreach (PlayerStats playerStats in Players)
				playerStats.IsTargeted = false;
		}

		public void TargetAll()
		{
			foreach (PlayerStats playerStats in Players)
				playerStats.IsTargeted = true;
		}

		public List<PlayerStats> GetTargeted()
		{
			return Players.Where(x => x.IsTargeted).ToList();
		}
	}
}

// HubtasticBaseStation.ChangePlayerHealth(JsonConvert.SerializeObject(damageHealthChange));