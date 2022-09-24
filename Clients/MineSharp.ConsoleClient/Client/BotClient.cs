﻿namespace MineSharp.ConsoleClient.Client {
    public static class BotClient {

        public static MemoryStream BotLog = new MemoryStream();
        private static StreamWriter? BotLogWriter;

        public static void Initialize (Bot.MinecraftBot.BotOptions options) {
            BotLogWriter = new StreamWriter(BotLog) {
                AutoFlush = true,
            };

            Core.Logging.Logger.AddScope(Core.Logging.LogLevel.DEBUG3, (s) => BotLogWriter.WriteLine(s));
            Bot = new Bot.MinecraftBot(options);
        }

        public static Bot.MinecraftBot? Bot;

    }
}
