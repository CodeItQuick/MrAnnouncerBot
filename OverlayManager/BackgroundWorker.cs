using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using TwitchLib.Client.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client;
using Microsoft.AspNetCore.SignalR;
using OverlayManager.Hubs;

namespace OverlayManager
{
	public class BackgroundWorker: IHostedService
	{
		private const string STR_ChannelName = "CodeRushed";
		private const string STR_TwitchUserName = "MrAnnouncerGuy";
		TwitchClient twitchClient = new TwitchClient();
		readonly IHubContext<CodeRushedHub, IOverlayCommands> hub;
		public BackgroundWorker(IConfiguration configuration, IHubContext<CodeRushedHub, IOverlayCommands> hub)
		{
			this.hub = hub;
			Configuration = configuration;
		}

		private void ConnectTwitchClient()
		{
			var oAuthToken = Configuration["Secrets:TwitchBotOAuthToken"];
			var connectionCredentials = new ConnectionCredentials(STR_TwitchUserName, oAuthToken);
			twitchClient.Initialize(connectionCredentials, STR_ChannelName);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			ConnectTwitchClient();
			HookEvents();
			twitchClient.Connect();
			BotCore.Twitch.InitializeConnections();
			return Task.CompletedTask;
		}

		public void HookEvents()
		{
			twitchClient.OnLog += TwitchClient_OnLog;
			twitchClient.OnConnectionError += TwitchClient_OnConnectionError;
			twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
			twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
			twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
			twitchClient.OnUserJoined += TwitchClient_OnUserJoined;
			twitchClient.OnUserLeft += TwitchClient_OnUserLeft;
		}

		private void TwitchClient_OnUserLeft(object sender, TwitchLib.Client.Events.OnUserLeftArgs e)
		{
			
		}

		private void TwitchClient_OnUserJoined(object sender, TwitchLib.Client.Events.OnUserJoinedArgs e)
		{
			
		}

		private void TwitchClient_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
		{
			
		}

		void Launch(ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Launch", "", chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Right(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Right", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Drone(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Drone", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Bee(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Bee", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void MoveAbsolute(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("MoveAbsolute", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void MoveRelative(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("MoveRelative", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Left(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Left", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Up(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Up", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void ChangePlanet(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("ChangePlanet", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void ClearQuiz(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("ClearQuiz", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void StartQuiz(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("StartQuiz", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void ShowLastQuizResults(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("ShowLastQuizResults", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void AnswerQuiz(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("AnswerQuiz", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Drop(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Drop", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Dock(ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Dock", "", chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Down(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Down", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Chutes(ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Chutes", "", chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Extend(ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Extend", "", chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}
		void Retract(ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Retract", "", chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}

		void PlantSeed(string args, ChatMessage chatMessage)
		{
			hub.Clients.All.ExecuteCommand("Seed", args, chatMessage.UserId, chatMessage.DisplayName, chatMessage.ColorHex);
		}

		private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
		{
			string cmdText = e.Command.CommandText.ToLower();
			string args = e.Command.ArgumentsAsString;
			ChatMessage chatMessage = e.Command.ChatMessage;
			switch (cmdText)
			{
				case "cmd":
				case "?":
					Chat($"CodeRushed Rocket controls: launch, dock, retract, extend, drop, up, down, left, & right. Bee controls: \"bee me\", splat {{color}}, say {{message}}, and \"fly x, y\", where x and y are relative coordinates to your bee, in bee units.");
					break;
				case "launch": Launch(chatMessage); break;
				case "up": Up(args, chatMessage); break;
				case "down": Down(args, chatMessage); break;
				case "left": Left(args, chatMessage); break;
				case "right": Right(args, chatMessage); break;
				case "bee": Bee(args, chatMessage); break;
				case "moverel":
				case "mr":
					MoveRelative(args, chatMessage); break;
				case "moveabs":
				case "ma":
					MoveAbsolute(args, chatMessage); break;
				case "drone": Drone(args, chatMessage); break;
				case "dock": Dock(chatMessage); break;
				case "drop": Drop(args, chatMessage); break;
				case "extend": Extend(chatMessage); break;
				case "retract": Retract(chatMessage); break;
				case "chutes": Chutes(chatMessage); break;
				case "seed": PlantSeed(args, chatMessage); break;
				case "planet": ChangePlanet(args, chatMessage); break;
				case "quiz": StartQuiz(args, chatMessage); break; // Posed in the form of "!quiz What would you rather be? 1. Bee, 2. Drone"
				case "clearquiz": ClearQuiz(args, chatMessage); break; // Posed in the form of "!quiz What would you rather be? 1. Bee, 2. Drone"
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					AnswerQuiz(cmdText, chatMessage); break;
			}
		}

		private void Chat(string message)
		{
			twitchClient.SendMessage(STR_ChannelName, message);
		}

		private void TwitchClient_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
		{
			
		}

		private void TwitchClient_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
		{
			
		}

		private void TwitchClient_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
		{
			
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			twitchClient.Disconnect();
			return null;
		}
		public IConfiguration Configuration { get; set; }
	}
}
